using Microsoft.Extensions.DependencyInjection;

namespace Fu.Infrastructure
{
    public interface IModuleInitializer
    {
        void Init(IServiceCollection serviceCollection);
    }
}
