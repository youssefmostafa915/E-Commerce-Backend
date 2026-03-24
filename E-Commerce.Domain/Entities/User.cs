using E_Commerce.Domain.ValueObjects;
using E_Commerce.Domain.Enums; // Required for UserRole enum

namespace E_Commerce.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public Email Email { get; set; } = default!; 
    public PhoneNumber? Phone { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public List<Address> SavedAddresses { get; set; } = new();
    
    // Logic: Changed from string to UserRole Enum
    public UserRole Role { get; set; } = UserRole.Customer; 

    // --- ADVANCED VERIFICATION LOGIC ---
    public bool IsEmailVerified { get; private set; } = false;
    public string? VerificationCode { get; private set; }
    public DateTime? VerificationExpiry { get; private set; }

    // This method resolves the CS1061 error in your Handler
    public void GenerateVerification(string code)
    {
        VerificationCode = code;
        VerificationExpiry = DateTime.UtcNow.AddMinutes(15);
    }

    public void ConfirmEmail(string code)
    {
        if (VerificationCode == code && VerificationExpiry > DateTime.UtcNow)
        {
            IsEmailVerified = true;
            VerificationCode = null;
            VerificationExpiry = null;
            UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            throw new InvalidOperationException("Invalid or expired verification code.");
        }
    }
}