using System;

namespace DaprHospital.Patient.Api.Domain.Commands;

public record SetBloodTypeCommand(Guid Id, string BloodType);
