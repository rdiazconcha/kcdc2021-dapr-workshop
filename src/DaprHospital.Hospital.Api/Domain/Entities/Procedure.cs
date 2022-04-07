using DaprHospital.Hospital.Api.Domain.ValueObjects;
using System;

namespace DaprHospital.Hospital.Api.Domain.Entities;

public class Procedure
{
    public Guid Id { get; init; }
    public ProcedureName Name { get; private set; }

    private Procedure()
    {
    }

    public Procedure(string name)
    {
        Name = ProcedureName.Create(name);
    }

    public void SetProcedureName(ProcedureName name)
    {
        Name = name;
    }
}
