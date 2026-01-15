# MAS Student Services Platform - AI Agent Guide

## Project Overview
ASP.NET Core 8 Blazor WebAssembly + Server application for student services marketplace. Full-stack Arabic-first platform with bilingual support (AR/EN), e-commerce capabilities, and comprehensive admin CMS.

## Architecture Pattern: Layered + Repository/UoW

### Core Layers
- **Presentation**: Blazor Server components (`mas/Components/Pages/`) + WebAssembly client (`mas.Client/`)
- **API**: RESTful controllers (`mas/Controllers/`) - no `/api/` prefix routing convention
- **Business Logic**: Service layer (`Services/`) with caching via `ICacheService`
- **Data Access**: Repository pattern + Unit of Work (`Repositories/`) wrapping EF Core
- **Domain**: Models with base entities (`Models/`, `Models/Base/`)

### Key Architectural Decisions
- **DbContext Factory Pattern**: Use `AddDbContextFactory<ApplicationDbContext>()` for Blazor Server (line 26-32 in [mas/Program.cs](mas/Program.cs#L26-L32)) to avoid long-lived contexts in SignalR circuits
- **Dual Render Modes**: Server-side for admin, WebAssembly for public pages - components use `@rendermode` directives
- **No API Controllers in WebAssembly**: Client uses HttpClient to call server API endpoints

## Critical UTF-8/Arabic Handling
Arabic text corruption is a known issue. **Always** enforce UTF-8:
- Set console encoding at app start ([Program.cs](mas/Program.cs#L11-L13)):
  ```csharp
  Console.OutputEncoding = Encoding.UTF8;
  Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
  ```
- SQLite pragma in seeder ([DatabaseSeeder.cs](mas/Data/DatabaseSeeder.cs#L12-L13)): `PRAGMA encoding = 'UTF-8';`
- Arabic fonts (Cairo, Tajawal) loaded via `wwwroot/css/arabic-fonts.css`

## Data Access Conventions

### Repository Pattern Usage
- Repositories expose `AsNoTracking()` queries for read operations ([Repository.cs](Repositories/Repository.cs#L24))
- Services use `IUnitOfWork` injected via DI, never `ApplicationDbContext` directly
- Example: [ProductService.cs](Services/ProductService.cs#L9-L25) shows proper layering

### Base Entity Features
All models inherit `BaseEntity` ([Models/Base/BaseEntity.cs](Models/Base/BaseEntity.cs)) providing:
- Auto-timestamps: `CreatedAt`, `UpdatedAt`
- Audit trails: `CreatedBy`, `UpdatedBy` (set in `SaveChangesAsync()` override)
- Soft delete: Implement `ISoftDelete` interface for logical deletion

### Transaction Management
Use `UnitOfWork` for multi-repository operations ([UnitOfWork.cs](Repositories/UnitOfWork.cs#L43-L60)):
```csharp
await _unitOfWork.BeginTransactionAsync();
try {
    await _unitOfWork.Orders.AddAsync(order);
    await _unitOfWork.SaveChangesAsync();
    await _unitOfWork.CommitTransactionAsync();
}
catch { await _unitOfWork.RollbackTransactionAsync(); }
```

## Service Layer Patterns

### Caching Strategy
- All read services inject `ICacheService` ([Services/ICacheService.cs](Services/ICacheService.cs))
- Use `GetOrCreateAsync()` pattern with expiration timeouts (see [ProductService.cs](Services/ProductService.cs#L31-L39))
- Cache keys follow `{entity}_{scope}_{id}` convention (e.g., `products_category_5`)
- Invalidate by prefix on updates: `await _cacheService.RemoveByPrefixAsync("products_");`

### AutoMapper DTOs
- All API responses/requests use DTOs, never domain models directly
- Mappings in [Mappings/MappingProfile.cs](Mappings/MappingProfile.cs)
- Controllers inject `IMapper` but services handle mapping logic

## Bilingual Content Management

### LanguageService
- Scoped service ([mas/Services/LanguageService.cs](mas/Services/LanguageService.cs)) with `OnLanguageChanged` event
- All entities have `NameAr`/`NameEn`, `DescriptionAr`/`DescriptionEn` properties
- UI displays via `CurrentLanguage` check: `@(lang.IsArabic ? product.NameAr : product.NameEn)`

### RTL/LTR Handling
- `Direction` property in LanguageService returns `"rtl"` or `"ltr"`
- Apply to layout: `<div dir="@languageService.Direction">`

## Authentication & Authorization

### Identity Setup
- ASP.NET Core Identity with `ApplicationUser : IdentityUser`
- Admin role seeded on startup ([Program.cs](mas/Program.cs#L119-L143))
- Default admin: `admin@mas.com` / `Admin@123`
- **All admin pages protected**: `[Authorize(Policy = "AdminOnly")]` on all `/admin/*` pages
- **Admin panel hidden**: Not visible in main navigation - direct access only
- **Login required**: Accessing `/admin` without login redirects to `/Account/Login`

### Access Admin Panel
1. Navigate directly to: `/Account/Login`
2. Use credentials: `admin@mas.com` / `Admin@123`
3. After login, redirects to `/admin` dashboard

### JWT (Currently Configured, Not Fully Implemented)
- Settings in [appsettings.json](mas/appsettings.json#L5-L10) under `JwtSettings`
- Rate limiting enabled (60 req/min) via `IpRateLimiting` config

## Database & Migrations

### SQLite Database
- Connection string: `mas.db` in project root
- Run migrations: `dotnet ef database update` (or auto-applied on startup)
- Reset script: [START_FRESH.ps1](START_FRESH.ps1) deletes DB + rebuilds

### Seeding Strategy
- Arabic data seeded via [DatabaseSeeder.cs](mas/Data/DatabaseSeeder.cs#L9)
- **Important**: Seeder clears existing Categories/Products before re-seeding (lines 17-21)

## Admin CMS Structure

### Security & Access
- **All admin pages require authentication** with `[Authorize(Policy = "AdminOnly")]`
- **Hidden from public navigation** - no links in main menu
- **Login page**: `/Account/Login` - Arabic interface with credentials display
- Admin pages redirect unauthenticated users to login automatically

### Page Routing Convention
Admin pages use `/admin/*` routes:
- Dashboard: `/admin` ([Admin/Dashboard.razor](mas/Components/Pages/Admin/Dashboard.razor))
- Products: `/admin/products` ([Admin/Products.razor](mas/Components/Pages/Admin/Products.razor))
- Add/Edit: `/admin/products/add` or `/admin/products/edit/{id}` ([Admin/AddProduct.razor](mas/Components/Pages/Admin/AddProduct.razor))

### Content Management
- Dynamic pages stored in `Pages` table with slug-based routing (`/page/{slug}`)
- CMS endpoints: `HomeContentController`, `AboutContentController`, `ContactContentController`
- Images uploaded via [ImageController.cs](mas/Controllers/ImageController.cs) - uses `SixLabors.ImageSharp` for processing

## Build & Run Commands

### Development
```powershell
dotnet build                    # Build solution
dotnet run --project mas/mas.csproj  # Run server
```

### Database Reset
```powershell
.\START_FRESH.ps1              # Drops DB, cleans bin/obj, rebuilds, seeds data
```

### Common Issues
- **Arabic shows `???`**: Re-run `START_FRESH.ps1` to re-seed with proper UTF-8
- **Build errors on Migrations**: Delete `Migrations/` folder and run `dotnet ef migrations add InitialCreate`
- **Port conflicts**: Check [launchSettings.json](mas/Properties/launchSettings.json) - default is `https://localhost:5001`

## Testing & Development Patterns

### Admin Access
- Login URL: `/Account/Login` (not linked in public navigation)
- Admin credentials: `admin@mas.com` / `Admin@123`
- After login, access dashboard at `/admin`
- Password requirements: 6+ chars, uppercase, lowercase, digit (no special char required)

### API Testing
- No Swagger configured in this codebase (despite package reference in [mas.csproj](mas/mas.csproj#L42))
- Test endpoints directly via browser or Postman: `GET /api/Products`, `POST /api/ContactController`

## File Organization Rules
- **Never** create files in root `Models/`, `Repositories/`, `Services/` - these are shared across projects
- Blazor components go in `mas/Components/Pages/` or `mas.Client/Pages/`
- Controllers belong in `mas/Controllers/`
- Static assets in `mas/wwwroot/` or `mas.Client/wwwroot/`

## Third-Party Dependencies
- **EF Core 8.0.11**: ORM with SQLite provider
- **AutoMapper 12.0.1**: Object mapping (register profiles in DI)
- **Serilog 8.0.3**: Logging to console + file (`Logs/` directory, 30-day retention)
- **AspNetCoreRateLimit 5.0.0**: API throttling
- **SixLabors.ImageSharp 3.1.8**: Image processing (WebP support included)

## Key Configuration Patterns
- Connection strings in [appsettings.json](mas/appsettings.json#L2-L4) - switch to SQL Server/PostgreSQL by changing provider
- Redis caching: Set `"Caching:UseRedis": true` and provide Redis connection string
- Email config: `Email` section (SMTP not actively used in current code)

## Debugging Tips
- **Admin panel access**: Navigate to `/Account/Login` directly (not visible in public menu)
- **Authorization issues**: Ensure user is logged in and has "Admin" role
- DbContext logging enabled in Development ([Program.cs](mas/Program.cs#L22)): check console for SQL queries
- Blazor Server uses SignalR - check browser console for circuit errors
