# دليل الترحيل والتحديث 🔄

## نظرة عامة

هذا الدليل يساعدك على تطبيق التحديثات على مشروع قائم أو البدء من الصفر.

---

## 📋 خيارات الترحيل

### الخيار 1: مشروع جديد (موصى به)
ابدأ بهذا المشروع المحسّن مباشرة.

### الخيار 2: ترحيل مشروع قائم
اتبع الخطوات أدناه لترحيل مشروعك القديم.

---

## 🚀 البدء من الصفر (مشروع جديد)

### 1. استنساخ أو تحميل المشروع
```bash
cd path/to/mas
```

### 2. استعادة الحزم
```bash
dotnet restore
```

### 3. تحديث قاعدة البيانات
```bash
# إنشاء Migration جديد (إذا لزم الأمر)
dotnet ef migrations add InitialCreate --project mas

# تطبيق Migration
dotnet ef database update --project mas
```

أو ببساطة شغّل المشروع وسيتم التحديث تلقائياً:
```bash
dotnet run --project mas
```

### 4. الوصول للتطبيق
- **الموقع**: https://localhost:5001
- **Swagger**: https://localhost:5001/swagger
- **Admin**: admin@mas.com / Admin@123

---

## 🔄 ترحيل مشروع قائم

### الخطوة 1: Backup
```bash
# نسخة احتياطية من قاعدة البيانات
cp mas.db mas.db.backup

# نسخة احتياطية من الكود
git commit -am "Backup before enhancements"
```

### الخطوة 2: تحديث الحزم

أضف هذه الحزم إلى `mas.csproj`:

```xml
<!-- JWT Authentication -->
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.11" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.2.1" />

<!-- Logging -->
<PackageReference Include="Serilog.AspNetCore" Version="8.0.3" />
<PackageReference Include="Serilog.Sinks.Console" Version="6.0.0" />
<PackageReference Include="Serilog.Sinks.File" Version="6.0.0" />
<PackageReference Include="Serilog.Enrichers.Environment" Version="3.1.0" />
<PackageReference Include="Serilog.Enrichers.Thread" Version="4.0.0" />

<!-- Caching -->
<PackageReference Include="Microsoft.Extensions.Caching.StackExchangeRedis" Version="8.0.11" />

<!-- Rate Limiting -->
<PackageReference Include="AspNetCoreRateLimit" Version="5.0.0" />

<!-- AutoMapper -->
<PackageReference Include="AutoMapper" Version="13.0.1" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />

<!-- Swagger -->
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.9.0" />

<!-- FluentValidation -->
<PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />

<!-- Image Processing -->
<PackageReference Include="SixLabors.ImageSharp.Web" Version="3.1.5" />
```

ثم:
```bash
dotnet restore
```

### الخطوة 3: إضافة Base Models

1. أنشئ مجلد `Models/Base/`
2. أضف الملفات:
   - `BaseEntity.cs`
   - `ISoftDelete.cs`
   - `IAuditable.cs`

(انظر المشروع المحسّن للكود الكامل)

### الخطوة 4: إضافة Models الجديدة

أضف النماذج التالية في `Models/`:
- `Review.cs`
- `Order.cs`
- `OrderItem.cs`
- `Cart.cs`
- `CartItem.cs`
- `Coupon.cs`
- `Notification.cs`

### الخطوة 5: إنشاء Repositories

1. أنشئ مجلد `Repositories/`
2. أضف الملفات:
   - `IRepository.cs`
   - `Repository.cs`
   - `IUnitOfWork.cs`
   - `UnitOfWork.cs`

### الخطوة 6: إنشاء Services

1. أنشئ مجلد `Services/`
2. أضف الملفات:
   - `ICacheService.cs`
   - `CacheService.cs`
   - `IProductService.cs`
   - `ProductService.cs`
   - `IOrderService.cs`
   - `OrderService.cs`

### الخطوة 7: إنشاء DTOs

1. أنشئ مجلد `DTOs/`
2. أضف الملفات:
   - `ProductDto.cs`
   - `OrderDto.cs`
   - `ReviewDto.cs`
   - `CategoryDto.cs`

### الخطوة 8: إضافة AutoMapper

1. أنشئ مجلد `Mappings/`
2. أضف `MappingProfile.cs`

### الخطوة 9: تحديث ApplicationDbContext

```csharp
// أضف DbSets الجديدة
public DbSet<Review> Reviews { get; set; }
public DbSet<Order> Orders { get; set; }
public DbSet<OrderItem> OrderItems { get; set; }
public DbSet<Cart> Carts { get; set; }
public DbSet<CartItem> CartItems { get; set; }
public DbSet<Coupon> Coupons { get; set; }
public DbSet<Notification> Notifications { get; set; }

// أضف SaveChangesAsync Override
public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
{
    // Auto-update audit fields
    foreach (var entry in ChangeTracker.Entries<BaseEntity>())
    {
        if (entry.State == EntityState.Added)
            entry.Entity.CreatedAt = DateTime.UtcNow;
        else if (entry.State == EntityState.Modified)
            entry.Entity.UpdatedAt = DateTime.UtcNow;
    }
    return base.SaveChangesAsync(cancellationToken);
}

// أضف Query Filters في OnModelCreating
protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);
    
    builder.Entity<Review>().HasQueryFilter(e => !e.IsDeleted);
    builder.Entity<Order>().HasQueryFilter(e => !e.IsDeleted);
    builder.Entity<Coupon>().HasQueryFilter(e => !e.IsDeleted);
    
    // ... باقي التكوينات
}
```

### الخطوة 10: تحديث Program.cs

استبدل `Program.cs` بالنسخة المحسّنة التي تحتوي على:
- Serilog Configuration
- JWT Authentication
- Rate Limiting
- Caching
- AutoMapper
- Repositories & Services Registration
- Swagger

### الخطوة 11: تحديث appsettings.json

أضف التكوينات الجديدة:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=mas.db",
    "Redis": "localhost:6379"
  },
  "JwtSettings": {
    "SecretKey": "YourVerySecureSecretKeyMinimum32CharactersLong!",
    "Issuer": "MAS",
    "Audience": "MAS-Users",
    "ExpirationMinutes": 60
  },
  "IpRateLimiting": {
    "EnableEndpointRateLimiting": true,
    "GeneralRules": [
      {
        "Endpoint": "*",
        "Period": "1m",
        "Limit": 60
      }
    ]
  },
  "Caching": {
    "UseRedis": false,
    "DefaultExpirationMinutes": 30
  }
}
```

### الخطوة 12: إضافة صفحة Contact

1. أنشئ `Components/Pages/Contact.razor`
2. انسخ محتوى صفحة Contact من المشروع المحسّن

### الخطوة 13: إضافة Dark Mode

1. أنشئ `wwwroot/css/dark-mode.css`
2. أنشئ `wwwroot/js/dark-mode.js`
3. أضف reference في `App.razor` أو `MainLayout.razor`

### الخطوة 14: إنشاء Migration

```bash
# إنشاء Migration للتغييرات الجديدة
dotnet ef migrations add AddEnhancements --project mas

# معاينة SQL
dotnet ef migrations script --project mas

# تطبيق Migration
dotnet ef database update --project mas
```

### الخطوة 15: اختبار

```bash
# تشغيل المشروع
dotnet run --project mas

# فتح المتصفح
# https://localhost:5001
# https://localhost:5001/swagger
```

---

## ⚠️ ملاحظات مهمة

### قاعدة البيانات

#### إذا كانت لديك بيانات موجودة:
```bash
# لا تحذف قاعدة البيانات!
# فقط أضف Migration وطبّقه
dotnet ef migrations add AddEnhancements --project mas
dotnet ef database update --project mas
```

#### إذا كنت تبدأ من الصفر:
```bash
# يمكنك حذف القاعدة وإعادة إنشائها
dotnet ef database drop --project mas
dotnet ef migrations add InitialCreate --project mas
dotnet ef database update --project mas
```

### Breaking Changes

#### Models
- إذا كنت تستخدم Models مباشرة في Views/Controllers:
  - استخدم DTOs بدلاً منها
  - أو أضف using `using mas.Models;`

#### Repositories
- إذا كنت تستخدم DbContext مباشرة:
  - غيّر إلى استخدام `IUnitOfWork`
  - مثال: `_unitOfWork.Products.GetAllAsync()`

#### Caching
- إذا كنت تستخدم Memory Cache يدوياً:
  - استخدم `ICacheService` بدلاً منه

---

## 🔍 استكشاف الأخطاء

### خطأ: "Package not found"
```bash
dotnet restore --force
dotnet clean
dotnet build
```

### خطأ: "Migration failed"
```bash
# تأكد من وجود using statements صحيحة
# تأكد من تحديث ApplicationDbContext

# حاول:
dotnet ef migrations remove --project mas
dotnet ef migrations add InitialCreate --project mas
dotnet ef database update --project mas
```

### خطأ: "Cannot resolve service"
```bash
# تأكد من تسجيل Services في Program.cs:
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IProductService, ProductService>();
```

### خطأ: "JWT Configuration missing"
```bash
# تأكد من إضافة JwtSettings في appsettings.json
# تأكد من أن SecretKey طويل بما يكفي (32+ حرف)
```

---

## ✅ Checklist

قبل التشغيل، تأكد من:

- [ ] جميع الحزم مثبتة (`dotnet restore`)
- [ ] `appsettings.json` محدّث بالتكوينات الجديدة
- [ ] Base Models موجودة
- [ ] Repositories موجودة
- [ ] Services موجودة
- [ ] DTOs موجودة
- [ ] AutoMapper Profile موجود
- [ ] `ApplicationDbContext` محدّث
- [ ] `Program.cs` محدّث
- [ ] Migration تم إنشاؤه وتطبيقه
- [ ] المشروع يُبنى بنجاح (`dotnet build`)

---

## 🎯 التحقق من النجاح

بعد تطبيق التحسينات، يجب أن ترى:

1. ✅ المشروع يعمل بدون أخطاء
2. ✅ Swagger متاح على `/swagger`
3. ✅ Logs تُكتب في مجلد `logs/`
4. ✅ صفحة Contact تعمل على `/contact`
5. ✅ Dark Mode يعمل (زر في الأسفل)
6. ✅ Admin Panel يعمل
7. ✅ قاعدة البيانات تحتوي على الجداول الجديدة

---

## 📚 مراجع إضافية

- `IMPLEMENTATION.md` - شرح تفصيلي للتحسينات
- `README.md` - دليل المشروع
- `ENHANCEMENTS_SUMMARY.md` - ملخص التحسينات

---

## 🆘 الدعم

إذا واجهت مشاكل:
1. راجع `logs/` للأخطاء
2. تحقق من `IMPLEMENTATION.md`
3. تواصل معنا: info@almass.com

---

**حظاً موفقاً في الترحيل!** 🚀
