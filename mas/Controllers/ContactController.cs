using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mas.Data;
using mas.Models;
using System.Net;
using System.Net.Mail;

namespace mas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ContactController> _logger;

  public ContactController(ApplicationDbContext context, IConfiguration configuration, ILogger<ContactController> logger)
    {
        _context = context;
      _configuration = configuration;
      _logger = logger;
    }

    // POST: api/Contact
    [HttpPost]
    public async Task<ActionResult<ContactMessage>> SubmitContact(ContactMessageDto dto)
    {
        var message = new ContactMessage
      {
  Name = dto.Name,
  Email = dto.Email,
            Phone = dto.Phone,
      Subject = dto.Subject,
            Message = dto.Message,
            CreatedAt = DateTime.UtcNow
        };

        _context.ContactMessages.Add(message);
        await _context.SaveChangesAsync();

     // Send email notification to admin (optional)
  try
        {
       await SendEmailNotification(message);
        }
        catch (Exception ex)
        {
    _logger.LogError(ex, "Failed to send email notification");
   }

        return Ok(new { success = true, message = "?? ????? ?????? ?????" });
    }

    // GET: api/Contact (Admin only)
    [Authorize(Policy = "AdminOnly")]
  [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactMessage>>> GetMessages([FromQuery] bool? isRead = null)
    {
    var query = _context.ContactMessages.AsQueryable();

        if (isRead.HasValue)
            query = query.Where(m => m.IsRead == isRead.Value);

 return await query.OrderByDescending(m => m.CreatedAt).ToListAsync();
    }

    // PUT: api/Contact/5/mark-read
    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id}/mark-read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var message = await _context.ContactMessages.FindAsync(id);
        if (message == null)
         return NotFound();

        message.IsRead = true;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Contact/5
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMessage(int id)
    {
  var message = await _context.ContactMessages.FindAsync(id);
        if (message == null)
            return NotFound();

        _context.ContactMessages.Remove(message);
        await _context.SaveChangesAsync();

    return NoContent();
    }

    private async Task SendEmailNotification(ContactMessage message)
    {
        // This is a basic implementation - you'll need to configure SMTP settings
 var smtpHost = _configuration["Email:SmtpHost"];
      var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
        var smtpUser = _configuration["Email:SmtpUser"];
    var smtpPass = _configuration["Email:SmtpPassword"];
        var adminEmail = _configuration["Email:AdminEmail"];

        if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(adminEmail))
            return;

        using var client = new SmtpClient(smtpHost, smtpPort)
        {
     Credentials = new NetworkCredential(smtpUser, smtpPass),
          EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
    From = new MailAddress(smtpUser ?? "noreply@almass.com"),
Subject = $"????? ????? ?? {message.Name}",
       Body = $@"
          <h3>????? ????? ?? ???? ALMASS</h3>
 <p><strong>?????:</strong> {message.Name}</p>
           <p><strong>??????:</strong> {message.Email}</p>
         <p><strong>??????:</strong> {message.Phone}</p>
       <p><strong>???????:</strong> {message.Subject}</p>
     <p><strong>???????:</strong></p>
        <p>{message.Message}</p>
            ",
            IsBodyHtml = true
        };

        mailMessage.To.Add(adminEmail);

        await client.SendMailAsync(mailMessage);
    }
}

public class ContactMessageDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
  public required string Subject { get; set; }
    public required string Message { get; set; }
}
