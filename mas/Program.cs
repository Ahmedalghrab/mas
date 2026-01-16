using mas.Client.Pages;
using mas.Components;
using mas.Data;
using mas.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Serialization;

// Force UTF-8 encoding for console and all text operations
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

// Configure dynamic port for Railway (Railway provides PORT environment variable)
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Force Production environment on Railway
if (Environment.GetEnvironmentVariable("RAILWAY_ENVIRONMENT") != null)
{
    builder.Environment.EnvironmentName = "Production";
}

// Determine database provider
// Railway provides DATABASE_URL environment variable - read it directly
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

// If Postgres service variables are present, prefer them (internal network) over any public/proxy URL.
var pgHostEnv = Environment.GetEnvironmentVariable("PGHOST");
if (!string.IsNullOrWhiteSpace(pgHostEnv))
{
    var pgDatabaseEnv = Environment.GetEnvironmentVariable("PGDATABASE") ?? "railway";
    var pgUserEnv = Environment.GetEnvironmentVariable("PGUSER") ?? "postgres";
    var pgPasswordEnv = Environment.GetEnvironmentVariable("PGPASSWORD") ?? "";
    var pgPortEnvRaw = Environment.GetEnvironmentVariable("PGPORT") ?? "5432";
    _ = int.TryParse(pgPortEnvRaw, out var pgPortEnv);
    if (pgPortEnv <= 0)
    {
        pgPortEnv = 5432;
    }

    // Internal Railway Postgres is typically plain TCP; avoid SSL negotiation against internal endpoints.
    var csbInternal = new NpgsqlConnectionStringBuilder
    {
        Host = pgHostEnv,
        Port = pgPortEnv,
        Database = pgDatabaseEnv,
        Username = pgUserEnv,
        Password = pgPasswordEnv,
        SslMode = SslMode.Require,
        Timeout = 120,
        CommandTimeout = 120,
        KeepAlive = 30,
        Pooling = true,
        MaxPoolSize = 20,
        MinPoolSize = 0
    };

    connectionString = csbInternal.ConnectionString;
    Console.WriteLine("✓ Using PG* environment variables for PostgreSQL (internal Railway network)");
}

// Debug logging
Console.WriteLine("=== DATABASE CONNECTION DEBUG ===");
Console.WriteLine($"RAILWAY_ENVIRONMENT: {Environment.GetEnvironmentVariable("RAILWAY_ENVIRONMENT") ?? "NULL"}");
Console.WriteLine($"ASPNETCORE_ENVIRONMENT: {builder.Environment.EnvironmentName}");
Console.WriteLine($"DATABASE_URL exists: {!string.IsNullOrEmpty(connectionString)}");

// Fallback to Configuration if env variable not found
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    Console.WriteLine("Using ConnectionStrings:DefaultConnection from appsettings");
}

// Critical: Exit if no connection string found to prevent infinite error loops
if (string.IsNullOrEmpty(connectionString) || connectionString.Length < 10)
{
    Console.WriteLine("==========================================");
    Console.WriteLine("FATAL ERROR: No database connection string found!");
    Console.WriteLine("==========================================");
    var dbUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
    Console.WriteLine($"DATABASE_URL length: {(dbUrl?.Length ?? 0)}");
    Console.WriteLine($"Config DefaultConnection: {builder.Configuration.GetConnectionString("DefaultConnection") ?? "NULL"}");
    Console.WriteLine();
    Console.WriteLine("Available Environment Variables:");
    foreach (System.Collections.DictionaryEntry env in Environment.GetEnvironmentVariables())
    {
        var key = env.Key?.ToString() ?? "";
        if (key.Contains("DATABASE", StringComparison.OrdinalIgnoreCase) || 
            key.Contains("CONNECTION", StringComparison.OrdinalIgnoreCase) ||
            key.Contains("POSTGRES", StringComparison.OrdinalIgnoreCase) ||
            key.Contains("SQL", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"  {key}: [set]");
        }
    }
    Console.WriteLine("==========================================");
    Environment.Exit(1);
}

Console.WriteLine($"✓ Connection String Found (length: {connectionString.Length})");
Console.WriteLine($"✓ Connection starts with: {connectionString.Substring(0, Math.Min(15, connectionString.Length))}...");

var usePostgres = connectionString.Contains("Host=") || connectionString.Contains("postgres");
Console.WriteLine($"✓ Using PostgreSQL: {usePostgres}");

// Convert PostgreSQL URI format to keyword format if needed
if (usePostgres && (connectionString.StartsWith("postgresql://") || connectionString.StartsWith("postgres://")))
{
    try
    {
        var uri = new Uri(connectionString);
        var userInfo = uri.UserInfo.Split(':', 2);
        var username = userInfo.Length > 0 ? userInfo[0] : "";
        var password = userInfo.Length > 1 ? userInfo[1] : "";

        var host = uri.Host;
        var pgPort = uri.Port;
        var database = uri.AbsolutePath.TrimStart('/');

        // Railway TCP proxy typically requires SSL; use higher timeouts to tolerate transient network delays
        var csb = new NpgsqlConnectionStringBuilder
        {
            Host = host,
            Port = pgPort,
            Database = database,
            Username = username,
            Password = password,
            SslMode = SslMode.Require,
            Timeout = 120,
            CommandTimeout = 120,
            KeepAlive = 30,
            Pooling = true,
            MaxPoolSize = 20,
            MinPoolSize = 0
        };

        connectionString = csb.ConnectionString;
        Console.WriteLine($"✓ Converted URI to keyword format: Host={host};Port={pgPort};SSL=Require");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠ Failed to convert URI format: {ex.Message}");
    }
}

// Quick connectivity probe (best-effort) so we can distinguish DNS/TCP issues from SSL/handshake issues
if (usePostgres)
{
    try
    {
        var csbProbe = new NpgsqlConnectionStringBuilder(connectionString);
        if (!string.IsNullOrWhiteSpace(csbProbe.Host))
        {
            await LogTcpConnectivityAsync(csbProbe.Host, csbProbe.Port);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠ Connectivity probe skipped: {ex.GetType().Name}: {ex.Message}");
    }
}

Console.WriteLine("=== END DEBUG ===");

// Add database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (usePostgres)
    {
        options.UseNpgsql(connectionString, npgsql =>
        {
            npgsql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        });
    }
    else
    {
        options.UseSqlite(connectionString, 
            sqliteOptions => sqliteOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery));
    }
    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
});

// DbContext factory for Blazor Server components (avoid long-lived DbContext in circuits)
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
{
    if (usePostgres)
    {
        options.UseNpgsql(connectionString, npgsql =>
        {
            npgsql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        });
    }
    else
    {
        options.UseSqlite(connectionString,
            sqliteOptions => sqliteOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery));
    }
    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
},
    ServiceLifetime.Scoped);

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Add authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// Register image upload service
builder.Services.AddScoped<mas.Services.IImageUploadService, mas.Services.CloudinaryImageService>();

// Add HttpClient for server-side Blazor components
builder.Services.AddHttpClient();
builder.Services.AddScoped(sp =>
{
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    return new HttpClient
    {
        BaseAddress = new Uri(navigationManager.BaseUri)
    };
});

// Add Language Service (Singleton for shared state)
builder.Services.AddScoped<mas.Services.LanguageService>();

// Add controllers for API endpoints
builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(mas.Client._Imports).Assembly);

static async Task LogTcpConnectivityAsync(string host, int port)
{
    try
    {
        using var client = new TcpClient();
        var connectTask = client.ConnectAsync(host, port);
        var completed = await Task.WhenAny(connectTask, Task.Delay(TimeSpan.FromSeconds(5)));
        if (completed != connectTask)
        {
            Console.WriteLine($"✗ TCP connect timeout to {host}:{port}");
            return;
        }
        await connectTask;
        Console.WriteLine($"✓ TCP connect OK to {host}:{port}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"✗ TCP connect failed to {host}:{port}: {ex.GetType().Name}: {ex.Message}");
    }
}

static string FormatExceptionChain(Exception ex)
{
    var parts = new List<string>();
    Exception? current = ex;
    var depth = 0;
    while (current != null && depth < 6)
    {
        parts.Add($"{current.GetType().Name}: {current.Message}");
        current = current.InnerException;
        depth++;
    }
    return string.Join(" | ", parts);
}

static async Task<bool> TryApplyMigrationsAsync(ApplicationDbContext context)
{
    Console.WriteLine("Applying database migrations...");

    const int maxDbAttempts = 6;
    for (var attempt = 1; attempt <= maxDbAttempts; attempt++)
    {
        try
        {
            await context.Database.MigrateAsync();
            Console.WriteLine("✓ Migrations applied successfully");
            return true;
        }
        catch (Exception ex) when (attempt < maxDbAttempts)
        {
            Console.WriteLine($"Database attempt {attempt}/{maxDbAttempts} failed: {FormatExceptionChain(ex)}");
            await Task.Delay(TimeSpan.FromSeconds(10 * attempt));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database attempt {attempt}/{maxDbAttempts} failed (final): {FormatExceptionChain(ex)}");
            return false;
        }
    }

    return false;
}

static async Task InitializeDatabaseAsync(IServiceProvider rootServices)
{
    using var scope = rootServices.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var logger = services.GetRequiredService<ILogger<Program>>();

        // Optional: quick connectivity checks (do not log credentials)
        try
        {
            var db = context.Database.GetDbConnection();
            if (!string.IsNullOrWhiteSpace(db.DataSource))
            {
                // For Npgsql, DataSource contains host (and sometimes port); keep best-effort.
                Console.WriteLine($"DB DataSource: {db.DataSource}");
            }
        }
        catch
        {
            // ignore connectivity probe failures
        }

        // Apply migrations; if DB isn't reachable, skip seeding to avoid long startup failures
        var migrated = await TryApplyMigrationsAsync(context);
        if (!migrated)
        {
            logger.LogError("Database is not reachable; skipping role/user creation and seeding for now.");
            return;
        }

        // Create Admin role if it doesn't exist
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        // Create default admin user
        var adminEmail = "admin@mas.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Administrator",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // Seed Arabic data (fix ??? issue)
        await DatabaseSeeder.SeedArabicDataAsync(context);

        logger.LogInformation("Database seeded successfully with Arabic data");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}

// On Railway: initialize DB before serving requests (so tables exist), but cap the wait.
if (Environment.GetEnvironmentVariable("RAILWAY_ENVIRONMENT") != null)
{
    var initTask = InitializeDatabaseAsync(app.Services);
    var completed = await Task.WhenAny(initTask, Task.Delay(TimeSpan.FromSeconds(60)));
    if (completed != initTask)
    {
        Console.WriteLine("⚠ Database initialization is taking too long; continuing startup (will retry on next deploy/restart).");
    }
}
else
{
    await InitializeDatabaseAsync(app.Services);
}

app.Run();
