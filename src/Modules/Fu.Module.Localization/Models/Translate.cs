using Fu.Infrastructure.Models;

namespace Fu.Module.Localization.Models
{
    public class Translate : EntityBase
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public Culture Culture { get; set; }
    }
}
