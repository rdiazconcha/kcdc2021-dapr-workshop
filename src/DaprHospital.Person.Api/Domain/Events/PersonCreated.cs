using DaprHospital.Core;
using System;

namespace DaprHospital.Person.Api.Domain.Events;

public class PersonCreated : IDomainEvent
{
    public Guid Id { get; init; }
}
