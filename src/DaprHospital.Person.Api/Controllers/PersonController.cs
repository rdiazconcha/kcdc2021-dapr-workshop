using Dapr.Client;
using DaprHospital.IntegrationEvents;
using DaprHospital.Person.Api.Domain.Commands;
using DaprHospital.Person.Api.Domain.ValueObjects;
using DaprHospital.Person.Api.Infrastructure;
using DaprHospital.Person.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DaprHospital.Person.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> logger;
        private readonly PersonDbContext dbContext;
        private readonly DaprClient daprClient;

        public PersonController(ILogger<PersonController> logger, PersonDbContext dbContext, DaprClient daprClient)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.daprClient = daprClient;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreatePersonCommand command)
        {
            var person = await CreatePersonAndPublishEventAsync(command);
            return Ok(person.Id);
        }

        [HttpPost("/azurequeueinput")]
        public async Task<IActionResult> OnInputBinding()
        {
            var command = await GetCommandFromRequestAsync();
            var person = await CreatePersonAndPublishEventAsync(command);
            return Ok(person.Id);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var all = await dbContext.People.ToListAsync();
            var result = all.Select(p => new PersonModel(p.Id, p.Name.FullName));
            return Ok(result);
        }

        private async Task<Domain.Entities.Person> CreatePersonAndPublishEventAsync(CreatePersonCommand command)
        {
            var person = new Domain.Entities.Person();
            person.SetName(PersonName.Create(command.FirstName, command.LastName));
            dbContext.People.Add(person);
            await dbContext.SaveChangesAsync();

            var personCreated = new PersonCreatedIntegrationEvent(person.Id);
            logger?.LogInformation($"Sending message: {personCreated}");
            await daprClient.PublishEventAsync("pubsub", "person-topic", personCreated);
            return person;
        }

        private async Task<CreatePersonCommand> GetCommandFromRequestAsync()
        {
            using var streamReader = new StreamReader(Request.Body);
            var body = await streamReader.ReadToEndAsync();
            var bytes = Convert.FromBase64String(body);
            var decodedString = System.Text.Encoding.UTF8.GetString(bytes);
            var command = JsonConvert.DeserializeObject<CreatePersonCommand>(decodedString);
            return command;
        }
    }
}