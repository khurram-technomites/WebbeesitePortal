using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using HelperClasses.DTOs.SparePartsDealer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.SparePart.Controllers
{
    [Area("SparePart")]
    public class SparePartBusinessSettingController : Controller
    {
        private readonly ISparePartBusinessSettingClient _businessSetting;
        private readonly ISparePartBranchBusinessSettingClient _branchBusinessSetting;
        private readonly ISparePartContentManagementClient _sparePartContentManagement;
        private readonly ISparePartsDealerClient _SparePartClient;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSession;
        public SparePartBusinessSettingController(ISparePartBusinessSettingClient businessSetting, IMapper mapper, IUserSessionManager userSession,
            ISparePartBranchBusinessSettingClient branchBusinessSetting, ISparePartsDealerClient SparePartClient , ISparePartContentManagementClient sparePartContentManagement)
        {
            _businessSetting = businessSetting;
            _sparePartContentManagement = sparePartContentManagement;
            _SparePartClient = SparePartClient;
            _branchBusinessSetting = branchBusinessSetting;
            _mapper = mapper;
            _SparePartClient = SparePartClient;
            _userSession = userSession;

        }
        public async Task<IActionResult> Index()
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;
            var info = _mapper.Map<IEnumerable<SparePartBusinessSettingViewModel>>(await _businessSetting.GetBusinessSettings(SparePartId));
            if (info.Count() == 0)
            {
                SparePartBusinessSettingViewModel setting = new SparePartBusinessSettingViewModel();
                return View(setting);
            }

            IEnumerable<SparePartContentManagementDTO> Content = await _sparePartContentManagement.GetAllBySparePartDealerIdAsync(SparePartId);

            if (Content.Any())
            {
                info.FirstOrDefault().ContentManagementId = Content.FirstOrDefault().Id;
                info.FirstOrDefault().ShortIntro = Content.FirstOrDefault().ShortIntro;
            }
            return View(info.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Update(long? Id, SparePartBusinessSettingViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    long SparePartId = _userSession.GetSparePartDealerStore().Id;
                    if (Id.HasValue && Id > 0)
                    {

                        SparePartsDealerDTO SparePart = new SparePartsDealerDTO();
                        SparePart.Id = SparePartId;
                        SparePart.CompleteAddress = model.CompleteAddress;
                        SparePart.ContactPersonNumber = model.Contact01;
                        SparePart.ContactPersonNumber01 = model.Contact02;
                        SparePart.ContactPersonEmail = model.Email;
                        SparePart.Latitude = model.Latitude;
                        SparePart.Longitude = model.Longitude;


                        await _SparePartClient.UpdateSparePartsDealer(SparePart);
                         SparePartContentManagementViewModel content = new();

                        if (model.ContentManagementId != 0)
                        {
                            content.Id = model.ContentManagementId;
                            content.SparePartDealerId = SparePartId;
                            content.ShortIntro = model.ShortIntro;

                            await _sparePartContentManagement.UpdateSparePartContentManagementAsync(_mapper.Map<SparePartContentManagementDTO>(content));
                        }
                        else
                        {
                            content.ShortIntro = model.ShortIntro;
                            content.SparePartDealerId = SparePartId;

                            await _sparePartContentManagement.AddSparePartContentManagementAsync(_mapper.Map<SparePartContentManagementDTO>(content));
                        }

                        model.SparePartId = SparePartId;
                        SparePartBusinessSettingDTO Result = await _businessSetting.Update(_mapper.Map<SparePartBusinessSettingDTO>(model));

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

                        model.SparePartId = SparePartId;
                        SparePartBusinessSettingDTO Result = await _businessSetting.Create(_mapper.Map<SparePartBusinessSettingDTO>(model));

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
            var info = _mapper.Map<IEnumerable<SparePartBranchBusinessSettingViewModel>>(await _branchBusinessSetting.GetBusinessSettings(id));

            return PartialView(info);
        }

        [HttpPost]
        public async Task<IActionResult> BranchSettings(SparePartBranchBusinessSettingViewModel model)
        {
            var result = new SparePartBranchBusinessSettingDTO();

            if (model.Id > 0)
            {
                result = await _branchBusinessSetting.Update(_mapper.Map<SparePartBranchBusinessSettingDTO>(model));

            }
            else
            {
                result = await _branchBusinessSetting.Create(_mapper.Map<SparePartBranchBusinessSettingDTO>(model));

            }

            return Json(new
            {
                success = true,
                message = "Branch setting added successfully !",
                data = new
                {
                    Id = result.Id,
                    Name = result.Name,
                    BusinessSettingId = result.SparePartBusinessSettingId,
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
