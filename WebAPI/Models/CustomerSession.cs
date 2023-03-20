using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class CustomerSession
    {
        public long ID { get; set; }
        public Nullable<long> CustomerID { get; set; }
        public string AccessToken { get; set; }
        public string FirebaseToken { get; set; }
        public string DeviceID { get; set; }
        public Nullable<bool> SessionState { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
