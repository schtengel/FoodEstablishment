using FoodEstablishment.Api.DTOs;
using FoodEstablishment.Api.Modules.Inventory.DTOs.Requests;
using FoodEstablishment.Api.Modules.Inventory.DTOs.Responses;
using FoodEstablishment.Api.Modules.Inventory.Entities;
using FoodEstablishment.Api.Modules.Inventory.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodEstablishment.Api.Modules.Inventory.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class StorageZonesController(IStorageZoneRepository storageZoneRepository) : ControllerBase
{
    private readonly IStorageZoneRepository _storageZoneRepository = storageZoneRepository;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StorageZoneResponse>))]
    public async Task<IActionResult> GetAll()
    {
        var storageZones = await _storageZoneRepository.GetAllAsync();

        var response = storageZones.Select(s => new StorageZoneResponse
        {
            Id = s.Id,
            Name = s.Name,
            RecommendedTemperature = s.RecommendedTemperature
        });
        
        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StorageZoneResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var storageZone = await _storageZoneRepository.GetByIdAsync(id);
        if (storageZone == null) return NotFound();

        return Ok(new StorageZoneResponse
        {
            Id = storageZone.Id,
            Name = storageZone.Name,
            RecommendedTemperature = storageZone.RecommendedTemperature
        });
    }

    [HttpPost]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StorageZoneResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] StorageZoneCreateRequest request)
    {
        var newStorageZone = new StorageZone
        {
            Name = request.Name,
            RecommendedTemperature = request.RecommendedTemperature

        };

        await _storageZoneRepository.AddAsync(newStorageZone);

        var response = new StorageZoneResponse()
        {
            Id = newStorageZone.Id,
            Name = newStorageZone.Name,
            RecommendedTemperature = newStorageZone.RecommendedTemperature
        };
        
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] StorageZoneCreateRequest request)
    {
        var existingZone = await _storageZoneRepository.GetByIdAsync(id);
        if (existingZone == null) return NotFound();
        
        existingZone.Name = request.Name;
        existingZone.RecommendedTemperature = request.RecommendedTemperature;
        
        await _storageZoneRepository.UpdateAsync(existingZone);
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var existingZone = await _storageZoneRepository.GetByIdAsync(id);
        if (existingZone == null) return NotFound();

        existingZone.IsDeleted = true;
        
        await _storageZoneRepository.UpdateAsync(existingZone);
        
        return NoContent();
    }
}