﻿using System;

namespace DaprHospital.Person.Api.Domain.ValueObjects;

public record PersonName
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string FullName => $"{FirstName} {LastName}";
    private PersonName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static PersonName Create(string firstName, string lastName)
    {
        Validate(firstName, lastName);
        return new PersonName(firstName, lastName);
    }

    private static void Validate(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("Invalid first name");
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Invalid last name");
        }
    }
}
