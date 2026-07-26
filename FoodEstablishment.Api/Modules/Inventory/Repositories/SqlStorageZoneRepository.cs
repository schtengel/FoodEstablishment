using FoodEstablishment.Api.Data;
using FoodEstablishment.Api.Modules.Inventory.Entities;
using FoodEstablishment.Api.Modules.Inventory.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodEstablishment.Api.Modules.Inventory.Repositories;

public class SqlStorageZoneRepository(ApplicationDbContext context) : IStorageZoneRepository
{
    public async Task<IEnumerable<StorageZone>> GetAllAsync()
    {
        return await context.StorageZones
            .ToListAsync();
    }

    public async Task AddAsync(StorageZone storageZone)
    {
        await context.StorageZones.AddAsync(storageZone);
        await context.SaveChangesAsync();
    }

    public async Task<StorageZone?> GetByIdAsync(int id)
    {
        return await context.StorageZones.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task UpdateAsync(StorageZone storageZone)
    {
        storageZone.UpdatedAt = DateTime.UtcNow;
        
        context.StorageZones.Update(storageZone);
        await context.SaveChangesAsync();
    }
}