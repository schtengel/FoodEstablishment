namespace FoodEstablishment.Api.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? PasswordHash { get; set; } = string.Empty;
    public int BonusPoints { get; set; } = 0;
    
    public string? VerificationCode { get; set; }
    public DateTime? VerificationCodeExpiresAt { get; set; }
    
    public ICollection<Order> Orders { get; set; } = new List<Order>();

}