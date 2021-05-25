using System.Threading.Tasks;

namespace VacationRental.Application.Common.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
    }
}
