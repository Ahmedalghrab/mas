using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace mas.Migrations
{
    /// <inheritdoc />
    public partial class AddSiteSettingsAndTestimonialsAndFAQs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FAQs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuestionAr = table.Column<string>(type: "TEXT", nullable: false),
                    QuestionEn = table.Column<string>(type: "TEXT", nullable: false),
                    AnswerAr = table.Column<string>(type: "TEXT", nullable: false),
                    AnswerEn = table.Column<string>(type: "TEXT", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SiteName = table.Column<string>(type: "TEXT", nullable: false),
                    SiteNameEn = table.Column<string>(type: "TEXT", nullable: false),
                    LogoPath = table.Column<string>(type: "TEXT", nullable: true),
                    FaviconPath = table.Column<string>(type: "TEXT", nullable: true),
                    WhatsAppNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    FacebookUrl = table.Column<string>(type: "TEXT", nullable: true),
                    TwitterUrl = table.Column<string>(type: "TEXT", nullable: true),
                    InstagramUrl = table.Column<string>(type: "TEXT", nullable: true),
                    LinkedInUrl = table.Column<string>(type: "TEXT", nullable: true),
                    AboutAr = table.Column<string>(type: "TEXT", nullable: true),
                    AboutEn = table.Column<string>(type: "TEXT", nullable: true),
                    VisionAr = table.Column<string>(type: "TEXT", nullable: true),
                    VisionEn = table.Column<string>(type: "TEXT", nullable: true),
                    MissionAr = table.Column<string>(type: "TEXT", nullable: true),
                    MissionEn = table.Column<string>(type: "TEXT", nullable: true),
                    MetaDescriptionAr = table.Column<string>(type: "TEXT", nullable: true),
                    MetaDescriptionEn = table.Column<string>(type: "TEXT", nullable: true),
                    MetaKeywords = table.Column<string>(type: "TEXT", nullable: true),
                    EnableWhatsAppButton = table.Column<bool>(type: "INTEGER", nullable: false),
                    EnableDarkMode = table.Column<bool>(type: "INTEGER", nullable: false),
                    EnableTestimonials = table.Column<bool>(type: "INTEGER", nullable: false),
                    MaintenanceMode = table.Column<bool>(type: "INTEGER", nullable: false),
                    PrimaryColor = table.Column<string>(type: "TEXT", nullable: true),
                    SecondaryColor = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Testimonials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerName = table.Column<string>(type: "TEXT", nullable: false),
                    CustomerTitle = table.Column<string>(type: "TEXT", nullable: true),
                    CustomerImage = table.Column<string>(type: "TEXT", nullable: true),
                    TestimonialText = table.Column<string>(type: "TEXT", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testimonials", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 5, 10, 51, 0, 852, DateTimeKind.Utc).AddTicks(1182));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 5, 10, 51, 0, 852, DateTimeKind.Utc).AddTicks(1184));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 5, 10, 51, 0, 852, DateTimeKind.Utc).AddTicks(1185));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 5, 10, 51, 0, 852, DateTimeKind.Utc).AddTicks(1186));

            migrationBuilder.InsertData(
                table: "FAQs",
                columns: new[] { "Id", "AnswerAr", "AnswerEn", "CreatedAt", "DisplayOrder", "IsActive", "QuestionAr", "QuestionEn" },
                values: new object[,]
                {
                    { 1, "????? ??? ?? ???? ?????? ??? ?? '???? ??? ????????' ?? ???? ?????? ??????.", "You can order any service by clicking the 'Order via WhatsApp' button.", new DateTime(2025, 11, 5, 10, 51, 0, 852, DateTimeKind.Utc).AddTicks(1303), 1, true, "??? ?????? ??? ?????", "How can I order a service?" },
                    { 2, "????? ??? ??????? ??? ??? ??????. ???? ????? ?? ???? ?????? ??????.", "Delivery time varies by service type. Check the service details page.", new DateTime(2025, 11, 5, 10, 51, 0, 852, DateTimeKind.Utc).AddTicks(1305), 2, true, "?? ?? ??? ????????", "What is the delivery time?" },
                    { 3, "???? ???? ???? ????? ????? ??????? ?? ???? ?? ?????????.", "Yes, we guarantee high quality with professional specialists.", new DateTime(2025, 11, 5, 10, 51, 0, 852, DateTimeKind.Utc).AddTicks(1306), 3, true, "?? ?????? ???? ??????", "Do you guarantee work quality?" }
                });

            migrationBuilder.InsertData(
                table: "SiteSettings",
                columns: new[] { "Id", "AboutAr", "AboutEn", "Address", "Email", "EnableDarkMode", "EnableTestimonials", "EnableWhatsAppButton", "FacebookUrl", "FaviconPath", "InstagramUrl", "LinkedInUrl", "LogoPath", "MaintenanceMode", "MetaDescriptionAr", "MetaDescriptionEn", "MetaKeywords", "MissionAr", "MissionEn", "PhoneNumber", "PrimaryColor", "SecondaryColor", "SiteName", "SiteNameEn", "TwitterUrl", "UpdatedAt", "VisionAr", "VisionEn", "WhatsAppNumber" },
                values: new object[] { 1, "ALMASS ?? ???? ????? ?????? ????? ?????? ?????????? ???????? ????? ???? ????? ???????.", "ALMASS is a leading platform for providing academic and technical student services.", "??????? ??????? ????????", "info@almass.com", true, true, true, null, null, null, null, null, false, null, null, null, "????? ????? ???????? ?????? ????? ?????? ??? ?????? ???????", "Providing distinguished academic services that help students excel and succeed", null, "#0d6efd", "#6c757d", "ALMASS", "ALMASS", null, new DateTime(2025, 11, 5, 10, 51, 0, 852, DateTimeKind.Utc).AddTicks(1271), "?? ???? ?????? ?????? ??????? ??? ?????? ?????? ?? ?????? ??????", "To be the first and most trusted platform for student services in the Arab world", "+966500000000" });

            migrationBuilder.InsertData(
                table: "Testimonials",
                columns: new[] { "Id", "CreatedAt", "CustomerImage", "CustomerName", "CustomerTitle", "DisplayOrder", "IsActive", "Rating", "TestimonialText" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 5, 10, 51, 0, 852, DateTimeKind.Utc).AddTicks(1286), null, "???? ????", "???? ?????", 1, true, 5, "???? ?????? ??????! ??????? ?? ????? ???? ????????? ????? ?????." },
                    { 2, new DateTime(2025, 11, 5, 10, 51, 0, 852, DateTimeKind.Utc).AddTicks(1287), null, "????? ?????", "????? ????? ?????", 2, true, 5, "???? ????? ????? ???? ?????? ???????? ????." },
                    { 3, new DateTime(2025, 11, 5, 10, 51, 0, 852, DateTimeKind.Utc).AddTicks(1289), null, "???? ??????", "???? ???? ????", 3, true, 5, "????? ?????! ?????? ??????? ??? ??????." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FAQs");

            migrationBuilder.DropTable(
                name: "SiteSettings");

            migrationBuilder.DropTable(
                name: "Testimonials");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 5, 10, 41, 1, 228, DateTimeKind.Utc).AddTicks(2496));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 5, 10, 41, 1, 228, DateTimeKind.Utc).AddTicks(2498));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 5, 10, 41, 1, 228, DateTimeKind.Utc).AddTicks(2499));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 5, 10, 41, 1, 228, DateTimeKind.Utc).AddTicks(2500));
        }
    }
}
