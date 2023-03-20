using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Authentication
{
    public class ConfirmEmailDTO
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string ConfirmationLink { get; set; }
        public int AuthCode { get; set; }
        public string Title { get; set; }
    }
}
