using System;

namespace DaprHospital.Patient.Api.Models
{
    public record PatientModel(Guid Id, string BloodType, string Status);
}