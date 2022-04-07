using DaprHospital.Patient.Api.Domain.ValueObjects;
using System;

namespace DaprHospital.Patient.Api.Domain.Entities;

public class Patient
{
    public Guid Id { get; init; }
    public PatientBloodType BloodType { get; private set; }
    public PatientStatus Status { get; private set; }

    public Patient(Guid id)
    {
        Id = id;
    }

    public void SetBloodType(PatientBloodType bloodType)
    {
        BloodType = bloodType;
    }

    public void Admit()
    {
        Status = PatientStatus.Admitted;
    }

    public void Discharge()
    {
        Status = PatientStatus.Discharged;
    }
}
