using FoodEstablishment.Api.Data;
using FoodEstablishment.Api.Modules.Identity.Entities;
using FoodEstablishment.Api.Modules.Identity.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodEstablishment.Api.Modules.Identity.Repositories;

public class SqlUserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(int id) =>
        await context.Users.FirstOrDefaultAsync(u => u.Id == id);
    
    public async Task<User?> GetByPhoneNumberAsync(string phoneNumber) =>
        await context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

    public async Task<User?> GetByDeviceIdAsync(string deviceId) => 
        await context.Users.FirstOrDefaultAsync(u => u.DeviceId == deviceId);
    
    public async Task AddAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        user.UpdatedAt = DateTime.UtcNow;
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }
}