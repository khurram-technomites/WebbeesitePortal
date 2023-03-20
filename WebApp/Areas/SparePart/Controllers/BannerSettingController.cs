using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.SparePartCMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;
using WebApp.ViewModels.SparePart;

namespace WebApp.Areas.SparePart.Controllers
{
    [Area("SparePart")]
    [Authorize(Roles = "SparePartDealer")]
    public class BannerSettingController : Controller
    {
        private readonly ISparePartBannerSettingClient _client;
        private readonly IUserSessionManager _userSessionManager;
        private readonly IMapper _mapper;
        private readonly IFileUpload _fileUpload;

        public BannerSettingController(ISparePartBannerSettingClient client, IUserSessionManager userSessionManager, IMapper mapper, IFileUpload fileUpload)
        {
            _client = client;
            _userSessionManager = userSessionManager;
            _mapper = mapper;
            _fileUpload = fileUpload;
        }
        public async Task<IActionResult> Index()
        {
            long SparePartId = _userSessionManager.GetSparePartDealerStore().Id;
            IEnumerable<SparePartBannerSettingViewModel> mainBanner = _mapper.Map<IEnumerable<SparePartBannerSettingViewModel>>(await _client.GetAllBySparePartDealerIdAsync(SparePartId));

            SparePartBannersViewModel result = new();

            result.Banners = mainBanner.Where(x => x.BannerType == Enum.GetName(typeof(BannerType), BannerType.Banner)).ToList();
            result.PromoBanners = mainBanner.Where(x => x.BannerType == Enum.GetName(typeof(BannerType), BannerType.PromotionBanner)).ToList();

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string image, string Title, string Description, string Tag)
        {
            string message = string.Empty;
            if (image != null && image.Length > 0)
                try
                {
                    SparePartBannerSettingViewModel objBannerImage = new SparePartBannerSettingViewModel();
                    objBannerImage.ImagePath = image;
                    objBannerImage.Title = Title;
                    objBannerImage.Description = Description;
                    objBannerImage.Tag = Tag;
                    objBannerImage.SparePartDealerId = _userSessionManager.GetSparePartDealerStore().Id;
                    objBannerImage.BannerType = Enum.GetName(typeof(BannerType), BannerType.Banner);

                    await _client.AddSparePartBannerSettingAsync(_mapper.Map<SparePartBannerSettingDTO>(objBannerImage));
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
            SparePartBannerSettingViewModel model = _mapper.Map<SparePartBannerSettingViewModel>(await _client.GetAllByIdAsync(id));
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(long id, string Title, string Description, string Tag)
        {
            try
            {

                SparePartBannerSettingViewModel model = _mapper.Map<SparePartBannerSettingViewModel>(await _client.GetAllByIdAsync(id));

                model.Title = Title;
                model.Description = Description;
                model.Tag = Tag;

                SparePartBannerSettingDTO result = await _client.UpdateSparePartBannerSettingAsync(_mapper.Map<SparePartBannerSettingDTO>(model));
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
                await _client.DeleteSparePartBannerSettingAsync(id);

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
        public async Task<IActionResult> PromotionCreate(string image, string Title, string Description, string Tag)
        {
            string message = string.Empty;
            if (image != null && image.Length > 0)
                try
                {
                    SparePartBannerSettingViewModel objBannerImage = new SparePartBannerSettingViewModel();
                    objBannerImage.ImagePath = image;
                    objBannerImage.Title = Title;
                    objBannerImage.Description = Description;
                    objBannerImage.Tag = Tag;
                    objBannerImage.SparePartDealerId = _userSessionManager.GetSparePartDealerStore().Id;
                    objBannerImage.BannerType = Enum.GetName(typeof(BannerType), BannerType.PromotionBanner);

                    await _client.AddSparePartBannerSettingAsync(_mapper.Map<SparePartBannerSettingDTO>(objBannerImage));
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
            SparePartBannerSettingViewModel model = _mapper.Map<SparePartBannerSettingViewModel>(await _client.GetAllByIdAsync(id));
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> PromotionEdit(long id, string Title, string Description, string Tag)
        {
            try
            {

                SparePartBannerSettingViewModel model = _mapper.Map<SparePartBannerSettingViewModel>(await _client.GetAllByIdAsync(id));

                model.Title = Title;
                model.Description = Description;
                model.Tag = Tag;

                SparePartBannerSettingDTO result = await _client.UpdateSparePartBannerSettingAsync(_mapper.Map<SparePartBannerSettingDTO>(model));
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
