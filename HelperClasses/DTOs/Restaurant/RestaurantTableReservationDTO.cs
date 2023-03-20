using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelperClasses.DTOs.Order;

namespace HelperClasses.DTOs.Restaurant
{
    public class RestaurantTableReservationDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public int SeatsReserved { get; set; }
        public int SeatsAvailable { get; set; }
        public string MergeTableIds { get; set; }
        public DateTime? ReservationDate { get; set; }
        public TimeSpan? ReservationTime { get; set; }
        public long RestaurantTableId { get; set; }
        public long? OrderId { get; set; }

        public RestaurantTableDTO RestaurantTable { get; set; }
        public OrderDTO Order { get; set; }
    }
}
