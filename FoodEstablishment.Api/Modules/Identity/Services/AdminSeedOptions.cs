namespace FoodEstablishment.Api.Modules.Identity.Services;

public class AdminSeedOptions
{
    public const string SectionName = "AdminSeed";

    public string? Username { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }

    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(Username) &&
        !string.IsNullOrWhiteSpace(PhoneNumber) &&
        !string.IsNullOrWhiteSpace(Password);
}