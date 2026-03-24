using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using E_Commerce.Application.Interfaces.Services;

namespace E_Commerce.Infrastructure.Services;

/// <summary>
/// Implementation of email service using SMTP.
/// Sends emails for user verification and notifications.
/// </summary>
public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes the email service with configuration.
    /// </summary>
    /// <param name="configuration">Application configuration containing SMTP settings.</param>
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Sends a verification email with the code to the user.
    /// Uses SMTP configuration from appsettings.json.
    /// </summary>
    /// <param name="email">Recipient's email address.</param>
    /// <param name="code">6-digit verification code.</param>
    /// <returns>True if email sent successfully, false otherwise.</returns>
    public async Task<bool> SendVerificationEmailAsync(string email, string code)
    {
        try
        {
            // Get SMTP settings from configuration
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            var smtpServer = smtpSettings["Server"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(smtpSettings["Port"] ?? "587");
            var smtpUsername = smtpSettings["Username"];
            var smtpPassword = smtpSettings["Password"];
            var fromEmail = smtpSettings["FromEmail"] ?? "noreply@ecommerce.com";
            var fromName = smtpSettings["FromName"] ?? "E-Commerce App";

            // Create the email message
            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = "Verify Your Email - E-Commerce",
                Body = GenerateVerificationEmailBody(code),
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            // Configure SMTP client
            using var smtpClient = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            // Send the email
            await smtpClient.SendMailAsync(mailMessage);
            return true;
        }
        catch (Exception)
        {
            // In production, log the exception
            // For now, return false to indicate failure
            return false;
        }
    }

    /// <summary>
    /// Sends a welcome email after successful registration.
    /// </summary>
    /// <param name="email">Recipient's email address.</param>
    /// <param name="userName">User's full name.</param>
    /// <returns>True if email sent successfully, false otherwise.</returns>
    public async Task<bool> SendWelcomeEmailAsync(string email, string userName)
    {
        try
        {
            // Get SMTP settings from configuration
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            var smtpServer = smtpSettings["Server"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(smtpSettings["Port"] ?? "587");
            var smtpUsername = smtpSettings["Username"];
            var smtpPassword = smtpSettings["Password"];
            var fromEmail = smtpSettings["FromEmail"] ?? "noreply@ecommerce.com";
            var fromName = smtpSettings["FromName"] ?? "E-Commerce App";

            // Create the welcome email message
            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = "Welcome to E-Commerce!",
                Body = GenerateWelcomeEmailBody(userName),
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            // Configure SMTP client
            using var smtpClient = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            // Send the email
            await smtpClient.SendMailAsync(mailMessage);
            return true;
        }
        catch (Exception)
        {
            // In production, log the exception
            return false;
        }
    }

    /// <summary>
    /// Generates the HTML body for verification email.
    /// </summary>
    /// <param name="code">Verification code to include in the email.</param>
    /// <returns>HTML formatted email body.</returns>
    private string GenerateVerificationEmailBody(string code)
    {
        return $@"
        <html>
        <body style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
            <div style='background-color: #f8f9fa; padding: 20px; text-align: center;'>
                <h1 style='color: #333;'>Verify Your Email</h1>
            </div>
            <div style='padding: 20px;'>
                <p>Hello!</p>
                <p>Thank you for registering with our E-Commerce platform. To complete your registration, please verify your email address using the code below:</p>
                
                <div style='background-color: #e9ecef; padding: 15px; text-align: center; margin: 20px 0; border-radius: 5px;'>
                    <h2 style='color: #495057; margin: 0; font-size: 24px; letter-spacing: 3px;'>{code}</h2>
                </div>
                
                <p>This code will expire in 15 minutes. If you didn't request this verification, please ignore this email.</p>
                
                <p>Best regards,<br>E-Commerce Team</p>
            </div>
        </body>
        </html>";
    }

    /// <summary>
    /// Generates the HTML body for welcome email.
    /// </summary>
    /// <param name="userName">User's full name.</param>
    /// <returns>HTML formatted welcome email body.</returns>
    private string GenerateWelcomeEmailBody(string userName)
    {
        return $@"
        <html>
        <body style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
            <div style='background-color: #f8f9fa; padding: 20px; text-align: center;'>
                <h1 style='color: #333;'>Welcome to E-Commerce!</h1>
            </div>
            <div style='padding: 20px;'>
                <p>Dear {userName},</p>
                <p>Welcome to our E-Commerce platform! Your account has been successfully created and verified.</p>
                
                <p>You can now:</p>
                <ul>
                    <li>Browse our product catalog</li>
                    <li>Add items to your cart</li>
                    <li>Place orders securely</li>
                    <li>Track your order history</li>
                </ul>
                
                <p>Start shopping today and enjoy a seamless online shopping experience!</p>
                
                <p>Best regards,<br>E-Commerce Team</p>
            </div>
        </body>
        </html>";
    }
}