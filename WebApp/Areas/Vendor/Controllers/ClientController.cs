using AutoMapper;
using Fingers10.ExcelExport.ActionResults;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Garage;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    [Authorize(Roles = "Vendor,Admin,Automobile Manager,GarageOwner")]
    //[Route("Client/[action]")]
    public class ClientController : Controller
    {
        private readonly IGarageClient _garageClient;
        private readonly IGarageBusinessSettingClient _garageBusinessSettingClient;
        private readonly IGarageContentManagementClient _garageContentManagementClient;
        private readonly IClientIndustriesClient _industriesClient;
        private readonly IClientSectionsClient _sectionClient;
        private readonly IClientTypesClient _typesClient;
        private readonly IGarageImageClient _garageImageClient;
        private readonly IGarageRepairClient _garageRepairClient;
        private readonly IGarageScheduleClient _garageScheduleClient;
        private readonly ICarMakeClient _CarMakeclient;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _sessionManager;
        private readonly IModuleClient _moduleClient;
        private readonly IClientModulePurchasesClient _purchasesClient;
        private readonly IModulePurchaseDetailsClient _modulePurchaseDetailsClient;
        private readonly ICountryClient _countryService;
        private readonly ICityClient _cityService;
        private readonly IClientContentMediaClient _contentMediaClient;
        private readonly IClientDomainSuggestionsClient _clientDomainSuggestionsClient;
        private readonly IClientEmailsClient _clientEmailsClient;
        private readonly IClientModulesClient _clientModule;

        public ClientController(IGarageClient garageClient, IMapper mapper, ICarMakeClient carMakeclient, IGarageImageClient garageImageClient,
            IGarageRepairClient garageRepairClient, IGarageScheduleClient garageScheduleClient ,
            IClientIndustriesClient industriesClient, IClientSectionsClient sectionClient,
            IClientTypesClient typesClient, IUserSessionManager userSessionManager,
            IGarageBusinessSettingClient garageBusinessSettingClient, IModuleClient moduleClient,
            IClientModulePurchasesClient purchasesClient, IModulePurchaseDetailsClient modulePurchaseDetailsClient,
            IGarageContentManagementClient garageContentManagementClient, ICountryClient countryService,
            ICityClient cityService, IClientContentMediaClient clientContentMediaClient, IClientDomainSuggestionsClient clientDomainSuggestionsClient, IClientEmailsClient clientEmailsClient, IClientModulesClient clientModules)
        {
            _garageClient = garageClient;
            _mapper = mapper;
            _CarMakeclient = carMakeclient;
            _garageImageClient = garageImageClient;
            _garageRepairClient = garageRepairClient;
            _garageScheduleClient = garageScheduleClient;
            _industriesClient = industriesClient;
            _sectionClient = sectionClient;
            _typesClient = typesClient;
            _sessionManager = userSessionManager;
            _garageBusinessSettingClient = garageBusinessSettingClient;
            _moduleClient = moduleClient;
            _purchasesClient = purchasesClient;
            _modulePurchaseDetailsClient = modulePurchaseDetailsClient;
            _garageContentManagementClient = garageContentManagementClient;
            _countryService = countryService;
            _cityService = cityService;
            _contentMediaClient = clientContentMediaClient;
            _clientDomainSuggestionsClient = clientDomainSuggestionsClient;
            _clientEmailsClient = clientEmailsClient;
            _clientModule = clientModules;
        }
        public async Task<IActionResult> Index()
        {
            long VendorId = _sessionManager.GetVendorStore().Id;
            var info = _mapper.Map<IEnumerable<GarageViewModel>>(await _garageClient.GetGarageByVendor(VendorId));
            return View(info);
        }
        public async Task<IActionResult> Client(long Id)
        {
            ViewBag.ClientId = Id;
            return View();
        }

        //Ater Client Approval
        public async Task<IActionResult> ClientEdit(long Id)
        {
            ViewBag.ClientId = Id;
            return View();
        }

        //Ater Client Approval
        public async Task<IActionResult> PackageEdit(long Id)
        {
            ViewBag.ClientId = Id;
            return View();
        }
        public async Task<IActionResult> List()
        {
            return View();
        }
        public async Task<IActionResult> QuickCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> QuickCreate(GarageRegisterDTO garageRegisterDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    long vendorId = _sessionManager.GetVendorStore().Id;
                    garageRegisterDTO.Garage.VendorId = vendorId;
                    object Result = await _garageClient.Add(garageRegisterDTO);

                    return Json(new
                    {
                        success = true,
                        message = "Record Added Successfully",
                        data = Result
                    });
                }
            }
            catch(ApiException ex)
            {
                ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                return Json(new
                {
                    success = false,
                    message = err.Message
                });
            }
           
            return View();
        }
        public async Task<IActionResult> Detail(long Id)
        {
            GarageViewModel garage = _mapper.Map<GarageViewModel>(await _garageClient.GetGarageByID(Id));
            return View(garage);
        }
        public async Task<IActionResult> GetCityBycountryId(long countryId)
        {
            var cities = await _cityService.GetCityByCountryId(countryId);

            return Json(new
            {
                success = true,
                data = cities
            });
        }
        public async Task<IActionResult> Edit(long Id)
        {
            var ClientIndustry = _mapper.Map<IEnumerable<ClientIndustriesViewModel>>(await _industriesClient.GetIndustries());
            ViewBag.ClientIndustries = ClientIndustry.OrderBy(x => x.Name);
            var countries = _mapper.Map<IEnumerable<CountryViewModel>>(await _countryService.GetCountries());
            ViewBag.Country = countries.OrderBy(x => x.Name);
            var ClientType = _mapper.Map<IEnumerable<ClientTypesViewModel>>(await _typesClient.GetCities());
            ViewBag.ClientType = ClientType.OrderBy(x => x.Name);
            GarageViewModel garage = _mapper.Map<GarageViewModel>(await _garageClient.GetGarageByID(Id));
            garage.GarageBusinessSetting = _mapper.Map<GarageBusinessSettingViewModel>(await _garageBusinessSettingClient.GetBusinessSetting(Id));
            var garagecontent = await _garageContentManagementClient.GetAllByGarageIdAsync(Id);
            garage.GarageContentManagement = _mapper.Map<GarageContentManagementViewModel>(garagecontent.FirstOrDefault()) ;
            var clientmodule = await _purchasesClient.GetPurchaseByGarageId(Id);
            if (clientmodule != null && clientmodule.Count() > 0)
            {
                garage.ClientModulePurchases = _mapper.Map<ClientModulePurchasesViewModel>(clientmodule.LastOrDefault());
            }
            ViewBag.ClientId = garage.Id;
            return View(garage);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(GarageViewModel model)
        {
            try
            {
                GarageRegisterDTO garage = new();
                garage.Garage = _mapper.Map<GarageDTO>(model);
                object Result1 = await _garageClient.UpdateVendorGarage(garage.Garage);
                return Json(new
                {
                    success = true,
                    url = "/Vendor/Client/Package?Id=" + model.Id,
                    message = "Record Updated Successfully",
                    result = Result1
                }); ;
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    //url = "/Vendor/Client/Content",
                    message = "An Error Occured"
                });

            }





        }
        public async Task<IActionResult> Package(long Id)
        {
            long clientid = Id;
            ViewBag.ClientId = clientid;
            ClientPackageViewModel clientPackageViewModel = new ClientPackageViewModel();
            var modules = _mapper.Map<IEnumerable<ModuleViewModel>>(await _moduleClient.GetAllAsync());
            clientPackageViewModel.ClientModules = _mapper.Map<List<ClientModulesViewModel>>(await _clientModule.GetClientModuleByClientId(Id));
            //For Edit Remove Already Purchase Default Packages
            if (clientPackageViewModel.ClientModules.Count() > 0)
            {
                foreach (var module in modules.Where(x => x.IsDefault == true && x.ManageQunatity == false))
                {
                    if (!clientPackageViewModel.ClientModules.Select(x => x.ModuleId).Contains(module.Id))
                    {
                        clientPackageViewModel.Modules.Add(module);
                    }
                }
                clientPackageViewModel.Modules.AddRange(modules.Where(x => x.IsDefault == false || x.IsDefault == true && x.ManageQunatity == true));
                clientPackageViewModel.Modules.OrderBy(x => x.ServiceName).OrderByDescending(x => x.IsDefault == true).ToList();
                clientPackageViewModel.Modules.ForEach(x => x.IsDefault = false);
            }
            else
            {
                clientPackageViewModel.Modules = modules.OrderBy(x => x.ServiceName).OrderByDescending(x => x.IsDefault == true && x.ManageQunatity == false).ToList();
            }
            var clientmodule = await _purchasesClient.GetPurchaseByGarageId(clientid);
            if (clientmodule != null && clientmodule.Where(x => x.PaymentStatus != Enum.GetName(typeof(ClientPaymentStatus), ClientPaymentStatus.Paid)).Count() > 0)
            {
                clientPackageViewModel.ClientModulePurchases = _mapper.Map <ClientModulePurchasesViewModel>(clientmodule.Where(x => x.PaymentStatus != Enum.GetName(typeof(ClientPaymentStatus), ClientPaymentStatus.Paid)).LastOrDefault());
                clientPackageViewModel.ModulePurchaseDetails = _mapper.Map<List<ModulePurchaseDetailsViewModel>>(await _modulePurchaseDetailsClient.GetDetailsByPurchaseId(clientPackageViewModel.ClientModulePurchases.Id));
            }
            return View(clientPackageViewModel);
        }
       
        [HttpPost]
        public async Task<IActionResult> Package(ClientPackageViewModel clientPackageViewModel)
        {
            try
            {
                var clientmodules =_mapper.Map<List<ClientModulesViewModel>>(await _clientModule.GetClientModuleByClientId(clientPackageViewModel.ClientModulePurchases.GarageID));
              
                //Amount Calculate for Package Addon
                if (clientmodules.Count() > 0)
                {
                   var _clientModule = clientmodules.LastOrDefault();
                    if (_clientModule.ExpiryDate > DateTime.UtcNow)
                    {
                        var noofdayleft = (_clientModule.ExpiryDate - DateTime.UtcNow).Days;
                        var totalnumerofdays = (_clientModule.ExpiryDate - _clientModule.PurchaseDate).Days;
                        var amount = clientPackageViewModel.ClientModulePurchases.Total / totalnumerofdays;
                        var costfordayleft = amount * noofdayleft;
                        clientPackageViewModel.ClientModulePurchases.AmountToBePaid = costfordayleft;
                    }
                }
                var ClientId = clientPackageViewModel.ClientModulePurchases.GarageID;
                if (clientPackageViewModel.ClientModulePurchases.Id == 0)
                {
                    ClientModulePurchasesDTO result = await _purchasesClient.Create(_mapper.Map<ClientModulePurchasesDTO>(clientPackageViewModel.ClientModulePurchases));
                    if (result != null)
                    {
                        clientPackageViewModel.ModulePurchaseDetails.ForEach(x => x.ClientModulePurchaseID = result.Id);
                        await _modulePurchaseDetailsClient.CreateRange((_mapper.Map<List<ModulePurchaseDetailsDTO>>(clientPackageViewModel.ModulePurchaseDetails)));
                        return Json(new
                        {
                            success = true,
                            url = "/Vendor/Client/Content?Id="+ ClientId,
                            message = "Record Added Successfully",
                            purchaseid= result.Id
                        });
                    }
                }
                else
                {
                    ClientModulePurchasesDTO clientModulePurchases = await _purchasesClient.Edit(_mapper.Map<ClientModulePurchasesDTO>(clientPackageViewModel.ClientModulePurchases));
                    if (clientModulePurchases != null)
                    {
                        var modulePurchaseDetails = clientPackageViewModel.ModulePurchaseDetails.Where(x => x.Id == 0).ToList();
                        modulePurchaseDetails.ForEach(x => x.ClientModulePurchaseID = clientModulePurchases.Id);
                        var result = await _modulePurchaseDetailsClient.CreateRange((_mapper.Map<List<ModulePurchaseDetailsDTO>>(modulePurchaseDetails)));

                        if (result != null)
                        {
                            modulePurchaseDetails = clientPackageViewModel.ModulePurchaseDetails.Where(x => x.Id != 0).ToList();
                            result = await _modulePurchaseDetailsClient.UpdateRange((_mapper.Map<List<ModulePurchaseDetailsDTO>>(modulePurchaseDetails)));
                        }
                        return Json(new
                        {
                            success = true,
                            url = "/Vendor/Client/Content?Id=" + ClientId,
                            message = "Record Updated Successfully"
                        });
                    }
                   
                }

                return Json(new
                {
                    success = false,
                    //url = "/Vendor/Client/Content",
                    message = "An Error Occured"
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    //url = "/Vendor/Client/Content",
                    message = "An Error Occured"
                });
            }
           
        }

        public async Task<IActionResult> ClientPackageDetails(long Id)
        {
            var clientmodules = _mapper.Map<List<ClientModulesViewModel>>(await _clientModule.GetClientModuleByClientId(Id));
            return View(clientmodules);
        }
        public async Task<IActionResult> Content(long Id)   
        {
            ViewBag.ClientId = Id;
            var clientmodule = (await _purchasesClient.GetPurchaseByGarageId(Id)).LastOrDefault();
            GarageViewModel garage = _mapper.Map<GarageViewModel>(await _garageClient.GetGarageByID(Id));
            ViewBag.EmailCount = await _modulePurchaseDetailsClient.GetDetailsByPurchaseIdAndName(clientmodule.Id, ModulesObject.Emails);
            ClientContentViewModel clientContentViewModel = new ClientContentViewModel();
            clientContentViewModel.clientEmailsViewModels = _mapper.Map<List<ClientEmailsViewModel>>(await _clientEmailsClient.GetClientEmailsByClientId(Id));
            clientContentViewModel.ClientContentMediaViewModels = _mapper.Map<List<ClientContentMediaViewModel>>(await _contentMediaClient.GetClientContentByClientId(Id));
            clientContentViewModel.ClientDomainSuggestionsViewModels = _mapper.Map<List<ClientDomainSuggestionsViewModel>>(await _clientDomainSuggestionsClient.GetClientContentDomainByClientId(Id));
            clientContentViewModel.Website = garage.Website;
            clientContentViewModel.DomainUserId = garage.DomainUserId;
            clientContentViewModel.DomainPassword = garage.DomainPassword;
            clientContentViewModel.DomainProvider = garage.DomainProvider;
            clientContentViewModel.IsDomainRequired = garage.IsDomainRequired;
            return View(clientContentViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Content(ClientContentViewModel clientContentView)
        {
            try
            {
                if (clientContentView != null)
                {
                    long ClientId = clientContentView.ClientId;
                    ViewBag.ClientId = ClientId;
                    GarageViewModel garage = _mapper.Map<GarageViewModel>(await _garageClient.GetGarageByID(ClientId));
                    if (clientContentView.IsDomainRequired)
                    {
                        garage.IsDomainRequired = true;
                        var result = await _clientDomainSuggestionsClient.CreateRange(_mapper.Map<IEnumerable<ClientDomainSuggestionsDTO>>(clientContentView.ClientDomainSuggestionsViewModels));
                    }
                    else
                    {
                        garage.IsDomainRequired = false;
                        garage.DomainPassword = clientContentView.DomainPassword;
                        garage.DomainProvider = clientContentView.DomainProvider;
                        garage.DomainUserId = clientContentView.DomainUserId;
                        garage.Website = clientContentView.Website;
                       
                    }
                    await _garageClient.UpdateVendorGarage(_mapper.Map<GarageDTO>(garage));
                    await _clientEmailsClient.CreateRange(_mapper.Map<IEnumerable<ClientEmailsDTO>>(clientContentView.clientEmailsViewModels));
                    await _contentMediaClient.CreateRange(_mapper.Map<IEnumerable<ClientContentMediaDTO>>(clientContentView.ClientContentMediaViewModels));
                    return Json(new
                    {
                        success = true,
                        url = "/Vendor/Client/Finalize?Id=" + ClientId,
                        message = "Record Added Successfully"
                    });
                }
            }
            catch (ApiException ex)
            {
                return Json(new
                {
                    success = false,
                    //url = "/Vendor/Client/Content",
                    message = ex.Message
                });
            }
            
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Finalize(long Id)
        {
            ClientPackageViewModel clientPackageViewModel = new ClientPackageViewModel();
            GarageViewModel garage = _mapper.Map<GarageViewModel>(await _garageClient.GetGarageByID(Id));
            var clientmodule = await _purchasesClient.GetPurchaseByGarageId(Id);
            garage.ClientModulePurchases = _mapper.Map<ClientModulePurchasesViewModel>(clientmodule.LastOrDefault());
            garage.ModulePurchaseDetails = _mapper.Map<List<ModulePurchaseDetailsViewModel>>(await _modulePurchaseDetailsClient.GetDetailsByPurchaseId(clientmodule.LastOrDefault().Id));
            garage.ClientContentMediaViewModels = _mapper.Map<List<ClientContentMediaViewModel>>(await _contentMediaClient.GetClientContentByClientId(Id));
            garage.ClientDomainSuggestionsViewModels = _mapper.Map<List<ClientDomainSuggestionsViewModel>>(await _clientDomainSuggestionsClient.GetClientContentDomainByClientId(Id));
            garage.ClientEmailsViewModels = _mapper.Map<List<ClientEmailsViewModel>>(await _clientEmailsClient.GetClientEmailsByClientId(Id));
            ViewBag.ClientId = Id;
            return View(garage);
        }

        [HttpPost]
        public async Task<IActionResult> Finalize(int PurchaseId)
        {
            try
            {
                var clientmodule = await _purchasesClient.GetPurchaseByID(PurchaseId);
                if (clientmodule != null)
                {
                    if(clientmodule.PaymentUrl != null)
                    {
                        return Json(new
                        {
                            success = true,
                            url = clientmodule.PaymentUrl,
                        });
                    }
                    var result = await _purchasesClient.GenerateInvoice(clientmodule);
                    return Json(new
                    {
                        success = true,
                        url = result,
                    });
                }
                return Json(new
                {
                    success = false,
                    message = "An error Occured"
                });
            }
            catch(Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });

            } 
        }
        

        public async Task<ActionResult> DeletePackage(long Id,long ClientModulePurchaseID)
        {
            try
            {
                if (Id != 0)
                {
                    var purhasedetial = await _modulePurchaseDetailsClient.GetDetailsByID(Id);

                    ClientModulePurchasesViewModel clientmodulespurhase = _mapper.Map < ClientModulePurchasesViewModel >(await _purchasesClient.GetPurchaseByID(ClientModulePurchaseID));
                decimal Total = clientmodulespurhase.Total - purhasedetial.TotalPrice;
                clientmodulespurhase.Total = Total;
                clientmodulespurhase.SubTotal = Total;
                clientmodulespurhase.AmountToBePaid = Total;

                //For Package Edit
                var clientmodules = _mapper.Map<List<ClientModulesViewModel>>(await _clientModule.GetClientModuleByClientId(clientmodulespurhase.GarageID));
                if (clientmodules.Count() > 0)
                {
                    var _clientModule = clientmodules.LastOrDefault();
                    if (_clientModule.ExpiryDate > DateTime.UtcNow)
                    {
                        var noofdayleft = (_clientModule.ExpiryDate - DateTime.UtcNow).Days;
                        var totalnumerofdays = (_clientModule.ExpiryDate - _clientModule.PurchaseDate).Days;
                        var amount = Total / totalnumerofdays;
                        var costfordayleft = amount * noofdayleft;
                        clientmodulespurhase.AmountToBePaid = costfordayleft;
                    }
                }
                await _purchasesClient.Edit(_mapper.Map<ClientModulePurchasesDTO>(clientmodulespurhase));
                
                    await _modulePurchaseDetailsClient.Delete(Id);
                }
               

                return Json(new
                {
                    success = true,
                    message = "Record Deleted Successfully"
                });
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
        }
        public async Task<ActionResult> DeleteDocument(long Id)
        {
            try
            {
                await _contentMediaClient.Delete(Id);

                return Json(new
                {
                    success = true,
                    message = "Document Deleted Successfully"
                });
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
        }
        public async Task<ActionResult> DeleteDomain(long Id)
        {
            try
            {
                await _clientDomainSuggestionsClient.Delete(Id);

                return Json(new
                {
                    success = true,
                    message = "Domain Deleted Successfully"
                });
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
        }
        public async Task<ActionResult> DeleteEmail(long Id)
        {
            try
            {
                await _clientEmailsClient.Delete(Id);

                return Json(new
                {
                    success = true,
                    message = "Email Deleted Successfully"
                });
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
        }
        public async Task<ActionResult> Delete(long Id)
        {
            try
            {
                await _garageClient.Delete(Id);

                return Json(new
                {
                    success = true,
                    message = "Record Deleted Successfully"
                });
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
        }

        public async Task<ActionResult> GetPackageEmail(long Id)
        {
            var clientmodule = (await _purchasesClient.GetPurchaseByGarageId(Id)).LastOrDefault();
            var Count =await _modulePurchaseDetailsClient.GetDetailsByPurchaseIdAndName(clientmodule.Id, ModulesObject.Emails);
            return Json(new
            {
                success = true,
                data = Count
            });

        }
        [HttpGet]
        public async Task<IActionResult> ClientPurchases(long Id)
        {
            List<ClientModulePurchasesViewModel> ClientModulePurchaseList = _mapper.Map<List<ClientModulePurchasesViewModel>>(await _purchasesClient.GetPurchaseByGarageId(Id));
            return View(ClientModulePurchaseList);
        }
        [HttpGet]
        public async Task<IActionResult> PurchaseDetails(long Id)
        {
            try
            {
                List<ModulePurchaseDetailsViewModel> PurchaseDetailsList = _mapper.Map<List<ModulePurchaseDetailsViewModel>>(await _modulePurchaseDetailsClient.GetDetailsByPurchaseId(Id));
                return Json(new
                {
                    success = true,
                    data = PurchaseDetailsList
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    Message = ex.Message
                });
            }
            
        }

    }
}
