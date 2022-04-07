using System;
using System.Collections.Generic;
using System.Linq;

namespace DaprHospital.Hospital.Api.Domain.Entities;

public class Inpatient
{
    private ICollection<Procedure> _procedures;
    public Guid Id { get; init; }
    public IEnumerable<Procedure> Procedures => _procedures.ToList();

    public Inpatient(Guid id)
    {
        Id = id;
        _procedures = new List<Procedure>();
    }

    public void AddProcedure(Procedure procedure)
    {
        _procedures.Add(procedure);
    }
}
