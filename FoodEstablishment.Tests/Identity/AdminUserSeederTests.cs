using FluentAssertions;
using FoodEstablishment.Api.Modules.Identity.Entities;
using FoodEstablishment.Api.Modules.Identity.Enums;
using FoodEstablishment.Api.Modules.Identity.Repositories.Interfaces;
using FoodEstablishment.Api.Modules.Identity.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace FoodEstablishment.Tests.Identity;

public class AdminUserSeederTests
{
    private readonly IUserRepository _users = Substitute.For<IUserRepository>();

    [Fact]
    public async Task SeedAsync_NoAdmin_CreatesOneWithHashedPassword()
    {
        _users.CountByRoleAsync(UserRoleType.Admin).Returns(0);
        _users.GetByPhoneNumberAsync("+79990000000").Returns((User?)null);
        var seeder = CreateSeeder(ValidOptions());

        await seeder.SeedAsync();

        await _users.Received(1).AddAsync(Arg.Is<User>(user =>
            user.Role == UserRoleType.Admin &&
            user.Username == "Администратор" &&
            user.PhoneNumber == "+79990000000" &&
            user.PasswordHash != "Admin12345" &&
            BCrypt.Net.BCrypt.Verify("Admin12345", user.PasswordHash)));
    }

    [Fact]
    public async Task SeedAsync_AdminAlreadyExists_DoesNothing()
    {
        _users.CountByRoleAsync(UserRoleType.Admin).Returns(1);
        var seeder = CreateSeeder(ValidOptions());

        await seeder.SeedAsync();

        await _users.DidNotReceive().AddAsync(Arg.Any<User>());
    }

    [Fact]
    public async Task SeedAsync_NotConfigured_DoesNothing()
    {
        var seeder = CreateSeeder(new AdminSeedOptions());

        await seeder.SeedAsync();

        await _users.DidNotReceive().CountByRoleAsync(Arg.Any<UserRoleType>());
        await _users.DidNotReceive().AddAsync(Arg.Any<User>());
    }

    [Fact]
    public async Task SeedAsync_PhoneTaken_ThrowsAndDoesNotPromote()
    {
        _users.CountByRoleAsync(UserRoleType.Admin).Returns(0);
        _users.GetByPhoneNumberAsync("+79990000000").Returns(new User
        {
            Id = 9,
            PhoneNumber = "+79990000000",
            Role = UserRoleType.Client,
        });
        var seeder = CreateSeeder(ValidOptions());

        var act = () => seeder.SeedAsync();

        await act.Should().ThrowAsync<InvalidOperationException>();
        await _users.DidNotReceive().AddAsync(Arg.Any<User>());
        await _users.DidNotReceive().UpdateAsync(Arg.Any<User>());
    }

    [Fact]
    public async Task SeedAsync_WeakPassword_Throws()
    {
        var options = ValidOptions();
        options.Password = "short";
        var seeder = CreateSeeder(options);

        var act = () => seeder.SeedAsync();

        await act.Should().ThrowAsync<InvalidOperationException>();
        await _users.DidNotReceive().AddAsync(Arg.Any<User>());
    }

    private AdminUserSeeder CreateSeeder(AdminSeedOptions options) =>
        new(_users, Options.Create(options), NullLogger<AdminUserSeeder>.Instance);

    private static AdminSeedOptions ValidOptions() => new()
    {
        Username = "Администратор",
        PhoneNumber = "+79990000000",
        Password = "Admin12345",
    };
}