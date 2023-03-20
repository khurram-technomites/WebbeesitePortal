using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class UserRefreshToken
    {
        [Key]
        public long Id { get; set; }
        public string UserId { get; set; }
        public string RefreshToken { get; set; }
        public string DeviceTrackId { get; set; }
        public DateTime TokenDate { get; set; }
    }
}
