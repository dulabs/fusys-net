﻿using Fu.Infrastructure.Models;

namespace Fu.Module.Core.Models
{
    public class EntityType : EntityBase
    {
        public string Name { get; set; }

        public string RoutingController { get; set; }

        public string RoutingAction { get; set; }
    }
}
