using System;

namespace DaprHospital.Patient.Api.Domain.ValueObjects;

public record PatientBloodType
{
    public string Value { get; private set; }
    private PatientBloodType(string value)
    {
        Value = value;
    }

    public static PatientBloodType Create(string value)
    {
        Validate(value);
        return new PatientBloodType(value);
    }

    private static void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException("Invalid blood type");
        }
    }

    public static implicit operator string (PatientBloodType bloodType)
    {
        return bloodType?.Value;
    }
}
