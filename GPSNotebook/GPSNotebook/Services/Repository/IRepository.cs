using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GPSNotebook.Models;

namespace GPSNotebook.Services.Repository
{
    public interface IRepository
    {
        Task CreateTableAsync<T>() where T : IEntityBase, new();

        Task<List<T>> GetItemsAsync<T>() where T : IEntityBase, new();
        Task<List<T>> GetItemsAsync<T>(Expression<Func<T, bool>> predicate) where T : IEntityBase, new();

        Task<T> GetItemAsync<T>(int id) where T : IEntityBase, new();
        Task<T> GetItemAsync<T>(Expression<Func<T, bool>> predicate) where T : IEntityBase, new();

        Task<int> InsertItemAsync<T>(T item) where T : IEntityBase, new();
        Task<int> UpdateItemAsync<T>(T item) where T : IEntityBase, new();

        Task<int> DeleteItemAsync<T>(T item) where T : IEntityBase, new();
    }
}
