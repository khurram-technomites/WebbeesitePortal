using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.SparePart.Controllers
{
    [Area("SparePart")]
    [Authorize(Roles = "SparePartDealer")]
    public class ContentManagementController : Controller
    {
        private readonly ISparePartsDealerClient _SparePartClient;
        private readonly ISparePartContentManagementClient _SparePartContentManagementClient;
        private readonly IUserSessionManager _userSession;
        private readonly IMapper _mapper;

        public ContentManagementController(ISparePartsDealerClient SparePartClient, ISparePartContentManagementClient SparePartContentManagementClient, IMapper mapper, IUserSessionManager userSession)
        {
            _SparePartClient = SparePartClient;
            _SparePartContentManagementClient = SparePartContentManagementClient;
            _mapper = mapper;
            _userSession = userSession;
        }

        public async Task<IActionResult> Index()
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;
            IEnumerable<SparePartContentManagementViewModel> result = _mapper.Map<IEnumerable<SparePartContentManagementViewModel>>(await _SparePartContentManagementClient.GetAllBySparePartDealerIdAsync(SparePartId));

            if (result.Any())
                return View(result.FirstOrDefault());
            else
            {
                var model = new SparePartContentManagementViewModel();

                SparePartsDealerViewModel SparePart = _mapper.Map<SparePartsDealerViewModel>(await _SparePartClient.GetSparePartsDealerByID(SparePartId));
                model.SparePartDealer = SparePart;
                return View(model);
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> UpdateProfile(string filePath)
        //{
        //    long SparePartId = _userSession.GetSparePartDealerStore().Id;

        //    await _SparePartClient.Update(SparePartId, filePath);

        //    return Json(new
        //    {
        //        success = true,
        //        message = "Logo Updated Successfully!"
        //    });
        //}

        [HttpPost]
        public async Task<IActionResult> AboutUsImage(string filePath)
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;

            IEnumerable<SparePartContentManagementDTO> content = await _SparePartContentManagementClient.GetAllBySparePartDealerIdAsync(SparePartId);

            if (content.Any())
            {
                content.FirstOrDefault().AboutUsImage = filePath;

                var result = await _SparePartContentManagementClient.UpdateSparePartContentManagementAsync(_mapper.Map<SparePartContentManagementDTO>(content.FirstOrDefault()));

                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully!",
                    data = result.Id
                });
            }
            else
            {
                SparePartContentManagementViewModel PostContent = new()
                {
                    AboutUsImage = filePath,
                    SparePartDealerId = SparePartId
                };

                var result = await _SparePartContentManagementClient.AddSparePartContentManagementAsync(_mapper.Map<SparePartContentManagementDTO>(PostContent));

                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully!",
                    data = result.Id
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> FooterImage(string filePath)
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;

            IEnumerable<SparePartContentManagementDTO> content = await _SparePartContentManagementClient.GetAllBySparePartDealerIdAsync(SparePartId);

            if (content.Any())
            {
                content.FirstOrDefault().FooterImage = filePath;

                var result = await _SparePartContentManagementClient.UpdateSparePartContentManagementAsync(_mapper.Map<SparePartContentManagementDTO>(content.FirstOrDefault()));

                return Json(new
                {
                    success = true,
                    message = "Footer Updated Successfully!",
                    data = result.Id
                });
            }
            else
            {
                SparePartContentManagementViewModel PostContent = new()
                {
                    AboutUsImage = filePath,
                    SparePartDealerId = SparePartId
                };

                var result = await _SparePartContentManagementClient.AddSparePartContentManagementAsync(_mapper.Map<SparePartContentManagementDTO>(PostContent));

                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully!",
                    data = result.Id
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CEOImage(string filePath)
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;

            IEnumerable<SparePartContentManagementDTO> content = await _SparePartContentManagementClient.GetAllBySparePartDealerIdAsync(SparePartId);

            if (content.Any())
            {
                content.FirstOrDefault().CEOImagePath = filePath;

                var result = await _SparePartContentManagementClient.UpdateSparePartContentManagementAsync(_mapper.Map<SparePartContentManagementDTO>(content.FirstOrDefault()));

                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully!",
                    data = result.Id
                });
            }
            else
            {
                SparePartContentManagementViewModel PostContent = new()
                {
                    CEOImagePath = filePath,
                    SparePartDealerId = SparePartId
                };

                var result = await _SparePartContentManagementClient.AddSparePartContentManagementAsync(_mapper.Map<SparePartContentManagementDTO>(PostContent));

                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully!",
                    data = result.Id
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateThemeColor(string ThemeColor)
        {
            long GarageId = _userSession.GetGarageStore().Id;

            await _SparePartClient.UpdateTheme(GarageId, ThemeColor);

            return Json(new
            {
                success = true,
                message = "Theme Color Updated Successfully!"
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SparePartContentManagementViewModel Model)
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;
            Model.SparePartDealerId = SparePartId;

            if (Model.Id == 0)
            {
                var result = await _SparePartContentManagementClient.AddSparePartContentManagementAsync(_mapper.Map<SparePartContentManagementDTO>(Model));

                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully!",
                    data = result.Id
                });
            }
            else
            {
                var result = await _SparePartContentManagementClient.UpdateSparePartContentManagementAsync(_mapper.Map<SparePartContentManagementDTO>(Model));

                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully!",
                    data = result.Id
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ThumbnailImage(string filePath)
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;
            string result = await _SparePartClient.UpdateThumbnail(SparePartId, filePath);

            return Json(new
            {
                success = true,
                message = "Thumbnail updated successfully!",
                filepath = result
            });
        }

        [HttpPost]
        public async Task<IActionResult> SecondaryLogo(string filePath)
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;
            string result = await _SparePartClient.UpdateSecondaryLogo(SparePartId, filePath);

            return Json(new
            {
                success = true,
                message = "Logo updated successfully!",
                filepath = result
            });
        }
    }
}
