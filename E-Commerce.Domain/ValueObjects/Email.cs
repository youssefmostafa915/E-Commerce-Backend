namespace E_Commerce.Domain.ValueObjects;

public record Email
{
    public string Value { get; init; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty.");

        // Simple professional validation logic
        if (!value.Contains("@") || !value.Contains("."))
            throw new ArgumentException("Invalid email format.");

        Value = value.ToLower().Trim();
    }
}