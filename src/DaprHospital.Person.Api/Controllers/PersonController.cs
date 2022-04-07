using Dapr.Client;
using DaprHospital.IntegrationEvents;
using DaprHospital.Person.Api.Domain.Commands;
using DaprHospital.Person.Api.Domain.ValueObjects;
using DaprHospital.Person.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DaprHospital.Person.Api.Controllers;

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
}
