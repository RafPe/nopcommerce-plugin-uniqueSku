using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RafTech.Nop.Plugin.Misc.SKU.Unique.Data
{
    using Autofac;

    using global::Nop.Core.Infrastructure;
    using global::Nop.Core.Infrastructure.DependencyManagement;
    using global::Nop.Core.Plugins;

    public class DependencyRegistrar :  IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            var pluginFinderTypes = typeFinder.FindClassesOfType<IPluginFinder>();

            bool isInstalled = false;

            foreach (var pluginFinderType in pluginFinderTypes)
            {
                var pluginFinder = Activator.CreateInstance(pluginFinderType) as IPluginFinder;
                var pluginDescriptor = pluginFinder.GetPluginDescriptorBySystemName("RafTech.Nop.Plugin.Misc.SKU.Unique");

                if (pluginDescriptor != null && pluginDescriptor.Installed)
                {
                    isInstalled = true;
                    break;
                }
            }

            // If plugin installed - then can be registered
            if (isInstalled) builder.RegisterType<ActionFilterUniqueSKU>().As<System.Web.Mvc.IFilterProvider>();
        }

        public int Order                            
        {
            get { return 10; } // we return 10 as we might be interfering with something else
        }
        
    }
}
