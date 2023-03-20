using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Fatoorah.WebHook
{
    public class WebHookResponseDTO<T>
    {
        public int EventType { get; set; }
        public string Event { get; set; }
        public DateTime DateTime { get; set; }
        public string CountryIsoCode { get; set; }
        public T Data { get; set; }
    }
}
