using FoodEstablishment.Api.DTOs;
using FoodEstablishment.Api.Entities;
using FoodEstablishment.Api.Repositories;
using FoodEstablishment.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodEstablishment.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(IUserRepository userRepository, TokenService tokenService) : ControllerBase
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly TokenService _tokenService = tokenService;

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var existingUser = await _userRepository.GetByPhoneNumberAsync(request.PhoneNumber);
        if(existingUser != null) return BadRequest(
            new { message = "Пользователь с таким номером телефона уже существует." });

        var newUser = new User
        {
            Username = request.Username,
            PhoneNumber = request.PhoneNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            BonusPoints = 0
        };

        await _userRepository.AddAsync(newUser);

        var token = _tokenService.GenerateToken(newUser);
        return Ok(new AuthResponse { Token = token, Username = newUser.Username, BonusPoints = newUser.BonusPoints });
    }

    [HttpPost("login-password")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginPassword([FromBody] LoginRequest request)
    {
        var user = await _userRepository.GetByPhoneNumberAsync(request.PhoneNumber);
        if (user == null || string.IsNullOrEmpty(user.PasswordHash))
            return Unauthorized(new { message = "Неверный номер телефона или пароль." });

        var token = _tokenService.GenerateToken(user);
        return Ok(new AuthResponse { Token = token, Username = user.Username, BonusPoints = user.BonusPoints });
    }

    [HttpPost("send-code")]
    public async Task<IActionResult> SendCode([FromBody] SendCodeRequest request)
    {
        var user = await _userRepository.GetByPhoneNumberAsync(request.PhoneNumber);

        if (user == null)
        {
            user = new User
            {
                Username = "Новый клиент",
                PhoneNumber = request.PhoneNumber,
                BonusPoints = 0
            };
            await _userRepository.AddAsync(user);
        }
        
        var randomCode = new Random().Next(1000, 9999).ToString();

        user.VerificationCode = randomCode;
        user.VerificationCodeExpiresAt = DateTime.UtcNow.AddMinutes(5);

        await _userRepository.UpdateAsync(user);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n[SMS SERVICE] Код подтверждения для {request.PhoneNumber}: {randomCode}");
        Console.ResetColor();
        
        return Ok(new { message = "Код подтверждения успешно отправлен."});
    }

    [HttpPost("verify-code")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeRequest request)
    {
        var user = await _userRepository.GetByPhoneNumberAsync(request.PhoneNumber);
        if (user == null || user.VerificationCode != request.Code)
            return BadRequest(new { message = "Неверный или просроченный код подтверждения.." });

        if (user.VerificationCodeExpiresAt < DateTime.UtcNow)
            return BadRequest(new { message = "Срок действия кода подтверждения истек." });

        user.VerificationCode = null;
        user.VerificationCodeExpiresAt = null;
        await _userRepository.UpdateAsync(user);

        var token = _tokenService.GenerateToken(user);
        return Ok(new AuthResponse { Token = token, Username = user.Username, BonusPoints = user.BonusPoints });
    }
}