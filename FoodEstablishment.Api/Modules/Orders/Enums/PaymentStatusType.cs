namespace FoodEstablishment.Api.Modules.Orders.Enums;

public enum PaymentStatusType
{
    Created = 1,
    InProgress = 2,
    Paid = 3,
    InsufficientFunds = 4,
    Cancelled = 5
}