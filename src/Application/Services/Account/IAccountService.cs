using Application.DTO;
using Application.Models;
using Domain.Common.Ids;

namespace Application.Services.Account
{
    public interface IAccountService
    {
        Task RegisterUser(RegisterUserDto registerUserDto);
        string Login(LoginUserDto loginUserDto);
        Task VerifyEmail(VerifyEmailDto verifyEmailDto);
        Task ChangePassword(EntityId userId, ChangePasswordDto changePasswordDto);
        Task DeleteUser(EntityId userId);
        Task RequestPasswordReset(string email);
        Task ResetPassword(ResetPasswordDto resetPasswordDto);
    }
}