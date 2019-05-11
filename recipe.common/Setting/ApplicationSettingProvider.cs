using System;
using System.Linq;
using ch.thommenmedia.common.Cache;
using ch.thommenmedia.common.Helper;
using ch.thommenmedia.common.Interfaces;
using ch.thommenmedia.common.Utils;

namespace ch.thommenmedia.common.Setting
{
    /// <summary>
    /// handles all application settings
    /// </summary>
    public class ApplicationSettingProvider : IApplicationSettingDbProvider
    {
        private readonly IServiceProvider _serviceProvider;
        public ApplicationSettingProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private IDbContext CreateContext()
        {
            return _serviceProvider.GetService(typeof(IDbContext)) as IDbContext;
        }

        /// <summary>
        ///     Gibt einen Eintrag aus der Config Tabelle zurück (als String)
        /// </summary>
        /// <param name="configKey">Name des Schlüssels in der Konfig</param>
        /// <param name="context">Datenbank Kontext</param>
        /// <param name="ignoreError">False = Es wird eine Exception geworfen wenn der Wert nicht gefunden wird</param>
        /// <returns>Den Wert des Konfig Schlüssels als String</returns>
        public virtual string GetConfigStringValue(string configKey, IDbContext context = null,
            bool ignoreError = false)
        {
            if (context == null)
                context = CreateContext();

            if (context == null && ignoreError)
                return null;
            else if (context == null)
            {
                throw new Exception(
                    "We couldn't create a IDbContext. Did you register the dependency in the IServiceProvider?");
            }

            //check caching
            var key = "ApplicationConfig-" + configKey;
            var result = CacheAccessor.TryGet<IApplicationSetting>(key);
            if (result == null)
            {
                result = context.ApplicationSettings.FirstOrDefault(q => q.Name == configKey);
                if (result != null)
                    CacheAccessor.AddOrUpdate(key, result);
                else if (!ignoreError)
                {
                    throw new Exception("Config Parameter " + configKey +
                                        " not found. Please add this config to the table Setting.");
                }
            }

            return result == null ? "" : result.Value;
        }

        /// <summary>
        ///     Gibt einen Eintrag aus der Config Tabelle zurück (als Int)
        /// </summary>
        /// <param name="configKey">Name des Schlüssels in der Konfig</param>
        /// <param name="context">Datenbank Kontext</param>
        /// <returns>Den Wert des Konfig Schlüssels als String</returns>
        public virtual int GetConfigIntValue(string configKey, IDbContext context = null)
        {
            var configValue = GetConfigStringValue(configKey, context);

            var parseSuccess = int.TryParse(configValue, out var configIntegerValue);
            if (!parseSuccess)
                throw new Exception(string.Format("Could not find an integer config setting for \"{0}\"", configKey));

            return configIntegerValue;
        }

        public virtual Guid GetConfigGuidValue(string configKey, IDbContext context = null)
        {
            var configValue = GetConfigStringValue(configKey, context);

            var parseSuccess = ShortGuid.TryParse(configValue, out var configIntegerValue);
            if (!parseSuccess)
                throw new Exception(string.Format("Could not find an guid config setting for \"{0}\"", configKey));

            return configIntegerValue;
        }

        /// <summary>
        /// loads the setting (string) and parses it to the target object
        /// expects the setting to be stored as json
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="configKey"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual TOut GetConfigObjectValue<TOut>(string configKey, IDbContext context = null)
            where TOut : class
        {
            var configValue = GetConfigStringValue(configKey, context);
            return JsonHelper.Deserialize<TOut>(configValue);
        }

        /// <summary>
        ///     Gibt einen Eintrag aus der Config Tabelle zurück (als Boolean)
        /// </summary>
        /// <param name="configKey">Name des Schlüssels in der Konfig</param>
        /// <param name="context">Datenbank Kontext</param>
        /// <returns>Den Wert des Konfig Schlüssels als String</returns>
        public virtual bool GetConfigBoolValue(string configKey, IDbContext context = null)
        {
            var configValue = GetConfigStringValue(configKey, context);

            var parseSuccess = bool.TryParse(configValue, out var configBoolValue);
            if (!parseSuccess)
                throw new Exception(string.Format("Could not find a bool config setting for \"{0}\"", configKey));

            return configBoolValue;
        }

        /// <summary>
        ///     Gibt einen Eintrag aus der Config Tabelle zurück (als TimeSpan)
        /// </summary>
        /// <param name="configKey">Name des Schlüssels in der Konfig</param>
        /// <param name="context">Datenbank Kontext</param>
        /// <returns>Den Wert des Konfig Schlüssels als String</returns>
        public virtual TimeSpan GetConfigTimeSpanValue(string configKey, IDbContext context = null)
        {
            var configValue = GetConfigStringValue(configKey, context);

            var parseSuccess = TimeSpan.TryParse(configValue, out var configTimeSpanValue);
            if (!parseSuccess)
                throw new Exception(string.Format("Could not find a timespan config setting for \"{0}\"", configKey));

            return configTimeSpanValue;
        }

    }
}
