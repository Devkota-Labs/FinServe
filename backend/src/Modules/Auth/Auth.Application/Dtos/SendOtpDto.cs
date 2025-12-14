using Auth.Domain.Entities;

namespace Auth.Application.Dtos;

public sealed record SendOtpDto(OtpPurpose Purpose);
