using FoodEstablishment.Api.Modules.Identity.Entities;

namespace FoodEstablishment.Api.Modules.Identity.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByPhoneNumberAsync(string phoneNumber);
    Task<User?> GetByDeviceIdAsync(string deviceId);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}