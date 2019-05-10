using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Text;
using ch.thommenmedia.common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using recipe.business.Security;

namespace recipe.business.Helper
{
    public static class DependencyInjectionHelper
    {
        private static IServiceProvider _serviceProvider { get; set; }

        /// <summary>
        /// static service provider accessor (should be initialized to be able to use di)
        /// </summary>
        public static IServiceProvider ServiceProvider {
            get => _serviceProvider ?? BuildServiceProvider().BuildServiceProvider();
            set => _serviceProvider = value;
        }

        /// <summary>
        /// build the .net Dependency Injection service
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IServiceCollection BuildServiceProvider(IServiceCollection collection = null)
        {
            if(collection==null)
                collection = new ServiceCollection();

            collection.AddScoped<ISecurityAccessor, SecurityAccessor>();
            
            return collection;
        }
    }
}
