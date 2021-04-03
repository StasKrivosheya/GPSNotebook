using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GPSNotebook.Models;

namespace GPSNotebook.Services.PinService
{
    public interface IPinService
    {
        Task<List<PinModel>> GetPinsListAsync();
        Task<List<PinModel>> GetPinsListAsync(Expression<Func<PinModel, bool>> predicate);

        Task<PinModel> GetPinAsync(int id);
        Task<PinModel> GetPinAsync(Expression<Func<PinModel, bool>> predicate);

        Task<int> InsertPinAsync(PinModel pin);
        Task<int> UpdatePinAsync(PinModel pin);

        Task<int> DeletePinAsync(PinModel pin);
    }
}
