using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fu.Infrastructure.Models
{
    public abstract class EntityBaseWithTypeId<TId> : ValidatableObject, IEntityWithTypedId<TId>
    {
        public TId Id { get; protected set; }
    }
}
