using mas.Client.Pages;
using mas.Components;
using mas.Data;
using mas.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    Console.WriteLine($"DATABASE_URL: {Environment.GetEnvironmentVariable("DATABASE_URL") ?? "NULL"}");
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
            Console.WriteLine($"  {key}: {env.Value}");
        }
    }
    Console.WriteLine("==========================================");
    Environment.Exit(1);
}

Console.WriteLine($"✓ Connection String Found (length: {connectionString.Length})");
Console.WriteLine($"✓ Connection starts with: {connectionString.Substring(0, Math.Min(15, connectionString.Length))}...");

var usePostgres = connectionString.Contains("Host=") || connectionString.Contains("postgres");
Console.WriteLine($"✓ Using PostgreSQL: {usePostgres}");
Console.WriteLine("=== END DEBUG ===");

// Add database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (usePostgres)
    {
        options.UseNpgsql(connectionString);
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
        options.UseNpgsql(connectionString);
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

// Initialize database and create admin user
using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
      var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        
        // Apply migrations - use EnsureCreated for production to avoid connection string parsing issues
        Console.WriteLine("Ensuring database is created...");
        await context.Database.EnsureCreatedAsync();
        Console.WriteLine("✓ Database ready");
        
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
    
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("? Database seeded successfully with Arabic data");
    }
 catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
   logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}

app.Run();
