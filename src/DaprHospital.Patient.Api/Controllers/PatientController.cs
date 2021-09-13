using Dapr.Client;
using DaprHospital.IntegrationEvents;
using DaprHospital.Patient.Api.Domain.Commands;
using DaprHospital.Patient.Api.Domain.ValueObjects;
using DaprHospital.Patient.Api.Infrastructure;
using DaprHospital.Patient.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DaprHospital.Patient.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly ILogger<PatientController> logger;
        private readonly PatientDbContext dbContext;
        private readonly DaprClient daprClient;

        public PatientController(ILogger<PatientController> logger, PatientDbContext dbContext, DaprClient daprClient)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.daprClient = daprClient;
        }

        [HttpPost("admit")]
        public async Task<IActionResult> AdmitPatient(AdmitPatientCommand command)
        {
            var patientToAdmit = await dbContext.Patients.FindAsync(command.Id);
            if (patientToAdmit == null)
            {
                return NotFound();
            }
            patientToAdmit.Admit();
            dbContext.Patients.Update(patientToAdmit);
            logger?.LogInformation($"Admitting patient: {command.Id}");
            await dbContext.SaveChangesAsync();
            var patientAdmitted = new PatientAdmittedIntegrationEvent(command.Id);
            await daprClient.PublishEventAsync("pubsub", "patient-topic", patientAdmitted);

            return Ok();
        }

        [HttpPost("discharge")]
        public async Task<IActionResult> DischargePatient(DischargePatientCommand command)
        {
            var patientToDischarge = await dbContext.Patients.FindAsync(command.Id);
            if (patientToDischarge == null)
            {
                return NotFound();
            }
            patientToDischarge.Discharge();
            dbContext.Patients.Update(patientToDischarge);
            logger?.LogInformation($"Discharging patient: {command.Id}");
            await dbContext.SaveChangesAsync();
            var patientDischarged = new PatientDischargedIntegrationEvent(command.Id);
            await daprClient.PublishEventAsync("pubsub", "patient-topic", patientDischarged);

            return Ok();
        }

        [HttpPost("bloodtype")]
        public async Task<IActionResult> SetBloodType(SetBloodTypeCommand command)
        {
            var patientToSetBloodType = await dbContext.Patients.FindAsync(command.Id);
            if (patientToSetBloodType == null)
            {
                return NotFound();
            }
            var bloodType = PatientBloodType.Create(command.BloodType);
            patientToSetBloodType.SetBloodType(bloodType);
            dbContext.Patients.Update(patientToSetBloodType);
            logger?.LogInformation($"Setting the patient {command.Id} blood type to: {command.BloodType}");
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var all = await dbContext.Patients.ToListAsync();
            var result = all.Select(p => new PatientModel(p.Id, p.BloodType, Enum.GetName(typeof(PatientStatus), p.Status)));
            return Ok(result);
        }
    }
}