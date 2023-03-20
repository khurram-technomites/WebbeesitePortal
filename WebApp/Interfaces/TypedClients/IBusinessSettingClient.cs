using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IBusinessSettingClient
    {
        Task<IEnumerable<BusinessSettingDTO>> GetBusinessSettings();
        Task<BusinessSettingDTO> Create(BusinessSettingDTO model);
        Task<BusinessSettingDTO> Update(BusinessSettingDTO model);

    }
}
