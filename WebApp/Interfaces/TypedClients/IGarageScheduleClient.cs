using HelperClasses.DTOs.Garage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageScheduleClient
    {
        Task<IEnumerable<GarageScheduleViewModel>> GetAllByGarage(long GarageId);
        Task<IEnumerable<GarageScheduleViewModel>> AddandUpdateGarageSchedule(List<GarageScheduleDTO> Model);
        Task Delete(long Id);
    }
}
