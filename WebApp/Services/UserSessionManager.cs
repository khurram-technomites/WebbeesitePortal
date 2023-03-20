using HelperClasses.DTOs;
using HelperClasses.DTOs.Authentication;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.SparePartsDealer;
using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApp.Interfaces;

namespace WebApp.Services
{
    public class UserSessionManager : IUserSessionManager
    {
        private readonly IHttpContextAccessor _context;
        private readonly ILogger<UserSessionManager> _logger;

        private string _StoreDataKey = "userStore";

        public UserSessionManager(IHttpContextAccessor Context, ILogger<UserSessionManager> logger)
        {
            _context = Context;
            _logger = logger;
        }

        public Task AddStoreToSession(RestaurantDTO Restaurant)
        {
            throw new NotImplementedException();
        }

        public void ClearSession()
        {
            _context.HttpContext.Session.Clear();
            _context.HttpContext.Response.Cookies.Delete("FougitoWebAppSession");
        }

        public RestaurantDTO GetUserStore()
        {
            byte[] StoreBytes = _context.HttpContext.Session.Get(_StoreDataKey);

            // if session is not empty
            if ((StoreBytes != null) && (StoreBytes.Length > 0))
            {
                string StoreString = Encoding.UTF8.GetString(StoreBytes);

                if (!string.IsNullOrEmpty(StoreString))
                    return JsonConvert.DeserializeObject<RestaurantDTO>(StoreString);
            }

            // if data was not found in session, pick it up from the cookie
            Claim Claim = _context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserStoreInfo");

            if (Claim != null)
            {
                string StoreString = Claim.Value;

                if (!string.IsNullOrEmpty(StoreString))
                    return JsonConvert.DeserializeObject<RestaurantDTO>(StoreString);
            }
            else
            {
                string User = "";

                Claim Clm = _context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

                if (Clm != null)
                    User = Clm.Value;

                _logger.LogInformation("Token doesn't have StoreInfo for User: {0}", User);
            }

            return new RestaurantDTO();
        }
        public SupplierDTO GetSupplierStore()
        {
            byte[] StoreBytes = _context.HttpContext.Session.Get(_StoreDataKey);

            // if session is not empty
            if ((StoreBytes != null) && (StoreBytes.Length > 0))
            {
                string StoreString = Encoding.UTF8.GetString(StoreBytes);

                if (!string.IsNullOrEmpty(StoreString))
                    return JsonConvert.DeserializeObject<SupplierDTO>(StoreString);
            }

            // if data was not found in session, pick it up from the cookie
            Claim Claim = _context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserStoreInfo");

            if (Claim != null)
            {
                string StoreString = Claim.Value;

                if (!string.IsNullOrEmpty(StoreString))
                    return JsonConvert.DeserializeObject<SupplierDTO>(StoreString);
            }
            else
            {
                string User = "";

                Claim Clm = _context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

                if (Clm != null)
                    User = Clm.Value;

                _logger.LogInformation("Token doesn't have StoreInfo for User: {0}", User);
            }

            return new SupplierDTO();
        }
        public UserAuthData GetUser()
        {
            byte[] StoreBytes = _context.HttpContext.Session.Get(_StoreDataKey);

            // if session is not empty
            if ((StoreBytes != null) && (StoreBytes.Length > 0))
            {
                string StoreString = Encoding.UTF8.GetString(StoreBytes);

                if (!string.IsNullOrEmpty(StoreString))
                    return JsonConvert.DeserializeObject<UserAuthData>(StoreString);
            }

            // if data was not found in session, pick it up from the cookie
            Claim Claim = _context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserStoreInfo");

            if (Claim != null)
            {
                string StoreString = Claim.Value;

                if (!string.IsNullOrEmpty(StoreString))
                    return JsonConvert.DeserializeObject<UserAuthData>(StoreString);
            }
            else
            {
                string User = "";

                Claim Clm = _context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

                if (Clm != null)
                    User = Clm.Value;

                _logger.LogInformation("Token doesn't have StoreInfo for User: {0}", User);
            }

            return new UserAuthData();
        }
        public void SetUserStore(RestaurantDTO Restaurant)
        {
            string StoreString = JsonConvert.SerializeObject(Restaurant);

            _context.HttpContext.Session.Set(_StoreDataKey, Encoding.UTF8.GetBytes(StoreString));
        }
        public void SetSupplierStore(SupplierDTO Supplier)
        {
            string StoreString = JsonConvert.SerializeObject(Supplier);

            _context.HttpContext.Session.Set(_StoreDataKey, Encoding.UTF8.GetBytes(StoreString));
        }
        public void SetUser(UserAuthData User)
        {
            string StoreString = JsonConvert.SerializeObject(User);

            _context.HttpContext.Session.Set(_StoreDataKey, Encoding.UTF8.GetBytes(StoreString));
        }
        public async Task UpdateStoreData(RestaurantDTO Restaurant)
        {
            RestaurantDTO UserStore = GetUserStore();

            await UpdateStoreDataInContextAsync(Restaurant);
        }

        public async Task UpdateStoreDataInContextAsync(RestaurantDTO Restaurant)
        {
            string StoreInfoStr = JsonConvert.SerializeObject(Restaurant);

            var Identity = new ClaimsIdentity(_context.HttpContext.User.Identity);

            // ReplaceClaim doesn't work
            Identity.RemoveClaim(Identity.FindFirst("UserStoreInfo"));
            Identity.AddClaim(new Claim("UserStoreInfo", StoreInfoStr));

            var principal = new ClaimsPrincipal(Identity);

            // Without re-signing, claims are not getting updated.
            await _context.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                    principal, new AuthenticationProperties
                                                    {
                                                        IsPersistent = false
                                                    });

            SetUserStore(Restaurant);
        }

        public void SetGarageStore(GarageDTO Garage)
        {
            string StoreString = JsonConvert.SerializeObject(Garage);

            _context.HttpContext.Session.Set(_StoreDataKey, Encoding.UTF8.GetBytes(StoreString));
        }
        public void SetSparePartDealerStore(SparePartsDealerDTO dealer)
        {
            string StoreString = JsonConvert.SerializeObject(dealer);

            _context.HttpContext.Session.Set(_StoreDataKey, Encoding.UTF8.GetBytes(StoreString));
        }
        public void SetVendorStore(VendorDTO vendor)
        {
            string StoreString = JsonConvert.SerializeObject(vendor);

            _context.HttpContext.Session.Set(_StoreDataKey, Encoding.UTF8.GetBytes(StoreString));
        }
        public GarageDTO GetGarageStore()
        {
            byte[] StoreBytes = _context.HttpContext.Session.Get(_StoreDataKey);

            // if session is not empty
            if ((StoreBytes != null) && (StoreBytes.Length > 0))
            {
                string StoreString = Encoding.UTF8.GetString(StoreBytes);

                if (!string.IsNullOrEmpty(StoreString))
                    return JsonConvert.DeserializeObject<GarageDTO>(StoreString);
            }

            // if data was not found in session, pick it up from the cookie
            Claim Claim = _context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserStoreInfo");

            if (Claim != null)
            {
                string StoreString = Claim.Value;

                if (!string.IsNullOrEmpty(StoreString))
                    return JsonConvert.DeserializeObject<GarageDTO>(StoreString);
            }
            else
            {
                string User = "";

                Claim Clm = _context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

                if (Clm != null)
                    User = Clm.Value;

                _logger.LogInformation("Token doesn't have StoreInfo for User: {0}", User);
            }

            return new GarageDTO();
        }

        public SparePartsDealerDTO GetSparePartDealerStore()
        {
            byte[] StoreBytes = _context.HttpContext.Session.Get(_StoreDataKey);

            // if session is not empty
            if ((StoreBytes != null) && (StoreBytes.Length > 0))
            {
                string StoreString = Encoding.UTF8.GetString(StoreBytes);

                if (!string.IsNullOrEmpty(StoreString))
                    return JsonConvert.DeserializeObject<SparePartsDealerDTO>(StoreString);
            }

            // if data was not found in session, pick it up from the cookie
            Claim Claim = _context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserStoreInfo");

            if (Claim != null)
            {
                string StoreString = Claim.Value;

                if (!string.IsNullOrEmpty(StoreString))
                    return JsonConvert.DeserializeObject<SparePartsDealerDTO>(StoreString);
            }
            else
            {
                string User = "";

                Claim Clm = _context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

                if (Clm != null)
                    User = Clm.Value;

                _logger.LogInformation("Token doesn't have StoreInfo for User: {0}", User);
            }

            return new SparePartsDealerDTO();
        }

       

        public VendorDTO GetVendorStore()
        {
            byte[] StoreBytes = _context.HttpContext.Session.Get(_StoreDataKey);

            // if session is not empty
            if ((StoreBytes != null) && (StoreBytes.Length > 0))
            {
                string StoreString = Encoding.UTF8.GetString(StoreBytes);

                if (!string.IsNullOrEmpty(StoreString))
                    return JsonConvert.DeserializeObject<VendorDTO>(StoreString);
            }

            // if data was not found in session, pick it up from the cookie
            Claim Claim = _context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserStoreInfo");

            if (Claim != null)
            {
                string StoreString = Claim.Value;

                if (!string.IsNullOrEmpty(StoreString))
                    return JsonConvert.DeserializeObject<VendorDTO>(StoreString);
            }
            else
            {
                string User = "";

                Claim Clm = _context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

                if (Clm != null)
                    User = Clm.Value;

                _logger.LogInformation("Token doesn't have StoreInfo for User: {0}", User);
            }

            return new VendorDTO();
        }

       
    }
}
