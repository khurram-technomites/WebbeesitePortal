using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Authentication
{
    public class ConfirmUser
    {
        public string UserId { get; set; }
        public int AuthCode { get; set; }
    }
}
