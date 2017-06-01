using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Fu.Infrastructure;
using Fu.Infrastructure.Web;
using Microsoft.AspNetCore.Routing;

namespace Fu.Module.Core.Extensions
{
    public class RoutesManager
    {
        public static void RegisterRoutes(IRouteBuilder routes)
        {
            List<Type> RoutingTypes = new List<Type>();
            foreach (var module in GlobalConfiguration.Modules)
            {
                RoutingTypes.AddRange(module.Assembly.GetTypes().Where(x => typeof(IRouting).IsAssignableFrom(x) && x.GetTypeInfo().IsClass));
            }

            foreach (var builderType in RoutingTypes)
            {
                if (builderType != null && builderType != typeof(IRouting))
                {
                    var builder = (IRouting)Activator.CreateInstance(builderType);
                    builder.RegisterRoutes(routes);
                }
            }

        }
    }
}
