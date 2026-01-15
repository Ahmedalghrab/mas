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
[Authorize(Policy = "AdminOnly")]
public class MarketingController : ControllerBase
{
    private readonly ApplicationDbContext _context;
 private readonly IConfiguration _configuration;
 private readonly ILogger<MarketingController> _logger;

    public MarketingController(ApplicationDbContext context, IConfiguration configuration, ILogger<MarketingController> logger)
 {
  _context = context;
        _configuration = configuration;
    _logger = logger;
    }

    // GET: api/Marketing/customers
    [HttpGet("customers")]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
   return await _context.Customers
     .Where(c => c.IsActive && c.SubscribedToMarketing)
       .ToListAsync();
    }

  // POST: api/Marketing/customers
  [HttpPost("customers")]
    public async Task<ActionResult<Customer>> AddCustomer(CustomerDto dto)
    {
      var customer = new Customer
        {
 Name = dto.Name,
     Email = dto.Email,
       Phone = dto.Phone,
       WhatsAppNumber = dto.WhatsAppNumber,
      SubscribedToMarketing = dto.SubscribedToMarketing
     };

   _context.Customers.Add(customer);
     await _context.SaveChangesAsync();

  return CreatedAtAction(nameof(GetCustomers), new { id = customer.Id }, customer);
    }

 // DELETE: api/Marketing/customers/5
    [HttpDelete("customers/{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
     if (customer == null)
      return NotFound();

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();

     return NoContent();
    }

    // POST: api/Marketing/send
    [HttpPost("send")]
    public async Task<ActionResult> SendMarketingCampaign(MarketingCampaignDto dto)
  {
     var campaign = new MarketingCampaign
     {
       Title = dto.Title,
      MessageContent = dto.MessageContent,
  Type = dto.Type,
            CreatedAt = DateTime.UtcNow
      };

        var customers = await _context.Customers
     .Where(c => c.IsActive && c.SubscribedToMarketing)
         .ToListAsync();

        int sentCount = 0;
  var errors = new List<string>();

     // Send emails
     if (dto.Type == CampaignType.Email || dto.Type == CampaignType.Both)
        {
          foreach (var customer in customers.Where(c => !string.IsNullOrEmpty(c.Email)))
 {
     try
     {
 await SendEmail(customer.Email, dto.Title, dto.MessageContent);
     sentCount++;
     }
    catch (Exception ex)
          {
   _logger.LogError(ex, $"Failed to send email to {customer.Email}");
     errors.Add($"??? ????? ?????? ??? {customer.Name}");
   }
        }
        }

        // Send WhatsApp (Note: Requires WhatsApp Business API integration)
 if (dto.Type == CampaignType.WhatsApp || dto.Type == CampaignType.Both)
  {
     foreach (var customer in customers.Where(c => !string.IsNullOrEmpty(c.WhatsAppNumber)))
   {
    try
         {
          // TODO: Integrate with WhatsApp Business API
     // For now, we'll just generate a link
      var whatsappLink = GenerateWhatsAppLink(customer.WhatsAppNumber!, dto.MessageContent);
        _logger.LogInformation($"WhatsApp link for {customer.Name}: {whatsappLink}");
    sentCount++;
   }
       catch (Exception ex)
       {
         _logger.LogError(ex, $"Failed to prepare WhatsApp for {customer.WhatsAppNumber}");
    errors.Add($"??? ????? ?????? ?? {customer.Name}");
   }
          }
        }

   campaign.SentCount = sentCount;
 campaign.SentAt = DateTime.UtcNow;
        _context.MarketingCampaigns.Add(campaign);
    await _context.SaveChangesAsync();

        return Ok(new 
   {
   success = true,
            message = $"?? ????? ?????? ??? {sentCount} ????",
   sentCount,
 errors = errors.Any() ? errors : null
  });
    }

    // GET: api/Marketing/campaigns
    [HttpGet("campaigns")]
    public async Task<ActionResult<IEnumerable<MarketingCampaign>>> GetCampaigns()
    {
 return await _context.MarketingCampaigns
  .OrderByDescending(c => c.CreatedAt)
        .ToListAsync();
  }

    private async Task SendEmail(string email, string subject, string message)
  {
      var smtpHost = _configuration["Email:SmtpHost"];
        var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
      var smtpUser = _configuration["Email:SmtpUser"];
     var smtpPass = _configuration["Email:SmtpPassword"];

        if (string.IsNullOrEmpty(smtpHost))
  throw new InvalidOperationException("SMTP settings not configured");

    using var client = new SmtpClient(smtpHost, smtpPort)
        {
       Credentials = new NetworkCredential(smtpUser, smtpPass),
  EnableSsl = true
        };

    var mailMessage = new MailMessage
        {
   From = new MailAddress(smtpUser ?? "noreply@almass.com", "ALMASS"),
     Subject = subject,
       Body = $@"
    <div dir='rtl' style='font-family: Arial, sans-serif;'>
       <h2>{subject}</h2>
     <p>{message.Replace("\n", "<br/>")}</p>
           <hr/>
     <p style='color: #666;'>??? ????? ?? ???? ALMASS</p>
 </div>
          ",
  IsBodyHtml = true
    };

        mailMessage.To.Add(email);

    await client.SendMailAsync(mailMessage);
    }

    private string GenerateWhatsAppLink(string phoneNumber, string message)
    {
  var cleanNumber = phoneNumber.Replace("+", "").Replace(" ", "").Replace("-", "");
        var encodedMessage = Uri.EscapeDataString(message);
    return $"https://wa.me/{cleanNumber}?text={encodedMessage}";
    }
}

public class CustomerDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public string? WhatsAppNumber { get; set; }
    public bool SubscribedToMarketing { get; set; } = true;
}

public class MarketingCampaignDto
{
    public required string Title { get; set; }
    public required string MessageContent { get; set; }
    public CampaignType Type { get; set; }
}
