using Fu.Infrastructure.Models;
using System.Linq;

namespace Fu.Infrastructure.Data
{
    public interface IRepository<T> : IRepositoryWithTypedId<T, long> where T : IEntityWithTypedId<long>
    {
    }
}
