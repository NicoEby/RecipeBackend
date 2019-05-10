using System;

namespace ch.thommenmedia.common.Interfaces
{
    public interface ILanguage
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string IsoCode { get; set; }
        bool IsDefault { get; set; }
        bool IsUiLanguage { get; set; }
    }
}
