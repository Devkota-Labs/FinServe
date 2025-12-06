using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Shared.Application.Api;
using Shared.Security;
using Users.Application.Dtos.User;
using Users.Application.Interfaces.Repositories;
using Users.Domain.Entities;

namespace Users.Api.Controllers;

[ApiVersion("1.0")]
//[Authorize]
//[Authorize(Roles = "Admin")]
public sealed class UsersController(ILogger logger, IUserRepository UserRepository, IConfiguration configuration) 
    : BaseApiController(logger.ForContext<UsersController>())
{
    private readonly IUserRepository _userRepository = UserRepository;
    private readonly IConfiguration _config = configuration;
    private UserDto MapToDto(User User) =>
        new(User.FirstName, User.MiddleName, User.LastName, User.Mobile, User.Address, User.ProfileImageUrl, User.CountryId, User.StateId, User.CityId);

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var data = await _userRepository.GetAllAsync().ConfigureAwait(false);

        var Users = data.Select(MapToDto);

        return Ok(Users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var User = await _userRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (User == null)
            return NotFound($"User not found with id {id}");

        return Ok(MapToDto(User));
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = GetCurrentUserId();

        var user = await _userRepository.GetByIdAsync(userId).ConfigureAwait(false);

        if (user == null)
            return NotFound("User not found.");

        var profile = new UserProfileDto(user.Id, user.Email, user.FirstName, user.MiddleName, user.LastName, user.Mobile, user.Address, user.ProfileImageUrl, user.CountryId, 
            user.StateId, user.CityId, default, user.CreatedTime, user.LastUpdatedTime);

        Logger.Information("Profile viewed by user {UserId}", userId);
        return Ok(profile);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Post(CreateUserDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest("Email/password required");

        var policy = HttpContext.RequestServices.GetRequiredService<IPasswordPolicyService>();

        var (valid, message) = policy.ValidatePassword(dto.Password);

        if (!valid)
            return BadRequest(message);

        var existing = await _userRepository.GetByEmailAsync(dto.Email).ConfigureAwait(false);

        if (existing != null)
            return BadRequest("Email already exists");

        var exists = await _userRepository.GetByNameAsync(dto.FullName).ConfigureAwait(false);

        if (exists is not null)
        {
            return BadRequest($"User with name {exists.FullName} already exists.");
        }

        var user = new User
        {
            Email = dto.Email,
            Mobile = dto.Mobile,
            Gender = dto.Gender,
            DateOfBirth = dto.DateOfBirth,
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName,
            CountryId = dto.CountryId,
            CityId = dto.CityId,
            StateId = dto.StateId,
            Address = dto.Address,
            PinCode = dto.PinCode,
            IsActive = true,
            IsApproved = false,
            PasswordHash = dto.Password,
            PasswordLastChanged = DateTime.UtcNow,
            PasswordExpiryDate = DateTime.UtcNow.AddDays(_config.GetValue("Security:PasswordExpiryDays", 90))
        };

        await _userRepository.AddAsync(user).ConfigureAwait(false);

        return Created("User created.", MapToDto(user));
    }

    [HttpPatch("profile")]
    public async Task<IActionResult> Put(UserProfileDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var userId = GetCurrentUserId();

        var user = await _userRepository.GetByIdAsync(userId).ConfigureAwait(false);

        if (user == null)
            return NotFound("User not found.");

        // Basic info
        if (dto.FirstName is not null) user.FirstName = dto.FirstName;
        if (dto.MiddleName is not null) user.MiddleName = dto.MiddleName;
        if (dto.LastName is not null) user.LastName = dto.LastName;
        if (dto.Mobile is not null) user.Mobile = dto.Mobile;
        if (dto.Address is not null) user.Address = dto.Address;
        if (dto.ProfileImageUrl is not null) user.ProfileImageUrl = dto.ProfileImageUrl;

        // Location info
        if (dto.CountryId is not null) user.CountryId = dto.CountryId.Value;
        if (dto.StateId is not null) user.StateId = dto.StateId.Value;
        if (dto.CityId is not null) user.CityId = dto.CityId.Value;

        user.LastUpdatedTime = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user).ConfigureAwait(false);

        Logger.Information("User {UserId} updated profile successfully", userId);

        return Ok("Profile updated successfully.", MapToDto(user));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var User = await _userRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (User == null)
            return NotFound($"User not found with id {id}");

        await _userRepository.DeleteAsync(User).ConfigureAwait(false);

        return Ok("User deleted.", MapToDto(User));
    }

    // Helper method
    private int GetCurrentUserId()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
        return claim != null ? int.Parse(claim.Value) : 0;
    }
}