using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.GarageAndSPWebsiteResponses.WebsiteResponses;
using HelperClasses.DTOs.SparePartCMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.ErrorHandling;
using WebAPI.Helpers;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Customers
{
    [Route("api/Customer/SparePart")]
    [ApiController]
    public class CustomerSparePartController : ControllerBase
    {
        private readonly ISparePartsDealerService _dealerService;
        private readonly ISparePartServiceManagement _dealerServiceManagement;
        private readonly ISparePartBlogService _SparePartBlogService;
        private readonly ISparePartCustomerFeedbackService _SparePartCustomerFeedback;
        private readonly ISparePartSubscriberService _SparePartSubscribers;
        private readonly ISparePartCustomerAppointmentService _customerAppointmentService;
        private readonly ISparePartCareerService _SparePartCareers;
        private readonly INotificationService _notificationService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        public CustomerSparePartController(ISparePartsDealerService dealerService, IMapper mapper,
            IFTPUpload fTPUpload, IEmailService emailService, ISparePartServiceManagement dealerServiceManagement,
            ISparePartBlogService sparePartBlogService, ISparePartCustomerFeedbackService sparePartCustomerFeedback,
            INotificationService notificationService, ISparePartCareerService sparePartCareers,
            ISparePartCustomerAppointmentService customerAppointmentService, ISparePartSubscriberService sparePartSubscribers)
        {
            _dealerService = dealerService;
            _mapper = mapper;
            _fTPUpload = fTPUpload;
            _emailService = emailService;
            _dealerServiceManagement = dealerServiceManagement;
            _SparePartBlogService = sparePartBlogService;
            _SparePartCustomerFeedback = sparePartCustomerFeedback;
            _notificationService = notificationService;
            _SparePartCareers = sparePartCareers;
            _customerAppointmentService = customerAppointmentService;
            _SparePartSubscribers = sparePartSubscribers;
        }

        [HttpGet]
        public async Task<IActionResult> GetSPByOrigin()
        {
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Models.SparePartsDealer> result = await _dealerService.GetDealerByOrigin(origin, "Header");

            return Ok(new SuccessResponse<WebsiteHeaderContentDTO>("", _mapper.Map<WebsiteHeaderContentDTO>(result.FirstOrDefault())));
        }

        [HttpGet("Content")]
        public async Task<IActionResult> GetSparePartContentByOrigin()
        {
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Models.SparePartsDealer> result = await _dealerService.GetDealerByOrigin(origin, "Content");

            SparePartContentManagementDTO content = _mapper.Map<SparePartContentManagementDTO>(result.FirstOrDefault().SparePartContentManagement);

            content.SparePartDealer = null;

            return Ok(new SuccessResponse<SparePartContentManagementDTO>("", content));
        }

        [HttpGet("Services")]
        public async Task<IActionResult> GetSparePartServicesByOrigin(bool ForHomePage = false)
        {
            List<SparePartServiceManagement> result;
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Models.SparePartsDealer> SpareParts = await _dealerService.GetDealerByOrigin(origin, "Services");

            if (ForHomePage)
                result = SpareParts.FirstOrDefault().SparePartServiceManagement.Where(x => x.Status == Enum.GetName(typeof(Status), Status.Active)).Take(6).ToList();
            else
                result = SpareParts.FirstOrDefault().SparePartServiceManagement.Where(x => x.Status == Enum.GetName(typeof(Status), Status.Active)).ToList();

            return Ok(new SuccessResponse<IEnumerable<WebsiteServiceManagementResponseDTO>>("", _mapper.Map<IEnumerable<WebsiteServiceManagementResponseDTO>>(result)));
        }
        [HttpGet("Services/{Slug}")]
        public async Task<IActionResult> GetSparePartServiceBySlug(string Slug)
        {
            IEnumerable<SparePartServiceManagement> services = await _dealerServiceManagement.GetSparePartServiceManagementBySlugAsync(Slug);
            return Ok(new SuccessResponse<SparePartServiceManagementDTO>("", _mapper.Map<SparePartServiceManagementDTO>(services.FirstOrDefault())));
        }

        [HttpGet("Team")]
        public async Task<IActionResult> GetSparePartTeamByOrigin(bool ForHomePage = false)
        {
            List<SparePartTeamManagement> result;
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Models.SparePartsDealer> SpareParts = await _dealerService.GetDealerByOrigin(origin, "Team");

            if (ForHomePage)
                result = SpareParts.FirstOrDefault().SparePartTeamManagement.Take(6).ToList();
            else
                result = SpareParts.FirstOrDefault().SparePartTeamManagement.ToList();

            return Ok(new SuccessResponse<IEnumerable<WebsiteTeamResponseDTO>>("", _mapper.Map<IEnumerable<WebsiteTeamResponseDTO>>(result)));
        }

        [HttpGet("Expertise")]
        public async Task<IActionResult> GetSparePartExpertiseByOrigin()
        {
            SparePartExpertiseManagement result;
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Models.SparePartsDealer> SpareParts = await _dealerService.GetDealerByOrigin(origin, "Expertise");
            result = SpareParts.FirstOrDefault().SparePartExpertiseManagements.FirstOrDefault();

            return Ok(new SuccessResponse<WebsiteExpertiseResponseDTO>("", _mapper.Map<WebsiteExpertiseResponseDTO>(result)));
        }

        [HttpGet("Testimonials")]
        public async Task<IActionResult> GetSparePartTestimonialsByOrigin()
        {
            List<SparePartTestimonial> result;
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Models.SparePartsDealer> SpareParts = await _dealerService.GetDealerByOrigin(origin, "Testimonials");
            result = SpareParts.FirstOrDefault().SparePartTestimonials.Where(x => x.ShowOnWebsite).ToList();

            return Ok(new SuccessResponse<IEnumerable<WebsiteTestimonialsResponseDTO>>("", _mapper.Map<IEnumerable<WebsiteTestimonialsResponseDTO>>(result)));
        }

        [HttpGet("Blogs")]
        public async Task<IActionResult> GetSparePartBlogsByOrigin(bool ForHomePage = false)
        {
            List<SparePartBlog> result;
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Models.SparePartsDealer> SpareParts = await _dealerService.GetDealerByOrigin(origin, "Blogs");

            if (ForHomePage)
                result = SpareParts.FirstOrDefault().SparePartBlogs.Where(x => x.Status == Enum.GetName(typeof(Status), Status.Active)).Take(3).ToList();
            else
                result = SpareParts.FirstOrDefault().SparePartBlogs.Where(x => x.Status == Enum.GetName(typeof(Status), Status.Active)).ToList();

            return Ok(new SuccessResponse<IEnumerable<WebsiteBlogResponseDTO>>("", _mapper.Map<IEnumerable<WebsiteBlogResponseDTO>>(result)));
        }

        [HttpGet("Blogs/{Slug}/RelatedBlogs")]
        public async Task<IActionResult> GetSparePartRelatedBlogsByOrigin(string Slug)
        {
            IEnumerable<SparePartBlog> Blogs = await _SparePartBlogService.GetSparePartBlogBySlugAsync(Slug);

            if (!Blogs.Any())
                return Conflict(new ErrorDetails(409, "Invalid BlogId", ""));

            return Ok(new SuccessResponse<IEnumerable<WebsiteBlogResponseDTO>>("",
                _mapper.Map<IEnumerable<WebsiteBlogResponseDTO>>(await _SparePartBlogService.GetSparePartRelatedBlogByIdAsync(Blogs.FirstOrDefault().Id, Blogs.FirstOrDefault().BlogCategoryId.Value))));
        }

        [HttpGet("Blogs/{Slug}")]
        public async Task<IActionResult> GetSparePartBlogsBySlug(string Slug)
        {
            IEnumerable<SparePartBlog> blogs = await _SparePartBlogService.GetSparePartBlogBySlugAsync(Slug);
            return Ok(new SuccessResponse<SparePartBlogDTO>("", _mapper.Map<SparePartBlogDTO>(blogs.FirstOrDefault())));
        }

        [HttpGet("Partner")]
        public async Task<IActionResult> GetSparePartPartnerByOrigin()
        {
            List<SparePartPartnersManagement> result;
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Models.SparePartsDealer> SpareParts = await _dealerService.GetDealerByOrigin(origin, "Partner");

            result = SpareParts.FirstOrDefault().SparePartPartnersManagement.ToList();

            return Ok(new SuccessResponse<IEnumerable<WebsitePartnersResponseDTO>>("", _mapper.Map<IEnumerable<WebsitePartnersResponseDTO>>(result.OrderBy(x => x.Position))));
        }

        [HttpGet("Appointment")]
        public async Task<IActionResult> GetSparePartAppointmentByOrigin()
        {
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Models.SparePartsDealer> SpareParts = await _dealerService.GetDealerByOrigin(origin, "Appointment");

            return Ok(new SuccessResponse<WebsiteAppoinmentManagementResponseDTO>("", _mapper.Map<WebsiteAppoinmentManagementResponseDTO>(SpareParts.FirstOrDefault().SparePartAppointmentManagement)));
        }

        [HttpGet("HeroSection")]
        public async Task<IActionResult> GetSparePartHeroSectionByOrigin()
        {
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Models.SparePartsDealer> SpareParts = await _dealerService.GetDealerByOrigin(origin, "Content");

            return Ok(new SuccessResponse<string>("", SpareParts.FirstOrDefault().SparePartContentManagement.InnerBanner));
        }

        [HttpPost("FeedBack")]
        public async Task<IActionResult> CustomerFeedback(SparePartCustomerFeedbackDTO Model)
        {
            IEnumerable<Models.SparePartsDealer> SpareParts = await _dealerService.GetSparePartsDealerByIdAsync(Model.SparePartDealerId);
            SparePartCustomerFeedback result = await _SparePartCustomerFeedback.AddSparePartCustomerFeedbackAsync(_mapper.Map<SparePartCustomerFeedback>(Model));

            Notification notification = new()
            {
                OriginatorId = "",
                OriginatorName = Model.CustomerEmail,
                Description = "New Feedback Recieved",
                RecordId = result.Id,
                OriginatorType = Enum.GetName(typeof(Logins), Logins.Customer),
                Url = "/SparePart/SparePartCustomerFeedback/Index",
                NotificationReceivers = new List<NotificationReceiver>()
                {
                    new NotificationReceiver
                    {
                        ReceiverId = SpareParts.FirstOrDefault().UserId,
                        IsSeen = false,
                        IsDelivered = false,
                        IsRead = false,
                        ReceiverType = Enum.GetName(typeof(Logins), Logins.SparePartDealer),
                    }
                }
            };

            await _notificationService.AddNotification(notification);

            return Ok(new SuccessResponse<string>("Feedback submitted successfully!", ""));
        }

        [HttpPost("Career")]
        public async Task<IActionResult> CareerForm(SparePartCareerDTO Model)
        {
            IEnumerable<Models.SparePartsDealer> list = await _dealerService.GetSparePartsDealerByIdAsync(Model.SparePartDealerId);
            if (Model.CVPath is not null && Model.CVPath.Contains("Draft"))
            {
                string LogoPath = "/Images/SparePart/" + list.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.CVPath, ref LogoPath))
                {
                    Model.CVPath = LogoPath;
                }
            }

            await _SparePartCareers.AddSparePartCareerAsync(_mapper.Map<SparePartCareer>(Model));

            Notification notification = new()
            {
                OriginatorId = "",
                OriginatorName = Model.FulName,
                Description = "New CV Recieved",
                RecordId = 0,
                OriginatorType = Enum.GetName(typeof(Logins), Logins.Customer),
                Url = "/SparePart/Career/Index",
                NotificationReceivers = new List<NotificationReceiver>()
                {
                    new NotificationReceiver
                    {
                        ReceiverId = list.FirstOrDefault().UserId,
                        IsSeen = false,
                        IsDelivered = false,
                        IsRead = false,
                        ReceiverType = Enum.GetName(typeof(Logins), Logins.SparePartDealer),
                    }
                }
            };

            await _notificationService.AddNotification(notification);

            return Ok(new SuccessResponse<string>("CV submitted successfully!", ""));
        }

        [HttpPost("Appointment")]
        public async Task<IActionResult> CustomerAppointment(SparePartCustomerAppointmentDTO Model)
        {
            IEnumerable<Models.SparePartsDealer> list = await _dealerService.GetSparePartsDealerByIdAsync(Model.SparePartDealerId);

            Model.AppointmentDate = Model.AppointmentDate.ToDubaiDateTime();
            Model.AppointmentTime = Model.AppointmentTime.ToDubaiDateTime();

            await _customerAppointmentService.AddSparePartCustomerAppointAsync(_mapper.Map<SparePartCustomerAppointment>(Model));

            Notification notification = new()
            {
                OriginatorId = "",
                OriginatorName = Model.CustomerName,
                Description = "New Appointment Request Recieved",
                RecordId = 0,
                OriginatorType = Enum.GetName(typeof(Logins), Logins.Customer),
                Url = "/SparePart/CustomerAppointment/Index",
                NotificationReceivers = new List<NotificationReceiver>()
                {
                    new NotificationReceiver
                    {
                        ReceiverId = list.FirstOrDefault().UserId,
                        IsSeen = false,
                        IsDelivered = false,
                        IsRead = false,
                        ReceiverType = Enum.GetName(typeof(Logins), Logins.SparePartDealer),
                    }
                }
            };

            await _notificationService.AddNotification(notification);

            return Ok(new SuccessResponse<string>("Appointment request submitted successfully!", ""));
        }

        [HttpPost("Subscriber")]
        public async Task<IActionResult> Subscribers(SparePartSubscriberDTO Model)
        {
            IEnumerable<SparePartSubscriber> IsSubscribed = await _SparePartSubscribers.GetSparePartSubscribersByEmailAsync(Model.SparePartDealerId, Model.Email);

            if (IsSubscribed.Any())
                return Conflict(new ErrorDetails(409, "Already Subscribed", ""));

            await _SparePartSubscribers.AddSparePartSubscriberAsync(_mapper.Map<SparePartSubscriber>(Model));

            return Ok(new SuccessResponse<string>("Successfully subscribed to news letter", ""));
        }

        [HttpGet("Footer")]
        public async Task<IActionResult> GetSparePartFooterByOrigin()
        {
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Models.SparePartsDealer> result = await _dealerService.GetDealerByOrigin(origin, "Footer");

            return Ok(new SuccessResponse<WebsiteFooterContentResponseDTO>("", _mapper.Map<WebsiteFooterContentResponseDTO>(result.FirstOrDefault())));
        }

        [HttpGet("TermsAndConditions")]
        public async Task<IActionResult> GetSparePartTACByOrigin()
        {
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Models.SparePartsDealer> result = await _dealerService.GetDealerByOrigin(origin, "Content");

            return Ok(new SuccessResponse<string>("", result.FirstOrDefault().SparePartContentManagement.TermsAndConditions));
        }

        [HttpGet("PrivacyPolicy")]
        public async Task<IActionResult> GetSparePartPrivacyPolicyByOrigin()
        {
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Models.SparePartsDealer> result = await _dealerService.GetDealerByOrigin(origin, "Content");

            return Ok(new SuccessResponse<string>("", result.FirstOrDefault().SparePartContentManagement.PrivacyPolicy));
        }

        [HttpGet("FAQ")]
        public async Task<IActionResult> GetSparePartFAQByOrigin()
        {
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Models.SparePartsDealer> SpareParts = await _dealerService.GetDealerByOrigin(origin, "FAQ");

            return Ok(new SuccessResponse<IEnumerable<WebsiteFAQResponseDTO>>("", _mapper.Map<IEnumerable<WebsiteFAQResponseDTO>>(SpareParts.FirstOrDefault().SparePartFAQ)));
        }

        [HttpPost("ContactUs")]
        public async Task<IActionResult> ContactUs(WebsiteContactUsDTO Model)
        {
            IEnumerable<Models.SparePartsDealer> SpareParts = await _dealerService.GetSparePartsDealerByIdAsync(Model.GarageId);

            if (!SpareParts.Any())
                return Conflict(new ErrorDetails(409, "Invalid SparePartId", ""));

            Models.SparePartsDealer SparePart = SpareParts.FirstOrDefault();

            return Ok(new SuccessResponse<string>(await _emailService.SendContactEmail(Model.Email, Model.FullName, SparePart.ContactPersonEmail, Model.Message), ""));
        }

        [HttpGet("PromoContent")]
        public async Task<IActionResult> GetSparePartPromoContentByOrigin()
        {
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<Models.SparePartsDealer> SpareParts = await _dealerService.GetDealerByOrigin(origin, "Promo");

            return Ok(new SuccessResponse<WebsitePromoSectionResponseDTO>("", _mapper.Map<WebsitePromoSectionResponseDTO>(SpareParts.FirstOrDefault())));
        }
    }
}
