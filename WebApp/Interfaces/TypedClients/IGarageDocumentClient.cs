using HelperClasses.DTOs.Garage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageDocumentClient
    {
        Task<IEnumerable<GarageDocumentViewModel>> GetAllByGarage(long GarageId);
        Task<GarageDocumentViewModel> AddGarage(GarageDocumentDTO Model);
        Task Delete(long Id);
    }
}
