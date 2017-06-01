using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;
using Fu.Infrastructure.Models;
using System.Threading.Tasks;

namespace Fu.Infrastructure.Data
{
    public interface IRepositoryWithTypedId<T, in TId> where T : IEntityWithTypedId<TId>
    {
        IQueryable<T> Query();

        void Add(T entity);

        void Update(T entity);

        IDbContextTransaction BeginTransaction();

        void SaveChange();

        Task<int> SaveChangesAsync();

        void Remove(T entity);
    }
}
