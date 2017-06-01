using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fu.Infrastructure.Models
{
    public interface IEntityWithTypedId<TId>
    {
        TId Id { get; }
    }
}
