using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class MyFatoorahPaymentGatewaySetting  : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(200, ErrorMessage="ApiKey must be less than 200 characters")]
        public string ApiKey { get; set; }
        public bool IsLive { get; set; }
        [MaxLength(400, ErrorMessage = "TestEndpoint must be less than 200 characters")]
        public bool TestEndpoint { get; set; }
        [MaxLength(400, ErrorMessage = "LiveEndpoint must be less than 200 characters")]
        public bool LiveEndpoint { get; set; }
    }
}
