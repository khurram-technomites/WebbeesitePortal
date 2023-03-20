using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class FCMUserSession
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string FirebaseToken { get; set; }
        public string DeviceId { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public string UserRole { get; set; }
        public AppUser User { get; set; }
    }
}
