using FoodEstablishment.Api.Modules.Orders.Enums;
using FoodEstablishment.Api.Modules.Orders.Repositories.Interfaces;

namespace FoodEstablishment.Api.Modules.Orders.Services;

public class OrderAutoCancellationService(
    IServiceScopeFactory scopeFactory,
    ILogger<OrderAutoCancellationService> logger) : BackgroundService
{
    private static readonly TimeSpan UnpaidOrderTimeout = TimeSpan.FromMinutes(15);
    private static readonly TimeSpan CheckInterval = TimeSpan.FromMinutes(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CancelExpiredUnpaidOrderAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при автоотмене неоплаченных заказов.");
            }
            
            await Task.Delay(CheckInterval, stoppingToken);
        }
    }

    private async Task CancelExpiredUnpaidOrderAsync()
    {
        using var scope = scopeFactory.CreateScope();
        var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
        
        var expiredOrders = await orderRepository.GetUnpaidExpiredOrdersAsync(UnpaidOrderTimeout);

        foreach (var order in expiredOrders)
        {
            order.OrderStatusId = (int)OrderStatusType.Cancelled;
            await orderRepository.UpdateAsync(order);
            
            logger.LogInformation(
                "Заказ {OrderId} автоматически отменён: не оплачен в течение {Timeout} минут.",
                order.Id, UnpaidOrderTimeout.TotalMinutes);
        }
    }
}