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

        public Task<List<UserModel>> GetItemsAsync()
        {
            return _repository.GetItemsAsync<UserModel>();
        }

        public Task<UserModel> GetItemAsync(int id)
        {
            return _repository.GetItemAsync<UserModel>(id);
        }

        public Task<UserModel> GetItemAsync(Expression<Func<UserModel, bool>> predicate)
        {
            return _repository.GetItemAsync(predicate);
        }

        public Task<int> InsertItemAsync(UserModel item)
        {
            return _repository.InsertItemAsync(item);
        }

        public Task<int> UpdateItemAsync(UserModel item)
        {
            return _repository.UpdateItemAsync(item);
        }

        public Task<int> DeleteItemAsync(UserModel item)
        {
            return _repository.DeleteItemAsync(item);
        }

        #endregion
    }
}
