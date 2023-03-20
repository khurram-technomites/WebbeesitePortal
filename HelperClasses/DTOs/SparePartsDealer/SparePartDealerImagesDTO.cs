using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.SparePartsDealer
{
    public class SparePartDealerImagesDTO
    {
        public long Id { get; set; }
        public long SparePartDealerId { get; set; }
        public string Image { get; set; }
    }
}
