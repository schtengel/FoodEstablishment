using FoodEstablishment.Api.DTOs;
using FoodEstablishment.Api.Entities;
using FoodEstablishment.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodEstablishment.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductCompositionsController(IProductCompositionRepository compositionRepository, 
    IProductRepository productRepository, IIngredientRepository ingredientRepository) : ControllerBase
{
    private readonly IProductCompositionRepository _compositionRepository = compositionRepository;
    private readonly IIngredientRepository _ingredientRepository = ingredientRepository;
    private readonly IProductRepository _productRepository = productRepository;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductCompositionResponse>))]
    public async Task<IActionResult> GetByProductId([FromQuery] int productId)
    {
        var compositions = await _compositionRepository.GetByProductIdAsync(productId);

        var response = compositions.Select(c => new ProductCompositionResponse
        {
            IngredientId = c.IngredientId,
            ProductId = c.ProductId,
            Quantity = c.Quantity
        });
        
        return Ok(response);
    }

    [HttpGet("{productId}/{ingredientId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductCompositionResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int productId, int ingredientId)
    {
        var composition = await _compositionRepository.GetAsync(productId, ingredientId);
        if (composition == null) return NotFound();

        return Ok(new ProductCompositionResponse
        {
            IngredientId = composition.IngredientId,
            ProductId = composition.ProductId,
            Quantity = composition.Quantity
        });
    }

    [HttpPost]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductCompositionResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] ProductCompositionCreateRequest request)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
            return NotFound($"Продукт с Id = {request.ProductId} не найден.");
        
        var ingredient = await _ingredientRepository.GetByIdAsync(request.IngredientId);
        if (ingredient == null)
            return NotFound($"Ингредиент с Id = {request.IngredientId} не найден.");
        
        var existing = await _compositionRepository.GetAsync(request.ProductId, request.IngredientId);
        if (existing != null)
            return Conflict($"Ингредиент с Id = {request.IngredientId} " +
                            $"уже присутствует в составе продукта {request.ProductId}");
        
        var composition = new ProductComposition
        {
            ProductId = request.ProductId,
            IngredientId = request.IngredientId,
            Quantity = request.Quantity
        };
        
        await _compositionRepository.AddAsync(composition);

        var response = new ProductCompositionResponse
        {
            ProductId = composition.ProductId,
            IngredientId = composition.IngredientId,
            Quantity = composition.Quantity
        };

        return CreatedAtAction(nameof(GetById), new { productId = response.ProductId, 
            ingredientId = response.IngredientId }, response);
    }

    [HttpPut("{productId}/{ingredientId}")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int productId, int ingredientId, [FromBody] decimal quantity)
    {
        var composition = await _compositionRepository.GetAsync(productId, ingredientId);
        if (composition == null) return NotFound();

        if (quantity <= 0)
            return BadRequest("Количество ингредиента должно быть больше нуля.");

        composition.Quantity = quantity;
        await _compositionRepository.UpdateAsync(composition);

        return NoContent();
    }

    [HttpDelete("{productId}/{ingredientId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int productId, int ingredientId)
    {
        var composition = await _compositionRepository.GetAsync(productId, ingredientId);
        if (composition == null) return NotFound();

        await _compositionRepository.DeleteAsync(composition);
        return NoContent();
    }
}