using DaprHospital.Patient.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DaprHospital.Patient.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly ILogger<PatientController> logger;
        private readonly PatientDbContext dbContext;

        public PatientController(ILogger<PatientController> logger, PatientDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }
    }
}