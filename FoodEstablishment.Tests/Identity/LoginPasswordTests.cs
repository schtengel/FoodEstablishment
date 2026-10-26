using FluentAssertions;
using FoodEstablishment.Api.Modules.Identity.Controllers;
using FoodEstablishment.Api.Modules.Identity.DTOs.Requests;
using FoodEstablishment.Api.Modules.Identity.DTOs.Responses;
using FoodEstablishment.Api.Modules.Identity.Entities;
using FoodEstablishment.Api.Modules.Identity.Enums;
using FoodEstablishment.Api.Modules.Identity.Repositories.Interfaces;
using FoodEstablishment.Api.Modules.Identity.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace FoodEstablishment.Tests.Identity;

public class LoginPasswordTests
{
    private const string Phone = "+79991234567";
    private const string Password = "Password1";

    private readonly IUserRepository _users = Substitute.For<IUserRepository>();
    private readonly AuthController _controller;

    public LoginPasswordTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["JwtSettings:Secret"] = "SUPER_SECRET_KEY_FOR_MY_APP_MUST_BE_LONG",
                ["JwtSettings:Issuer"] = "FoodEstablishmentApi",
                ["JwtSettings:Audience"] = "FoodEstablishmentClients",
                ["JwtSettings:ExpiryInMinutes"] = "60",
            })
            .Build();

        _controller = new AuthController(_users, new TokenService(configuration));
    }

    [Fact]
    public async Task LoginPassword_CorrectPassword_ReturnsToken()
    {
        var user = UserWithPassword(Password);
        _users.GetByPhoneNumberAsync(Phone).Returns(user);

        var result = await _controller.LoginPassword(new LoginRequest
        {
            PhoneNumber = Phone,
            Password = Password,
        });

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var body = ok.Value.Should().BeOfType<AuthResponse>().Subject;
        body.Token.Should().NotBeNullOrWhiteSpace();
        body.Username.Should().Be(user.Username);
        body.BonusPoints.Should().Be(user.BonusPoints);
    }

    [Fact]
    public async Task LoginPassword_WrongPassword_ReturnsUnauthorized()
    {
        _users.GetByPhoneNumberAsync(Phone).Returns(UserWithPassword(Password));

        var result = await _controller.LoginPassword(new LoginRequest
        {
            PhoneNumber = Phone,
            Password = "WrongPass1",
        });

        result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public async Task LoginPassword_UnknownPhone_ReturnsUnauthorized()
    {
        _users.GetByPhoneNumberAsync(Phone).Returns((User?)null);

        var result = await _controller.LoginPassword(new LoginRequest
        {
            PhoneNumber = Phone,
            Password = Password,
        });

        result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public async Task LoginPassword_UserWithoutPassword_ReturnsUnauthorized()
    {
        var user = UserWithPassword(Password);
        user.PasswordHash = null;
        _users.GetByPhoneNumberAsync(Phone).Returns(user);

        var result = await _controller.LoginPassword(new LoginRequest
        {
            PhoneNumber = Phone,
            Password = Password,
        });

        result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    private static User UserWithPassword(string password) => new()
    {
        Id = 7,
        Username = "Анна",
        PhoneNumber = Phone,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
        BonusPoints = 12,
        Role = UserRoleType.Client,
    };
}