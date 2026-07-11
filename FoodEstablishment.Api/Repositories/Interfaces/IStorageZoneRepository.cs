using FoodEstablishment.Api.Entities;

namespace FoodEstablishment.Api.Repositories;

public interface IStorageZoneRepository
{
    Task<IEnumerable<StorageZone>> GetAllAsync();
    
    Task AddAsync(StorageZone storageZone);
    
    Task<StorageZone?> GetByIdAsync(int id);
    
    Task UpdateAsync(StorageZone storageZone);
}