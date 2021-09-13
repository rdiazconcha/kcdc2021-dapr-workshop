using DaprHospital.Person.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DaprHospital.Person.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> logger;
        private readonly PersonDbContext dbContext;

        public PersonController(ILogger<PersonController> logger, PersonDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }
    }
}