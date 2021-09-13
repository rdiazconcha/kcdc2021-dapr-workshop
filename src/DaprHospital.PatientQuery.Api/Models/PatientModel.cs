using System;

namespace DaprHospital.PatientQuery.Api.Models
{
    public record PatientModel(Guid Id, string BloodType, string Status);
}