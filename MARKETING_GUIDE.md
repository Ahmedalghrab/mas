# ?? Contact Form & Marketing Features - ALMASS

## ?? New Features Added

### 1. ?? **Contact Page** (`/contact`)
Beautiful contact form where customers can send messages to you.

**Features:**
- ? Full contact form with validation
- ? Name, Email, Phone, Subject, Message fields
- ? Email notification to admin
- ? Success message after submission
- ? Contact information display (WhatsApp, Email, Address)
- ? Working hours section
- ? "Why Contact Us" features

**How it works:**
1. Customer fills the form at `/contact`
2. Message is saved to database
3. Email notification sent to admin (if configured)
4. Customer sees success message

### 2. ?? **Marketing Campaign System** (`/admin/marketing`)
Send marketing campaigns to your customers via Email and WhatsApp!

**Features:**
- ? **Send Campaigns** - Create and send marketing messages
- ? **Customer List** - Manage customer emails and WhatsApp numbers
- ? **Campaign History** - View all sent campaigns
- ? **Multiple Channels** - Email only, WhatsApp only, or Both
- ? **Bulk Sending** - Send to all subscribed customers at once

**Campaign Types:**
1. **Email Only** - Send via email to all customers
2. **WhatsApp Only** - Generate WhatsApp links (requires WhatsApp Business API)
3. **Both** - Send via email and prepare WhatsApp messages

---

## ?? **How to Use**

### For Customers - Contact Form

1. Visit: `https://localhost:5001/contact`
2. Fill in your information
3. Write your message
4. Click "????? ???????" (Send Message)
5. You'll see a success confirmation

### For Admin - Marketing Campaigns

1. Login to admin panel
2. Go to **???????** (Marketing) from dashboard
3. Click "????? ????" (Send Campaign) tab

**To Send a Campaign:**
```
1. Enter campaign title (e.g., "Special Ramadan Offer")
2. Select type:
   - ???? ???????? ??? (Email only)
   - ?????? ??? (WhatsApp only)
   - ?????? (Both)
3. Write your marketing message
4. Click "????? ?????? ????" (Send Campaign Now)
```

**To Manage Customers:**
1. Click "????? ???????" (Customer List) tab
2. View all customers with their contact info
3. Add/Remove customers as needed

**To View Campaign History:**
1. Click "??? ???????" (Campaign History) tab
2. See all sent campaigns with:
   - Campaign title
   - Message content
   - Type (Email/WhatsApp/Both)
   - Number of recipients
   - Send date and time

---

## ?? **Email Configuration**

To send emails, you need to configure SMTP settings in `appsettings.json`:

```json
{
  "Email": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": "587",
    "SmtpUser": "your-email@gmail.com",
"SmtpPassword": "your-app-password",
    "AdminEmail": "admin@almass.com"
  }
}
```

### Using Gmail:

1. **Enable 2-Factor Authentication** on your Gmail account
2. **Generate App Password:**
   - Go to: https://myaccount.google.com/apppasswords
   - Select "Mail" and your device
   - Copy the generated password
3. **Update appsettings.json:**
   ```json
   "SmtpUser": "youremail@gmail.com",
   "SmtpPassword": "your-16-digit-app-password",
   "AdminEmail": "admin@almass.com"
   ```

### Using Other Email Providers:

**Outlook/Hotmail:**
```json
"SmtpHost": "smtp-mail.outlook.com",
"SmtpPort": "587"
```

**Yahoo:**
```json
"SmtpHost": "smtp.mail.yahoo.com",
"SmtpPort": "587"
```

**Custom Domain:**
```json
"SmtpHost": "mail.yourdomain.com",
"SmtpPort": "587"
```

---

## ?? **WhatsApp Integration**

### Current Implementation (Free):
- Generates WhatsApp links for each customer
- Admin can click links to send messages individually
- Works with regular WhatsApp account

### Advanced Implementation (Requires WhatsApp Business API):
To enable **automatic bulk WhatsApp sending**, you need:

1. **WhatsApp Business API Account**
   - Sign up at: https://business.whatsapp.com/
   - Get API credentials

2. **Third-Party Service** (Recommended):
   - **Twilio**: https://www.twilio.com/whatsapp
   - **MessageBird**: https://www.messagebird.com/
   - **Vonage**: https://www.vonage.com/
   - **WATI**: https://www.wati.io/

3. **Update MarketingController.cs** with API integration

Example with Twilio:
```csharp
// Install: dotnet add package Twilio
using Twilio;
using Twilio.Rest.Api.V2010.Account;

// In SendWhatsAppMessage method:
TwilioClient.Init(accountSid, authToken);

var message = MessageResource.Create(
  from: new PhoneNumber("whatsapp:+14155238886"),
    to: new PhoneNumber($"whatsapp:{customer.WhatsAppNumber}"),
    body: messageContent
);
```

---

## ?? **Database Tables**

### ContactMessages Table
Stores all contact form submissions:
```
- Id
- Name
- Email
- Phone
- Subject
- Message
- IsRead (bool)
- CreatedAt
```

### Customers Table
Stores customer contact information:
```
- Id
- Name
- Email
- Phone
- WhatsAppNumber
- IsActive
- SubscribedToMarketing
- CreatedAt
```

### MarketingCampaigns Table
Stores sent marketing campaigns:
```
- Id
- Title
- MessageContent
- Type (Email/WhatsApp/Both)
- SentCount
- SentAt
- CreatedAt
- CreatedBy
```

---

## ?? **Marketing Tips**

### Writing Effective Campaigns:

1. **Catchy Title** ?
   - "??? ???: ??? 30% ????? ??????"
   - "????: ???? ??????? ????????"

2. **Clear Message** ??
   ```
   ?????? ????? ???????

   ???? ?? ????? ????? ?? ?????!
   
   ? ??? 30% ??? ???? ????? ???????
   ? ????? ???? ??? ????? ?????
   
 ?????: ???? ??? [????]
   
   ???? ALMASS
   ```

3. **Call to Action** ??
   - ??? ???? ?????? ?? ???? ??????
   - ?????? ????? ???????: "???? ????", "?? ???? ??????"

4. **Best Time to Send** ?
   - ???? ?????-?????? (9:00 ? - 11:00 ?)
   - ???? ?????-?????? (7:00 ? - 9:00 ?)
 - ???? ????? ?????? ??????

5. **Frequency** ??
   - ?? ???? ???? ?? ??? ?? ???????
   - ??????? ?????? ??? ??? ??????

---

## ?? **Privacy & Permissions**

### GDPR Compliance:
- ? Customers must subscribe to marketing (SubscribedToMarketing flag)
- ? Option to unsubscribe (add in future)
- ? Data stored securely in database
- ? Admin-only access to customer data

### Best Practices:
1. Only send to customers who subscribed
2. Include unsubscribe option in emails
3. Don't share customer data with third parties
4. Keep messages relevant and valuable

---

## ?? **Admin Pages**

### `/admin/marketing` - Marketing Dashboard
**Features:**
- Send Campaign tab
- Customer List tab
- Campaign History tab
- Real-time statistics

### `/admin/messages` - Contact Messages (To be created)
View all contact form submissions from customers.

---

## ?? **Customization**

### Change Working Hours:
Edit `mas/Components/Pages/Contact.razor`:
```razor
<div class="d-flex justify-content-between mb-2">
    <span class="fw-bold">????? - ??????</span>
    <span>9:00 ? - 9:00 ?</span>
</div>
```

### Change Contact Info:
All contact info is loaded from Site Settings (`/admin/settings`):
- WhatsApp Number
- Email
- Address

### Email Templates:
Edit email templates in:
- `ContactController.cs` - Line ~60 (Contact form notification)
- `MarketingController.cs` - Line ~150 (Marketing email template)

---

## ?? **Troubleshooting**

### Email not sending?
1. Check SMTP settings in `appsettings.json`
2. Verify Gmail App Password (not regular password)
3. Check server logs for errors
4. Test SMTP connection

### WhatsApp links not working?
1. Verify phone numbers include country code (+966)
2. Remove spaces and special characters
3. Check WhatsApp is installed on device

### Campaign not sending?
1. Verify customers have valid email/WhatsApp
2. Check customer `SubscribedToMarketing` is `true`
3. Check `IsActive` is `true`
4. Review server logs

---

## ?? **Future Enhancements**

### Short Term:
- [ ] Admin page to view contact messages
- [ ] Mark messages as read/unread
- [ ] Reply to contact messages
- [ ] Export customer list to CSV
- [ ] Email templates library

### Medium Term:
- [ ] Automated email campaigns (scheduled)
- [ ] Email open tracking
- [ ] Link click tracking
- [ ] Customer segmentation
- [ ] A/B testing for campaigns

### Long Term:
- [ ] Full WhatsApp Business API integration
- [ ] SMS campaigns
- [ ] Push notifications
- [ ] Marketing analytics dashboard
- [ ] Customer journey tracking

---

## ?? **Support**

Need help with:
- Email configuration
- WhatsApp Business API
- Custom integrations
- Marketing strategy

Contact: **admin@almass.com**

---

## ?? **Example Use Cases**

### 1. New Service Launch
```
Title: ????? ???? ?????: ????? ??????? ??????
Type: Both (Email + WhatsApp)
Message:
"?? ????? ???? ?????!

????? ??????? ?????? ??????????
? iOS & Android
? ????? ??????
? ????? ????

???? ????: https://wa.me/966500000000"
```

### 2. Special Discount
```
Title: ??? ???: ??? 30% ??? ???? ???????
Type: Email
Message:
"????? ???????

???? ?? ????? ????? ??????? ??? ?????!

?? ??? 30% ??? ???? ???????
? ????? ???? ???: 30/03/2024

?? ???? ??????! ???? ????"
```

### 3. Reminder Campaign
```
Title: ?????: ???? ???? ????!
Type: WhatsApp
Message:
"??????! ??

?????? ??? ???? ????????.

?? ?????? ??????? ?? ????
?????? ???? ??????? ??? ??????????.

?? ??? ??? ??????? ?? ???? ???"
```

---

**Made with ?? for ALMASS**

?? **Start marketing and grow your business today!** ??
