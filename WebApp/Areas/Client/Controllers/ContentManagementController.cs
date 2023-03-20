using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Garage.Controllers
{
    [Area("Client")]
    public class ContentManagementController : Controller
    {
        private readonly IGarageClient _garageClient;
        private readonly IGarageContentManagementClient _garageContentManagementClient;
        private readonly IUserSessionManager _userSession;
        private readonly IMapper _mapper;

        public ContentManagementController(IGarageClient garageClient, IGarageContentManagementClient garageContentManagementClient, IMapper mapper, IUserSessionManager userSession)
        {
            _garageClient = garageClient;
            _garageContentManagementClient = garageContentManagementClient;
            _mapper = mapper;
            _userSession = userSession;
        }

        public async Task<IActionResult> Index()
        {
            long GarageId = _userSession.GetGarageStore().Id;
            IEnumerable<GarageContentManagementViewModel> result = _mapper.Map<IEnumerable<GarageContentManagementViewModel>>(await _garageContentManagementClient.GetAllByGarageIdAsync(GarageId));

            if (result.Any())
                return View(result.FirstOrDefault());
            else
            {
                var model = new GarageContentManagementViewModel();

                GarageViewModel garage = _mapper.Map<GarageViewModel>(await _garageClient.GetGarageByID(GarageId));
                model.Garage = garage;
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(string filePath)
        {
            long GarageId = _userSession.GetGarageStore().Id;

            await _garageClient.UpdateProfilePicture(GarageId, filePath);

            return Json(new
            {
                success = true,
                message = "Logo Updated Successfully!"
            });
        }

        [HttpPost]
        public async Task<IActionResult> AboutUsImage(string filePath)
        {
            long GarageId = _userSession.GetGarageStore().Id;

            IEnumerable<GarageContentManagementDTO> content = await _garageContentManagementClient.GetAllByGarageIdAsync(GarageId);

            if (content.Any())
            {
                content.FirstOrDefault().AboutUsImage = filePath;

                var result = await _garageContentManagementClient.UpdateGarageContentManagementAsync(_mapper.Map<GarageContentManagementDTO>(content.FirstOrDefault()));

                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully!",
                    data = result.Id
                });
            }
            else
            {
                GarageContentManagementViewModel PostContent = new()
                {
                    AboutUsImage = filePath,
                    GarageId = GarageId
                };

                var result = await _garageContentManagementClient.AddGarageContentManagementAsync(_mapper.Map<GarageContentManagementDTO>(PostContent));

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
            long GarageId = _userSession.GetGarageStore().Id;

            IEnumerable<GarageContentManagementDTO> content = await _garageContentManagementClient.GetAllByGarageIdAsync(GarageId);

            if (content.Any())
            {
                content.FirstOrDefault().FooterImage = filePath;

                var result = await _garageContentManagementClient.UpdateGarageContentManagementAsync(_mapper.Map<GarageContentManagementDTO>(content.FirstOrDefault()));

                return Json(new
                {
                    success = true,
                    message = "Footer Updated Successfully!",
                    data = result.Id
                });
            }
            else
            {
                GarageContentManagementViewModel PostContent = new()
                {
                    AboutUsImage = filePath,
                    GarageId = GarageId
                };

                var result = await _garageContentManagementClient.AddGarageContentManagementAsync(_mapper.Map<GarageContentManagementDTO>(PostContent));

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
            long GarageId = _userSession.GetGarageStore().Id;

            IEnumerable<GarageContentManagementDTO> content = await _garageContentManagementClient.GetAllByGarageIdAsync(GarageId);

            if (content.Any())
            {
                content.FirstOrDefault().CEOImagePath = filePath;

                var result = await _garageContentManagementClient.UpdateGarageContentManagementAsync(_mapper.Map<GarageContentManagementDTO>(content.FirstOrDefault()));

                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully!",
                    data = result.Id
                });
            }
            else
            {
                GarageContentManagementViewModel PostContent = new()
                {
                    CEOImagePath = filePath,
                    GarageId = GarageId
                };

                var result = await _garageContentManagementClient.AddGarageContentManagementAsync(_mapper.Map<GarageContentManagementDTO>(PostContent));

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

            await _garageClient.UpdateTheme(GarageId, ThemeColor);

            return Json(new
            {
                success = true,
                message = "Theme Color Updated Successfully!"
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GarageContentManagementViewModel Model)
        {
            long GarageId = _userSession.GetGarageStore().Id;
            Model.GarageId = GarageId;

            if (Model.Id == 0)
            {
                var result = await _garageContentManagementClient.AddGarageContentManagementAsync(_mapper.Map<GarageContentManagementDTO>(Model));

                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully!",
                    data = result.Id
                });
            }
            else
            {
                var result = await _garageContentManagementClient.UpdateGarageContentManagementAsync(_mapper.Map<GarageContentManagementDTO>(Model));

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
            long GarageId = _userSession.GetGarageStore().Id;
            string result = await _garageClient.UpdateThumbnail(GarageId, filePath);

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
            long GarageId = _userSession.GetGarageStore().Id;
            string result = await _garageClient.UpdateSecondaryLogo(GarageId, filePath);

            return Json(new
            {
                success = true,
                message = "Logo updated successfully!",
                filepath = result
            });
        }
        [HttpPost]
        public async Task<IActionResult> InnerPagesBanner(string filePath)
        {
            long GarageId = _userSession.GetGarageStore().Id;

            IEnumerable<GarageContentManagementDTO> content = await _garageContentManagementClient.GetAllByGarageIdAsync(GarageId);

            if (content.Any())
            {
                content.FirstOrDefault().InnerPagesBanner = filePath;

                var result = await _garageContentManagementClient.UpdateGarageContentManagementAsync(_mapper.Map<GarageContentManagementDTO>(content.FirstOrDefault()));

                return Json(new
                {
                    success = true,
                    message = "Inner Banner Updated Successfully!",
                    data = result.Id
                });
            }
            else
            {
                GarageContentManagementViewModel PostContent = new()
                {
                    AboutUsImage = filePath,
                    GarageId = GarageId
                };

                var result = await _garageContentManagementClient.AddGarageContentManagementAsync(_mapper.Map<GarageContentManagementDTO>(PostContent));

                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully!",
                    data = result.Id
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Favicon(string filePath)
        {
            long GarageId = _userSession.GetGarageStore().Id;

            IEnumerable<GarageContentManagementDTO> content = await _garageContentManagementClient.GetAllByGarageIdAsync(GarageId);

            if (content.Any())
            {
                content.FirstOrDefault().Favicon = filePath;

                var result = await _garageContentManagementClient.UpdateGarageContentManagementAsync(_mapper.Map<GarageContentManagementDTO>(content.FirstOrDefault()));

                return Json(new
                {
                    success = true,
                    message = "Favicon Updated Successfully!",
                    data = result.Id
                });
            }
            else
            {
                GarageContentManagementViewModel PostContent = new()
                {
                    Favicon = filePath,
                    GarageId = GarageId
                };

                var result = await _garageContentManagementClient.AddGarageContentManagementAsync(_mapper.Map<GarageContentManagementDTO>(PostContent));

                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully!",
                    data = result.Id
                });
            }
        }
    }
}
