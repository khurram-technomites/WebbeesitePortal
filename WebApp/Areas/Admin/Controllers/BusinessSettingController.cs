using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;
using WebApp.Interfaces.TypedClients;
using HelperClasses.DTOs;
using WebApp.ErrorHandling;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BusinessSettingController : Controller
    {
        private readonly IBusinessSettingClient _businessSetting;
        private readonly IMapper _mapper;
        public BusinessSettingController(IBusinessSettingClient businessSetting, IMapper mapper)
        {
            _businessSetting = businessSetting;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var info = _mapper.Map<IEnumerable<BusinessSettingViewModel>>(await _businessSetting.GetBusinessSettings());
            if (info.Count() == 0)
            {
                BusinessSettingViewModel setting = new BusinessSettingViewModel();
                return View(setting);
            }
            return View(info.FirstOrDefault());
        }
        [HttpPost]
        public async Task<IActionResult> Update(long? Id, BusinessSettingViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Id.HasValue && Id > 0)
                    {
                        BusinessSettingDTO Result = await _businessSetting.Update(_mapper.Map<BusinessSettingDTO>(model));

                        Result.Id = model.Id;

                        return Json(new
                        {
                            success = true,
                            message = "Record Updated Successfully",
                            data = new
                            {
                                ID = Result.Id,
                                Title = Result.Title,
                                TitleAr = Result.TitleAr,
                                WhatsApp = Result.WhatsApp,
                                FirstMessage = Result.FirstMessage,
                                FirstMessageAr = Result.FirstMessageAr,
                                MapIframe = Result.MapIframe,
                                StreetAddress = Result.StreetAddress,
                                StreetAddressAr = Result.StreetAddressAr,
                                OpeningHours = Result.WorkingDays,
                                PhoneCode = Result.PhoneCode,
                                PhoneCode2 = Result.PhoneCode2,
                                Contact = Result.Contact,
                                Contact2 = Result.Contact2,
                                Fax = Result.Fax,
                                Email = Result.Email,
                                Email2 = Result.Email2,
                                Facebook = Result.Facebook,
                                Instagram = Result.Instagram,
                                Youtube = Result.Youtube,
                                Twitter = Result.Twitter,
                                Snapchat = Result.Snapchat
                            }
                        });
                    }
                    else
                    {
                        BusinessSettingDTO Result = await _businessSetting.Create(_mapper.Map<BusinessSettingDTO>(model));

                        return Json(new
                        {
                            success = true,
                            message = "Record Updated Successfully",
                            data = new
                            {
                                ID = Result.Id,
                                Title = Result.Title,
                                TitleAr = Result.TitleAr,
                                WhatsApp = Result.WhatsApp,
                                FirstMessage = Result.FirstMessage,
                                FirstMessageAr = Result.FirstMessageAr,
                                MapIframe = Result.MapIframe,
                                StreetAddress = Result.StreetAddress,
                                StreetAddressAr = Result.StreetAddressAr,
                                OpeningHours = Result.WorkingDays,
                                PhoneCode = Result.PhoneCode,
                                PhoneCode2 = Result.PhoneCode2,
                                Contact = Result.Contact,
                                Contact2 = Result.Contact2,
                                Fax = Result.Fax,
                                Email = Result.Email,
                                Email2 = Result.Email2,
                                Facebook = Result.Facebook,
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
    }
}
