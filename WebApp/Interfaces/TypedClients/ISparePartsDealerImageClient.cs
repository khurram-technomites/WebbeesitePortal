using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartsDealerImageClient
    {
        Task<IEnumerable<SparePartDealerImagesViewModel>> GetBySpareParts(long Id);
        Task Delete(long Id);
    }
}
