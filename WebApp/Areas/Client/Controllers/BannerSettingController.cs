using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;
using WebApp.ViewModels.Garage;

namespace WebApp.Areas.Garage.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "GarageOwner")]
    public class BannerSettingController : Controller
    {
        private readonly IGarageBannerSettingClient _client;
        private readonly IUserSessionManager _userSessionManager;
        private readonly IMapper _mapper;
        private readonly IFileUpload _fileUpload;

        public BannerSettingController(IGarageBannerSettingClient client, IUserSessionManager userSessionManager, IMapper mapper, IFileUpload fileUpload)
        {
            _client = client;
            _userSessionManager = userSessionManager;
            _mapper = mapper;
            _fileUpload = fileUpload;
        }
        public async Task<IActionResult> Index()
        {
            long GarageId = _userSessionManager.GetGarageStore().Id;
            IEnumerable<GarageBannerSettingViewModel> mainBanner = _mapper.Map<IEnumerable<GarageBannerSettingViewModel>>(await _client.GetAllByGarageIdAsync(GarageId));

            GarageBannersViewModel result = new();

            result.Banners = mainBanner.Where(x => x.BannerType == Enum.GetName(typeof(BannerType), BannerType.Banner)).ToList();
            result.PromoBanners = mainBanner.Where(x => x.BannerType == Enum.GetName(typeof(BannerType), BannerType.PromotionBanner)).OrderByDescending(x=>x.Id).FirstOrDefault();

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string image, string Title, string Description, string Tag)
        {
            string message = string.Empty;
            if (image != null && image.Length > 0)
                try
                {
                    GarageBannerSettingViewModel objBannerImage = new GarageBannerSettingViewModel();
                    objBannerImage.ImagePath = image;
                    objBannerImage.Title = Title;
                    objBannerImage.Description = Description;
                    objBannerImage.Tag = Tag;
                    objBannerImage.GarageId = _userSessionManager.GetGarageStore().Id;
                    objBannerImage.BannerType = Enum.GetName(typeof(BannerType), BannerType.Banner);

                    await _client.AddGarageBannerSettingAsync(_mapper.Map<GarageBannerSettingDTO>(objBannerImage));
                    ViewBag.Message = "File uploaded successfully";
                    TempData["SuccessMessage"] = "File uploaded successfully";
                }
                catch (ApiException ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    TempData["ErrorMessage"] = "ERROR:" + ex.Message.ToString();
                }
            return RedirectPermanent("Index");
        }

        public async Task<ActionResult> Edit(long id)
        {
            GarageBannerSettingViewModel model = _mapper.Map<GarageBannerSettingViewModel>(await _client.GetAllByIdAsync(id));
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(long id, string Title, string Description, string Tag)
        {
            try
            {

                GarageBannerSettingViewModel model = _mapper.Map<GarageBannerSettingViewModel>(await _client.GetAllByIdAsync(id));

                model.Title = Title;
                model.Description = Description;
                model.Tag = Tag;

                GarageBannerSettingDTO result = await _client.UpdateGarageBannerSettingAsync(_mapper.Map<GarageBannerSettingDTO>(model));
                return Json(new
                {
                    success = true,
                    message = "Banner Details updated successfully!",
                });

            }
            catch (ApiException ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Oops! Something went wrong. Please try later."
                });
            }
        }

        public async Task<ActionResult> Delete(long id)
        {

            try
            {
                await _client.DeleteGarageBannerSettingAsync(id);

                return Json(new
                {
                    success = true,
                    message = "Record successfully deleted"
                });
            }
            catch (ApiException ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Oops! Something went wrong. Please try later."
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PromotionCreate(int Id,string Video, string Thumbnail, string Title, string Description, string Tag)
        {
            string message = string.Empty;
            if (Thumbnail != null && Thumbnail.Length > 0)
                try
                {
                    GarageBannerSettingViewModel objBannerImage = new GarageBannerSettingViewModel();
                    objBannerImage.Id = Id;
                    objBannerImage.ImagePath = Video;
                    objBannerImage.Thumbnail = Thumbnail;
                    objBannerImage.Title = Title;
                    objBannerImage.Description = Description;
                    objBannerImage.Tag = Tag;
                    objBannerImage.GarageId = _userSessionManager.GetGarageStore().Id;
                    objBannerImage.BannerType = Enum.GetName(typeof(BannerType), BannerType.PromotionBanner);
                    if(Id != 0)
                    {
                        await _client.UpdateGarageBannerSettingAsync(_mapper.Map<GarageBannerSettingDTO>(objBannerImage));
                    }
                    else
                    {
                        await _client.AddGarageBannerSettingAsync(_mapper.Map<GarageBannerSettingDTO>(objBannerImage));
                    }
                   
                    ViewBag.Message = "File uploaded successfully";
                    TempData["SuccessMessage"] = "File uploaded successfully";
                }
                catch (ApiException ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    TempData["ErrorMessage"] = "ERROR:" + ex.Message.ToString();
                }
            return RedirectPermanent("Index");
        }

        public async Task<ActionResult> PromotionEdit(long id)
        {
            GarageBannerSettingViewModel model = _mapper.Map<GarageBannerSettingViewModel>(await _client.GetAllByIdAsync(id));
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> PromotionEdit(long id, string Title, string Description, string Tag)
        {
            try
            {

                GarageBannerSettingViewModel model = _mapper.Map<GarageBannerSettingViewModel>(await _client.GetAllByIdAsync(id));

                model.Title = Title;
                model.Description = Description;
                model.Tag = Tag;

                GarageBannerSettingDTO result = await _client.UpdateGarageBannerSettingAsync(_mapper.Map<GarageBannerSettingDTO>(model));
                return Json(new
                {
                    success = true,
                    message = "Banner Details updated successfully!",
                });

            }
            catch (ApiException ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Oops! Something went wrong. Please try later."
                });
            }
        }

    }
}
