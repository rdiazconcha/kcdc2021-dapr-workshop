using Dapr;
using DaprHospital.IntegrationEvents;
using DaprHospital.Patient.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DaprHospital.Patient.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IntegrationEventsController : ControllerBase
    {
        private readonly ILogger<IntegrationEventsController> logger;
        private readonly PatientDbContext dbContext;

        public IntegrationEventsController(ILogger<IntegrationEventsController> logger, PatientDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        [Topic("pubsub", "person-topic")]
        public async Task<IActionResult> OnPersonCreated(PersonCreatedIntegrationEvent personCreated)
        {
            logger?.LogInformation($"Message received: {personCreated}");
            var patient = new Domain.Entities.Patient(personCreated.Id);
            dbContext.Patients.Add(patient);
            await dbContext.SaveChangesAsync();
            return Ok(personCreated.Id);
        }
    }
}