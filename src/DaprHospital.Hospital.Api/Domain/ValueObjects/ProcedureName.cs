using System;

namespace DaprHospital.Hospital.Api.Domain.ValueObjects
{
    public record ProcedureName
    {
        public string Value { get; init; }
        private ProcedureName(string value)
        {
            Value = value;
        }

        public static ProcedureName Create(string value)
        {
            Validate(value);
            return new ProcedureName(value);
        }

        private static void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException("Invalid procedure name");
            }
        }
    }
}