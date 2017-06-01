using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace Fu.Infrastructure.Web
{
    public interface IRouting
    {
        void RegisterRoutes(IRouteBuilder routes);
    }
}
