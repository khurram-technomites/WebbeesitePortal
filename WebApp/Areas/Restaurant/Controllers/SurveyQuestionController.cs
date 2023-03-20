using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Survey;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
using WebApp.ViewModels.Survey;

namespace WebApp.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    [Authorize(Roles = "RestaurantOwner")]
    public class SurveyQuestionController : Controller
    {
        private readonly ISurveyQuestionClient _client;
        private readonly ISurveyOptionClient _clientOption;
        private readonly ISurveyClient _surveyClient;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSessionManager;

        public SurveyQuestionController(ISurveyClient surveyClient, ISurveyQuestionClient client, IMapper mapper, IUserSessionManager userSessionManager, ISurveyOptionClient clientOption)
        {
            _client = client;
            _surveyClient = surveyClient;
            _clientOption = clientOption;
            _mapper = mapper;
            _userSessionManager = userSessionManager;
        }
        public async Task<IActionResult> Index()
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            var Info = _mapper.Map<IEnumerable<SurveyQuestionViewModel>>(await _client.GetAllSurveyQuestionAsync(restaurantId));
            return View(Info);
        }

        public async Task<IActionResult> Create()
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            var parent = _mapper.Map<IEnumerable<SurveyViewModel>>(await (_surveyClient.GetAllSurveyByRestaurantAsync(restaurantId)));
            ViewBag.Survey = new SelectList(parent, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SurveyQuestionViewModel model)
        {
            try
            {
                long restaurantId = _userSessionManager.GetUserStore().Id;
                model.CreationDate = DateTime.Now;
                model.RestaurantId = restaurantId;

                SurveyQuestionViewModel Result = _mapper.Map<SurveyQuestionViewModel>(await _client.AddSurveyQuestionAsync(_mapper.Map<SurveyQuestionDTO>(model)));

                if (Result.Survey == null)
                    Result.Survey = _mapper.Map<SurveyViewModel>(await _surveyClient.GetSurveyByIdAsync((long)Result.SurveyId));

                return Json(new
                {
                    success = true,
                    url = "/Restaurant/Survey/Index",
                    message = "Record Added Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
                        Name = Result.Name,
                        SurveyName = Result.Survey != null ? Result.Survey.Name : "-",
                        Position = Result.Position,
                        Type = Result.Type,
                        IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false
                    }
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

        public async Task<IActionResult> Edit(long id)
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            SurveyQuestionViewModel model = _mapper.Map<SurveyQuestionViewModel>(await _client.GetSurveyQuestionByIdAsync(id));
            var parent = _mapper.Map<IEnumerable<SurveyViewModel>>(await (_surveyClient.GetAllSurveyByRestaurantAsync(restaurantId)));
            ViewBag.Survey = new SelectList(parent, "Id", "Name", model.SurveyId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SurveyQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SurveyQuestionViewModel Result = _mapper.Map<SurveyQuestionViewModel>(await _client.UpdateSurveyQuestionAsync(_mapper.Map<SurveyQuestionDTO>(model)));

                    if (Result.Survey == null)
                        Result.Survey = _mapper.Map<SurveyViewModel>(await _surveyClient.GetSurveyByIdAsync((long)Result.SurveyId));

                    return Json(new
                    {
                        success = true,
                        message = "Record Updated Successfully",
                        data = new
                        {
                            ID = Result.Id,
                            Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
                            Name = Result.Name,
                            SurveyName = Result.Survey != null ? Result.Survey.Name : "-",
                            Position = Result.Position,
                            Type = Result.Type,
                            IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false


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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await _client.DeleteSurveyQuestionAsync(id);

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

        public async Task<ActionResult> ToggleActiveStatus(long id)
        {
            try
            {
                SurveyQuestionViewModel Result = _mapper.Map<SurveyQuestionViewModel>(await _client.ToggleActiveStatus(id));

                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
                        Name = Result.Name,
                        SurveyName = Result.Survey != null ? Result.Survey.Name : "-",
                        Position = Result.Position,
                        Type = Result.Type,
                        IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false
                    }

                }); ;
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


        public async Task<ActionResult> Details(long id)
        {
            return View(_mapper.Map<SurveyQuestionViewModel>(await _client.GetSurveyQuestionByIdAsync(id)));
        }


        public async Task<ActionResult> GetPosition(long id)
        {
            IEnumerable<SurveyQuestionViewModel> questions = _mapper.Map<IEnumerable<SurveyQuestionViewModel>>(await _client.GetAllSurveyQuestionBySurveyAsync(id));
            int count = 0;

            count = questions.Count() + 1;

            return Json(new
            {
                success = true,
                data = count,
            });
        }

        public async Task<IActionResult> Options(long Id)
        {
            IEnumerable<SurveyOptionViewModel> list = _mapper.Map<IEnumerable<SurveyOptionViewModel>>(await _clientOption.GetAllBySurveyIdAsync(Id));
            ViewBag.SurveyQuestionId = Id;
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Options(string option, long surveyQuestionId)
        {
            try
            {
                SurveyOptionViewModel model = new SurveyOptionViewModel();
                model.Option = option;
                model.QuestionId = surveyQuestionId;
                SurveyOptionViewModel Result = _mapper.Map<SurveyOptionViewModel>(await _clientOption.AddSurveyOptionAsync(_mapper.Map<SurveyOptionDTO>(model)));

                return Json(new
                {
                    success = true,
                    url = "/Restaurant/SurveyOption/Index",
                    message = "Record Added Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Option = Result.Option,
                    }
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

        [HttpGet]
        public async Task<ActionResult> DeleteOption(long id)
        {
            try
            {
                await _clientOption.DeleteSurveyOptionAsync(id);

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
    }
}
