using FoodEstablishment.Api.Modules.Inventory.Entities;

namespace FoodEstablishment.Api.Modules.Inventory.Repositories.Interfaces;

public interface IStorageZoneRepository
{
    Task<IEnumerable<StorageZone>> GetAllAsync();
    
    Task AddAsync(StorageZone storageZone);
    
    Task<StorageZone?> GetByIdAsync(int id);
    
    Task UpdateAsync(StorageZone storageZone);
}