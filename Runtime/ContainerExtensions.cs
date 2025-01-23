using System;
using System.Collections.Generic;
using ModestTree;
using Zenject;

namespace DITools
{
    public struct ConfigureType
    {
        public ConfigureType(Type configuredType, ScopeTypes scopeTypes, bool lazy)
        {
            ConfiguredType = configuredType;
            ScopeTypes = scopeTypes;
            Lazy = lazy;
        }

        public readonly Type ConfiguredType;
        public readonly ScopeTypes ScopeTypes;
        public readonly bool Lazy;
    }

    public static class ContainerExtensions
    {
        public static void Configure(this DiContainer container, List<ConfigureType> configureTypes, params string[] assemblyNames)
        {
            var conventionBindInfo = new ConventionBindInfo();
            conventionBindInfo.AddAssemblyFilter(assembly =>
            {
                var assemblyNameInfo = assembly.GetName();
                var assemblyName = assemblyNameInfo.Name;
                if (assemblyName.Contains("Assembly-CSharp"))
                    return true;

                foreach (var name in assemblyNames)
                {
                    if (assemblyName.Contains(name))
                        return true;
                }

                if (assemblyName.Contains("_Scripts"))
                    return true;
                return false;
            });

            conventionBindInfo.AddTypeFilter(type =>
            {
                foreach (var configureType in configureTypes)
                {
                    if (type.DerivesFrom(configureType.ConfiguredType))
                        return true;
                }
                return false;
            });
            
            var typesBinder = new ConventionSelectTypesBinder(conventionBindInfo);
            typesBinder.AllNonAbstractClasses();

            foreach (var serviceType in conventionBindInfo.ResolveTypes())
            {
                var binder = container.BindInterfacesAndSelfTo(serviceType);
                var configureType = configureTypes.Find(x => serviceType.DerivesFrom(x.ConfiguredType));
                
                binder.BindInfo.Scope = configureType.ScopeTypes;
                if (!configureType.Lazy)
                    binder.NonLazy();
                else
                    binder.Lazy();
            }
        }
    }
}