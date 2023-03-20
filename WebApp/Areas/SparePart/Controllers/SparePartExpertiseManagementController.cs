using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class SparePartExpertiseManagementController : Controller
    {
        private readonly ISparePartExpertiseManagementClient _client;
        private readonly ISparePartExpertiseClient _SparePartExpertiseClient;
        private readonly IExpertiseClient _expertiseClient;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSessionManager;

        public SparePartExpertiseManagementController(
              ISparePartExpertiseManagementClient client
            , ISparePartExpertiseClient SparePartExpertiseClient
            , IExpertiseClient expertiseClient
            , IMapper mapper
            , IUserSessionManager userSessionManager)
        {
            _client = client;
            _SparePartExpertiseClient = SparePartExpertiseClient;
            _expertiseClient = expertiseClient;
            _mapper = mapper;
            _userSessionManager = userSessionManager;

        }
        public async Task<IActionResult> Index()
        {
            long SparePartId = _userSessionManager.GetSparePartDealerStore().Id;
            var expertise = _mapper.Map<IEnumerable<SparePartExpertiseManagementViewModel>>(await _client.GetAllBySparePartDealerIdAsync(SparePartId));
            return View(expertise);
        }

        public async Task<IActionResult> Details(long Id)
        {
            IEnumerable<SparePartExpertiseManagementViewModel> SparePartManagements = _mapper.Map<IEnumerable<SparePartExpertiseManagementViewModel>>(await _client.GetAllByIdAsync(Id));
            return View(SparePartManagements.FirstOrDefault());
        }
        public ActionResult Create()
        {
            SparePartExpertiseManagementViewModel expertise = new SparePartExpertiseManagementViewModel();
            return View(expertise);
        }

        [HttpPost]
        public async Task<ActionResult> Create(SparePartExpertiseManagementViewModel model)
        {

            model.CreationDate = DateTime.UtcNow;
            string message = string.Empty;
            long SparePartId = _userSessionManager.GetSparePartDealerStore().Id;
            model.SparePartDealerId = SparePartId;
            if (ModelState.IsValid)
            {
                try
                {

                    SparePartExpertiseManagementDTO Result = await _client.AddSparePartExpertiseManagementAsync(_mapper.Map<SparePartExpertiseManagementDTO>(model));

                    return Json(new
                    {
                        success = true,
                        url = "/SparePart/SparePartExpertiseManagement/Index",
                        message = "Expertise Created Successfully",
                        data = new
                        {
                            Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            Description = Result.Description,
                            Id = Result.Id
                        }
                    });
                }
                catch (ApiException ex)
                {
                    ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                    return Json(new
                    {
                        url = "/SparePart/SparePartExpertiseManagement/Index",
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

        [HttpPost]
        public async Task<IActionResult> Edit(SparePartExpertiseManagementViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    long SparePartId = _userSessionManager.GetSparePartDealerStore().Id;
                    model.SparePartDealerId = SparePartId;
                    SparePartExpertiseManagementDTO Result = await _client.UpdateSparePartExpertiseManagementAsync(_mapper.Map<SparePartExpertiseManagementDTO>(model));

                    Result.Id = model.Id;

                    return Json(new
                    {
                        success = true,
                        url = "/SparePart/SparePartExpertiseManagement/Index",
                        message = "Expertise Created Successfully",
                        data = new
                        {
                            Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            Description = Result.Description,
                            Id = Result.Id
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
        public async Task<IActionResult> Edit(long Id)
        {
            IEnumerable<SparePartExpertiseManagementViewModel> expertise = _mapper.Map<IEnumerable<SparePartExpertiseManagementViewModel>>(await _client.GetAllByIdAsync(Id));
            var parent = _mapper.Map<IEnumerable<ExpertiseViewModel>>(await _expertiseClient.GetAllAsync());
            ViewBag.ExpertiseId = new SelectList(parent, "Id", "Title");
            return View(expertise.FirstOrDefault());
        }

        public async Task<IActionResult> Delete(long id)
        {
            var getExpertiseManagement = await _SparePartExpertiseClient.GetAllBySparePartExpertiseManagementIdAsync(id);
            if (getExpertiseManagement.Any())
            {
                foreach (var item in getExpertiseManagement)
                {
                    await _SparePartExpertiseClient.DeleteSparePartExpertiseAsync(item.Id);
                }
            }

            await _client.DeleteSparePartExpertiseManagementAsync(id);

            return Json(new
            {

                success = true,
                message = "Expertise Management Deleted Successfully!"

            });

        }

        [HttpPost]
        public async Task<IActionResult> AddExpertise(long id, long managementId)
        {
            SparePartExpertiseViewModel model = new SparePartExpertiseViewModel();
            model.ExpertiseId = id;
            model.SparePartExpertiseManagementId = managementId;

            var result = await _SparePartExpertiseClient.AddSparePartExpertiseAsync(_mapper.Map<SparePartExpertiseDTO>(model));

            var resultSets = await _expertiseClient.GetAllByIdAsync(result.ExpertiseId);
            var resultSet = resultSets.FirstOrDefault();


            return Json(new
            {

                success = true,
                data = new
                {
                    Title = resultSet.Title,
                    Id = result.Id,
                }

            });
        }

        public async Task<IActionResult> DeleteExpertise(long id)
        {
            await _SparePartExpertiseClient.DeleteSparePartExpertiseAsync(id);

            return Json(new
            {

                success = true,
                message = "Expertise Deleted Successfully!"

            });

        }

        public async Task<IActionResult> GettAllExpertise(long id)
        {
            IEnumerable<SparePartExpertiseViewModel> model = _mapper.Map<IEnumerable<SparePartExpertiseViewModel>>(await _SparePartExpertiseClient.GetAllBySparePartExpertiseManagementIdAsync(id));

            return Json(new
            {
                success = true,
                data = model.Select(i => new {

                    id = i.Id,
                    title = i.Expertise.Title,
                    i.ExpertiseId

                })

            });
        }

    }
}
