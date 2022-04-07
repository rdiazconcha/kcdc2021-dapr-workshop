using DaprHospital.Core;
using System;

namespace DaprHospital.Person.Api.Domain.Events;

public class PatientAdmitted : IDomainEvent
{
    public Guid Id { get; init; }
    public PatientAdmitted(Guid id)
    {
        Id = id;
    }
}
