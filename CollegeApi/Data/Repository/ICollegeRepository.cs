using CollegeApi.Data;
using System.Linq.Expressions;

namespace CollegeApi.Data.Repository
{
    public interface ICollegeRepository<T>
    {
        Task<List<T>> GetAllAsync();

        Task<List<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false);


        Task<T> GetAsync(Expression<Func<T, bool>> filter, bool useNotracking = false);

        //Task<T> GetByNameAsync(Expression<Func<T, bool>> filter);

        Task<T> CreateAsync(T dbrecord);

        Task<T> UpdateAsync(T dbrecord);

        Task<bool> DeleteAsync(T dbrecord);

    }
}
