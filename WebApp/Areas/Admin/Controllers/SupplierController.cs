using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelperClasses.DTOs;
using Newtonsoft.Json;
using WebApp.ErrorHandling;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;
using Fingers10.ExcelExport.ActionResults;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Handlers;
using Microsoft.AspNetCore.Authorization;
using HelperClasses.Classes;
using WebApp.Interfaces;
using HelperClasses.DTOs.Supplier;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, B2B Manager")]
    public class SupplierController : Controller
    {
        private readonly ISupplierClient _client;
        private readonly IUserSessionManager _userSessionManager;
        private readonly ICustomerTransactionHistoryClient _trasactionClient;
        private readonly IMapper _mapper;
        private readonly ISupplierPackageClient _supplierPackageClient;
        private readonly ICountryClient _countryClient;
        private readonly ICityClient _cityClient;
        private readonly ISupplierDocumentClient _supplierDocumentClient;
        public SupplierController(ISupplierClient client, IUserSessionManager userSessionManager
            , IMapper mapper, ICustomerTransactionHistoryClient trasactionClient,
            ICountryClient countryClient, ICityClient cityClient, ISupplierPackageClient supplierPackageClient
            , ISupplierDocumentClient supplierDocumentClient)
        {
            _client = client;
            _userSessionManager = userSessionManager;
            _mapper = mapper;
            _trasactionClient = trasactionClient;
            _supplierPackageClient = supplierPackageClient;
            _cityClient = cityClient;
            _countryClient = countryClient;
            _supplierDocumentClient = supplierDocumentClient;
        }
        #region Supplier 
        public async Task<IActionResult> Index()
        {
            var Info = _mapper.Map<IEnumerable<SupplierViewModel>>(await _client.GetAllSuppliersListAsync());
            return View(Info);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.CountryId = _mapper.Map<IEnumerable<CountryViewModel>>(await _countryClient.GetCountriesByMaster()).OrderBy(x => x.Name);
            ViewBag.CityId = _mapper.Map<IEnumerable<CityViewModel>>(await _cityClient.GetCitiesMaster());
            ViewBag.PackageId = _mapper.Map<IEnumerable<SupplierPackageViewModel>>(await _supplierPackageClient.GetAllAsync());
            return View(/*model*/);
        }
        [HttpPost]
        public async Task<ActionResult> Create(SupplierViewModel model , string logoPath , string BankName)
        {
            model.Logo = logoPath;
            model.Bank = BankName;
            string message = string.Empty;
            //if (ModelState.IsValid)
            //{

            if (model.Email == "admin@fougito.com")
                return Json(new { success = false, message = "Email cannot be admin@fougito.com" });
            try
            {
                model.Status = Enum.GetName(typeof(Status), Status.Active);
                SupplierDTO Result = await _client.AddSupplierAsync(_mapper.Map<SupplierDTO>(model));

                return Json(new
                {
                    success = true,
                    url = "/Admin/Supplier/Index",
                    message = "Supplier Created Successfully",
                });
            }
            catch (ApiException ex)
            {
                ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                return Json(new
                {
                    url = "/Admin/City/Index",
                    success = false,
                    message = err.Message
                });
            }
            //}

            //return Json(new
            //{
            //    success = false,
            //    message = "Fill all required fields and submit the form again"
            //});
        }
        public async Task<IActionResult> SupplierDetails(SupplierViewModel model)
        {
            ViewBag.CountryId = _mapper.Map<IEnumerable<CountryViewModel>>(await _countryClient.GetCountriesByMaster()).OrderBy(x => x.Name);
            ViewBag.CityId = _mapper.Map<IEnumerable<CityViewModel>>(await _cityClient.GetCitiesMaster());
            ViewBag.PackageId = _mapper.Map<IEnumerable<SupplierPackageViewModel>>(await _supplierPackageClient.GetAllAsync());
            var Info = _mapper.Map<IEnumerable<SupplierViewModel>>(await _client.GetSupplierByIDAsync(model.Id));
            return View(Info.FirstOrDefault());
        }
        [HttpGet]
        public async Task<IActionResult> Edit(long Id)
        {
            IEnumerable<SupplierDTO> store = await _client.GetSupplierByIDAsync(Id);
            ViewBag.CountryId = _mapper.Map<IEnumerable<CountryViewModel>>(await _countryClient.GetCountriesByMaster()).OrderBy(x => x.Name);
            ViewBag.CityId = _mapper.Map<IEnumerable<CityViewModel>>(await _cityClient.GetCitiesMaster());
            ViewBag.PackageId = _mapper.Map<IEnumerable<SupplierPackageViewModel>>(await _supplierPackageClient.GetAllAsync());
            return View(_mapper.Map<SupplierViewModel>(store.FirstOrDefault()));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(SupplierViewModel Model , string logoPath,  string BankName)
        {
            if (Model.Email == "admin@fougito.com")
                return Json(new { success = false, message = "Email cannot be admin@fougito.com" });
            //Model.Logo = logoPath;
            //Model.Bank = BankName;
            ViewBag.CountryId = _mapper.Map<IEnumerable<CountryViewModel>>(await _countryClient.GetCountriesByMaster()).OrderBy(x => x.Name);
            ViewBag.CityId = _mapper.Map<IEnumerable<CityViewModel>>(await _cityClient.GetCitiesMaster());
            ViewBag.PackageId = _mapper.Map<IEnumerable<SupplierPackageViewModel>>(await _supplierPackageClient.GetAllAsync());
            var result = await _client.UpdateSupplierAsync(_mapper.Map<SupplierDTO>(Model));
            return RedirectPermanent($"/Admin/Supplier/Edit?Id={result.Id}");
        }
        [HttpPost]
        public async Task<IActionResult> AddDocument(SupplierDocumentViewModel Model)
        {
            var result = await _supplierDocumentClient.AddSupplierDocument(_mapper.Map<SupplierDocumentDTO>(Model));

            return Json(
                new
                {
                    success = true,
                    message = "Document Added Successfully",
                    data = new
                    {
                        result.Id,
                        result.Path,
                        ExpiryDateTime = result.ExpiryDateTime.ToShortDateString(),
                        result.DocumentType
                    }
                });
        }

        public async Task<IActionResult> Delete(long Id)
        {
            var result = await _client.DeleteSupplier(Id);

            return Json(new
            {
                success = true,
                message = "Supplier deleted successfully",
                data = result.Id
            });
        }
        public async Task<IActionResult> DeleteDocument(long Id)
        {
            await _supplierDocumentClient.DeleteSupplierDocument(Id);

            return Json(
                new
                {
                    success = true,
                    message = "Document Deleted Successfully",
                });
        }
        public IActionResult DocumentModel(long SupplierId)
        {
            return View(SupplierId);
        }
        #endregion
        #region Supplier Approval
        public async Task<IActionResult> Approvals()
        {
            var Info = _mapper.Map<IEnumerable<SupplierViewModel>>(await _client.GetAllForApproval());
            return View(Info);
        }
        public async Task<IActionResult> Approve(SupplierViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    SupplierDTO Result = await _client.Approve(_mapper.Map<SupplierDTO>(model));

                    Result.Id = model.Id;

                    return Json(new
                    {
                        success = true,
                        message = "Profile Approved Successfully!",
                        data = new
                        {
                            Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            Name = Result.NameAsPerTradeLicense,
                            SupplierCode = Result.SupplierCode,
                            Email = Result.Email,
                            Status = Result.Status,
                            ID = Result.Id
                        }
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

            return Json(new
            {
                success = false,
                message = "Fill all required fields and submit the form again"
            });
        }
        public async Task<IActionResult> Reject(SupplierViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    SupplierDTO Result = await _client.Reject(_mapper.Map<SupplierDTO>(model));

                    Result.Id = model.Id;
                    return Json(new
                    {
                        success = false,
                        message = "Profile Rejected Successfully!",
                        data = new
                        {
                            Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            Name = Result.NameAsPerTradeLicense,
                            SupplierCode = Result.SupplierCode,
                            Email = Result.Email,
                            Status = Result.Status,
                            ID = Result.Id
                        }
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
            return Json(new
            {
                success = false,
                message = "Fill all required fields and submit the form again"
            });
        }
        public async Task<IActionResult> Details(SupplierViewModel model)
        {
            ViewBag.CountryId = _mapper.Map<IEnumerable<CountryViewModel>>(await _countryClient.GetCountriesByMaster()).OrderBy(x => x.Name);
            ViewBag.CityId = _mapper.Map<IEnumerable<CityViewModel>>(await _cityClient.GetCitiesMaster());
            ViewBag.PackageId = _mapper.Map<IEnumerable<SupplierPackageViewModel>>(await _supplierPackageClient.GetAllAsync());
            var Info = _mapper.Map<IEnumerable<SupplierViewModel>>(await _client.GetSupplierByIDAsync(model.Id));
            return View(Info.FirstOrDefault());
        }

        public async Task<IActionResult> DocumentForApproval(long SupplierId)
        {
            IEnumerable<SupplierDTO> store = await _client.GetSupplierByIDAsync(SupplierId);
            return View(_mapper.Map<SupplierViewModel>(store.FirstOrDefault()));
        }
        public async Task<IActionResult> SupplierTransaction(long SupplierId)
        {
            IEnumerable<SupplierDTO> store = await _client.GetSupplierByIDAsync(SupplierId);
            return View(_mapper.Map<SupplierViewModel>(store.FirstOrDefault()));
        }
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            var result = await _client.ToggleActiveStatusAsync(Id);

            return Json(new
            {
                success = true,
                message = "Status changed successfully",
                data = result.Id
            });
        } 
        public async Task<IActionResult> Activate(long Id)
        {
            var result = await _client.ToggleActiveStatusAsync(Id);

            return Json(new
            {
                success = true,
                message = "Status changed successfully",
            });
        }
        #endregion
    }
}
