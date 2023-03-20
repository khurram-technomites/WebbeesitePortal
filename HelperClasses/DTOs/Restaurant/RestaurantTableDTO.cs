using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
    public class RestaurantTableDTO
    {
        public RestaurantTableDTO()
        {
            RestaurantTableReservations = new List<RestaurantTableReservationDTO>();
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string ActiveStatus { get; set; }
        public int Serving { get; set; }
        public DateTime CreationDate { get; set; }
        public long RestaurantId { get; set; }
        public long RestaurantBranchId { get; set; }

        public bool IsTableReserved { get; set; }
        //public bool IsOrderGet { get; set; }
        public long? OrderId { get; set; }
        public string OrderNo { get; set; }

        public RestaurantDTO Restaurant { get; set; }
        public RestaurantBranchDTO RestaurantBranch { get; set; }
        //public RestaurantTableReservationDTO LatestReservation { get; set; }
        public List<RestaurantTableReservationDTO> RestaurantTableReservations { get; set; }

    }
}
