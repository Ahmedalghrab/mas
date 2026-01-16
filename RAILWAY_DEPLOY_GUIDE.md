# 🚀 دليل نشر MAS على Railway - من الصفر

## ✅ الإصلاحات المطبقة

تم تطبيق جميع الإصلاحات الحرجة:
- ✅ Dynamic port binding من Railway PORT environment variable
- ✅ تحسين logging لأخطاء connection string
- ✅ DatabaseSeeder متوافق مع PostgreSQL
- ✅ إعدادات Cloudinary جاهزة

---

## 📋 المتطلبات

### 1. حساب Railway
- سجل على: https://railway.app
- أضف بطاقة ائتمان للحصول على $5/شهر مجاناً (اختياري)

### 2. حساب Cloudinary (للصور)
- سجل على: https://cloudinary.com/users/register_free
- احصل على: Cloud Name, API Key, API Secret

### 3. GitHub Repository
- انشر الكود على GitHub (إذا لم يكن منشوراً)

---

## 🛠️ خطوات النشر

### الخطوة 1: تثبيت Railway CLI

```powershell
npm install -g @railway/cli
```

### الخطوة 2: تسجيل الدخول

```powershell
railway login
```

### الخطوة 3: إنشاء مشروع جديد

```powershell
cd C:\halah\mas
railway init
```

اختر:
- Create a new project
- اسم المشروع: `mas-platform` (أو أي اسم)

### الخطوة 4: إضافة PostgreSQL

في Railway Dashboard:
1. اضغط على `+ New`
2. اختر `Database` → `PostgreSQL`
3. انتظر حتى يتم إنشاء القاعدة

### الخطوة 5: نسخ DATABASE_URL

من PostgreSQL service في Dashboard:
1. اضغط على PostgreSQL service
2. اذهب إلى `Variables`
3. انسخ قيمة `DATABASE_URL`

### الخطوة 6: إضافة Environment Variables

في mas service (التطبيق الرئيسي):

```bash
# Database (انسخ من PostgreSQL service)
DATABASE_URL=postgresql://postgres:xxxxx@containers-us-west-xxx.railway.app:xxxx/railway

# Environment
ASPNETCORE_ENVIRONMENT=Production
RAILWAY_ENVIRONMENT=production

# JWT Settings (مهم جداً: غيّر SecretKey)
JwtSettings__SecretKey=YOUR-SECURE-SECRET-KEY-MINIMUM-32-CHARACTERS-LONG-2026
JwtSettings__Issuer=https://your-app-name.up.railway.app
JwtSettings__Audience=https://your-app-name.up.railway.app
JwtSettings__ExpirationMinutes=60

# Cloudinary (املأ من حسابك)
Cloudinary__CloudName=your_cloud_name
Cloudinary__ApiKey=your_api_key
Cloudinary__ApiSecret=your_api_secret
```

**ملاحظة مهمة**: غيّر `your-app-name` في JWT URLs إلى اسم التطبيق الفعلي من Railway.

### الخطوة 7: النشر

#### الطريقة الأولى: من GitHub (موصى بها)

1. في Railway Dashboard، اربط GitHub repo:
   - Settings → Connect GitHub Repository
   - اختر repository الخاص بك
2. Railway سينشر تلقائياً عند كل push

#### الطريقة الثانية: Railway CLI

```powershell
railway up
```

---

## 🔍 مراقبة النشر

### عرض Logs مباشرة

```powershell
railway logs
```

أو من Dashboard → Deployments → View Logs

### ما يجب أن تراه:

```
=== DATABASE CONNECTION DEBUG ===
RAILWAY_ENVIRONMENT: production
ASPNETCORE_ENVIRONMENT: Production
DATABASE_URL exists: True
✓ Connection String Found (length: 150)
✓ Using PostgreSQL: True
=== END DEBUG ===

Applying migration...
Seeding database...
Application started
```

### علامات النجاح ✅

- ✅ "Application started"
- ✅ "Database connection successful"
- ✅ لا يوجد أخطاء ArgumentException
- ✅ Migrations applied successfully

### علامات الخطأ ❌

- ❌ "FATAL ERROR: No database connection string found"
- ❌ "ArgumentException: Format of initialization string"
- ❌ "Railway rate limit reached" (تكرار أخطاء)

---

## 🧪 اختبار التطبيق

### 1. الوصول للموقع

افتح URL من Railway Dashboard:
```
https://your-app-name.up.railway.app
```

### 2. اختبار الصفحة الرئيسية

- ✅ الصفحة تفتح بدون أخطاء
- ✅ النصوص العربية تظهر صحيحة (ليس ???)
- ✅ الخدمات/المنتجات تظهر

### 3. اختبار لوحة الإدارة

1. اذهب إلى: `https://your-app.up.railway.app/Account/Login`
2. سجل دخول:
   - Email: `admin@mas.com`
   - Password: `Admin@123`
3. تحقق من:
   - ✅ تسجيل الدخول نجح
   - ✅ Dashboard يعمل
   - ✅ يمكن عرض المنتجات
   - ✅ رفع صورة جديدة يعمل (Cloudinary)

### 4. اختبار استمرارية البيانات

1. أضف منتج جديد
2. أعد deployment (push to GitHub أو `railway up`)
3. تحقق من:
   - ✅ المنتج الجديد لا يزال موجوداً
   - ✅ البيانات لم تُحذف

---

## 🐛 حل المشاكل الشائعة

### المشكلة 1: "No database connection string found"

**الحل:**
1. تأكد من إضافة `DATABASE_URL` في Environment Variables
2. تأكد من نسخ القيمة كاملة من PostgreSQL service
3. أعد deployment

### المشكلة 2: النصوص العربية تظهر ???

**الحل:**
- هذا لا يجب أن يحدث مع PostgreSQL
- تحقق من Logs: هل تم تشغيل Seeder بنجاح؟
- جرب إعادة deployment

### المشكلة 3: الصور لا ترفع

**الحل:**
1. تحقق من Cloudinary credentials في Environment Variables
2. تأكد من عدم وجود مسافات في القيم
3. اختبر credentials على موقع Cloudinary مباشرة

### المشكلة 4: "Cold Start" بطيء

**الحل:**
- في Environment Variables، غيّر:
  ```
  RAILWAY_MIN_INSTANCES=1
  ```
- سيبقي instance واحد دائماً نشط (تكلفة إضافية ~$5-10/شهر)

### المشكلة 5: خطأ في Migrations

**الحل:**
```powershell
# اتصل بقاعدة البيانات
railway run dotnet ef database update

# أو امسح القاعدة وابدأ من جديد
# (سيحذف جميع البيانات!)
railway run dotnet ef database drop -f
railway up
```

---

## 💰 التكلفة المتوقعة

### Free Tier (بدون بطاقة)
- ⏰ 500 ساعة/شهر (~21 يوم)
- 💾 1GB PostgreSQL
- 🌐 100GB bandwidth
- **التكلفة: $0**

### مع إضافة بطاقة
- 💳 $5/شهر رصيد مجاني
- ⏰ ساعات غير محدودة
- **التكلفة الفعلية: $0-5/شهر**

### Hobby Plan ($5/شهر)
- كل ما سبق +
- ⚡ Priority support
- 📊 Advanced metrics
- **التكلفة: $5/شهر**

### الاستخدام المتوقع للموقع:
- Traffic منخفض: **$0** (Free tier كافي)
- Traffic متوسط: **$5-10/شهر**
- Traffic عالي: **$20-40/شهر**

---

## 🔗 روابط مفيدة

- Railway Dashboard: https://railway.app/dashboard
- Railway Docs: https://docs.railway.app
- Cloudinary Dashboard: https://cloudinary.com/console
- GitHub Repository: [أضف رابط repo هنا]

---

## 📞 الدعم

### إذا واجهت مشاكل:

1. **تحقق من Logs:**
   ```powershell
   railway logs
   ```

2. **راجع Environment Variables:**
   - هل DATABASE_URL موجود؟
   - هل Cloudinary credentials صحيحة؟
   - هل JWT SecretKey تم تغييره؟

3. **اختبر محلياً مع PostgreSQL:**
   - غيّر connection string في appsettings.json
   - شغّل التطبيق محلياً
   - تأكد من عدم وجود أخطاء

4. **Railway Community:**
   - Discord: https://discord.gg/railway
   - Forum: https://help.railway.app

---

## ✅ Checklist النشر النهائي

قبل اعتبار النشر ناجح، تأكد من:

- [ ] التطبيق يعمل على Railway URL
- [ ] النصوص العربية تظهر صحيحة
- [ ] يمكن تسجيل دخول Admin
- [ ] Dashboard يعمل بالكامل
- [ ] رفع الصور يعمل (Cloudinary)
- [ ] البيانات تبقى بعد redeploy
- [ ] لا توجد أخطاء في Logs
- [ ] SSL يعمل (https://)
- [ ] الموقع سريع (< 2 ثانية)
- [ ] Cloudinary credentials في Environment Variables (ليس في appsettings)

---

**تم التحديث:** 16 يناير 2026
**الإصدار:** 2.0 - نشر من الصفر
