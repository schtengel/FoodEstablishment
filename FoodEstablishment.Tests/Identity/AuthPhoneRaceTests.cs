using FluentAssertions;
using FoodEstablishment.Api.Modules.Identity.Controllers;
using FoodEstablishment.Api.Modules.Identity.DTOs.Requests;
using FoodEstablishment.Api.Modules.Identity.Entities;
using FoodEstablishment.Api.Modules.Identity.Repositories.Interfaces;
using FoodEstablishment.Api.Modules.Identity.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace FoodEstablishment.Tests.Identity;

public class AuthPhoneRaceTests
{
    private const string Phone = "+79991234567";

    private readonly IUserRepository _users = Substitute.For<IUserRepository>();
    private readonly AuthController _controller;

    public AuthPhoneRaceTests()
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
    public async Task Register_UniqueViolation_ReturnsBadRequest()
    {
        _users.GetByPhoneNumberAsync(Phone).Returns((User?)null);
        _users.AddAsync(Arg.Any<User>()).Throws(UniqueViolation());

        var result = await _controller.Register(new RegisterRequest
        {
            Username = "Анна",
            PhoneNumber = Phone,
            Password = "Password1",
        });

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task SendCode_UniqueViolation_ContinuesWithExistingUser()
    {
        var winner = new User
        {
            Id = 3,
            Username = "Новый клиент",
            PhoneNumber = Phone,
            BonusPoints = 0,
        };

        _users.GetByPhoneNumberAsync(Phone).Returns((User?)null, winner);
        _users.AddAsync(Arg.Any<User>()).Throws(UniqueViolation());

        var result = await _controller.SendCode(new SendCodeRequest { PhoneNumber = Phone });

        result.Should().BeOfType<OkObjectResult>();
        await _users.Received(1).UpdateAsync(winner);
    }

    private static DbUpdateException UniqueViolation()
    {
        var postgres = new PostgresException(
            "duplicate key value violates unique constraint \"IX_users_phonenumber\"",
            "ERROR",
            "ERROR",
            PostgresErrorCodes.UniqueViolation);

        return new DbUpdateException("duplicate", postgres);
    }
}