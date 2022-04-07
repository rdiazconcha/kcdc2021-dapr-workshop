using System;

namespace DaprHospital.IntegrationEvents;

public record PatientDischargedIntegrationEvent(Guid PatientId);
