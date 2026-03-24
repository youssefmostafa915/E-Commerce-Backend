using System.Net.Mail;

namespace E_Commerce.Application.Interfaces.Services;

/// <summary>
/// Service interface for sending emails.
/// Handles email delivery for verification codes and notifications.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an email verification code to the user.
    /// </summary>
    /// <param name="email">Recipient's email address.</param>
    /// <param name="code">Verification code to send.</param>
    /// <returns>True if email was sent successfully, false otherwise.</returns>
    Task<bool> SendVerificationEmailAsync(string email, string code);

    /// <summary>
    /// Sends a welcome email after successful registration.
    /// </summary>
    /// <param name="email">Recipient's email address.</param>
    /// <param name="userName">User's full name.</param>
    /// <returns>True if email was sent successfully, false otherwise.</returns>
    Task<bool> SendWelcomeEmailAsync(string email, string userName);
}