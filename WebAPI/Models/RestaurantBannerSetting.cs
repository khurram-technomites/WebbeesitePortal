using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class RestaurantBannerSetting
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }
        [MaxLength(10, ErrorMessage = "Language length must be less than 10 characters")]
        public string Lang { get; set; }
        [MaxLength(4000, ErrorMessage = "Image length must be less than 4000 characters")]
        public string ImagePath { get; set; }
        [MaxLength(4000, ErrorMessage = "Url length must be less than 4000 characters")]
        public string Url { get; set; }
        [MaxLength(100, ErrorMessage = "BannerType length must be less than 100 characters")]
        public string BannerType { get; set; }
        public Restaurant Restaurant { get; set; }

    }
}
