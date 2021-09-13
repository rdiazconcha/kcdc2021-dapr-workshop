using System.Collections.Generic;

namespace DaprHospital.Core
{
    public interface IEntity
    {
        IReadOnlyCollection<IDomainEvent> Events { get; }
    }
}