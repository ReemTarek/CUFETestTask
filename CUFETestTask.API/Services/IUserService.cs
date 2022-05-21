using CUFETestTask.API.Data.Models;

namespace CUFETestTask.API.Services
{
    public interface IUserService
    {
        Task AddNewUser(UserModel user);
        Task<UserModel?> GetUserData(UserModel searchUser);
        Task<bool> ResetPassword(UserModel searchUser, string newPassword);
    }
}