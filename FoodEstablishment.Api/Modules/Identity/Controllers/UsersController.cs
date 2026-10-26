using FoodEstablishment.Api.Common.Extensions;
using FoodEstablishment.Api.Modules.Identity.DTOs.Requests;
using FoodEstablishment.Api.Modules.Identity.Enums;
using FoodEstablishment.Api.Modules.Identity.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodEstablishment.Api.Modules.Identity.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController(IUserRepository userRepository) : ControllerBase
{
    private readonly IUserRepository _userRepository = userRepository;

    [HttpPut("{id}/role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRole(int id, [FromBody] UserRoleUpdateRequest request)
    {
        if (request.Role is not (UserRoleType.Client or UserRoleType.Manager or UserRoleType.Admin))
            return BadRequest("Через этот метод можно назначить только Client, Manager или Admin.");

        if (id == User.GetUserId())
            return BadRequest("Нельзя изменить собственную роль.");

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        if (user.Role is UserRoleType.Guest or UserRoleType.Terminal)
            return BadRequest("Роль гостя или терминала здесь не меняется.");

        if (user.Role == UserRoleType.Admin && request.Role != UserRoleType.Admin)
        {
            var admins = await _userRepository.CountByRoleAsync(UserRoleType.Admin);
            if (admins <= 1)
                return BadRequest("Нельзя снять роль с последнего администратора.");
        }

        user.Role = request.Role;
        await _userRepository.UpdateAsync(user);

        return NoContent();
    }
}