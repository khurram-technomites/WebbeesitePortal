using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "SparePartDealer")]
    public class SparePartTestimonialsController : Controller
    {
        private readonly ISparePartTestimonialClient _serviceTestimonialsClient;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSession;
        public SparePartTestimonialsController(ISparePartTestimonialClient serviceTestimonialsClient, IMapper mapper, IUserSessionManager userSession)
        {
            _serviceTestimonialsClient = serviceTestimonialsClient;
            _mapper = mapper;
            _userSession = userSession;
        }

        public async Task<IActionResult> Index()
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;
            return View(_mapper.Map<IEnumerable<SparePartTestimonialViewModel>>(await _serviceTestimonialsClient.GetAllBySparePartDealerIdAsync(SparePartId)));
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SparePartTestimonialViewModel Model)
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;

            Model.SparePartDealerId = SparePartId;
            SparePartTestimonialDTO result = await _serviceTestimonialsClient.AddSparePartTestimonialAsync(_mapper.Map<SparePartTestimonialDTO>(Model));

            return Json(new
            {
                success = true,
                url = "/SparePart/SparePartTestimonials/Index",
                message = "Record Added Successfully",
                data = new
                {
                    Date = result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                    Name = result.CustomerName,
                    Image = result.CustomerImage,
                    Testimonial = result.Testimonial,
                    Rating = result.Rating,
                    Id = result.Id,
                }
            });
        }

        public async Task<IActionResult> Edit(long Id)
        {
            IEnumerable<SparePartTestimonialViewModel> result = _mapper.Map<IEnumerable<SparePartTestimonialViewModel>>(await _serviceTestimonialsClient.GetAllByIdAsync(Id));
            return View(result.FirstOrDefault());
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Edit(SparePartTestimonialViewModel Model)
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;

            Model.SparePartDealerId = SparePartId;
            SparePartTestimonialDTO result = await _serviceTestimonialsClient.UpdateGSparePartTestimonialAsync(_mapper.Map<SparePartTestimonialDTO>(Model));

            return Json(new
            {
                success = true,
                url = "/SparePart/SparePartTestimonials/Index",
                message = "Record Updated Successfully",
                data = new
                {
                    Date = result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                    Name = result.CustomerName,
                    Testimonial = result.Testimonial,
                    Rating = result.Rating,
                    Image = result.CustomerImage,
                    Show = result.ShowOnWebsite,
                    Id = result.Id,
                }
            });
        }
        public async Task<IActionResult> Delete(long Id)
        {
            try
            {
                await _serviceTestimonialsClient.DeleteSparePartTestimonialAsync(Id);

                return Json(new
                {
                    success = true,
                    message = "Team Deleted Successfully"
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
                SparePartTestimonialViewModel result = _mapper.Map<SparePartTestimonialViewModel>(await _serviceTestimonialsClient.ToggleActiveStatus(id));
                return Json(new
                {
                    success = true,
                    url = "/SparePart/SparePartTestimonials/Index",
                    message = "Record Added Successfully",
                    data = new
                    {
                        Date = result.CreationDate,
                        Name = result.CustomerName,
                        Testimonial = result.Testimonial,
                        Rating = result.Rating,
                        Id = result.Id,
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
        public async Task<IActionResult> Detail(long Id)
        {
            IEnumerable<SparePartTestimonialViewModel> result = _mapper.Map<IEnumerable<SparePartTestimonialViewModel>>(await _serviceTestimonialsClient.GetAllByIdAsync(Id));
            return View(result.FirstOrDefault());
        }
    }
}
