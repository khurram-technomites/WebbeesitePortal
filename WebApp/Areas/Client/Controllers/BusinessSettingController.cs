using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Garage;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.Services.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Garage.Controllers
{
    [Area("Client")]
    public class BusinessSettingController : Controller
    {
        private readonly IGarageBusinessSettingClient _businessSetting;
        private readonly IGarageBranchBusinessSettingClient _branchBusinessSetting;
        private readonly IGarageContentManagementClient _garageContentManagement;
        private readonly IGarageClient _garageClient;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSession;
        public BusinessSettingController(IGarageBusinessSettingClient businessSetting, IMapper mapper, IUserSessionManager userSession,
            IGarageBranchBusinessSettingClient branchBusinessSetting, IGarageClient garageClient, IGarageContentManagementClient garageContentManagement)
        {
            _businessSetting = businessSetting;
            _garageClient = garageClient;
            _branchBusinessSetting = branchBusinessSetting;
            _mapper = mapper;
            _garageClient = garageClient;
            _userSession = userSession;
            _garageContentManagement = garageContentManagement;
        }
        public async Task<IActionResult> Index()
        {
            long garageId = _userSession.GetGarageStore().Id;
            var info = _mapper.Map<IEnumerable<GarageBusinessSettingViewModel>>(await _businessSetting.GetBusinessSettings(garageId));
            if (info.Count() == 0)
            {
                GarageBusinessSettingViewModel setting = new GarageBusinessSettingViewModel();
                return View(setting);
            }

            IEnumerable<GarageContentManagementDTO> Content = await _garageContentManagement.GetAllByGarageIdAsync(garageId);

            if (Content.Any())
            {
                info.FirstOrDefault().ContentManagementId = Content.FirstOrDefault().Id;
                info.FirstOrDefault().ShortIntro = Content.FirstOrDefault().ShortIntro;
            }

            return View(info.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Update(long? Id, GarageBusinessSettingViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    long garageId = _userSession.GetGarageStore().Id;
                    GarageViewModel garages = _mapper.Map<GarageViewModel>(await _garageClient.GetGarageByID(garageId));
                    if (Id.HasValue && Id > 0)
                    {

                        GarageContentManagementViewModel content = new();
                        IEnumerable<GarageContentManagementViewModel> contentManagement = _mapper.Map<IEnumerable<GarageContentManagementViewModel>>(await _garageContentManagement.GetAllByIdAsync(model.ContentManagementId));
                        var contents = contentManagement.FirstOrDefault();
                        if (model.ContentManagementId != 0)
                        {
                            content.Id = model.ContentManagementId;
                            content.GarageId = garageId;
                            content.ShortIntro = model.ShortIntro;
                            content.PromoSection01Count = contents.PromoSection01Count;
                            content.PromoSection02Count = contents.PromoSection02Count;
                            content.PromoSection03Count = contents.PromoSection03Count;
                            content.Garage = null;
                            await _garageContentManagement.UpdateGarageContentManagementAsync(_mapper.Map<GarageContentManagementDTO>(content));
                        }
                        else
                        {
                            content.ShortIntro = model.ShortIntro;
                            content.GarageId = garageId;

                            await _garageContentManagement.AddGarageContentManagementAsync(_mapper.Map<GarageContentManagementDTO>(content));
                        }

                        model.GarageId = garageId;
                        GarageBusinessSettingDTO businessSetting = _mapper.Map<GarageBusinessSettingDTO>(model);
                        businessSetting.Garage = null;
                        GarageBusinessSettingDTO Result = await _businessSetting.Update(_mapper.Map<GarageBusinessSettingDTO>(businessSetting));

                        Result.Id = model.Id;

                        return Json(new
                        {
                            success = true,
                            message = "Record Updated Successfully",
                            data = new
                            {
                                ID = Result.Id,
                                Title = Result.Title,
                                FirstMessage = Result.FirstMessage,
                                StreetAddress = Result.StreetAddress,
                                CompleteAddress = Result.CompleteAddress,
                                Latitude = Result.Latitude,
                                Longitude = Result.Longitude,
                                ContactPersonName = Result.ContactPersonName,
                                Contact01 = Result.Contact01,
                                Contact02 = Result.Contact02,
                                Email = Result.Email,
                                Fax = Result.Fax,
                                Facebook = Result.Facebook,
                                Whatsapp = Result.Whatsapp,
                                Instagram = Result.Instagram,
                                Youtube = Result.Youtube,
                                Twitter = Result.Twitter,
                                Snapchat = Result.Snapchat
                            }
                        });
                    }
                    else
                    {

                        model.GarageId = garageId;
                        GarageBusinessSettingDTO Result = await _businessSetting.Create(_mapper.Map<GarageBusinessSettingDTO>(model));

                        return Json(new
                        {
                            success = true,
                            message = "Record Updated Successfully",
                            data = new
                            {
                                ID = Result.Id,
                                Title = Result.Title,
                                FirstMessage = Result.FirstMessage,
                                StreetAddress = Result.StreetAddress,
                                CompleteAddress = Result.CompleteAddress,
                                Latitude = Result.Latitude,
                                Longitude = Result.Longitude,
                                ContactPersonName = Result.ContactPersonName,
                                Contact01 = Result.Contact01,
                                Contact02 = Result.Contact02,
                                Email = Result.Email,
                                Fax = Result.Fax,
                                Facebook = Result.Facebook,
                                Whatsapp = Result.Whatsapp,
                                Instagram = Result.Instagram,
                                Youtube = Result.Youtube,
                                Twitter = Result.Twitter,
                                Snapchat = Result.Snapchat
                            }
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

            }

            return Json(new
            {
                success = false,
                message = "Fill all required fields and submit the form again"
            });

        }

        #region Branch Setting

        public async Task<IActionResult> GetBranchSetting(long id)
        {

            ViewBag.BusinessSettingID = id;
            var info = _mapper.Map<IEnumerable<GarageBranchBusinessSettingViewModel>>(await _branchBusinessSetting.GetBusinessSettings(id));

            return PartialView(info);
        }

        [HttpPost]
        public async Task<IActionResult> BranchSettings(GarageBranchBusinessSettingViewModel model)
        {
            var result = new GarageBranchBusinessSettingDTO();

            if (model.Id > 0)
            {
                result = await _branchBusinessSetting.Update(_mapper.Map<GarageBranchBusinessSettingDTO>(model));

            }
            else
            {
                result = await _branchBusinessSetting.Create(_mapper.Map<GarageBranchBusinessSettingDTO>(model));

            }

            return Json(new
            {
                success = true,
                message = "Branch setting added successfully !",
                data = new
                {
                    Id = result.Id,
                    Name = result.Name,
                    BusinessSettingId = result.GarageBusinessSettingId,
                    Contact1 = result.Contact1,
                    Contact2 = result.Contact2,
                    ContactPersonName = result.ContactPersonName,
                    Address = result.Address,


                },

            });

        }

        public IActionResult DeleteBranchSetting(long id)
        {

            var result = _branchBusinessSetting.ArchiveBranchBusinessSetting(id);
            return Json(new
            {

                success = true,
                message = "Branch Deleted Successfully!"
            });

        }

        #endregion

    }
}
