# 🚂 نشر التطبيق على Railway - مجاناً

## ✅ المميزات
- 💰 **500 ساعة مجانية شهرياً** (يكفي لتطبيق صغير)
- 🚀 **نشر تلقائي من GitHub**
- 🔒 **SSL مجاني**
- 📊 **لوحة تحكم سهلة**
- 🗄️ **قواعد بيانات مجانية**

---

## 📋 خطوات النشر (5 دقائق)

### 1️⃣ إنشاء حساب Railway
1. اذهب إلى: https://railway.app
2. اضغط **Login with GitHub**
3. وافق على الصلاحيات

### 2️⃣ رفع الكود على GitHub (إذا لم يكن موجوداً)

افتح terminal وشغل:

```powershell
cd C:\halah\mas

# تهيئة Git
git init
git add .
git commit -m "Initial commit for Railway deployment"

# إنشاء repository على GitHub من VS Code
# اضغط Ctrl+Shift+P واكتب: Git: Publish to GitHub
# أو استخدم GitHub Desktop
```

### 3️⃣ نشر على Railway

#### طريقة 1: من واجهة Railway (الأسهل)
1. افتح: https://railway.app/dashboard
2. اضغط **New Project**
3. اختر **Deploy from GitHub repo**
4. اختر repository الخاص بمشروع `mas`
5. Railway سيكتشف Dockerfile تلقائياً
6. اضغط **Deploy**

#### طريقة 2: استخدام Railway CLI

```powershell
# تثبيت Railway CLI
npm i -g @railway/cli

# تسجيل الدخول
railway login

# ربط المشروع
cd C:\halah\mas
railway init

# النشر
railway up
```

### 4️⃣ إعداد متغيرات البيئة

في لوحة Railway، اذهب إلى **Variables** وأضف:

```
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=Data Source=/app/mas.db
```

### 5️⃣ الحصول على الرابط

بعد النشر، ستحصل على رابط مثل:
```
https://mas-almass-production.up.railway.app
```

---

## 🗄️ إضافة قاعدة بيانات PostgreSQL (اختياري)

إذا أردت استخدام PostgreSQL بدلاً من SQLite:

1. في Railway، اضغ **New Service**
2. اختر **PostgreSQL**
3. انسخ Connection String
4. حدّث `appsettings.Production.json`

---

## 🔄 التحديثات التلقائية

Railway يراقب GitHub تلقائياً. كل push جديد = نشر تلقائي!

```powershell
git add .
git commit -m "تحديث التطبيق"
git push
# Railway سينشر تلقائياً
```

---

## 📊 مراقبة التطبيق

- **Logs**: Railway Dashboard → Deployments → View Logs
- **Metrics**: Railway Dashboard → Metrics
- **استخدام الساعات**: Railway Dashboard → Usage

---

## 💰 حدود الخطة المجانية

- ⏰ **500 ساعة/شهر** (حوالي 21 يوم تشغيل متواصل)
- 💾 **1GB Storage**
- 🌐 **100GB Bandwidth/شهر**

### لزيادة الساعات:
- أضف بطاقة ائتمان: تحصل على **$5/شهر رصيد مجاني**
- أو اشترك في **Hobby Plan**: $5/شهر = ساعات غير محدودة

---

## ⚠️ استكشاف الأخطاء

### إذا فشل البناء:
```bash
# تحقق من الـ Dockerfile محلياً
docker build -t mas-test .
docker run -p 8080:8080 mas-test
```

### إذا لم يعمل التطبيق:
- تحقق من Logs في Railway Dashboard
- تأكد من PORT = 8080 في Dockerfile
- تأكد من وجود mas.db في المشروع

---

## 🎯 ملخص الأوامر السريعة

```powershell
# 1. رفع على GitHub
git init
git add .
git commit -m "Ready for Railway"
# ثم استخدم VS Code لرفع على GitHub

# 2. نشر على Railway
npm i -g @railway/cli
railway login
railway init
railway up

# 3. فتح التطبيق
railway open
```

✅ **جاهز للنشر المجاني!**
