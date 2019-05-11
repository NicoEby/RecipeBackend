using System;
using System.Collections.Generic;
using System.Text;
using ch.thommenmedia.common.Interfaces;

namespace ch.thommenmedia.common.Setting
{
    public interface IApplicationSettingDbProvider
    {
        string GetConfigStringValue(string configKey, IDbContext context = null,
            bool ignoreError = false);
        int GetConfigIntValue(string configKey, IDbContext context = null);
        Guid GetConfigGuidValue(string configKey, IDbContext context = null);
        TOut GetConfigObjectValue<TOut>(string configKey, IDbContext context = null)
            where TOut : class;
        bool GetConfigBoolValue(string configKey, IDbContext context = null);
        TimeSpan GetConfigTimeSpanValue(string configKey, IDbContext context = null);
    }
}
