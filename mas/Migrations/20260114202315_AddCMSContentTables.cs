using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace mas.Migrations
{
    /// <inheritdoc />
    public partial class AddCMSContentTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.CreateTable(
                name: "AboutContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    TitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    SubtitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    SubtitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    MainContentAr = table.Column<string>(type: "TEXT", nullable: true),
                    MainContentEn = table.Column<string>(type: "TEXT", nullable: true),
                    MainImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    VisionTitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    VisionTitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    VisionContentAr = table.Column<string>(type: "TEXT", nullable: true),
                    VisionContentEn = table.Column<string>(type: "TEXT", nullable: true),
                    VisionIcon = table.Column<string>(type: "TEXT", nullable: true),
                    MissionTitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    MissionTitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    MissionContentAr = table.Column<string>(type: "TEXT", nullable: true),
                    MissionContentEn = table.Column<string>(type: "TEXT", nullable: true),
                    MissionIcon = table.Column<string>(type: "TEXT", nullable: true),
                    ValuesTitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    ValuesTitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    Value1TitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    Value1TitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    Value1DescriptionAr = table.Column<string>(type: "TEXT", nullable: true),
                    Value1DescriptionEn = table.Column<string>(type: "TEXT", nullable: true),
                    Value1Icon = table.Column<string>(type: "TEXT", nullable: true),
                    Value2TitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    Value2TitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    Value2DescriptionAr = table.Column<string>(type: "TEXT", nullable: true),
                    Value2DescriptionEn = table.Column<string>(type: "TEXT", nullable: true),
                    Value2Icon = table.Column<string>(type: "TEXT", nullable: true),
                    Value3TitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    Value3TitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    Value3DescriptionAr = table.Column<string>(type: "TEXT", nullable: true),
                    Value3DescriptionEn = table.Column<string>(type: "TEXT", nullable: true),
                    Value3Icon = table.Column<string>(type: "TEXT", nullable: true),
                    TeamSectionTitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    TeamSectionTitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    TeamSectionDescriptionAr = table.Column<string>(type: "TEXT", nullable: true),
                    TeamSectionDescriptionEn = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutContents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PageTitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    PageTitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    PageSubtitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    PageSubtitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    AddressAr = table.Column<string>(type: "TEXT", nullable: true),
                    AddressEn = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    EmailAddress = table.Column<string>(type: "TEXT", nullable: true),
                    WhatsAppNumber = table.Column<string>(type: "TEXT", nullable: true),
                    WorkingHoursTitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    WorkingHoursTitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    WorkingHoursAr = table.Column<string>(type: "TEXT", nullable: true),
                    WorkingHoursEn = table.Column<string>(type: "TEXT", nullable: true),
                    MapEmbedUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Latitude = table.Column<decimal>(type: "TEXT", nullable: true),
                    Longitude = table.Column<decimal>(type: "TEXT", nullable: true),
                    FormTitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    FormTitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    FormDescriptionAr = table.Column<string>(type: "TEXT", nullable: true),
                    FormDescriptionEn = table.Column<string>(type: "TEXT", nullable: true),
                    SuccessMessageAr = table.Column<string>(type: "TEXT", nullable: true),
                    SuccessMessageEn = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactContents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HeroTitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    HeroTitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    HeroSubtitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    HeroSubtitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    HeroImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    HeroButtonTextAr = table.Column<string>(type: "TEXT", nullable: true),
                    HeroButtonTextEn = table.Column<string>(type: "TEXT", nullable: true),
                    HeroButtonLink = table.Column<string>(type: "TEXT", nullable: true),
                    FeaturesSectionTitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    FeaturesSectionTitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    Feature1TitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    Feature1TitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    Feature1DescriptionAr = table.Column<string>(type: "TEXT", nullable: true),
                    Feature1DescriptionEn = table.Column<string>(type: "TEXT", nullable: true),
                    Feature1Icon = table.Column<string>(type: "TEXT", nullable: true),
                    Feature2TitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    Feature2TitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    Feature2DescriptionAr = table.Column<string>(type: "TEXT", nullable: true),
                    Feature2DescriptionEn = table.Column<string>(type: "TEXT", nullable: true),
                    Feature2Icon = table.Column<string>(type: "TEXT", nullable: true),
                    Feature3TitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    Feature3TitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    Feature3DescriptionAr = table.Column<string>(type: "TEXT", nullable: true),
                    Feature3DescriptionEn = table.Column<string>(type: "TEXT", nullable: true),
                    Feature3Icon = table.Column<string>(type: "TEXT", nullable: true),
                    Feature4TitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    Feature4TitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    Feature4DescriptionAr = table.Column<string>(type: "TEXT", nullable: true),
                    Feature4DescriptionEn = table.Column<string>(type: "TEXT", nullable: true),
                    Feature4Icon = table.Column<string>(type: "TEXT", nullable: true),
                    ServicesSectionTitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    ServicesSectionTitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    ServicesSectionSubtitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    ServicesSectionSubtitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    AboutSectionTitleAr = table.Column<string>(type: "TEXT", nullable: true),
                    AboutSectionTitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    AboutSectionContentAr = table.Column<string>(type: "TEXT", nullable: true),
                    AboutSectionContentEn = table.Column<string>(type: "TEXT", nullable: true),
                    AboutImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeContents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TitleAr = table.Column<string>(type: "TEXT", nullable: false),
                    TitleEn = table.Column<string>(type: "TEXT", nullable: true),
                    Slug = table.Column<string>(type: "TEXT", nullable: false),
                    ContentAr = table.Column<string>(type: "TEXT", nullable: false),
                    ContentEn = table.Column<string>(type: "TEXT", nullable: true),
                    MetaDescriptionAr = table.Column<string>(type: "TEXT", nullable: true),
                    MetaDescriptionEn = table.Column<string>(type: "TEXT", nullable: true),
                    MetaKeywords = table.Column<string>(type: "TEXT", nullable: true),
                    FeaturedImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsPublished = table.Column<bool>(type: "INTEGER", nullable: false),
                    ShowInMenu = table.Column<bool>(type: "INTEGER", nullable: false),
                    IconClass = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutContents");

            migrationBuilder.DropTable(
                name: "ContactContents");

            migrationBuilder.DropTable(
                name: "HomeContents");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "DescriptionAr", "DescriptionEn", "IconClass", "IsActive", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 5, 10, 58, 42, 671, DateTimeKind.Utc).AddTicks(371), "????? ????? ??????? ????????? ??????????", null, "bi bi-file-earmark-text", true, "????? ????? ?????????", "Research & Reports Services" },
                    { 2, new DateTime(2025, 11, 5, 10, 58, 42, 671, DateTimeKind.Utc).AddTicks(373), "????? ??????? ????????? ??????? ?????????", null, "bi bi-palette", true, "??????? ?????????", "Graphic Design" },
                    { 3, new DateTime(2025, 11, 5, 10, 58, 42, 671, DateTimeKind.Utc).AddTicks(374), "????? ??????? ?????? ???????? ???????", null, "bi bi-code-slash", true, "??????? ????????", "Programming & Development" },
                    { 4, new DateTime(2025, 11, 5, 10, 58, 42, 671, DateTimeKind.Utc).AddTicks(375), "????? ??????? ?????????? ?????????", null, "bi bi-translate", true, "???????", "Translation" }
                });

            migrationBuilder.InsertData(
                table: "FAQs",
                columns: new[] { "Id", "AnswerAr", "AnswerEn", "CreatedAt", "DisplayOrder", "IsActive", "QuestionAr", "QuestionEn" },
                values: new object[,]
                {
                    { 1, "????? ??? ?? ???? ?????? ??? ?? '???? ??? ????????' ?? ???? ?????? ??????.", "You can order any service by clicking the 'Order via WhatsApp' button.", new DateTime(2025, 11, 5, 10, 58, 42, 671, DateTimeKind.Utc).AddTicks(532), 1, true, "??? ?????? ??? ?????", "How can I order a service?" },
                    { 2, "????? ??? ??????? ??? ??? ??????. ???? ????? ?? ???? ?????? ??????.", "Delivery time varies by service type. Check the service details page.", new DateTime(2025, 11, 5, 10, 58, 42, 671, DateTimeKind.Utc).AddTicks(534), 2, true, "?? ?? ??? ????????", "What is the delivery time?" },
                    { 3, "???? ???? ???? ????? ????? ??????? ?? ???? ?? ?????????.", "Yes, we guarantee high quality with professional specialists.", new DateTime(2025, 11, 5, 10, 58, 42, 671, DateTimeKind.Utc).AddTicks(535), 3, true, "?? ?????? ???? ??????", "Do you guarantee work quality?" }
                });

            migrationBuilder.InsertData(
                table: "SiteSettings",
                columns: new[] { "Id", "AboutAr", "AboutEn", "Address", "Email", "EnableDarkMode", "EnableTestimonials", "EnableWhatsAppButton", "FacebookUrl", "FaviconPath", "InstagramUrl", "LinkedInUrl", "LogoPath", "MaintenanceMode", "MetaDescriptionAr", "MetaDescriptionEn", "MetaKeywords", "MissionAr", "MissionEn", "PhoneNumber", "PrimaryColor", "SecondaryColor", "SiteName", "SiteNameEn", "TwitterUrl", "UpdatedAt", "VisionAr", "VisionEn", "WhatsAppNumber" },
                values: new object[] { 1, "ALMASS ?? ???? ????? ?????? ????? ?????? ?????????? ???????? ????? ???? ????? ???????.", "ALMASS is a leading platform for providing academic and technical student services.", "??????? ??????? ????????", "info@almass.com", true, true, true, null, null, null, null, null, false, null, null, null, "????? ????? ???????? ?????? ????? ?????? ??? ?????? ???????", "Providing distinguished academic services that help students excel and succeed", null, "#0d6efd", "#6c757d", "ALMASS", "ALMASS", null, new DateTime(2025, 11, 5, 10, 58, 42, 671, DateTimeKind.Utc).AddTicks(471), "?? ???? ?????? ?????? ??????? ??? ?????? ?????? ?? ?????? ??????", "To be the first and most trusted platform for student services in the Arab world", "+966500000000" });

            migrationBuilder.InsertData(
                table: "Testimonials",
                columns: new[] { "Id", "CreatedAt", "CustomerImage", "CustomerName", "CustomerTitle", "DisplayOrder", "IsActive", "Rating", "TestimonialText" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 5, 10, 58, 42, 671, DateTimeKind.Utc).AddTicks(490), null, "???? ????", "???? ?????", 1, true, 5, "???? ?????? ??????! ??????? ?? ????? ???? ????????? ????? ?????." },
                    { 2, new DateTime(2025, 11, 5, 10, 58, 42, 671, DateTimeKind.Utc).AddTicks(492), null, "????? ?????", "????? ????? ?????", 2, true, 5, "???? ????? ????? ???? ?????? ???????? ????." },
                    { 3, new DateTime(2025, 11, 5, 10, 58, 42, 671, DateTimeKind.Utc).AddTicks(493), null, "???? ??????", "???? ???? ????", 3, true, 5, "????? ?????! ?????? ??????? ??? ??????." }
                });
        }
    }
}
