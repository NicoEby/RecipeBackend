using System;

namespace ch.thommenmedia.common.Interfaces
{
    public interface IAccessRight
    {
        Guid Id { get; set; }
        string Name { get; set; }
    }
}
