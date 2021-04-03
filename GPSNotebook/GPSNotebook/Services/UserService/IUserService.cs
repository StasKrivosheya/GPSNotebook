using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GPSNotebook.Models;

namespace GPSNotebook.Services.UserService
{
    public interface IUserService
    {
        Task<List<UserModel>> GetUsersListAsync();

        Task<UserModel> GetUserAsync(int id);
        Task<UserModel> GetUserAsync(Expression<Func<UserModel, bool>> predicate);

        Task<int> InsertUserAsync(UserModel item);
        Task<int> UpdateUserAsync(UserModel item);

        Task<int> DeleteUserAsync(UserModel item);
    }
}
