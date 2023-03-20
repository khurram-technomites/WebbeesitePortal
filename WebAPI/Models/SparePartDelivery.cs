using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SparePartDeliveries : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey("DeliveryStaff")]
        public long FougitoDeliveryStaffId { get; set; }
        [ForeignKey("SparePartRequest")]
        public long SpartPartRequestId { get; set; }
        [MaxLength(250, ErrorMessage = "PickupAddress length must be less than 200 characters")]
        public string PickupAddress { get; set; }
        public decimal PickupLatitude { get; set; }
        public decimal PickupLongitude { get; set; }
        [MaxLength(250, ErrorMessage = "DropAddress length must be less than 200 characters")]
        public string DropAddress { get; set; }
        public decimal DropLatitude { get; set; }
        public decimal DropLongitude { get; set; }
        [MaxLength(20, ErrorMessage = "Status length must be less than 20 characters")]
        public string Status { get; set; }
        public string ImageWhilePickingUp { get; set; }
        public string ImageAfterDelivery { get; set; }

        public DeliveryStaff DeliveryStaff { get; set; }
        public SparePartRequest SparePartRequest { get; set; }
    }
}
