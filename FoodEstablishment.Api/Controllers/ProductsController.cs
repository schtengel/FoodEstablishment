using FoodEstablishment.Api.DTOs;
using FoodEstablishment.Api.Entities;
using FoodEstablishment.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodEstablishment.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository) : ControllerBase
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductResponse>))]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productRepository.GetAllAsync();

        var response = products.Select(p => new ProductResponse
        {
            Id = p.Id,
            Name = p.Name,
            Calories = p.Calories,
            CategoryId = p.CategoryId,
            Description = p.Description,
            IsStopListed = p.IsStopListed,
            Price = p.Price,
            Unit = p.Unit.ToString(),
            VolumeOrWeight = p.VolumeOrWeight
        });
        
        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return NotFound();

        var response = new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Calories = product.Calories,
            CategoryId = product.CategoryId,
            Description = product.Description,
            IsStopListed = product.IsStopListed,
            Price = product.Price,
            Unit = product.Unit.ToString(),
            VolumeOrWeight = product.VolumeOrWeight
        };
        
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] ProductCreateRequest request)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
        if (category == null) 
            return NotFound($"Категория с Id = {request.CategoryId} не найдена.");
        
        var newProduct = new Product
        {
            Name = request.Name,
            Calories = request.Calories,
            CategoryId = request.CategoryId,
            Description = request.Description,
            IsStopListed = request.IsStopListed,
            Price = request.Price,
            Unit = request.Unit,
            VolumeOrWeight = request.VolumeOrWeight
        };
        
        await _productRepository.AddAsync(newProduct);

        var response = new ProductResponse()
        {
            Id = newProduct.Id,
            Name = newProduct.Name,
            Calories = newProduct.Calories,
            CategoryId = newProduct.CategoryId,
            Description = newProduct.Description,
            IsStopListed = newProduct.IsStopListed,
            Price = newProduct.Price,
            Unit = newProduct.Unit.ToString(),
            VolumeOrWeight = newProduct.VolumeOrWeight
        };
        
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] ProductCreateRequest request)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null) return NotFound();
        
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
        if (category == null) 
            return NotFound($"Категория с Id = {request.CategoryId} не найдена.");
        
        existingProduct.Name = request.Name;
        existingProduct.Description = request.Description;
        existingProduct.Calories = request.Calories;
        existingProduct.CategoryId = request.CategoryId;
        existingProduct.IsStopListed = request.IsStopListed;
        existingProduct.Price = request.Price;
        existingProduct.Unit = request.Unit;
        existingProduct.VolumeOrWeight = request.VolumeOrWeight;
        
        await _productRepository.UpdateAsync(existingProduct);
        
        return NoContent();
    }
    
    [HttpPatch("{id}/toggle-stop-list")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleStopList(int id)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null) return NotFound();

        existingProduct.IsStopListed = !existingProduct.IsStopListed;
        
        await  _productRepository.UpdateAsync(existingProduct);
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null) return NotFound();

        existingProduct.IsDeleted = true;
        
        await _productRepository.UpdateAsync(existingProduct);
        
        return NoContent();
    }
}