using FoodEstablishment.Api.Common;
using FoodEstablishment.Api.Modules.Identity.DTOs.Requests;
using FoodEstablishment.Api.Modules.Identity.DTOs.Validators;
using FoodEstablishment.Api.Modules.Identity.Entities;
using FoodEstablishment.Api.Modules.Identity.Enums;
using FoodEstablishment.Api.Modules.Identity.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FoodEstablishment.Api.Modules.Identity.Services;

public class AdminUserSeeder(
    IUserRepository userRepository,
    IOptions<AdminSeedOptions> options,
    ILogger<AdminUserSeeder> logger)
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly AdminSeedOptions _options = options.Value;
    private readonly ILogger<AdminUserSeeder> _logger = logger;

    public async Task SeedAsync()
    {
        if (!_options.IsConfigured)
        {
            _logger.LogWarning("Секция AdminSeed не задана. Администратор при запуске не создаётся.");
            return;
        }

        var validation = new RegisterRequestValidator().Validate(new RegisterRequest
        {
            Username = _options.Username!,
            PhoneNumber = _options.PhoneNumber!,
            Password = _options.Password!,
        });

        if (!validation.IsValid)
        {
            var message = string.Join(" ", validation.Errors.Select(error => error.ErrorMessage));
            throw new InvalidOperationException($"AdminSeed заполнен неверно: {message}");
        }

        if (await _userRepository.CountByRoleAsync(UserRoleType.Admin) > 0)
        {
            _logger.LogInformation("Администратор уже есть. AdminSeed пропущен.");
            return;
        }

        if (await _userRepository.GetByPhoneNumberAsync(_options.PhoneNumber!) != null)
        {
            throw new InvalidOperationException(
                "Телефон из AdminSeed уже занят, а администратора в базе нет. Укажите свободный номер.");
        }

        var admin = new User
        {
            Username = _options.Username!,
            PhoneNumber = _options.PhoneNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(_options.Password),
            BonusPoints = 0,
            Role = UserRoleType.Admin,
        };

        try
        {
            await _userRepository.AddAsync(admin);
        }
        catch (DbUpdateException ex) when (DatabaseExceptions.IsUniqueViolation(ex))
        {
            throw new InvalidOperationException(
                "Телефон из AdminSeed уже занят, а администратора в базе нет. Укажите свободный номер.", ex);
        }

        _logger.LogInformation("Создан администратор {PhoneNumber}.", admin.PhoneNumber);
    }
}