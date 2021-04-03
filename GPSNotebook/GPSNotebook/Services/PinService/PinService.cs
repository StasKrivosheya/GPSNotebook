using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GPSNotebook.Models;
using GPSNotebook.Services.Repository;

namespace GPSNotebook.Services.PinService
{
    public class PinService : IPinService
    {
        private readonly IRepository _repository;

        public PinService(IRepository repository)
        {
            _repository = repository;

            _repository.CreateTableAsync<PinModel>();
        }

        #region -- IPinService Implementation --

        public Task<List<PinModel>> GetPinsListAsync()
        {
            return _repository.GetItemsAsync<PinModel>();
        }

        public Task<List<PinModel>> GetPinsListAsync(Expression<Func<PinModel, bool>> predicate)
        {
            return _repository.GetItemsAsync(predicate);
        }

        public Task<PinModel> GetPinAsync(int id)
        {
            return _repository.GetItemAsync<PinModel>(id);
        }

        public Task<PinModel> GetPinAsync(Expression<Func<PinModel, bool>> predicate)
        {
            return _repository.GetItemAsync(predicate);
        }

        public Task<int> InsertPinAsync(PinModel pin)
        {
            return _repository.InsertItemAsync(pin);
        }

        public Task<int> UpdatePinAsync(PinModel pin)
        {
            return _repository.UpdateItemAsync(pin);
        }

        public Task<int> DeletePinAsync(PinModel pin)
        {
            return _repository.DeleteItemAsync(pin);
        }

        #endregion
    }
}
