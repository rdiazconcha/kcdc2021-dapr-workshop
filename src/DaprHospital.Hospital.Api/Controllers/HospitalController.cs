using DaprHospital.Hospital.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DaprHospital.Hospital.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HospitalController : ControllerBase
    {
        private readonly ILogger<HospitalController> logger;
        private readonly HospitalDbContext dbContext;

        public HospitalController(ILogger<HospitalController> logger, HospitalDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }
    }
}