
using FoodEstablishment.Api.Modules.Inventory.DTOs.Requests;
using FoodEstablishment.Api.Modules.Inventory.DTOs.Responses;
using FoodEstablishment.Api.Modules.Inventory.Entities;
using FoodEstablishment.Api.Modules.Inventory.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodEstablishment.Api.Modules.Inventory.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class IngredientsController(IIngredientRepository ingredientRepository, 
    IStorageZoneRepository storageZoneRepository) : ControllerBase
{
    private readonly IIngredientRepository _ingredientRepository = ingredientRepository;
    private readonly IStorageZoneRepository _storageZoneRepository = storageZoneRepository;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<IngredientResponse>))]
    public async Task<IActionResult> GetAll()
    {
        var ingredients = await _ingredientRepository.GetAllAsync();

        var response = ingredients.Select(i => new IngredientResponse
        {
            Id = i.Id,
            Name = i.Name,
            StockQuantity = i.StockQuantity,
            StorageZoneId = i.StorageZoneId,
            Unit = i.Unit
        });
        
        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IngredientResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var ingredient = await _ingredientRepository.GetByIdAsync(id);
        if (ingredient == null) return NotFound();

        var response = new IngredientResponse
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            StockQuantity = ingredient.StockQuantity,
            StorageZoneId = ingredient.StorageZoneId,
            Unit = ingredient.Unit
        };

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IngredientResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] IngredientCreateRequest request)
    {
        var storageZone = await _storageZoneRepository.GetByIdAsync(request.StorageZoneId);
        if (storageZone == null) 
            return NotFound($"Зона хранения с Id = {request.StorageZoneId} не найдена.");

        var newIngredient = new Ingredient
        {
            Name = request.Name,
            StockQuantity = request.StockQuantity,
            StorageZoneId = request.StorageZoneId,
            Unit = request.Unit
        };

        await _ingredientRepository.AddAsync(newIngredient);

        var response = new IngredientResponse
        {
            Id = newIngredient.Id,
            Name = newIngredient.Name,
            StockQuantity = newIngredient.StockQuantity,
            StorageZoneId = newIngredient.StorageZoneId,
            Unit = newIngredient.Unit
        };
        
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] IngredientCreateRequest request)
    {
        var existingIngredient = await _ingredientRepository.GetByIdAsync(id);
        if (existingIngredient == null) return NotFound();
        
        var storageZone = await _storageZoneRepository.GetByIdAsync(request.StorageZoneId);
        if (storageZone == null) 
            return NotFound($"Зона хранения с Id = {request.StorageZoneId} не найдена.");
        
        existingIngredient.Name = request.Name;
        existingIngredient.StockQuantity = request.StockQuantity;
        existingIngredient.StorageZoneId = request.StorageZoneId;
        existingIngredient.Unit = request.Unit;
        
        await _ingredientRepository.UpdateAsync(existingIngredient);
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var existingIngredient = await _ingredientRepository.GetByIdAsync(id);
        if (existingIngredient == null) return NotFound();

        existingIngredient.IsDeleted = true;
        
        await _ingredientRepository.UpdateAsync(existingIngredient);
        
        return NoContent();
    }
}