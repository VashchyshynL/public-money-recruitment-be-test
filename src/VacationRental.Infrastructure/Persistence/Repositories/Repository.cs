using System.Threading.Tasks;
using VacationRental.Application.Common.Interfaces;

namespace VacationRental.Infrastructure.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly VacationRentalDbContext Context;

        public Repository(VacationRentalDbContext context)
        {
            Context = context;
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }
    }
}
