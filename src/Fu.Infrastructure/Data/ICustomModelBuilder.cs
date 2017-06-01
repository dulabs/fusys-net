using Microsoft.EntityFrameworkCore;

namespace Fu.Infrastructure.Data
{
    public interface ICustomModelBuilder
    {
        void Build(ModelBuilder modelBuilder);
    }
}
