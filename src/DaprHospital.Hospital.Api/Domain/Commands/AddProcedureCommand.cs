using System;

namespace DaprHospital.Hospital.Api.Domain.Commands;

public record AddProcedureCommand(Guid PatientId, string ProcedureName);
