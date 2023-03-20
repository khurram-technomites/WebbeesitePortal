using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebAPI.Models
{
    public class RestaurantContentManagement : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string PrivacyPolicy { get; set; }
        public string DeliveryPolicy { get; set; }
        public string ReturnPolicy { get; set; }
        public string TermsAndConditions { get; set; }
        public string AboutUs { get; set; }
        public string FooterImage { get; set; }
        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
