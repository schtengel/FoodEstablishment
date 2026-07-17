using FoodEstablishment.Api.DTOs;
using FoodEstablishment.Api.Entities;
using FoodEstablishment.Api.Enums;
using FoodEstablishment.Api.Extensions;
using FoodEstablishment.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodEstablishment.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class OrdersController(
    IOrderRepository orderRepository,
    IUserRepository userRepository,
    IProductRepository productRepository) : ControllerBase
{
    private readonly IOrderRepository _orderRepository =  orderRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IProductRepository _productRepository =  productRepository;

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null) return NotFound();

        var isPrivileged = User.IsInRole("Manager") || User.IsInRole("Admin");
        if (!isPrivileged && order.UserId != User.GetUserId())
            return NotFound();

        return Ok(MapToResponse(order));
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] OrderCreateRequest request)
    {
        var userId = User.GetUserId();
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return NotFound("Пользователь не найден.");

        if (!await _orderRepository.OrderSourceExistsAsync(request.OrderSourceId))
            return NotFound($"Источник заказа с Id = {request.OrderSourceId} не найден.");

        var compositions = new List<OrderComposition>();

        foreach (var item in request.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
                return NotFound($"Продукт с Id = {item.ProductId} не найден.");

            if (product.IsStopListed)
                return BadRequest($"Продукт \"{product.Name}\" сейчас недоступен для заказа (стоп-лист).");

            compositions.Add(new OrderComposition
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                PriceAtOrderTime = product.Price
            });
        }
        
        var order = new Order
        {
            UserId = userId,
            OrderSourceId = request.OrderSourceId,
            OrderStatusId = (int)OrderStatusType.Created,
            OrderCompositions = compositions,
            Receipts = new List<Receipt>
            {
                new Receipt
                {
                    PaymentStatusId = (int)PaymentStatusType.Created,
                    PaymentMethod = request.PaymentMethod
                }
            }
        };
        
        await _orderRepository.AddAsync(order);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, MapToResponse(order));
    }
    
    [HttpPatch("{id}/start-progress")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> StartProgress(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null) return NotFound();
    
        if (order.OrderStatusId != (int)OrderStatusType.Created)
            return BadRequest("В работу можно взять только заказ в статусе \"Создан\".");
    
        var isPaid = order.Receipts.Any(r => r.PaymentStatusId == (int)PaymentStatusType.Paid);
        if (!isPaid)
            return BadRequest("Заказ ещё не оплачен, взять в работу нельзя.");
    
        order.OrderStatusId = (int)OrderStatusType.InProgress;
        await _orderRepository.UpdateAsync(order);
    
        return NoContent();
    }
    
    [HttpPatch("{id}/mark-ready")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkReady(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null) return NotFound();
    
        if (order.OrderStatusId != (int)OrderStatusType.InProgress)
            return BadRequest("Готовым можно пометить только заказ в статусе \"В процессе\".");
    
        order.OrderStatusId = (int)OrderStatusType.Ready;
        await _orderRepository.UpdateAsync(order);
    
        return NoContent();
    }
    
    [HttpPatch("{id}/mark-given")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkGiven(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null) return NotFound();
    
        if (order.OrderStatusId != (int)OrderStatusType.Ready)
            return BadRequest("Выдать можно только заказ в статусе \"Готов\".");

        foreach (var orderComposition in order.OrderCompositions)
        {
            foreach (var productComposition in orderComposition.Product.ProductCompositions)
            {
                productComposition.Ingredient.StockQuantity -= productComposition.Quantity * orderComposition.Quantity;
            }
        }

        order.OrderStatusId = (int)OrderStatusType.Given;
        await _orderRepository.UpdateAsync(order);
    
        return NoContent();
    }

    [HttpPatch("{id}/cancel")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null) return NotFound();
    
        var cancellableStatuses = new[]
        {
            (int)OrderStatusType.Created,
            (int)OrderStatusType.InProgress,
            (int)OrderStatusType.Ready
        };
    
        if (!cancellableStatuses.Contains(order.OrderStatusId))
            return BadRequest("Этот заказ уже нельзя отменить (уже выдан или отменён).");
    
        order.OrderStatusId = (int)OrderStatusType.Cancelled;
        await _orderRepository.UpdateAsync(order);
    
        return NoContent();
    }

    private static OrderResponse MapToResponse(Order order) => new()
    {
        Id = order.Id,
        UserId = order.UserId,
        OrderSourceId = order.OrderSourceId,
        OrderStatusId = order.OrderStatusId,
        CreatedAt = order.CreatedAt,
        Items = order.OrderCompositions.Select(oc => new OrderCompositionResponse
        {
            ProductId = oc.ProductId,
            Quantity = oc.Quantity,
            PriceAtOrderTime = oc.PriceAtOrderTime
        }).ToList(),
        Receipts = order.Receipts.Select(r => new ReceiptResponse
        {
            Id = r.Id,
            OrderId = r.OrderId,
            PaymentStatusId = r.PaymentStatusId,
            PaymentMethod = r.PaymentMethod.ToString(),
            TotalAmount = order.OrderCompositions.Sum(oc => oc.Quantity * oc.PriceAtOrderTime),
            PaidAt = r.PaidAt,
            CreatedAt = r.CreatedAt
        }).ToList()
    };
}