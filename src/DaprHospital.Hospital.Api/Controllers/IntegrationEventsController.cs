using Dapr;
using DaprHospital.Hospital.Api.Domain.Entities;
using DaprHospital.Hospital.Api.Infrastructure;
using DaprHospital.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace DaprHospital.Hospital.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class IntegrationEventsController : ControllerBase
{
    private readonly ILogger<IntegrationEventsController> logger;
    private readonly HospitalDbContext dbContext;

    public IntegrationEventsController(ILogger<IntegrationEventsController> logger,
                                       HospitalDbContext dbContext)
    {
        this.logger = logger;
        this.dbContext = dbContext;
    }

    [Topic("pubsub", "patient-topic")]
    public async Task<IActionResult> OnPatientAdmitted(PatientAdmittedIntegrationEvent @event)
    {
        var existingInpatient = dbContext.Inpatients.FirstOrDefault(i => i.Id == @event.PatientId);
        if (existingInpatient != null)
        {
            return NoContent();
        }
        var newInpatient = new Inpatient(@event.PatientId);
        dbContext.Inpatients.Add(newInpatient);
        logger?.LogInformation($"Adding inpatient: {@event.PatientId}");
        await dbContext.SaveChangesAsync();

        return Ok();
    }
}
