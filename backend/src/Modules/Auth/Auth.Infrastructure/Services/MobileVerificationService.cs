//using Auth.Application.Interfaces.Repositories;
//using Auth.Application.Interfaces.Services;
//using Auth.Domain.Entities;
//using Microsoft.Extensions.Configuration;
//using Serilog;
//using Shared.Application.Responses;
//using Shared.Common.Utils;
//using System.Net;
//using System.Security.Cryptography;

//namespace Auth.Infrastructure.Services
//{
//    internal sealed class MobileVerificationService : BaseService, IMobileVerificationService
//    {
//        private readonly IOtpRepository _otpRepository;
//        private readonly ISmsSender _smsSender;
//        private readonly IUserRepository _userRepository;
//        private readonly IConfiguration _config;

//        public MobileVerificationService(ILogger logger, IOtpRepository otpRepository, IUserRepository userRepository, ISmsSender smsSender, IConfiguration config)
//            : base(logger.ForContext<MobileVerificationService>(), null)
//        {
//            _userRepository = userRepository;
//            _smsSender = smsSender;
//            _config = config;
//            _otpRepository = otpRepository;
//        }

//        private static OtpVerification GenerateOtp(int digits, int userId, OtpPurpose purpose, int expiryMinutes)
//        {
//            var rng = RandomNumberGenerator.GetInt32(0, (int)Math.Pow(10, digits));

//            var token = rng.ToString($"D{digits}");

//            return new OtpVerification
//            {
//                UserId = userId,
//                Token = token,
//                Purpose = purpose,
//                CreatedAt = DateTime.UtcNow,
//                ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes),
//            };
//        }

//        public async Task<ApiResponse<string>> SendOtpAsync(int userId, CancellationToken cancellationToken)
//        {
//            var user = await _userRepository.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);

//            if (user == null)
//                return new ApiResponse<string>(HttpStatusCode.NotFound, "User not found.");

//            if (user.MobileVerified)
//                return new ApiResponse<string>(HttpStatusCode.BadRequest, "Mobile already verified.");

//            var otpLength = _config.GetValue("Sms:OtpLength", 6);

//            var expiryMinutes = _config.GetValue("Sms:VerificationExpiryMinute", 15);

//            var record = GenerateOtp(otpLength, userId, OtpPurpose.MobileVerification, expiryMinutes);

//            await _otpRepository.AddAsync(record, cancellationToken).ConfigureAwait(false);

//            //await _smsSender.SendSmsAsync(user.Mobile, $"Your OTP is {token}, valid till {expiryMinutes} minutes only.");
//            await _smsSender.SendSmsAsync(user.FullName, user.Email, record.Token, expiryMinutes).ConfigureAwait(false);

//            return new ApiResponse<string>(HttpStatusCode.OK, "OTP sent successfully.");
//        }

//        public async Task<ApiResponse<string>> VerifyOtpAsync(int userId, string otp, CancellationToken cancellationToken)
//        {
//            var user = await _userRepository.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);

//            if (user == null)
//                return new ApiResponse<string>(HttpStatusCode.NotFound, "User not found.");

//            var record = await _otpRepository.GetActiveAsync(userId, otp, OtpPurpose.MobileVerification, cancellationToken).ConfigureAwait(false);

//            if (record == null)
//                return new ApiResponse<string>(HttpStatusCode.BadRequest, "Invalid or already used token.");

//            if (record.ExpiresAt < DateTime.UtcNow)
//                return new ApiResponse<string>(HttpStatusCode.BadRequest, "OTP expired.");

//            record.ConsumedAt = DateTimeUtil.Now;

//            await _otpRepository.UpdateAsync(record, cancellationToken).ConfigureAwait(false);

//            // Mark user verified
//            user.MobileVerified = true;
//            await _userRepository.UpdateAsync(user, cancellationToken).ConfigureAwait(false);

//            return new ApiResponse<string>(HttpStatusCode.OK, "Mobile number verified successfully!");
//        }
//    }
//}
