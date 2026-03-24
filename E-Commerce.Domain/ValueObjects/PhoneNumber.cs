namespace E_Commerce.Domain.ValueObjects;

public record PhoneNumber(string CountryCode, string Number)
{
    public string FormattedNumber => $"{CountryCode}{Number}";
}