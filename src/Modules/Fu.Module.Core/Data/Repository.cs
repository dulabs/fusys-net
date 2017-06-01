using Fu.Infrastructure.Models;
using Fu.Infrastructure.Data;
using System.Threading.Tasks;

namespace Fu.Module.Core.Data
{
    public class Repository<T> : RepositoryWithTypedId<T, long>, IRepository<T>
       where T : class, IEntityWithTypedId<long>
    {
        public Repository(MainDbContext context) : base(context)
        {
        }
    }
}
