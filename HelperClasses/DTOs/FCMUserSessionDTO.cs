using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class FCMUserSessionDTO
    {
        public long Id { get; set; }
        public string FirebaseToken { get; set; }
        public string DeviceId { get; set; }     
    }
}
