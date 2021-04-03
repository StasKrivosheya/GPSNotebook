using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GPSNotebook.Models;
using GPSNotebook.Services.Repository;

namespace GPSNotebook.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IRepository _repository;

        public UserService(IRepository repository)
        {
            _repository = repository;

            _repository.CreateTableAsync<UserModel>();
        }

        #region --- IUserService Implementation ---

        public Task<List<UserModel>> GetUsersListAsync()
        {
            return _repository.GetItemsAsync<UserModel>();
        }

        public Task<UserModel> GetUserAsync(int id)
        {
            return _repository.GetItemAsync<UserModel>(id);
        }

        public Task<UserModel> GetUserAsync(Expression<Func<UserModel, bool>> predicate)
        {
            return _repository.GetItemAsync(predicate);
        }

        public Task<int> InsertUserAsync(UserModel item)
        {
            return _repository.InsertItemAsync(item);
        }

        public Task<int> UpdateUserAsync(UserModel item)
        {
            return _repository.UpdateItemAsync(item);
        }

        public Task<int> DeleteUserAsync(UserModel item)
        {
            return _repository.DeleteItemAsync(item);
        }

        #endregion
    }
}
