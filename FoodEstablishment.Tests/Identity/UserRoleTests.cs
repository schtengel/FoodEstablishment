using System.Security.Claims;
using FluentAssertions;
using FoodEstablishment.Api.Modules.Identity.Controllers;
using FoodEstablishment.Api.Modules.Identity.DTOs.Requests;
using FoodEstablishment.Api.Modules.Identity.Entities;
using FoodEstablishment.Api.Modules.Identity.Enums;
using FoodEstablishment.Api.Modules.Identity.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace FoodEstablishment.Tests.Identity;

public class UserRoleTests
{
    private readonly IUserRepository _users = Substitute.For<IUserRepository>();
    private readonly UsersController _controller;

    public UserRoleTests()
    {
        _controller = new UsersController(_users);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Role, nameof(UserRoleType.Admin)),
                ], "test")),
            },
        };
    }

    [Fact]
    public async Task UpdateRole_ClientToManager_UpdatesUser()
    {
        var user = new User { Id = 5, Role = UserRoleType.Client };
        _users.GetByIdAsync(5).Returns(user);

        var result = await _controller.UpdateRole(5, new UserRoleUpdateRequest
        {
            Role = UserRoleType.Manager,
        });

        result.Should().BeOfType<NoContentResult>();
        user.Role.Should().Be(UserRoleType.Manager);
        await _users.Received(1).UpdateAsync(user);
    }

    [Fact]
    public async Task UpdateRole_OwnRole_IsRejected()
    {
        var result = await _controller.UpdateRole(1, new UserRoleUpdateRequest
        {
            Role = UserRoleType.Manager,
        });

        result.Should().BeOfType<BadRequestObjectResult>();
        await _users.DidNotReceive().UpdateAsync(Arg.Any<User>());
    }

    [Fact]
    public async Task UpdateRole_GuestTarget_IsRejected()
    {
        var result = await _controller.UpdateRole(5, new UserRoleUpdateRequest
        {
            Role = UserRoleType.Guest,
        });

        result.Should().BeOfType<BadRequestObjectResult>();
        await _users.DidNotReceive().GetByIdAsync(Arg.Any<int>());
    }

    [Fact]
    public async Task UpdateRole_LastAdmin_CannotBeDemoted()
    {
        var user = new User { Id = 2, Role = UserRoleType.Admin };
        _users.GetByIdAsync(2).Returns(user);
        _users.CountByRoleAsync(UserRoleType.Admin).Returns(1);

        var result = await _controller.UpdateRole(2, new UserRoleUpdateRequest
        {
            Role = UserRoleType.Manager,
        });

        result.Should().BeOfType<BadRequestObjectResult>();
        user.Role.Should().Be(UserRoleType.Admin);
        await _users.DidNotReceive().UpdateAsync(Arg.Any<User>());
    }
}