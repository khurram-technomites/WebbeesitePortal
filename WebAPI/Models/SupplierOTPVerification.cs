using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SupplierOTPVerification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int OTP { get; set; }
        public bool IsVerified { get; set; }
        public DateTime OTPExpiryTime { get; set; }
    }
}
