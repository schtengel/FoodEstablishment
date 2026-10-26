using System.Security.Claims;
using FluentAssertions;
using FoodEstablishment.Api.Modules.Identity.Repositories.Interfaces;
using FoodEstablishment.Api.Modules.Menu.Repositories.Interfaces;
using FoodEstablishment.Api.Modules.Orders.Controllers;
using FoodEstablishment.Api.Modules.Orders.DTOs.Requests;
using FoodEstablishment.Api.Modules.Orders.Entities;
using FoodEstablishment.Api.Modules.Orders.Enums;
using FoodEstablishment.Api.Modules.Orders.Repositories.Interfaces;
using FoodEstablishment.Api.Modules.Orders.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace FoodEstablishment.Tests.Orders;

public class ReceiptStatusTests
{
    private readonly IReceiptRepository _receipts = Substitute.For<IReceiptRepository>();
    private readonly IOrderRepository _orders = Substitute.For<IOrderRepository>();
    private readonly ReceiptsController _receiptsController;

    public ReceiptStatusTests()
    {
        _receiptsController = new ReceiptsController(_receipts, _orders);
    }

    [Fact]
    public async Task UpdateStatus_CreatedToPaid_IsRejected()
    {
        var receipt = Receipt(PaymentStatusType.Created, OrderStatusType.Created);
        _receipts.GetByIdAsync(1).Returns(receipt);

        var result = await _receiptsController.UpdateStatus(1, new ReceiptStatusUpdateRequest
        {
            PaymentStatus = PaymentStatusType.Paid,
        });

        result.Should().BeOfType<BadRequestObjectResult>();
        await _receipts.DidNotReceive().UpdateAsync(Arg.Any<Receipt>());
    }

    [Fact]
    public async Task UpdateStatus_CreatedToInProgress_UpdatesReceipt()
    {
        var receipt = Receipt(PaymentStatusType.Created, OrderStatusType.Created);
        _receipts.GetByIdAsync(1).Returns(receipt);

        var result = await _receiptsController.UpdateStatus(1, new ReceiptStatusUpdateRequest
        {
            PaymentStatus = PaymentStatusType.InProgress,
        });

        result.Should().BeOfType<NoContentResult>();
        receipt.PaymentStatusId.Should().Be((int)PaymentStatusType.InProgress);
        await _receipts.Received(1).UpdateAsync(receipt);
    }

    [Fact]
    public async Task UpdateStatus_InProgressToPaidOnCancelledOrder_IsRejected()
    {
        var receipt = Receipt(PaymentStatusType.InProgress, OrderStatusType.Cancelled);
        _receipts.GetByIdAsync(1).Returns(receipt);

        var result = await _receiptsController.UpdateStatus(1, new ReceiptStatusUpdateRequest
        {
            PaymentStatus = PaymentStatusType.Paid,
        });

        result.Should().BeOfType<BadRequestObjectResult>();
        receipt.PaidAt.Should().BeNull();
        await _receipts.DidNotReceive().UpdateAsync(Arg.Any<Receipt>());
    }

    [Fact]
    public async Task CreateRetry_CancelledOrder_IsRejected()
    {
        var order = new Order
        {
            Id = 10,
            UserId = 5,
            OrderStatusId = (int)OrderStatusType.Cancelled,
            OrderCompositions = new List<OrderComposition>(),
        };
        _orders.GetByIdAsync(10).Returns(order);
        _receiptsController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, "5"),
                    new Claim(ClaimTypes.Role, "Client"),
                ], "test")),
            },
        };

        var result = await _receiptsController.CreateRetry(10, PaymentMethodType.Card);

        result.Should().BeOfType<BadRequestObjectResult>();
        await _receipts.DidNotReceive().AddAsync(Arg.Any<Receipt>());
    }

    [Fact]
    public async Task Cancel_ClosesOpenReceiptAndKeepsPaidOne()
    {
        var controller = new OrdersController(
            _orders,
            Substitute.For<IUserRepository>(),
            Substitute.For<IProductRepository>());
        var order = new Order
        {
            Id = 4,
            OrderStatusId = (int)OrderStatusType.Created,
            Receipts =
            [
                new Receipt { Id = 1, PaymentStatusId = (int)PaymentStatusType.Created, PaymentMethod = PaymentMethodType.Cash },
                new Receipt { Id = 2, PaymentStatusId = (int)PaymentStatusType.Paid, PaymentMethod = PaymentMethodType.Card },
            ],
        };
        _orders.GetByIdAsync(4).Returns(order);

        var result = await controller.Cancel(4);

        result.Should().BeOfType<NoContentResult>();
        order.OrderStatusId.Should().Be((int)OrderStatusType.Cancelled);
        order.Receipts.Single(r => r.Id == 1).PaymentStatusId.Should().Be((int)PaymentStatusType.Cancelled);
        order.Receipts.Single(r => r.Id == 2).PaymentStatusId.Should().Be((int)PaymentStatusType.Paid);
    }

    [Fact]
    public void CancelOpenReceipts_DoesNotRefundPaidReceipt()
    {
        var receipts = new List<Receipt>
        {
            new() { PaymentStatusId = (int)PaymentStatusType.InProgress },
            new() { PaymentStatusId = (int)PaymentStatusType.Paid },
        };

        PaymentStatusTransitions.CancelOpenReceipts(receipts);

        receipts[0].PaymentStatusId.Should().Be((int)PaymentStatusType.Cancelled);
        receipts[1].PaymentStatusId.Should().Be((int)PaymentStatusType.Paid);
    }

    private static Receipt Receipt(PaymentStatusType payment, OrderStatusType order) => new()
    {
        Id = 1,
        OrderId = 10,
        PaymentStatusId = (int)payment,
        PaymentMethod = PaymentMethodType.Card,
        Order = new Order
        {
            Id = 10,
            OrderStatusId = (int)order,
            OrderCompositions = new List<OrderComposition>(),
        },
    };
}