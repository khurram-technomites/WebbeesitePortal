using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ErrorHandling
{
    public class Errors
    {
        [JsonProperty("")]
        public List<string> Items { get; set; }
    }

    public class BadRequestError
    {
        public Errors Errors { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string TraceId { get; set; }
    }
}
