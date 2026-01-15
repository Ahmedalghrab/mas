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

// Determine database provider
// Railway provides DATABASE_URL environment variable - read it directly
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

// Fallback to Configuration if env variable not found
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}

// Critical: Exit if no connection string found to prevent infinite error loops
if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("FATAL ERROR: No database connection string found!");
    Console.WriteLine($"DATABASE_URL: {Environment.GetEnvironmentVariable("DATABASE_URL") ?? "NULL"}");
    Console.WriteLine($"Config DefaultConnection: {builder.Configuration.GetConnectionString("DefaultConnection") ?? "NULL"}");
    Environment.Exit(1);
}

Console.WriteLine($"✓ Connection String Found (length: {connectionString.Length})");
Console.WriteLine($"✓ Connection type: {(connectionString.Contains("postgres") ? "PostgreSQL" : "SQLite")}");

var usePostgres = connectionString.Contains("Host=") || connectionString.Contains("postgres");
Console.WriteLine($"✓ Using PostgreSQL: {usePostgres}");

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
builder.Services.AddScoped<Services.IImageUploadService, Services.CloudinaryImageService>();

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
        
        context.Database.Migrate();
        
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
