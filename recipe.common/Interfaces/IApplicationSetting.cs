using System;
using System.Collections.Generic;
using System.Text;

namespace ch.thommenmedia.common.Interfaces
{
    /// <summary>
    /// generic interface for application settings
    /// </summary>
    public interface IApplicationSetting
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Value { get; set; }
    }
}
