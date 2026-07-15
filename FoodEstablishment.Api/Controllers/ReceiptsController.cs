using FoodEstablishment.Api.DTOs;
using FoodEstablishment.Api.Entities;
using FoodEstablishment.Api.Enums;
using FoodEstablishment.Api.Extensions;
using FoodEstablishment.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodEstablishment.Api.Controllers;

[ApiController]
[Route("api/v1")]
[Authorize]
public class ReceiptsController(
    IReceiptRepository receiptRepository,
    IOrderRepository orderRepository) : ControllerBase
{
    private readonly IReceiptRepository _receiptRepository = receiptRepository;
    private readonly IOrderRepository _orderRepository = orderRepository;

    [HttpPost("Orders/{orderId}/receipts")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReceiptResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateRetry(int orderId, [FromBody] PaymentMethodType paymentMethod)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null) return NotFound($"Заказ с Id = {orderId} не найден.");

        var isPrivileged = User.IsInRole("Manager") || User.IsInRole("Admin");
        if (!isPrivileged && order.UserId != User.GetUserId())
            return Forbid();

        var existingReceipts = await _receiptRepository.GetByOrderIdAsync(orderId);
        if (existingReceipts.Any(r => r.PaymentStatusId == (int)PaymentStatusType.Paid))
            return BadRequest("По этому заказу уже есть оплаченный чек, повторная попытка не требуется.");

        var receipt = new Receipt
        {
            OrderId = orderId,
            PaymentStatusId = (int)PaymentStatusType.Created,
            PaymentMethod = paymentMethod
        };

        await _receiptRepository.AddAsync(receipt);

        return CreatedAtAction(nameof(GetById), new { id = receipt.Id }, MapToResponse(receipt, order));
    }

    [HttpGet("Receipts/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReceiptResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var receipt = await _receiptRepository.GetByIdAsync(id);
        if (receipt == null) return NotFound();

        var isPrivileged = User.IsInRole("Manager") || User.IsInRole("Admin");
        if (!isPrivileged && receipt.Order.UserId != User.GetUserId())
            return Forbid();

        return Ok(MapToResponse(receipt, receipt.Order));
    }

    [HttpPatch("Receipts/{id}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] ReceiptStatusUpdateRequest request)
    {
        var receipt = await _receiptRepository.GetByIdAsync(id);
        if (receipt == null) return NotFound();

        receipt.PaymentStatusId = (int)request.PaymentStatus;

        if (request.PaymentStatus == PaymentStatusType.Paid)
            receipt.PaidAt = DateTime.UtcNow;

        await _receiptRepository.UpdateAsync(receipt);

        return NoContent();
    }

    private static ReceiptResponse MapToResponse(Receipt receipt, Order order) => new()
    {
        Id = receipt.Id,
        OrderId = receipt.OrderId,
        PaymentStatusId = receipt.PaymentStatusId,
        PaymentMethod = receipt.PaymentMethod.ToString(),
        TotalAmount = order.OrderCompositions.Sum(oc => oc.Quantity * oc.PriceAtOrderTime),
        PaidAt = receipt.PaidAt,
        CreatedAt = receipt.CreatedAt
    };
}