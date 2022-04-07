using System;

namespace DaprHospital.PatientQuery.Api.Models;

public record QueryModel(Guid Id, string FullName, string BloodType, string Status);
