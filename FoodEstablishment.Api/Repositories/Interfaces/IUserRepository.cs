using FoodEstablishment.Api.Entities;

namespace FoodEstablishment.Api.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByPhoneNumberAsync(string phoneNumber);
    Task<User?> GetByDeviceIdAsync(string deviceId);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}