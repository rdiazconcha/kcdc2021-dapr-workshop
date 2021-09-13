using System;
using System.Collections.Generic;

namespace DaprHospital.PatientQuery.Api.Models
{
    public class StateModel
    {
        public DateTime LastQuery { get; set; }
        public IEnumerable<QueryModel> Data { get; set; }
    }
}