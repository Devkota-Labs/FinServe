using FinServe.Api.Common;
using Modules.Auth.Application.Dtos;
using System.Threading.Tasks;
using static FinServe.Api.Common.ApiRoutes;

namespace Auth.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto?> RegisterAsync(string fullName, string email, string password);
        Task<string?> LoginAsync(string email, string password);
    }
    public interface IMobileVerificationService
    {
        Task<ApiResponse<string>> SendOtpAsync(int userId);
        Task<ApiResponse<string>> VerifyOtpAsync(int userId, string otp);
    }
    public interface ISmsSender
    {
        Task SendSmsAsync(string name, string mobileNo, string otp, int expiryMinutes);
    }
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string html);
    }
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByMobileAsync(string mobile);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<IEnumerable<User>> GetPendingApprovalsAsync();
        Task AddAsync(User user);
        Task SaveChangesAsync();
    }

    public interface IRefreshTokenService
    {
    }

    public interface IMfaService
    {

    }
}
