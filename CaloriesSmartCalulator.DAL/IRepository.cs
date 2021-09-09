using CaloriesSmartCalulator.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaloriesSmartCalulator.DAL
{
    public interface IRepository<T> where T: IEntity
    {
        Task<T> GetAsync(Guid Id);
        Task<IEnumerable<T>> FilterAsync(Func<T, bool> predicate, int count);
        Task<T> InsertAsync(T entity);
        Task<T> UpdateAsync(T entity);
    }
}
