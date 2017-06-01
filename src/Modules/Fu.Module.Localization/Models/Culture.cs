using System.Collections.Generic;
using Fu.Infrastructure.Models;

namespace Fu.Module.Localization.Models
{
    public class Culture : EntityBase
    {
        public string Name { get; set; }

        public IList<Translate> Resources { get; set; }
    }
}