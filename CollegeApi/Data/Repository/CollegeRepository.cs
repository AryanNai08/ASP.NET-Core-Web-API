using CollegeApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static System.Net.WebRequestMethods;

namespace CollegeApi.Data.Repository
{
    public class CollegeRepository<T> : ICollegeRepository<T> where T : class
    {

        private readonly CollegeDBContext _dbContext;
        private DbSet<T> _dbset;
        public CollegeRepository(CollegeDBContext dBContext) 
        {
            _dbContext = dBContext;
            _dbset= _dbContext.Set<T>();
        }

        public async Task<T> CreateAsync(T dbrecord)
        {
            _dbset.Add(dbrecord);
            await _dbContext.SaveChangesAsync();
            return dbrecord;
        }

        public async Task<bool> DeleteAsync(T dbrecord)
        {

            _dbset.Remove(dbrecord);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbset.ToListAsync();
        }

   

        public async Task<T> GetAsync(Expression<Func<T,bool>> filter, bool useNotracking = false)
        {
            if (useNotracking)
            {
                return await _dbset.AsNoTracking().Where(filter).FirstOrDefaultAsync();
            }
            else
            {
                return await _dbset.Where(filter).FirstOrDefaultAsync();
            }

        }

       

        async Task<List<T>> ICollegeRepository<T>.GetAllByFilterAsync(Expression<Func<T, bool>> filter, bool useNoTracking)
        {
            if (useNoTracking)
            {
                return await _dbset.AsNoTracking().Where(filter).ToListAsync();
            }
            else
            {
                return await _dbset.Where(filter).ToListAsync();
            }
        }

        //public async Task<T> GetByNameAsync(Expression<Func<T, bool>> filter)
        //{
        //    return await _dbset
        //         .Where(filter)
        //         .FirstOrDefaultAsync();
        //}

        public async Task<T> UpdateAsync(T dbrecord)
        {
            _dbset.Update(dbrecord);
            await _dbContext.SaveChangesAsync();

            return dbrecord;

        }

        
    }
}
