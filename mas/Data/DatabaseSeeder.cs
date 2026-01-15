using Microsoft.EntityFrameworkCore;
using mas.Data;
using mas.Models;
using System.Text;

namespace mas.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedArabicDataAsync(ApplicationDbContext context)
        {
            // Set connection to use UTF-8
            await context.Database.ExecuteSqlRawAsync("PRAGMA encoding = 'UTF-8';");
            await context.Database.ExecuteSqlRawAsync("PRAGMA journal_mode = WAL;");
            
            // حذف البيانات القديمة
            var oldProducts = await context.Products.ToListAsync();
            var oldCategories = await context.Categories.ToListAsync();

            context.Products.RemoveRange(oldProducts);
            context.Categories.RemoveRange(oldCategories);
            await context.SaveChangesAsync();

            // إضافة التصنيفات باللغة العربية
            var categories = new List<Category>
            {
                new Category
                {
                    NameAr = "الأبحاث والتقارير",
                    NameEn = "Research and Reports",
                    DescriptionAr = "خدمات كتابة الأبحاث والتقارير الأكاديمية",
                    DescriptionEn = "Scientific research and academic reports services",
                    IconClass = "bi-journal-text",
                    IsActive = true
                },
                new Category
                {
                    NameAr = "التصميم والجرافيكس",
                    NameEn = "Design and Graphics",
                    DescriptionAr = "تصميم الشعارات والهوية البصرية",
                    DescriptionEn = "Logo and visual identity design",
                    IconClass = "bi-palette",
                    IsActive = true
                },
                new Category
                {
                    NameAr = "البرمجة والتطوير",
                    NameEn = "Programming and Development",
                    DescriptionAr = "تطوير المواقع والتطبيقات",
                    DescriptionEn = "Website and application development",
                    IconClass = "bi-code-slash",
                    IsActive = true
                },
                new Category
                {
                    NameAr = "الترجمة",
                    NameEn = "Translation",
                    DescriptionAr = "خدمات الترجمة الاحترافية",
                    DescriptionEn = "Professional translation services",
                    IconClass = "bi-translate",
                    IsActive = true
                }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            // إضافة المنتجات باللغة العربية
            var products = new List<Product>
            {
                new Product
                {
                    NameAr = "بحث علمي متكامل",
                    NameEn = "Complete Scientific Research",
                    DescriptionAr = "إعداد بحث علمي متكامل بجودة عالية مع المراجع والتوثيق الكامل",
                    DescriptionEn = "Prepare complete scientific research with high quality references and full documentation",
                    Price = 500,
                    DiscountPrice = 400,
                    CategoryId = categories[0].Id,
                    IsFeatured = true,
                    IsActive = true,
                    DeliveryTimeDays = 7,
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    NameAr = "تصميم شعار احترافي",
                    NameEn = "Professional Logo Design",
                    DescriptionAr = "تصميم شعار احترافي مميز لشركتك أو مشروعك مع ملفات مفتوحة",
                    DescriptionEn = "Design a professional distinctive logo for your company or project with open files",
                    Price = 300,
                    DiscountPrice = 250,
                    CategoryId = categories[1].Id,
                    IsFeatured = true,
                    IsActive = true,
                    DeliveryTimeDays = 3,
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    NameAr = "تطوير موقع إلكتروني",
                    NameEn = "Website Development",
                    DescriptionAr = "تطوير موقع إلكتروني متجاوب وسريع بتقنيات حديثة",
                    DescriptionEn = "Develop a responsive and fast website with modern technologies",
                    Price = 2000,
                    DiscountPrice = 1800,
                    CategoryId = categories[2].Id,
                    IsFeatured = true,
                    IsActive = true,
                    DeliveryTimeDays = 14,
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    NameAr = "ترجمة احترافية",
                    NameEn = "Professional Translation",
                    DescriptionAr = "ترجمة نصوص من وإلى العربية بدقة عالية",
                    DescriptionEn = "Translate texts to and from Arabic with high accuracy",
                    Price = 150,
                    CategoryId = categories[3].Id,
                    IsFeatured = true,
                    IsActive = true,
                    DeliveryTimeDays = 2,
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    NameAr = "كتابة محتوى تسويقي",
                    NameEn = "Marketing Content Writing",
                    DescriptionAr = "كتابة محتوى تسويقي جذاب لمواقع التواصل الاجتماعي",
                    DescriptionEn = "Write attractive marketing content for social media",
                    Price = 200,
                    CategoryId = categories[0].Id,
                    IsFeatured = false,
                    IsActive = true,
                    DeliveryTimeDays = 3,
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    NameAr = "تصميم بطاقة أعمال",
                    NameEn = "Business Card Design",
                    DescriptionAr = "تصميم بطاقة أعمال احترافية بتصاميم عصرية",
                    DescriptionEn = "Design a professional business card with modern designs",
                    Price = 100,
                    CategoryId = categories[1].Id,
                    IsFeatured = false,
                    IsActive = true,
                    DeliveryTimeDays = 1,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();

            // إضافة إعدادات الموقع
            var settings = await context.SiteSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                settings = new SiteSettings
                {
                    SiteName = "الماسة",
                    SiteNameEn = "Almasa",
                    Email = "info@almasa.com",
                    PhoneNumber = "+966500000000",
                    WhatsAppNumber = "966500000000",
                    Address = "المملكة العربية السعودية",
                    AboutAr = "نقدم أفضل الخدمات الأكاديمية والتقنية للطلاب بجودة عالية وأسعار مناسبة",
                    AboutEn = "We provide the best academic and technical services for students with high quality and reasonable prices",
                    VisionAr = "أن نكون المنصة الأولى في تقديم الخدمات الأكاديمية المتميزة",
                    VisionEn = "To be the first platform in providing distinguished academic services",
                    MissionAr = "تقديم خدمات عالية الجودة تساعد الطلاب على تحقيق أهدافهم الأكاديمية",
                    MissionEn = "Providing high quality services that help students achieve their academic goals",
                    EnableWhatsAppButton = true,
                    PrimaryColor = "#9B59B6",
                    SecondaryColor = "#8E44AD",
                    FacebookUrl = "https://facebook.com/almasa",
                    InstagramUrl = "https://instagram.com/almasa",
                    TwitterUrl = "https://twitter.com/almasa"
                };
                await context.SiteSettings.AddAsync(settings);
                await context.SaveChangesAsync();
            }

            // إضافة آراء العملاء
            var testimonials = await context.Testimonials.ToListAsync();
            context.Testimonials.RemoveRange(testimonials);
            await context.SaveChangesAsync();

            var newTestimonials = new List<Testimonial>
            {
                new Testimonial
                {
                    CustomerName = "أحمد محمد",
                    CustomerTitle = "طالب جامعي",
                    TestimonialText = "خدمة ممتازة وسريعة! تم تسليم البحث في الوقت المحدد وبجودة عالية",
                    Rating = 5,
                    IsActive = true,
                    DisplayOrder = 1
                },
                new Testimonial
                {
                    CustomerName = "فاطمة علي",
                    CustomerTitle = "صاحبة مشروع صغير",
                    TestimonialText = "تصميم الشعار والهوية البصرية كانا رائعين وبسعر مناسب",
                    Rating = 5,
                    IsActive = true,
                    DisplayOrder = 2
                },
                new Testimonial
                {
                    CustomerName = "خالد العبدالله",
                    CustomerTitle = "مدير شركة تقنية",
                    TestimonialText = "الموقع الذي تم تطويره لنا كان أكثر مما توقعنا، فريق محترف جداً",
                    Rating = 5,
                    IsActive = true,
                    DisplayOrder = 3
                }
            };

            await context.Testimonials.AddRangeAsync(newTestimonials);
            await context.SaveChangesAsync();

            // إضافة الأسئلة الشائعة
            var faqs = await context.FAQs.ToListAsync();
            context.FAQs.RemoveRange(faqs);
            await context.SaveChangesAsync();

            var newFaqs = new List<FAQ>
            {
                new FAQ
                {
                    QuestionAr = "كيف يمكنني طلب خدمة؟",
                    QuestionEn = "How can I order a service?",
                    AnswerAr = "يمكنك التواصل معنا عبر الواتساب أو نموذج التواصل وسنرد عليك في أقرب وقت",
                    AnswerEn = "You can contact us via WhatsApp or contact form and we will respond to you as soon as possible",
                    DisplayOrder = 1,
                    IsActive = true
                },
                new FAQ
                {
                    QuestionAr = "ما هي طرق الدفع المتاحة؟",
                    QuestionEn = "What payment methods are available?",
                    AnswerAr = "نقبل الدفع عن طريق التحويل البنكي أو الدفع الإلكتروني",
                    AnswerEn = "We accept payment via bank transfer or electronic payment",
                    DisplayOrder = 2,
                    IsActive = true
                },
                new FAQ
                {
                    QuestionAr = "كم تستغرق مدة التنفيذ؟",
                    QuestionEn = "How long does it take to complete the service?",
                    AnswerAr = "تختلف المدة حسب نوع الخدمة، عادة يتم ذكر المدة المتوقعة عند الطلب",
                    AnswerEn = "The time varies depending on the type of service, usually the expected duration is mentioned when ordering",
                    DisplayOrder = 3,
                    IsActive = true
                }
            };

            await context.FAQs.AddRangeAsync(newFaqs);
            await context.SaveChangesAsync();

            // إضافة صفحات افتراضية إذا لم تكن موجودة
            if (!await context.Pages.AnyAsync())
            {
                var defaultPages = new List<Page>
                {
                    new Page
                    {
                        TitleAr = "سياسة الخصوصية",
                        TitleEn = "Privacy Policy",
                        Slug = "privacy-policy",
                        ContentAr = @"<h2>سياسة الخصوصية</h2>
<p>نحن نحترم خصوصيتك ونلتزم بحماية معلوماتك الشخصية.</p>
<h3>جمع المعلومات</h3>
<p>نقوم بجمع المعلومات التي تقدمها لنا عند استخدام خدماتنا.</p>
<h3>استخدام المعلومات</h3>
<p>نستخدم معلوماتك لتقديم وتحسين خدماتنا.</p>",
                        ContentEn = @"<h2>Privacy Policy</h2>
<p>We respect your privacy and are committed to protecting your personal information.</p>",
                        MetaDescriptionAr = "سياسة الخصوصية لموقع الماسة",
                        MetaKeywords = "خصوصية, حماية البيانات, سياسة",
                        IsPublished = true,
                        ShowInMenu = true,
                        DisplayOrder = 1
                    },
                    new Page
                    {
                        TitleAr = "الشروط والأحكام",
                        TitleEn = "Terms and Conditions",
                        Slug = "terms-conditions",
                        ContentAr = @"<h2>الشروط والأحكام</h2>
<p>مرحباً بك في موقع الماسة للخدمات الطلابية.</p>
<h3>استخدام الموقع</h3>
<p>باستخدامك لهذا الموقع، فإنك توافق على هذه الشروط والأحكام.</p>",
                        ContentEn = @"<h2>Terms and Conditions</h2>
<p>Welcome to AlMasa Student Services.</p>",
                        MetaDescriptionAr = "الشروط والأحكام الخاصة بموقع الماسة",
                        MetaKeywords = "شروط, أحكام, اتفاقية",
                        IsPublished = true,
                        ShowInMenu = true,
                        DisplayOrder = 2
                    },
                    new Page
                    {
                        TitleAr = "من نحن",
                        TitleEn = "About Us",
                        Slug = "about",
                        ContentAr = @"<h2>من نحن</h2>
<p>الماسة هي منصة رائدة في تقديم الخدمات الطلابية والأكاديمية.</p>
<h3>رؤيتنا</h3>
<p>نسعى لتكون المنصة الأولى لخدمات الطلاب في المنطقة.</p>
<h3>قيمنا</h3>
<ul>
<li>الجودة والاحترافية</li>
<li>الالتزام بالمواعيد</li>
<li>خدمة عملاء ممتازة</li>
</ul>",
                        ContentEn = @"<h2>About Us</h2>
<p>AlMasa is a leading platform for student and academic services.</p>",
                        MetaDescriptionAr = "تعرف على الماسة ورؤيتنا وقيمنا",
                        MetaKeywords = "من نحن, عن الماسة, رؤيتنا",
                        IconClass = "bi-info-circle",
                        IsPublished = true,
                        ShowInMenu = true,
                        DisplayOrder = 3
                    }
                };

                await context.Pages.AddRangeAsync(defaultPages);
                await context.SaveChangesAsync();
            }

            // رسالة نجاح
            Console.WriteLine("✅ تم تحديث البيانات بالعربية بنجاح!");
        }
    }
}
