using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mas.Data;
using mas.Models;

namespace mas.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactContentController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ContactContentController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/ContactContent
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<ContactContent>> GetContactContent()
    {
        var content = await _context.ContactContents.FirstOrDefaultAsync();
        
        if (content == null)
        {
            // Create default content
            content = new ContactContent
            {
                PageTitleAr = "اتصل بنا",
                PageTitleEn = "Contact Us",
                PageSubtitleAr = "نسعد بتواصلك معنا",
                PageSubtitleEn = "We are happy to hear from you",
                PhoneNumber = "+966500000000",
                EmailAddress = "info@almasa.com",
                WhatsAppNumber = "966500000000",
                AddressAr = "المملكة العربية السعودية",
                AddressEn = "Saudi Arabia",
                FormTitleAr = "أرسل لنا رسالة",
                FormTitleEn = "Send us a message",
                SuccessMessageAr = "تم إرسال رسالتك بنجاح! سنتواصل معك قريباً",
                SuccessMessageEn = "Your message has been sent successfully! We will contact you soon"
            };
            _context.ContactContents.Add(content);
            await _context.SaveChangesAsync();
        }

        return content;
    }

    // PUT: api/ContactContent
    [Authorize(Policy = "AdminOnly")]
    [HttpPut]
    public async Task<IActionResult> PutContactContent(ContactContent content)
    {
        var existingContent = await _context.ContactContents.FirstOrDefaultAsync();

        if (existingContent == null)
        {
            content.UpdatedAt = DateTime.UtcNow;
            _context.ContactContents.Add(content);
        }
        else
        {
            content.Id = existingContent.Id;
            content.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingContent).CurrentValues.SetValues(content);
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }

        return NoContent();
    }
}
