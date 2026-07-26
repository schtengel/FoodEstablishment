using FoodEstablishment.Api.Common.Entities;
using FoodEstablishment.Api.Modules.Identity.Enums;
using FoodEstablishment.Api.Modules.Orders.Entities;

namespace FoodEstablishment.Api.Modules.Identity.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? PasswordHash { get; set; } = string.Empty;
    public int BonusPoints { get; set; } = 0;
    
    public string? VerificationCode { get; set; }
    public DateTime? VerificationCodeExpiresAt { get; set; }
    
    public string? DeviceId { get; set; }

    public UserRoleType Role { get; set; } = UserRoleType.Client;
    
    public int VerificationAttempts { get; set; } = 0;
    public DateTime? VerificationLockedUntil { get; set; }
    public DateTime? VerificationCodeSentAt { get; set; }
    
    public ICollection<Order> Orders { get; set; } = new List<Order>();

}