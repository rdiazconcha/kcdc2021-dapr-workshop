using DaprHospital.Core;
using DaprHospital.Person.Api.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace DaprHospital.Person.Api.Domain.Entities
{
    public class Person : IEntity
    {
        private List<IDomainEvent> events = new();
        public Guid Id { get; init; }
        public PersonName Name { get; private set; }
        
        public IReadOnlyCollection<IDomainEvent> Events => events;

        public Person()
        {
            Id = Guid.NewGuid();
        }
        public void SetName(PersonName name)
        {
            Name = name;
        }
    }
}