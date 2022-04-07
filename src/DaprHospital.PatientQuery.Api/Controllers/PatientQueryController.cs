using Dapr.Client;
using DaprHospital.PatientQuery.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DaprHospital.PatientQuery.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientQueryController : ControllerBase
{
    private readonly DaprClient daprClient;

    public PatientQueryController(DaprClient daprClient)
    {
        this.daprClient = daprClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var lastQuery = await daprClient.GetStateEntryAsync<StateModel>("statestore", "lastquery");
        if (lastQuery.Value != null && DateTime.UtcNow <= lastQuery.Value.LastQuery.AddSeconds(30))
        {
            return Ok(lastQuery.Value.Data);
        }

        IEnumerable<QueryModel> result;
        result = await QueryPatients();

        bool saved = false;
        while (!saved)
        {
            result = await QueryPatients();
            lastQuery.Value = new StateModel()
            {
                LastQuery = DateTime.UtcNow,
                Data = result
            };
            saved = await lastQuery.TrySaveAsync();
        }

        return Ok(result);
    }

    private async Task<IEnumerable<QueryModel>> QueryPatients()
    {
        var patients = await daprClient.InvokeMethodAsync<IEnumerable<PatientModel>>(HttpMethod.Get, "patient", "patient/all");
        var people = await daprClient.InvokeMethodAsync<IEnumerable<PersonModel>>(HttpMethod.Get, "person", "person/all");
        var result = from patient in patients
                     join person in people on patient.Id equals person.Id
                     select new QueryModel(patient.Id, person.FullName, patient.BloodType, patient.Status);
        return result;
    }
}
