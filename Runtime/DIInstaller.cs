using System.Collections.Generic;
using Zenject;

namespace DITools
{
    public class BaseDIInstaller : MonoInstaller
    {
        protected virtual void ConfigureServices()
        {
            Container.Configure(new List<ConfigureType>
            {
                new ConfigureType(typeof(IContainerConstructable), ScopeTypes.Singleton, false),
            });
        }

        public virtual void InstallBindings()
        {
            ConfigureServices();
        }
    }
}