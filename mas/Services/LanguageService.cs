namespace mas.Services;

public class LanguageService
{
    private string _currentLanguage = "ar"; // Default is Arabic
    public event Action? OnLanguageChanged;

    public string CurrentLanguage => _currentLanguage;
    public bool IsArabic => _currentLanguage == "ar";
    public bool IsEnglish => _currentLanguage == "en";
    public string Direction => IsArabic ? "rtl" : "ltr";

    public void SetLanguage(string language)
    {
        if (_currentLanguage != language)
        {
            _currentLanguage = language;
            OnLanguageChanged?.Invoke();
        }
    }

    public void ToggleLanguage()
    {
        SetLanguage(IsArabic ? "en" : "ar");
    }

    public string Get(string key)
    {
        return Translations.Get(key, _currentLanguage);
    }
}

public static class Translations
{
    private static readonly Dictionary<string, Dictionary<string, string>> _translations = new()
    {
        // Navigation
        ["nav_home"] = new() { ["ar"] = "الرئيسية", ["en"] = "Home" },
        ["nav_services"] = new() { ["ar"] = "الخدمات", ["en"] = "Services" },
        ["nav_about"] = new() { ["ar"] = "من نحن", ["en"] = "About Us" },
        ["nav_contact"] = new() { ["ar"] = "اتصل بنا", ["en"] = "Contact Us" },
        ["nav_admin"] = new() { ["ar"] = "لوحة التحكم", ["en"] = "Admin Panel" },
        ["contact_us"] = new() { ["ar"] = "تواصل معنا", ["en"] = "Contact Us" },

        // About Page
        ["about_title"] = new() { ["ar"] = "من نحن", ["en"] = "About Us" },
        ["about_desc"] = new() { ["ar"] = "نقدم أفضل الخدمات للطلاب بجودة عالية", ["en"] = "We provide the best services for students with high quality" },
        ["vision"] = new() { ["ar"] = "رؤيتنا", ["en"] = "Our Vision" },
        ["vision_desc"] = new() { ["ar"] = "أن نكون المنصة الأولى لخدمات الطلاب في العالم العربي", ["en"] = "To be the leading platform for student services in the Arab world" },
        ["mission"] = new() { ["ar"] = "رسالتنا", ["en"] = "Our Mission" },
        ["mission_desc"] = new() { ["ar"] = "تقديم خدمات أكاديمية متميزة", ["en"] = "Providing distinguished academic services" },
        ["why_choose_us"] = new() { ["ar"] = "لماذا تختارنا", ["en"] = "Why Choose Us" },
        ["why_choose_desc"] = new() { ["ar"] = "نقدم لك أفضل الخدمات بأعلى جودة وأفضل الأسعار", ["en"] = "We offer you the best services with the highest quality and best prices" },
        ["high_quality"] = new() { ["ar"] = "جودة عالية", ["en"] = "High Quality" },
        ["high_quality_desc"] = new() { ["ar"] = "نضمن لك أعلى جودة في جميع خدماتنا", ["en"] = "We guarantee you the highest quality in all our services" },
        ["on_time"] = new() { ["ar"] = "التسليم في الوقت", ["en"] = "On-Time Delivery" },
        ["on_time_desc"] = new() { ["ar"] = "نلتزم بمواعيد التسليم المحددة", ["en"] = "We commit to scheduled delivery times" },
        ["expert_team"] = new() { ["ar"] = "فريق متخصص", ["en"] = "Expert Team" },
        ["expert_team_desc"] = new() { ["ar"] = "فريق من أفضل المتخصصين", ["en"] = "A team of the best specialists" },
        ["affordable"] = new() { ["ar"] = "أسعار مناسبة", ["en"] = "Affordable Prices" },
        ["affordable_desc"] = new() { ["ar"] = "أفضل الأسعار في السوق", ["en"] = "Best prices in the market" },
        ["happy_clients"] = new() { ["ar"] = "عميل سعيد", ["en"] = "Happy Clients" },
        ["completed_projects"] = new() { ["ar"] = "مشروع منجز", ["en"] = "Completed Projects" },
        ["rating"] = new() { ["ar"] = "التقييم", ["en"] = "Rating" },
        ["years_exp"] = new() { ["ar"] = "سنوات خبرة", ["en"] = "Years Experience" },
        ["have_question"] = new() { ["ar"] = "هل لديك استفسار؟", ["en"] = "Have a Question?" },
        ["here_to_help"] = new() { ["ar"] = "نحن هنا لمساعدتك في أي وقت", ["en"] = "We are here to help you anytime" },
        ["whatsapp_contact"] = new() { ["ar"] = "تواصل عبر واتساب", ["en"] = "Contact via WhatsApp" },
        ["email_us"] = new() { ["ar"] = "راسلنا", ["en"] = "Email Us" },

        // Services Page
        ["services_title"] = new() { ["ar"] = "جميع الخدمات", ["en"] = "All Services" },
        ["services_desc"] = new() { ["ar"] = "اكتشف خدماتنا المميزة واختر ما يناسبك", ["en"] = "Discover our services and choose what suits you" },
        ["search"] = new() { ["ar"] = "البحث", ["en"] = "Search" },
        ["search_placeholder"] = new() { ["ar"] = "ابحث عن خدمة...", ["en"] = "Search for a service..." },
        ["category"] = new() { ["ar"] = "التصنيف", ["en"] = "Category" },
        ["all_categories"] = new() { ["ar"] = "جميع التصنيفات", ["en"] = "All Categories" },
        ["sort_by"] = new() { ["ar"] = "الترتيب", ["en"] = "Sort By" },
        ["newest"] = new() { ["ar"] = "الأحدث", ["en"] = "Newest" },
        ["price_low"] = new() { ["ar"] = "السعر: الأقل", ["en"] = "Price: Low to High" },
        ["price_high"] = new() { ["ar"] = "السعر: الأعلى", ["en"] = "Price: High to Low" },
        ["loading"] = new() { ["ar"] = "جاري التحميل...", ["en"] = "Loading..." },
        ["loading_services"] = new() { ["ar"] = "جاري تحضير الخدمات", ["en"] = "Preparing services" },
        ["no_services"] = new() { ["ar"] = "لا توجد خدمات حالياً", ["en"] = "No services available" },
        ["details"] = new() { ["ar"] = "التفاصيل", ["en"] = "Details" },
        ["discount"] = new() { ["ar"] = "خصم", ["en"] = "Discount" },
        ["sar"] = new() { ["ar"] = "ر.س", ["en"] = "SAR" },

        // Contact Page
        ["contact_title"] = new() { ["ar"] = "تواصل معنا", ["en"] = "Contact Us" },
        ["contact_desc"] = new() { ["ar"] = "نحن هنا للإجابة على استفساراتكم ومساعدتكم", ["en"] = "We are here to answer your questions and help you" },
        ["full_name"] = new() { ["ar"] = "الاسم الكامل", ["en"] = "Full Name" },
        ["email"] = new() { ["ar"] = "البريد الإلكتروني", ["en"] = "Email" },
        ["phone"] = new() { ["ar"] = "رقم الهاتف", ["en"] = "Phone Number" },
        ["subject"] = new() { ["ar"] = "الموضوع", ["en"] = "Subject" },
        ["message"] = new() { ["ar"] = "الرسالة", ["en"] = "Message" },
        ["send"] = new() { ["ar"] = "إرسال الرسالة", ["en"] = "Send Message" },
        ["sending"] = new() { ["ar"] = "جارٍ الإرسال...", ["en"] = "Sending..." },
        ["message_sent"] = new() { ["ar"] = "تم إرسال رسالتك بنجاح! سنتواصل معك قريباً.", ["en"] = "Your message has been sent successfully! We will contact you soon." },
        ["address"] = new() { ["ar"] = "العنوان", ["en"] = "Address" },
        ["saudi_arabia"] = new() { ["ar"] = "المملكة العربية السعودية", ["en"] = "Saudi Arabia" },

        // Common
        ["copyright"] = new() { ["ar"] = "جميع الحقوق محفوظة", ["en"] = "All Rights Reserved" },
        ["quick_links"] = new() { ["ar"] = "روابط سريعة", ["en"] = "Quick Links" },
    };

    public static string Get(string key, string language)
    {
        if (_translations.TryGetValue(key, out var translations))
        {
            if (translations.TryGetValue(language, out var value))
            {
                return value;
            }
        }
        return key; // Return key if translation not found
    }
}
