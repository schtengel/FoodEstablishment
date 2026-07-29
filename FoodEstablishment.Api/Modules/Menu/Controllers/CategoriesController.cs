
using FoodEstablishment.Api.Modules.Menu.DTOs.Requests;
using FoodEstablishment.Api.Modules.Menu.DTOs.Responses;
using FoodEstablishment.Api.Modules.Menu.Entities;
using FoodEstablishment.Api.Modules.Menu.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodEstablishment.Api.Modules.Menu.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CategoriesController(ICategoryRepository categoryRepository) : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryResponse>))]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryRepository.GetAllAsync();

        var response = categories.Select(c => new CategoryResponse
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        });
        
        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) return NotFound();

        return Ok(new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        });
    }

    [HttpPost]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CategoryResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CategoryCreateRequest request)
    {
        var newCategory = new Category
        {
            Name = request.Name,
            Description = request.Description
        };
        
        await _categoryRepository.AddAsync(newCategory);

        var response = new CategoryResponse()
        {
            Id = newCategory.Id,
            Name = newCategory.Name,
            Description = newCategory.Description
        };
        
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryCreateRequest request)
    {
        var existingCategory = await _categoryRepository.GetByIdAsync(id);
        if (existingCategory == null) return NotFound();
        
        existingCategory.Name = request.Name;
        existingCategory.Description = request.Description;
        
        await _categoryRepository.UpdateAsync(existingCategory);
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var existingCategory = await _categoryRepository.GetByIdAsync(id);
        if (existingCategory == null) return NotFound();

        existingCategory.IsDeleted = true;
        
        await _categoryRepository.UpdateAsync(existingCategory);
        
        return NoContent();
    }
}