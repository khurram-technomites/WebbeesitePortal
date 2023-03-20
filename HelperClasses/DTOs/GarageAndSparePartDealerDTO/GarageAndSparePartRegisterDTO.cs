using HelperClasses.DTOs.Garage;
using HelperClasses.DTOs.SparePartsDealer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageAndSparePartDealerDTO
{
    public class GarageAndSparePartRegisterDTO
    {
        public string RegisteringFor { get; set; }
        public GarageRegisterDTO GarageRegisterDTO { get; set; }
        public SparePartsDealerRegisterDTO SparePartsDealerRegisterDTO { get; set; }

        public GarageAndSparePartRegisterDTO()
        {
            GarageRegisterDTO = new GarageRegisterDTO();
            SparePartsDealerRegisterDTO = new SparePartsDealerRegisterDTO();
        }
    }
}
