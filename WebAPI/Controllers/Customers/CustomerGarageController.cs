using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Emails;
using HelperClasses.DTOs.Garage;
using HelperClasses.DTOs.Garage.Filter;
using HelperClasses.DTOs.GarageAndSPWebsiteResponses;
using HelperClasses.DTOs.GarageAndSPWebsiteResponses.WebsiteResponses;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.ErrorHandling;
using WebAPI.Helpers;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Customers
{
    [Route("api/Customer/Garage")]
    [ApiController]
    public class CustomerGarageController : ControllerBase
    {
        private readonly IGarageService _garageService;
        private readonly IGarageRatingService _garageRatingService;
        private readonly IGarageBlogService _garageBlogService;
        private readonly IGarageServiceManagementService _garageServiceManagement;
        private readonly IGarageCustomerFeedbackService _garageCustomerFeedback;
        private readonly IGarageCareersService _garageCareers;
        private readonly IGarageCustomerAppointmentService _customerAppointmentService;
        private readonly INotificationService _notificationService;
        private readonly IGarageSubscribersService _garageSubscribers;
        private readonly IFTPUpload _fTPUpload;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly string _salesEmail;
        private readonly IIntegrationSettingService _integrationSettingService;
        private readonly ILogger<CustomerGarageController> _logger;

        public CustomerGarageController(IGarageService garageService, IMapper mapper, IGarageRatingService garageRatingService,
            IGarageBlogService garageBlogService, IGarageServiceManagementService garageServiceManagement,
            IGarageCustomerFeedbackService garageCustomerFeedback, IFTPUpload fTPUpload, IGarageCareersService garageCareers,
            IGarageCustomerAppointmentService customerAppointmentService, INotificationService notificationService, IGarageSubscribersService garageSubscribers,
            IEmailService emailService, IIntegrationSettingService integrationSettingService, ILogger<CustomerGarageController> logger)
        {
            _garageService = garageService;
            _mapper = mapper;
            _garageRatingService = garageRatingService;
            _garageBlogService = garageBlogService;
            _garageServiceManagement = garageServiceManagement;
            _garageCustomerFeedback = garageCustomerFeedback;
            _fTPUpload = fTPUpload;
            _garageCareers = garageCareers;
            _customerAppointmentService = customerAppointmentService;
            _notificationService = notificationService;
            _garageSubscribers = garageSubscribers;
            _emailService = emailService;
            _salesEmail = integrationSettingService.GetAllAsync().Result.FirstOrDefault().SalesEmail;
            _logger = logger;
        }

        [HttpPost("GetAll")]
        public IActionResult GetAllGaragesNearMe(GarageFilter Filter) => Ok(new SuccessResponse<IEnumerable<GarageCardResponseDTO>>("", _mapper.Map<IEnumerable<GarageCardResponseDTO>>(_garageService.GetAllNearMe(Filter))));
        [HttpGet("{slug}")]
        public async Task<IActionResult> GetBySlug(string slug, double? lat, double? lng)
        {
            List<GarageDTO> list = _mapper.Map<List<GarageDTO>>(await _garageService.GetGarageBySlugAsync(slug));

            if (!list.Any())
                return Conflict(new ErrorDetails(409, "No record found!", ""));

            if (!lat.HasValue && !lng.HasValue)
                list.ForEach(x => x.Distance = null);
            else
                list.ForEach(x => x.Distance = DistanceHelper.DistanceTo((double)lat, (double)lng, (double)x.Latitude, (double)x.Longitude).ToString("n2") + " Km");

            return Ok(new SuccessResponse<List<GarageDTO>>("", list));
        }
        [HttpPost("Rating")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PostReview(GarageRatingDTO Model)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Model.UserId = UserId;
            Model.Status = Enum.GetName(typeof(Status), Status.Processing);

            return Ok(new SuccessResponse<GarageRatingDTO>("", _mapper.Map<GarageRatingDTO>(await _garageRatingService.AddRatingAsync(_mapper.Map<GarageRating>(Model)))));
        }

        [HttpGet]
        public async Task<IActionResult> GetGarageByOrigin()
        {
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Garage> result = await _garageService.GetGarageByOrigin(origin, "Header");
            string code = result.FirstOrDefault().GarageBusinessSetting != null ? result.FirstOrDefault().GarageBusinessSetting.Contact01.Split("-").First() : "";
            string number = result.FirstOrDefault().GarageBusinessSetting != null ? result.FirstOrDefault().GarageBusinessSetting.Contact01.Split("-").Last() : "";
            if (!code.StartsWith("+"))
            {
                result.FirstOrDefault().GarageBusinessSetting.Contact01 = "+(" + code + ") " + number;
            }
            else if (!result.FirstOrDefault().GarageBusinessSetting.Contact01.StartsWith("+971"))
            {
                result.FirstOrDefault().GarageBusinessSetting.Contact01 = "(+971) " + number;
            }
            return Ok(new SuccessResponse<WebsiteHeaderContentDTO>("", _mapper.Map<WebsiteHeaderContentDTO>(result.FirstOrDefault())));
        }

        [HttpGet("Content")]
        public async Task<IActionResult> GetGarageContentByOrigin()
        {
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Garage> result = await _garageService.GetGarageByOrigin(origin, "Content");

            GarageContentManagementDTO content = _mapper.Map<GarageContentManagementDTO>(result.FirstOrDefault().GarageContentManagement);

            content.Garage = null;

            return Ok(new SuccessResponse<GarageContentManagementDTO>("", content));
        }

        [HttpGet("Services")]
        public async Task<IActionResult> GetGarageServicesByOrigin(bool ForHomePage = false)
        {
            List<GarageServiceManagement> result = new List<GarageServiceManagement>();
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Garage> Garages = await _garageService.GetGarageByOrigin(origin, "Services");
            if (Garages.Count() > 0)
            {
                if (ForHomePage)
                    result = Garages.FirstOrDefault().GarageServiceManagement.Where(x => x.Status == Enum.GetName(typeof(Status), Status.Active)).Take(6).ToList();
                else
                    result = Garages.FirstOrDefault().GarageServiceManagement.Where(x => x.Status == Enum.GetName(typeof(Status), Status.Active)).ToList();
            }
            return Ok(new SuccessResponse<IEnumerable<WebsiteServiceManagementResponseDTO>>("", _mapper.Map<IEnumerable<WebsiteServiceManagementResponseDTO>>(result)));
        }
        [HttpGet("Services/{Slug}")]
        public async Task<IActionResult> GetGarageServiceBySlug(string Slug)
        {
            IEnumerable<GarageServiceManagement> services = await _garageServiceManagement.GetGarageServiceManagementBySlugAsync(Slug);
            return Ok(new SuccessResponse<GarageServiceManagementDTO>("", _mapper.Map<GarageServiceManagementDTO>(services.FirstOrDefault())));
        }

        [HttpGet("Team")]
        public async Task<IActionResult> GetGarageTeamByOrigin(bool ForHomePage = false)
        {
            List<GarageTeamManagement> result = new List<GarageTeamManagement>();
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Garage> Garages = await _garageService.GetGarageByOrigin(origin, "Team");
            if (Garages.Count() > 0)
            {
                if (ForHomePage)
                result = Garages.FirstOrDefault().GarageTeamManagement.Take(6).ToList();
                else
                result = Garages.FirstOrDefault().GarageTeamManagement.ToList();
            }
            return Ok(new SuccessResponse<IEnumerable<WebsiteTeamResponseDTO>>("", _mapper.Map<IEnumerable<WebsiteTeamResponseDTO>>(result)));
        }

        [HttpGet("Expertise")]
        public async Task<IActionResult> GetGarageExpertiseByOrigin()
        {
            GarageExpertiseManagement result = new GarageExpertiseManagement();
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Garage> Garages = await _garageService.GetGarageByOrigin(origin, "Expertise");
            if (Garages.Count() > 0)
            {
                result = Garages.FirstOrDefault().GarageExpertiseManagements.FirstOrDefault();
            }
            return Ok(new SuccessResponse<WebsiteExpertiseResponseDTO>("", _mapper.Map<WebsiteExpertiseResponseDTO>(result)));
        }

        [HttpGet("Testimonials")]
        public async Task<IActionResult> GetGarageTestimonialsByOrigin()
        {
            List<GarageTestimonials> result = new List<GarageTestimonials>();
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Garage> Garages = await _garageService.GetGarageByOrigin(origin, "Testimonials");
            if (Garages.Count() > 0)
            {
                result = Garages.FirstOrDefault().GarageTestimonials.Where(x => x.ShowOnWebsite).ToList();
            }

            return Ok(new SuccessResponse<IEnumerable<WebsiteTestimonialsResponseDTO>>("", _mapper.Map<IEnumerable<WebsiteTestimonialsResponseDTO>>(result)));
        }

        [HttpPost("Blogs")]
        public async Task<IActionResult> GetGarageBlogsByOrigin(bool ForHomePage = false, PagingParameters Paging = null)
        {
            List<GarageBlog> result = new List<GarageBlog>();
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Garage> Garages = await _garageService.GetGarageByOrigin(origin, "Blogs");
            if(Garages.Count() > 0)
            {
                if (ForHomePage)
                    result = Garages.FirstOrDefault().GarageBlogs.Where(x => x.Status == Enum.GetName(typeof(Status), Status.Active ) ).Take(3).ToList();
                else
               if (Paging is not null)
                    result = Garages.FirstOrDefault().GarageBlogs.Where(x => x.Status == Enum.GetName(typeof(Status), Status.Active) )
                                                                 .Skip((Paging.PageNumber - 1) * Paging.PageSize).Take(Paging.PageSize).ToList();
                else
                    result = Garages.FirstOrDefault().GarageBlogs.Where(x => x.Status == Enum.GetName(typeof(Status), Status.Active) ).ToList();
            }
           

            return Ok(new SuccessResponse<IEnumerable<WebsiteBlogResponseDTO>>("", _mapper.Map<IEnumerable<WebsiteBlogResponseDTO>>(result)));
        }

        [HttpGet("Blogs/{Slug}/RelatedBlogs")]
        public async Task<IActionResult> GetGarageRelatedBlogsByOrigin(string Slug)
        {
            IEnumerable<GarageBlog> Blogs = await _garageBlogService.GetGarageBlogBySlugAsync(Slug);

            if (!Blogs.Any())
                return Conflict(new ErrorDetails(409, "Invalid BlogId", ""));

            return Ok(new SuccessResponse<IEnumerable<WebsiteBlogResponseDTO>>("",
                _mapper.Map<IEnumerable<WebsiteBlogResponseDTO>>(await _garageBlogService.GetGarageRelatedBlogByIdAsync(Blogs.FirstOrDefault().Id, Blogs.FirstOrDefault().BlogCategoryId.Value))));
        }

        [HttpGet("Blogs/{Slug}")]
        public async Task<IActionResult> GetGarageBlogsBySlug(string Slug)
        {
            IEnumerable<GarageBlog> blogs = await _garageBlogService.GetGarageBlogBySlugAsync(Slug);
            return Ok(new SuccessResponse<GarageBlogDTO>("", _mapper.Map<GarageBlogDTO>(blogs.FirstOrDefault())));
        }

        [HttpGet("Partner")]
        public async Task<IActionResult> GetGaragePartnerByOrigin()
        {
            List<GaragePartnersManagement> result = new List<GaragePartnersManagement>();
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Garage> Garages = await _garageService.GetGarageByOrigin(origin, "Partner");
            if (Garages.Count() > 0)
            {
                result = Garages.FirstOrDefault().GaragePartnersManagement.ToList();
            }

            return Ok(new SuccessResponse<IEnumerable<WebsitePartnersResponseDTO>>("", _mapper.Map<IEnumerable<WebsitePartnersResponseDTO>>(result.OrderBy(x => x.Position))));
        }
        [HttpGet("AllowCheck")]
        public async Task<IActionResult> GetAllowCheckByOrigin()
        {
            Garage result = new Garage();
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Garage> Garages = await _garageService.GetGarageByOrigin(origin, "Garage");
            result = Garages.FirstOrDefault();

            return Ok(new SuccessResponse<IEnumerable<WebsiteGarageResponseDTO>>("", _mapper.Map<IEnumerable<WebsiteGarageResponseDTO>>(result)));
        }
        [HttpGet("Appointment")]
        public async Task<IActionResult> GetGarageAppointmentByOrigin()
        {
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Garage> Garages = await _garageService.GetGarageByOrigin(origin, "Appointment");

            return Ok(new SuccessResponse<WebsiteAppoinmentManagementResponseDTO>("", _mapper.Map<WebsiteAppoinmentManagementResponseDTO>(Garages.FirstOrDefault().GarageAppointmentManagement)));
        }

        [HttpGet("HeroSection")]
        public async Task<IActionResult> GetGarageHeroSectionByOrigin()
        {
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Garage> Garages = await _garageService.GetGarageByOrigin(origin, "Content");
            return Ok(new SuccessResponse<string>("", Garages.FirstOrDefault().GarageContentManagement.InnerPagesBanner));
        }

        [HttpPost("FeedBack")]
        public async Task<IActionResult> CustomerFeedback(GarageCustomerFeedbackDTO Model)
        {
            IEnumerable<Garage> garages = await _garageService.GetGarageByIdAsync(Model.GarageId);
            GarageCustomerFeedback result = await _garageCustomerFeedback.AddGarageCustomerFeedbackAsync(_mapper.Map<GarageCustomerFeedback>(Model));

            Notification notification = new()
            {
                OriginatorId = "",
                OriginatorName = Model.CustomerEmail,
                Description = "New Feedback Recieved",
                RecordId = result.Id,
                OriginatorType = Enum.GetName(typeof(Logins), Logins.Customer),
                Url = "/Client/CustomerFeedback/Index",
                NotificationReceivers = new List<NotificationReceiver>()
                {
                    new NotificationReceiver
                    {
                        ReceiverId = garages.FirstOrDefault().UserId,
                        IsSeen = false,
                        IsDelivered = false,
                        IsRead = false,
                        ReceiverType = Enum.GetName(typeof(Logins), Logins.Garage),
                    }
                }
            };

            await _notificationService.AddNotification(notification);

            return Ok(new SuccessResponse<string>("Feedback submitted successfully!", ""));
        }

        [HttpPost("Career")]
        public async Task<IActionResult> CareerForm(GarageCareerDTO Model)
        {
            IEnumerable<Garage> list = await _garageService.GetGarageByIdAsync(Model.GarageId);
            if (Model.CVPath is not null && Model.CVPath.Contains("Draft"))
            {
                string LogoPath = "/Images/Garage/" + list.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.CVPath, ref LogoPath))
                {
                    Model.CVPath = LogoPath;
                }
            }

            await _garageCareers.AddGarageCareersAsync(_mapper.Map<GarageCareers>(Model));

            Notification notification = new()
            {
                OriginatorId = "",
                OriginatorName = Model.FulName,
                Description = "New CV Recieved",
                RecordId = 0,
                OriginatorType = Enum.GetName(typeof(Logins), Logins.Customer),
                Url = "/Garage/Career/Index",
                NotificationReceivers = new List<NotificationReceiver>()
                {
                    new NotificationReceiver
                    {
                        ReceiverId = list.FirstOrDefault().UserId,
                        IsSeen = false,
                        IsDelivered = false,
                        IsRead = false,
                        ReceiverType = Enum.GetName(typeof(Logins), Logins.Garage),
                    }
                }
            };

            await _notificationService.AddNotification(notification);

            return Ok(new SuccessResponse<string>("CV submitted successfully!", ""));
        }

        [HttpPost("Appointment")]
        public async Task<IActionResult> CustomerAppointment(GarageCustomerAppointmentDTO Model)
        {
            IEnumerable<Garage> list = await _garageService.GetGarageByIdAsync(Model.GarageId);

            Model.AppointmentDate = Model.AppointmentDate.ToDubaiDateTime();
            Model.AppointmentTime = Model.AppointmentTime.ToDubaiDateTime();

            await _customerAppointmentService.AddGarageCustomerAppointmentAsync(_mapper.Map<GarageCustomerAppointment>(Model));

            Notification notification = new()
            {
                OriginatorId = "",
                OriginatorName = Model.CustomerName,
                Description = "New Appointment Request Recieved",
                RecordId = 0,
                OriginatorType = Enum.GetName(typeof(Logins), Logins.Customer),
                Url = "/Garage/CustomerAppointment/Index",
                NotificationReceivers = new List<NotificationReceiver>()
                {
                    new NotificationReceiver
                    {
                        ReceiverId = list.FirstOrDefault().UserId,
                        IsSeen = false,
                        IsDelivered = false,
                        IsRead = false,
                        ReceiverType = Enum.GetName(typeof(Logins), Logins.Garage),
                    }
                }
            };

            await _notificationService.AddNotification(notification);

            return Ok(new SuccessResponse<string>("Appointment request submitted successfully!", ""));
        }

        [HttpPost("Subscriber")]
        public async Task<IActionResult> Subscribers(GarageSubscribersDTO Model)
        {
            IEnumerable<GarageSubscribers> IsSubscribed = await _garageSubscribers.GetGarageSubscribersByEmailAsync(Model.Email, Model.GarageId);

            if (IsSubscribed.Any())
                return Conflict(new ErrorDetails(409, "Already Subscribed", ""));

            await _garageSubscribers.AddGarageSubscribersAsync(_mapper.Map<GarageSubscribers>(Model));

            return Ok(new SuccessResponse<string>("Successfully subscribed to news letter", ""));
        }

        [HttpGet("Footer")]
        public async Task<IActionResult> GetGarageFooterByOrigin()
        {
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Garage> result = await _garageService.GetGarageByOrigin(origin, "Footer");
            string code = result.FirstOrDefault().GarageBusinessSetting != null ? result.FirstOrDefault().GarageBusinessSetting.Contact01.Split("-").First() : "";
            string number = result.FirstOrDefault().GarageBusinessSetting != null ? result.FirstOrDefault().GarageBusinessSetting.Contact01.Split("-").Last() : "";
            if (!code.StartsWith("+"))
            {
                result.FirstOrDefault().GarageBusinessSetting.Contact01 = "+(" + code + ") " + number;
            }
            else if (!result.FirstOrDefault().GarageBusinessSetting.Contact01.StartsWith("+971"))
            {
                result.FirstOrDefault().GarageBusinessSetting.Contact01 = "(+971) " + number;
            }
            return Ok(new SuccessResponse<WebsiteFooterContentResponseDTO>("", _mapper.Map<WebsiteFooterContentResponseDTO>(result.FirstOrDefault())));
        }

        [HttpGet("TermsAndConditions")]
        public async Task<IActionResult> GetGarageTACByOrigin()
        {
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Garage> result = await _garageService.GetGarageByOrigin(origin, "Content");

            return Ok(new SuccessResponse<string>("", result.FirstOrDefault().GarageContentManagement.TermsAndConditions));
        }

        [HttpGet("PrivacyPolicy")]
        public async Task<IActionResult> GetGaragePrivacyPolicyByOrigin()
        {
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Garage> result = await _garageService.GetGarageByOrigin(origin, "Content");

            return Ok(new SuccessResponse<string>("", result.FirstOrDefault().GarageContentManagement.PrivacyPolicy));
        }

        [HttpGet("FAQ")]
        public async Task<IActionResult> GetGarageFAQByOrigin()
        {
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Garage> Garages = await _garageService.GetGarageByOrigin(origin, "FAQ");

            return Ok(new SuccessResponse<IEnumerable<WebsiteFAQResponseDTO>>("", _mapper.Map<IEnumerable<WebsiteFAQResponseDTO>>(Garages.FirstOrDefault().GarageFAQs.Where(x=>x.ArchivedDate == null))));
        }

        [HttpPost("ContactUs")]
        public async Task<IActionResult> ContactUs(WebsiteContactUsDTO Model)
        {
            IEnumerable<Garage> Garages = await _garageService.GetGarageByIdAsync(Model.GarageId);

            if (!Garages.Any())
                return Conflict(new ErrorDetails(409, "Invalid GarageId", ""));

            Garage Garage = Garages.FirstOrDefault();

            return Ok(new SuccessResponse<string>(await _emailService.SendContactEmail(Model.Email, Model.FullName, Garage.ContactPersonEmail, Model.Message), ""));
        }

        [HttpGet("PromoContent")]
        public async Task<IActionResult> GetGaragePromoContentByOrigin()
        {
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Garage> Garages = await _garageService.GetGarageByOrigin(origin, "Promo");

            return Ok(new SuccessResponse<WebsitePromoSectionResponseDTO>("", _mapper.Map<WebsitePromoSectionResponseDTO>(Garages.FirstOrDefault())));
        }

        [HttpGet("BusinessSetting")]
        public async Task<IActionResult> GetGarageBusinessSettingByOrigin()
        {
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Garage> Garages = await _garageService.GetGarageByOrigin(origin, "BusinessSetting");
            if (Garages.FirstOrDefault().GarageBusinessSetting.Contact01.StartsWith("+"))
            {
                Garages.FirstOrDefault().GarageBusinessSetting.Contact01 = "+" + Garages.FirstOrDefault().ContactPersonNumber;
            }
            return Ok(new SuccessResponse<WebisteContactUsResponseDTO>("", _mapper.Map<WebisteContactUsResponseDTO>(Garages.FirstOrDefault())));
        }
        [HttpPost("RequestDemo")]
        public async Task<IActionResult> RequestDemo(BookDemoDTO Model)
        {
            await SendDemoBookingAsync(Model);

            return Ok(new SuccessResponse<string>("Email sent successfully", string.Empty));
        }
        private async Task SendDemoBookingAsync(BookDemoDTO Model)
        {
            try
            {
                await _emailService.SendDemoRequestEmail(new GeneralEmailDTO()
                {
                    Name = Model.Name,
                    Email = Model.Email,
                    PhoneNumber = Model.PhoneNumber,
                    HTMLBody = Model.Message,
                }, "Webeesite Demo Request", _salesEmail);

                _logger.LogInformation("Demo request Email Sent Successfully to " + _salesEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError("Demo request Email Failed for " + _salesEmail + " with message: " +
                                  ex.Message);
            }
        }
    }
}
