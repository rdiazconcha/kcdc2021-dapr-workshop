using System;

namespace DaprHospital.IntegrationEvents;

public record PatientAdmittedIntegrationEvent(Guid PatientId);
