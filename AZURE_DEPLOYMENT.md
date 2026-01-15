# 🚀 دليل نشر التطبيق على Azure مجاناً

## 📋 المتطلبات الأولية

### 1️⃣ إنشاء حساب Azure مجاني
1. اذهب إلى: https://azure.microsoft.com/free/
2. اضغط **Start free**
3. سجل بحساب Microsoft الخاص بك
4. ستحصل على:
   - 💰 **$200 رصيد مجاني** لمدة 30 يوم
   - 🎁 **12 شهر مجاني** لخدمات محددة
   - ✅ **خدمات مجانية دائماً**

### 2️⃣ تثبيت Azure CLI
افتح PowerShell كمسؤول وشغل:
```powershell
winget install Microsoft.AzureCLI
```

## 🔧 خطوات النشر

### الخطوة 1: تسجيل الدخول لـ Azure
```powershell
az login
```
سيفتح المتصفح - سجل دخولك

### الخطوة 2: إنشاء Resource Group
```powershell
az group create --name mas-rg --location "UAE North"
```

### الخطوة 3: إنشاء App Service Plan (المجاني F1)
```powershell
az appservice plan create `
  --name mas-plan `
  --resource-group mas-rg `
  --sku F1 `
  --is-linux
```

### الخطوة 4: إنشاء Web App
```powershell
az webapp create `
  --name mas-almass-app `
  --resource-group mas-rg `
  --plan mas-plan `
  --runtime "DOTNET|8.0"
```

### الخطوة 5: نشر التطبيق من VS Code

#### طريقة 1: استخدام Azure Extension (الأسهل)
1. ثبت extension: **Azure App Service** في VS Code
2. اضغط Ctrl+Shift+P واكتب: `Azure: Sign In`
3. بعد تسجيل الدخول، اضغط Ctrl+Shift+P واكتب: `Azure App Service: Deploy to Web App`
4. اختر مجلد `mas` (المشروع الرئيسي)
5. اختر الـ Web App الذي أنشأته

#### طريقة 2: استخدام Azure CLI
```powershell
cd C:\halah\mas\mas
dotnet publish -c Release -o ./publish
Compress-Archive -Path ./publish/* -DestinationPath ./publish.zip -Force
az webapp deployment source config-zip `
  --resource-group mas-rg `
  --name mas-almass-app `
  --src ./publish.zip
```

### الخطوة 6: تكوين الإعدادات
```powershell
# تعيين متغيرات البيئة
az webapp config appsettings set `
  --resource-group mas-rg `
  --name mas-almass-app `
  --settings `
    ASPNETCORE_ENVIRONMENT="Production" `
    WEBSITE_RUN_FROM_PACKAGE="1"
```

### الخطوة 7: فتح التطبيق
```powershell
az webapp browse --resource-group mas-rg --name mas-almass-app
```

أو اذهب إلى: `https://mas-almass-app.azurewebsites.net`

## 🗄️ قاعدة البيانات

### الخيار 1: رفع SQLite مع التطبيق (للتجربة)
- قاعدة البيانات `mas.db` سترفع مع التطبيق تلقائياً
- ⚠️ **تحذير**: البيانات ستُفقد عند إعادة النشر

### الخيار 2: Azure SQL Database (للإنتاج)
```powershell
# إنشاء SQL Server
az sql server create `
  --name mas-sql-server `
  --resource-group mas-rg `
  --location "UAE North" `
  --admin-user masadmin `
  --admin-password "YourStrongPassword123!"

# إنشاء Database
az sql db create `
  --resource-group mas-rg `
  --server mas-sql-server `
  --name mas-db `
  --service-objective Basic
```

ثم حدّث Connection String في Azure:
```powershell
az webapp config connection-string set `
  --resource-group mas-rg `
  --name mas-almass-app `
  --settings DefaultConnection="Server=tcp:mas-sql-server.database.windows.net,1433;Database=mas-db;User ID=masadmin;Password=YourStrongPassword123!;Encrypt=True;" `
  --connection-string-type SQLAzure
```

## 🔐 تكوين Admin Access

### إنشاء Admin User بعد النشر
1. افتح Azure Portal: https://portal.azure.com
2. اذهب إلى Web App > Console
3. شغل:
```bash
cd /home/site/wwwroot
dotnet mas.dll
```
أو استخدم SSH من VS Code

## 📊 مراقبة التطبيق

### عرض Logs
```powershell
az webapp log tail --resource-group mas-rg --name mas-almass-app
```

### فتح Azure Portal
```powershell
az webapp show --resource-group mas-rg --name mas-almass-app --query "defaultHostName" -o tsv
```

## 💰 التكاليف

### F1 (مجاني)
- ✅ **مجاني 100%**
- 💾 1GB Storage
- 🔄 60 دقيقة CPU يومياً
- 📈 1GB RAM

### B1 (أساسي) - $13/شهر
- 💾 10GB Storage
- 🔄 CPU غير محدود
- 📈 1.75GB RAM
- ⚡ أداء أفضل

### الترقية لـ B1
```powershell
az appservice plan update `
  --name mas-plan `
  --resource-group mas-rg `
  --sku B1
```

## 🌐 Domain مخصص (اختياري)

### ربط Domain
```powershell
az webapp config hostname add `
  --webapp-name mas-almass-app `
  --resource-group mas-rg `
  --hostname www.yourwebsite.com
```

### تفعيل HTTPS مجاني
```powershell
az webapp config ssl bind `
  --certificate-thumbprint <thumbprint> `
  --ssl-type SNI `
  --name mas-almass-app `
  --resource-group mas-rg
```

## 🔄 تحديث التطبيق

بعد أي تعديل في الكود:
```powershell
cd C:\halah\mas\mas
dotnet publish -c Release -o ./publish
Compress-Archive -Path ./publish/* -DestinationPath ./publish.zip -Force
az webapp deployment source config-zip `
  --resource-group mas-rg `
  --name mas-almass-app `
  --src ./publish.zip
```

## ⚠️ استكشاف الأخطاء

### إذا لم يعمل التطبيق:
```powershell
# تحقق من الـ logs
az webapp log tail --resource-group mas-rg --name mas-almass-app

# إعادة تشغيل التطبيق
az webapp restart --resource-group mas-rg --name mas-almass-app
```

### إذا ظهرت أخطاء قاعدة البيانات:
- تحقق من مسار `mas.db` في `appsettings.Production.json`
- تأكد من تشغيل Migrations

## 📞 الدعم

- Azure Portal: https://portal.azure.com
- Azure Docs: https://docs.microsoft.com/azure
- VS Code Azure Extension: https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azureappservice

---

## 🎯 الخطوات السريعة (ملخص)

1. `az login`
2. `az group create --name mas-rg --location "UAE North"`
3. `az appservice plan create --name mas-plan --resource-group mas-rg --sku F1`
4. `az webapp create --name mas-almass-app --resource-group mas-rg --plan mas-plan --runtime "DOTNET|8.0"`
5. استخدم VS Code Azure Extension للنشر
6. افتح: `https://mas-almass-app.azurewebsites.net`

✅ **جاهز للنشر!**
