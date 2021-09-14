using Dapr.Client;
using DaprHospital.Hospital.Api.Domain.Commands;
using DaprHospital.Hospital.Api.Domain.Entities;
using DaprHospital.Hospital.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DaprHospital.Hospital.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HospitalController : ControllerBase
    {
        private readonly ILogger<HospitalController> logger;
        private readonly HospitalDbContext dbContext;
        private readonly DaprClient daprClient;

        public HospitalController(ILogger<HospitalController> logger, HospitalDbContext dbContext, DaprClient daprClient)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.daprClient = daprClient;
        }

        [HttpPost("procedure")]
        public async Task<IActionResult> AddProcedure(AddProcedureCommand command)
        {
            var inpatientToAddProcedure = await dbContext.Inpatients.Include(p => p.Procedures).FirstAsync(i => i.Id == command.PatientId);
            if (inpatientToAddProcedure == null)
            {
                return NotFound();
            }
            var newProcedure = new Procedure(command.ProcedureName);
            inpatientToAddProcedure.AddProcedure(newProcedure);
            dbContext.Inpatients.Update(inpatientToAddProcedure);
            var message = $"Adding procedure {command.ProcedureName} to patient with Id: {command.PatientId}";
            logger?.LogInformation(message);
            await dbContext.SaveChangesAsync();

            await daprClient.InvokeBindingAsync("azurequeueoutput", "create", message);

            return Ok();
        }
    }
}