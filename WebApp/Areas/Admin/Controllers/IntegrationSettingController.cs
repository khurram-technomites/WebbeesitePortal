using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class IntegrationSettingController : Controller
    {
        private readonly IIntegrationSettingClient _integrationSetting;
        private readonly IResturantClient _resturant;
        private readonly ISupplierClient _supplier;
        private readonly IMapper _mapper;
        public IntegrationSettingController(IIntegrationSettingClient integrationSetting, IResturantClient resturant, IMapper mapper, ISupplierClient supplier)
        {
            _integrationSetting = integrationSetting;
            _resturant = resturant;
            _mapper = mapper;
            _supplier = supplier;
        }
        public async Task<IActionResult> Index()
        {
            var info = _mapper.Map<IEnumerable<IntegrationSettingViewModel>>(await _integrationSetting.GetIntegrationSettings());
            if (info.Count() == 0)
            {
                IntegrationSettingViewModel setting = new IntegrationSettingViewModel();
                return View(setting);
            }
            return View(info.FirstOrDefault());
        }
        public async Task<IActionResult> Update(long? Id, IntegrationSettingViewModel settingViewModel)
        {
            /*if (ModelState.IsValid)
            {*/
            try
            {
                if (Id.HasValue && Id > 0)
                {
                    IntegrationSettingDTO Result = await _integrationSetting.Update(_mapper.Map<IntegrationSettingDTO>(settingViewModel));

                    Result.Id = settingViewModel.Id;
                    IntegrationSettingViewModel model = _mapper.Map<IntegrationSettingViewModel>(Result);
                    return Json(new
                    {
                        success = true,
                        message = "Record Updated Successfully",
                        data = model
                    });
                }
                else
                {
                    IntegrationSettingDTO Result = await _integrationSetting.Create(_mapper.Map<IntegrationSettingDTO>(settingViewModel));
                    IntegrationSettingViewModel model = _mapper.Map<IntegrationSettingViewModel>(Result);
                    return Json(new
                    {
                        success = true,
                        message = "Record Updated Successfully",
                        data = model
                    });
                }


            }
            catch (ApiException ex)
            {
                ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                return Json(new
                {
                    success = false,
                    message = err.Message
                });
            }

            /*}*/

            return Json(new
            {
                success = false,
                message = "Fill all required fields and submit the form again"
            });
        }
        public async Task<IActionResult> Restaurant(PagingParameters model)
        {
            RestaurantOBj restaurant = new RestaurantOBj();
            var info = _mapper.Map<IEnumerable<RestaurantViewModel>>(await _resturant.GetRestaurantForDropDwonAsync(model));
            var infoList = _mapper.Map<IEnumerable<RestaurantViewModel>>(await _resturant.GetRestaurantForDropDwonAssignAsync(model));
            restaurant.restaurantList = infoList;

            ViewBag.Restaurant = info;
            return View(restaurant);
        }
        public async Task<IActionResult> Supplier()
        {
            SupplierOBj supplier = new SupplierOBj();
            var info = _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplier.GetSupplierForDropDwonAsync());
            var infoList = _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplier.GetSupplierForDropDwonAssignAsync());
            supplier.supplierList = infoList;
            ViewBag.Supplier = info;
            return View(supplier);
        }
        [HttpPost]
        public async Task<ActionResult> Restaurant(int restaurant_Id, String restaurant_SupplierCode)
        {

            string message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    RestaurantDTO model = new();
                    model.Id = restaurant_Id;
                    model.SupplierCode = restaurant_SupplierCode;
                    RestaurantDTO Result = await _resturant.Edit(model);

                    return Json(new
                    {
                        success = true,
                        message = "Restaurant Updated Successfully",
                        data = new
                        {
                            NameArAsPerTradeLicense = Result.NameArAsPerTradeLicense,
                            SupplierCode = Result.SupplierCode,
                            ID = Result.Id
                        }

                    });
                }
                catch (ApiException ex)
                {
                    ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                    return Json(new
                    {
                        url = "/Admin/IntegrationSetting/Index",
                        success = false,
                        message = err.Message
                    });
                }
            }

            return Json(new
            {
                success = false,
                message = "Fill all required fields and submit the form again"
            });
        }
        public async Task<ActionResult> UpdateSupplierCode(int Id)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    RestaurantDTO model = new RestaurantDTO();
                    model.Id = Id;
                    model.SupplierCode = null;
                    RestaurantDTO Result = await _resturant.UnAssignSupplierCode(model);

                    return Json(new
                    {
                        success = true,

                    });
                }
                catch (ApiException ex)
                {
                    ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                    return Json(new
                    {
                        url = "/Admin/IntegrationSetting/Index",
                        success = false,
                        message = err.Message
                    });
                }
            }

            return Json(new
            {
                success = false,
                message = "Fill all required fields and submit the form again"
            });
        }
    }
}
