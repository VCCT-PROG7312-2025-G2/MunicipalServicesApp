using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MunicipalServicesApp.Models
{
    public class StatusUpdate
    {
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public string UpdatedBy { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RequestStatus NewStatus { get; set; }
    }
}