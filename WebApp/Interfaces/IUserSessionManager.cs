using HelperClasses.DTOs;
using HelperClasses.DTOs.Authentication;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.SparePartsDealer;
using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces
{
    public interface IUserSessionManager
    {
        void ClearSession();
        void SetUserStore(RestaurantDTO Restaurant);
        void SetSupplierStore(SupplierDTO Supplier);
        void SetGarageStore(GarageDTO Garage);

        void SetVendorStore(VendorDTO vendor);
        void SetSparePartDealerStore(SparePartsDealerDTO Garage);
        RestaurantDTO GetUserStore();
        SupplierDTO GetSupplierStore();
        GarageDTO GetGarageStore();
        VendorDTO GetVendorStore();
        SparePartsDealerDTO GetSparePartDealerStore();
        UserAuthData GetUser();
        Task AddStoreToSession(RestaurantDTO Restaurant);
        Task UpdateStoreData(RestaurantDTO Restaurant);
        Task UpdateStoreDataInContextAsync(RestaurantDTO Restaurant);

    
        void SetUser(UserAuthData User);
    }
}
