using DaprHospital.Person.Api.Domain.ValueObjects;
using System;

namespace DaprHospital.Person.Api.Domain.Entities;

public class Person
{
    public Guid Id { get; init; }
    public PersonName Name { get; private set; }

    public Person()
    {
        Id = Guid.NewGuid();
    }
    public void SetName(PersonName name)
    {
        Name = name;
    }
}
