using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class GenericManager<T> : IGenericService<T> where T : class
    {
        private readonly IGenericRepository<T> _genericRepository;

        public GenericManager(IGenericRepository<T> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task AddAsync(T entity) => await _genericRepository.AddAsync(entity);

        public async Task DeleteAsync(T entity) => await _genericRepository.DeleteAsync(entity);

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, params string[] includes)
            => await _genericRepository.GetAllAsync(filter, includes);

        public async Task<T> GetByIdAsync(object id) => await _genericRepository.GetByIdAsync(id);

        public async Task<T> GetFirstAsync(Expression<Func<T, bool>> filter, params string[] includes)
            => await _genericRepository.GetFirstAsync(filter, includes);

        public async Task UpdateAsync(T entity) => await _genericRepository.UpdateAsync(entity);
    }
}
