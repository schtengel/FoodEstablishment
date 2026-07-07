using FoodEstablishment.Api.DTOs;
using FoodEstablishment.Api.Entities;
using FoodEstablishment.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FoodEstablishment.Api.Controllers;

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

    [HttpPost]
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
        
        return CreatedAtAction(nameof(GetAll), new { id = response.Id }, response);
    }

    [HttpPut("{id}")]
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