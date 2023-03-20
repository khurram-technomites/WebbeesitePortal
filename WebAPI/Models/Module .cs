using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace WebAPI.Models
{
    public class Module:GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        [MaxLength(4000, ErrorMessage = "Description length must be less than 4000 characters")]    

        public decimal Price { get; set; }

        public bool ManageQunatity { get; set; }
        public long Min { get; set; }
        public long Max { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public bool IsSystem { get; set; }

    }
}
