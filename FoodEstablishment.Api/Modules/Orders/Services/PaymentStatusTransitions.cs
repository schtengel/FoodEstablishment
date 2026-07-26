using FoodEstablishment.Api.Modules.Orders.Enums;

namespace FoodEstablishment.Api.Modules.Orders.Services;

public static class PaymentStatusTransitions
{
    private static readonly Dictionary<PaymentStatusType, PaymentStatusType[]> AllowedTransitions = new()
    {
        [PaymentStatusType.Created] = [PaymentStatusType.InProgress, PaymentStatusType.Cancelled],
        [PaymentStatusType.InProgress] = [PaymentStatusType.InsufficientFunds, PaymentStatusType.Paid, PaymentStatusType.Cancelled],
        [PaymentStatusType.InsufficientFunds] = [PaymentStatusType.Cancelled],
        [PaymentStatusType.Cancelled] = [],
        [PaymentStatusType.Paid] = []
    };

    public static bool IsAllowed(PaymentStatusType from, PaymentStatusType to)
    {
        return AllowedTransitions.TryGetValue(from, out var allowed) && allowed.Contains(to);
    }
}