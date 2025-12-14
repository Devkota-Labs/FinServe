using Auth.Domain.Entities;

namespace Auth.Application.Dtos;

public sealed record VerifyOtpDto(OtpPurpose Purpose, string Otp);
