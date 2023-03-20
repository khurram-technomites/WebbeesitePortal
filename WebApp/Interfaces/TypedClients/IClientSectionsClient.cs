using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IClientSectionsClient
    {
        Task<IEnumerable<ClientSectionsDTO>> GetCities();
        Task<ClientSectionsDTO> GetCityByID(long Id);
        Task<ClientSectionsDTO> Create(ClientSectionsDTO model);
        Task<ClientSectionsDTO> Edit(ClientSectionsDTO model);
        Task<ClientSectionsDTO> Delete(long Id);
        Task<ClientSectionsDTO> ToggleActiveStatus(long CityId);
        Task<IEnumerable<ClientSectionsDTO>> GetCitiesMaster();

    }
}
