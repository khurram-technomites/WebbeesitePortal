using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories;
using WebAPI.Interfaces.IServices;
using WebAPI.Models;
using WebAPI.Repositories;
using WebAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI.Helpers;
using Microsoft.AspNetCore.Mvc.Razor;
using WebAPI.ErrorHandling;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Services.Domains;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Repositories.Domains;
using Microsoft.AspNetCore.Http.Features;
using HelperClasses.DTOs.GarageCMS;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public IConfiguration _Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = DBConnectionStringBuilder.GetConnectionString(_Configuration);

            services.Configure<IISOptions>(options => { });

            services.AddDbContext<FougitoContext>(options =>
            {
                options.UseSqlServer(connectionString);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.EnableSensitiveDataLogging();

            }, ServiceLifetime.Transient);

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                //Forced Password setting
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 1;

                //Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                //User settings
                options.User.RequireUniqueEmail = false;

                //Signin settings
                //Setting this false here to allow identity to redirect to email confirmation page if email is not confirmed
                //Handling the behaviour on controller level
                options.SignIn.RequireConfirmedPhoneNumber = true;
            })
                .AddEntityFrameworkStores<FougitoContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            //Configuring JWT Bearer tokens
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = _Configuration["JWT:ValidAudience"],
                    ValidIssuer = _Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["JWT:Secret"]))
                };
            });

            services.AddRazorPages();
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    //If not done here. Api will raise error while fetching parent child relational objects
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();

                    options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                });

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationFormats.Clear();
                options.ViewLocationFormats.Add("View/{1}/{0}" + RazorViewEngine.ViewExtension);
                options.ViewLocationFormats.Add("View/Emails/{0}" + RazorViewEngine.ViewExtension);
            });

            services.Configure<EmailSettings>(_Configuration.GetSection("EmailSettings"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Webbeesite API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddHttpClient<IMapService, MapService>();

            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IFTPUpload, FTPUpload>();
            services.AddScoped<IFatoorahService, FatoorahService>();
            services.AddScoped<IUrlHelperService, UrlHelper>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IMapService, MapService>();

            services.AddScoped<ITestTableRepo, TestTableRepo>();
            services.AddScoped<ITestTableService, TestTableService>();

            services.AddScoped<IUserRefreshTokenRepo, UserRefreshTokenRepo>();
            services.AddScoped<IUserRefreshTokenService, UserRefreshTokenService>();

            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IRouteGroupService, RouteGroupService>();
            services.AddScoped<IRouteGroupsRepo, RouteGroupsRepo>();

            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IGroupRepo, GroupRepo>();

            services.AddScoped<IIntegrationSettingService, IntegrationSettingService>();
            services.AddScoped<IIntegrationSettingRepo, IntegrationSettingRepo>();

            services.AddScoped<ICarMakeService, CarMakeService>();
            services.AddScoped<ICarMakeRepo, CarMakeRepo>();

            services.AddScoped<ICarModelRepo, CarModelRepo>();
            services.AddScoped<ICarModelService, CarModelService>();

            services.AddScoped<IBlogRepo, BlogRepo>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IBlogsCategoryRepo, BlogsCategoryRepo>();
            services.AddScoped<IBlogCategoryService, BlogCategoryService>();
            services.AddScoped<ICityRepo, CityRepo>();
            services.AddScoped<ICityService, CityService>();

            services.AddScoped<ITicketMessageRepo, TicketMessagesRepo>();
            services.AddScoped<ITicketMessageService, TicketMessagesService>();

            services.AddScoped<ICountryRepo, CountryRepo>();
            services.AddScoped<ICountryService, CountryService>();

            services.AddScoped<IAreaRepo, AreaRepo>();
            services.AddScoped<IAreaService, AreaService>();

            services.AddScoped<ICustomerSessionRepo, CustomerSessionRepo>();
            services.AddScoped<ICustomerSessionService, CustomerSessionService>();

            services.AddScoped<ICustomerFavouriteBranchesRepo, CustomerFavouriteBranchesRepo>();
            services.AddScoped<ICustomerFavouriteBranchesService, CustomerFavouriteBranchesService>();

            services.AddScoped<IBusinessSettingRepo, BusinessSettingRepo>();
            services.AddScoped<IBusinessSettingService, BusinessSettingService>();

            services.AddScoped<INotificationRecieverRepo, NotificationRecieverRepo>();
            services.AddScoped<INotificationRecieverService, NotificationRecieverService>();

            services.AddScoped<INotificationRepo, NotificationRepo>();
            services.AddScoped<INotificationService, NotificationService>();

            services.AddScoped<INumberRangeRepo, NumberRangeRepo>();
            services.AddScoped<INumberRangeService, NumberRangeService>();

            services.AddScoped<ICustomerSessionRepo, CustomerSessionRepo>();
            //services.AddScoped<ICustomerSessionService, CustomerSessionService>();

            services.AddScoped<ICustomerFeedbackRepo, CustomerFeedbackRepo>();
            services.AddScoped<ICustomerFeedbackService, CustomerFeedbackService>();

            services.AddScoped<ICustomerFeedbackReviewRepo, CustomerFeedbackReviewRepo>();
            services.AddScoped<ICustomerFeedbackReviewService, CustomerFeedbackReviewService>();

            services.AddScoped<IFCMUserSessionRepo, FCMUserSessionRepo>();
            services.AddScoped<IFCMUserSessionService, FCMUserSessionService>();

            //Garage Services start
            services.AddScoped<IGarageService, GarageService>();
            services.AddScoped<IGarageRepo, GarageRepo>();

            services.AddScoped<IGarageImageService, GarageImageService>();
            services.AddScoped<IGarageImageRepo, GarageImageRepo>();

            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<IRequestRepo, RequestRepo>();

            services.AddScoped<ISubscribeService, SubscriberService>();
            services.AddScoped<ISubscriberRepo, SubscriberRepo>();

            services.AddScoped<ISparePartRequestImagesService, SparePartRequestImagesService>();
            services.AddScoped<ISparePartRequestImagesRepo, SparePartRequestImagesRepo>();

            services.AddScoped<IGarageRatingRepo, GarageRatingRepo>();
            services.AddScoped<IGarageRatingService, GarageRatingService>();

            services.AddScoped<IGarageScheduleRepo, GarageScheduleRepo>();
            services.AddScoped<IGarageScheduleService, GarageScheduleService>();

            services.AddScoped<IGarageSpecificationRepo, GarageSpecificationRepo>();
            services.AddScoped<IGarageSpecificationService, GarageSpecificationService>();

            services.AddScoped<IGarageDocumentRepo, GarageDocumentRepo>();
            services.AddScoped<IGarageDocumentService, GarageDocumentService>();


            services.AddScoped<IRestaurantImageRepo, RestaurantImageRepo>();
            services.AddScoped<IRestaurantImageService, RestaurantImageService>();

            services.AddScoped<IRestaurantServiceStaffRepo, RestaurantServiceStaffRepo>();
            services.AddScoped<IRestaurantServiceStaffService, RestaurantServiceStaffService>();
            //Garage Services end

            //SparePartsDealer Start
            services.AddScoped<ISparePartsDealerService, SparePartDealersService>();
            services.AddScoped<ISparePartDealRepo, SparePartsDealerRepo>();

            services.AddScoped<IDealerImageService, DealerImageService>();
            services.AddScoped<IDealerImageRepo, DealerImageRepo>();

            services.AddScoped<ISparePartDealerScheduleRepo, SparePartDealerScheduleRepo>();
            services.AddScoped<ISparePartDealerScheduleService, SparePartDealerScheduleService>();

            services.AddScoped<ISparePartDealerSpecificationRepo, SparePartDealerSpecificationRepo>();
            services.AddScoped<ISparePartDealerSpecificationService, SparePartDealerSpecificationService>();

            services.AddScoped<ISparePartsDealerDocumentRepo, SparePartsDealerDocumentRepo>();
            services.AddScoped<ISparePartsDealerDocumentService, SparePartsDealerDocumentService>();
            //SparePartsDealer End

            //Restaurant Start
            services.AddScoped<IRestaurantRepo, RestaurantRepo>();
            services.AddScoped<IRestaurantService, RestaurantService>();

            services.AddScoped<IRestaurantDocumentRepo, RestaurantDocumentRepo>();
            services.AddScoped<IRestaurantDocumentService, RestaurantDocumentService>();

            services.AddScoped<IRestaurantRatingRepo, RestaurantRatingRepo>();
            services.AddScoped<IRestaurantRatingService, RestaurantRatingService>();

            services.AddScoped<IRestaurantSubscriberRepo, RestaurantSubscriberRepo>();
            services.AddScoped<IRestaurantSubscriberService, RestaurantSubscriberService>();

            services.AddScoped<IRestaurantBranchRepo, RestaurantBranchRepo>();
            services.AddScoped<IRestaurantBranchService, RestaurantBranchService>();

            services.AddScoped<IMenuItemRepo, MenuItemRepo>();
            services.AddScoped<IMenuItemService, MenuItemService>();

            services.AddScoped<IMenuRepo, MenuRepo>();
            services.AddScoped<IMenuService, MenuService>();

            services.AddScoped<IMenuItemOptionRepo, MenuItemOptionRepo>();
            services.AddScoped<IMenuItemOptionService, MenuItemOptionService>();
            //Restaurant End

            services.AddScoped<ISupplierCouponRepo, SupplierCouponRepo>();
            services.AddScoped<ISupplierCouponService, SupplierCouponService>();

            //Service Staff Start
            services.AddScoped<IServiceStaffRepo, ServiceStaffRepo>();
            services.AddScoped<IServiceStaffService, ServiceStaffService>();
            //Service Staff End

            //Delivery Staff Start
            services.AddScoped<IDeliveryStaffRepo, DeliveryStaffRepo>();
            services.AddScoped<IDeliveryStaffService, DeliveryStaffService>();
            //Delivery Staff End

            //Restaurant Delivery Staff Start
            services.AddScoped<IRestaurantDeliveryStaffRepo, RestaurantDeliveryStaffRepo>();
            services.AddScoped<IRestaurantDeliveryStaffService, RestaurantDeliveryStaffService>();
            //Restaurant Delivery Staff End

            //Restaurant Cashier Staff Start
            services.AddScoped<IRestaurantCashierStaffRepo, RestaurantCashierStaffRepo>();
            services.AddScoped<IRestaurantCashierStaffService, RestaurantCashierStaffService>();
            //Restaurant Cashier Staff End

            //Restaurant Content Management Start
            services.AddScoped<IRestaurantContentManagementRepo, RestaurantContentManagementRepo>();
            services.AddScoped<IRestaurantContentManagementService, RestaurantContentManagementService>();
            //Restaurant Content Management End

            //Category Start
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            services.AddScoped<ICategoryService, CategoryService>();
            //Category End

            services.AddScoped<ICouponRedemptionRepo, CouponRedemptionRepo>();
            services.AddScoped<ICouponRedemptionService, CouponRedemptionService>();

            //Item Start
            services.AddScoped<IItemRepo, ItemRepo>();
            services.AddScoped<IItemService, ItemService>();
            //Item End

            //Coupon Start
            services.AddScoped<ICouponRepo, CouponRepo>();
            services.AddScoped<ICouponService, CouponService>();
            //Coupon End

            //Customer Start
            services.AddScoped<ICustomerRepo, CustomerRepo>();
            services.AddScoped<ICustomerService, CustomerService>();

            services.AddScoped<ICustomerAddressRepo, CustomerAddressRepo>();
            services.AddScoped<ICustomerAddressService, CustomerAddressService>();
            //Customer End

            // Customer Coupon Start
            services.AddScoped<IRestaurantCustomerRepo, RestaurantCustomerRepo>();
            services.AddScoped<IRestaurantCustomerService, RestaurantCustomerService>();
            //Customer Coupon End

            services.AddScoped<IRestaurantTransactionHistoryRepo, RestaurantTransactionHistoryRepo>();
            services.AddScoped<IRestaurantTransactionHistoryService, RestaurantTransactionHistoryService>();

            //Customer Coupon Start
            services.AddScoped<ICustomerCouponRepo, CustomerCouponRepo>();
            services.AddScoped<ICustomerCouponService, CustomerCouponService>();
            //Customer Coupon End


            //Customer Coupon Start
            services.AddScoped<ICouponCategoryRepo, CouponCategoryRepo>();
            services.AddScoped<ICouponCategoryService, CouponCategoryService>();
            //Customer Coupon End

            //Restaurant Banner Setting Start
            services.AddScoped<IRestaurantBannerSettingRepo, RestaurantBannerSettingRepo>();
            services.AddScoped<IRestaurantBannerSettingService, RestaurantBannerSettingService>();
            //Restaurant Banner Setting End

            //Restaurant Branch Start
            services.AddScoped<IRestaurantBranchRepo, RestaurantBranchRepo>();
            services.AddScoped<IRestaurantBranchService, RestaurantBranchService>();
            //Restaurant Branch End

            //Restaurant Branch Schedule Start
            services.AddScoped<IRestaurantBranchScheduleRepo, RestaurantBranchScheduleRepo>();
            services.AddScoped<Interfaces.IServices.Domains.IRestaurantBranchScheduleService, RestaurantBranchScheduleService>();
            //Restaurant Branch Schedule End

            //RestaurantServiceStaff Start
            services.AddScoped<IRestaurantServiceStaffRepo, RestaurantServiceStaffRepo>();
            services.AddScoped<Interfaces.IServices.Domains.IRestaurantServiceStaffService, RestaurantServiceStaffService>();
            //RestaurantServiceStaff End

            //RestaurantServiceStaff Start
            services.AddScoped<IOrderRepo, OrderRepo>();
            services.AddScoped<Interfaces.IServices.Domains.IOrderService, OrderService>();
            //RestaurantServiceStaff End

            //Item Option Start
            services.AddScoped<IItemOptionRepo, ItemOptionRepo>();
            services.AddScoped<IItemOptionService, ItemOptionService>();
            //Item Option  End

            //Item Option Value Start
            services.AddScoped<IItemOptionValueRepo, ItemOptionValueRepo>();
            services.AddScoped<IItemOptionValueService, ItemOptionValueService>();
            //Item Option Value End


            //Menu Item Option Value Start
            services.AddScoped<IMenuItemOptionValueRepo, MenuItemOptionValueRepo>();
            services.AddScoped<IMenuItemOptionValueService, MenuItemOptionValueService>();
            //Menu Item Option Value End

            //Order Detail Start
            services.AddScoped<IOrderDetailRepo, OrderDetailRepo>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
            //Order Detail End

            //Supplier
            services.AddScoped<ISupplierRepo, SupplierRepo>();
            services.AddScoped<ISupplierService, SupplierServices>();

            services.AddScoped<ISupplierDocumentRepo, SupplierDocumentRepo>();
            services.AddScoped<ISupplierDocumentService, SupplierDocumentService>();

            services.AddScoped<ISupplierItemRepo, SupplierItemRepo>();
            services.AddScoped<ISupplierItemService, SupplierItemService>();

            services.AddScoped<ISupplierItemImageRepo, SupplierItemImageRepo>();
            services.AddScoped<ISupplierItemImageService, SupplierItemImageService>();

            services.AddScoped<ISupplierOrderDetailRepo, SupplierOrderDetailRepo>();
            services.AddScoped<ISupplierOrderDetailService, SupplierOrderDetailService>();

            services.AddScoped<ISupplierOrderRepo, SupplierOrderRepo>();
            services.AddScoped<ISupplierOrderService, SupplierOrderService>();

            services.AddScoped<ISupplierItemCategoryRepo, SupplierItemCategoryRepo>();
            services.AddScoped<ISupplierItemCategoryService, SupplierItemCategoryService>();

            services.AddScoped<ISupplierOTPVerificationRepo, SupplierOTPVerificationRepo>();
            services.AddScoped<ISupplierOTPVerificationService, SupplierOTPVerificationService>();

            services.AddScoped<ISupplierPackageRepo, SupplierPackageRepo>();
            services.AddScoped<ISupplierPackageService, SupplierPackageService>();

            //Supplier End

            //Restaurant Start
            services.AddScoped<IRestaurantPrinterSettingRepo, RestaurantPrinterSettingRepo>();
            services.AddScoped<IRestaurantPrinterSettingService, RestaurantPrinterSettingService>();

            services.AddScoped<IRestaurantCardSchemeRepo, RestaurantCardSchemeRepo>();
            services.AddScoped<IRestaurantCardSchemeService, RestaurantCardSchemeService>();

            services.AddScoped<IRestaurantTaxSettingRepo, RestaurantTaxSettingRepo>();
            services.AddScoped<IRestaurantTaxSettingService, RestaurantTaxSettingService>();

            services.AddScoped<IRestaurantAggregatorRepo, RestaurantAggregatorRepo>();
            services.AddScoped<IRestaurantAggregatorService, RestaurantAggregatorService>();

            services.AddScoped<IRestaurantUserLogManagementRepo, RestaurantUserLogManagementRepo>();
            services.AddScoped<IRestaurantUserLogManagementService, RestaurantUserLogManagementService>();

            services.AddScoped<IRestaurantKitchenManagerRepo, RestaurantKitchenManagerRepo>();
            services.AddScoped<IRestaurantKitchenManagerService, RestaurantKitchenManagerService>();

            services.AddScoped<IRestaurantManagerRepo, RestaurantManagerRepo>();
            services.AddScoped<IRestaurantManagerService, RestaurantManagerService>();

            services.AddScoped<IRestaurantTableRepo, RestaurantTableRepo>();
            services.AddScoped<IRestaurantTableService, RestaurantTableService>();

            services.AddScoped<IRestaurantTableReservationRepo, RestaurantTableReservationRepo>();
            services.AddScoped<IRestaurantTableReservationService, RestaurantTableReservationService>();
            
            services.AddScoped<IRestaurantReservationRepo, RestaurantReservationRepo>();
            services.AddScoped<IRestaurantReservationService, RestaurantReservationService>();

            services.AddScoped<IRestaurantBalanceSheetRepo, RestaurantBalanceSheetRepo>();
            services.AddScoped<IRestaurantBalanceSheetService, RestaurantBalanceSheetService>();

            services.AddScoped<IRestaurantCashDenominationRepo, RestaurantCashDenominationRepo>();
            services.AddScoped<IRestaurantCashDenominationService, RestaurantCashDenominationService>();

            services.AddScoped<IRestaurantCashDenominationDetailRepo, RestaurantCashDenominationDetailRepo>();
            services.AddScoped<IRestaurantCashDenominationDetailService, RestaurantCashDenominationDetailService>();

            services.AddScoped<IRestaurantAggregatorWiseSaleRepo, RestaurantAggregatorWiseSaleRepo>();
            services.AddScoped<IRestaurantAggregatorWiseSaleService, RestaurantAggregatorWiseSaleService>();

            services.AddScoped<IRestaurantProductWiseSaleRepo, RestaurantProductWiseSaleRepo>();
            services.AddScoped<IRestaurantProductWiseSaleService, RestaurantProductWiseSaleService>();

            services.AddScoped<IRestaurantCategoryWiseSaleRepo, RestaurantCategoryWiseSaleRepo>();
            services.AddScoped<IRestaurantCategoryWiseSaleService, RestaurantCategoryWiseSaleService>();

            services.AddScoped<IGarageAwardRepo, GarageAwardRepo>();
            services.AddScoped<IGarageAwardService, GarageAwardService>();




            //Restaurant End

            //Aggregator Start
            services.AddScoped<IAggregatorRepo, AggregatorRepo>();
            services.AddScoped<IAggregatorService, AggregatorService>();
            //Aggregator End

            //CardScheme Start
            services.AddScoped<ICardSchemeRepo, CardSchemeRepo>();
            services.AddScoped<ICardSchemeService, CardSchemeService>();
            //CardScheme End

            //CurrencyNote Start
            services.AddScoped<ICurrencyNoteRepo, CurrencyNoteRepo>();
            services.AddScoped<ICurrencyNoteService, CurrencyNoteService>();
            //CurrencyNote End

            services.AddScoped<ISupplierPackageRepo, SupplierPackageRepo>();
            services.AddScoped<ISupplierPackageService, SupplierPackageService>();

            services.AddScoped<ICustomerTransactionHistoryRepo, CustomerTransactionHistoryRepo>();
            services.AddScoped<ICustomerTransactionHistoryService, CustomerTransactionHistoryService>();

            services.AddScoped<ITicketRepo, TicketRepo>();
            services.AddScoped<ITicketService, TicketService>();

            services.AddScoped<IRestaurantCouponRepo, RestaurantCouponRepo>();
            services.AddScoped<IRestaurantCouponService, RestaurantCouponService>();

            services.AddScoped<ISupplierCouponCategoryRepo, SupplierCouponCategoryRepo>();
            services.AddScoped<ISupplierCouponCategoryService, SupplierCouponCategoryService>();

            services.AddScoped<ISupplierCouponRedemptionRepo, SupplierCouponRedemptionRepo>();
            services.AddScoped<ISupplierCouponRedemptionService, SupplierCouponRedemptionService>();

            services.AddScoped<ISparePartRequestQuoteRepo, SparePartRequestQuoteRepo>();
            services.AddScoped<ISparePartRequestQuoteService, SparePartRequestQuoteService>();

            services.AddScoped<ISparePartRequestQuoteImageRepo, SparePartRequestQuoteImageRepo>();
            services.AddScoped<ISparePartRequestQuoteImageService, SparePartRequestQuoteImageService>();

            //Survey
            services.AddScoped<ISurveyRepo, SurveyRepo>();
            services.AddScoped<ISurveyService, SurveyService>();

            services.AddScoped<ISurveyQuestionRepo, SurveyQuestionRepo>();
            services.AddScoped<ISurveyQuestionService, SurveyQuestionService>();

            services.AddScoped<ISurveyOptionRepo, SurveyOptionRepo>();
            services.AddScoped<ISurveyOptionService, SurveyOptionService>();
            //End Survey
            services.AddScoped<ICustomerFeedbackService, CustomerFeedbackService>();
            services.AddScoped<ICustomerFeedbackRepo, CustomerFeedbackRepo>();



            //Garage
            services.AddScoped<IGarageBannerSettingRepo, GarageBannerSettingRepo>();
            services.AddScoped<IGarageBannerSettingService, GarageBannerSettingService>();

            services.AddScoped<IGarageContentManagementRepo, GarageContentManagementRepo>();
            services.AddScoped<IGarageContentManagementService, GarageContentManagementService>();

            services.AddScoped<IGarageMenuRepo, GarageMenuRepo>();
            services.AddScoped<IGarageMenuService, GarageMenuService>();

            services.AddScoped<IGarageMenuManagementRepo, GarageMenuManagementRepo>();
            services.AddScoped<IGarageMenuManagementService, GarageMenuManagementService>();

            services.AddScoped<IGarageServiceManagementRepo, GarageServiceManagementRepo>();
            services.AddScoped<IGarageServiceManagementService, GarageServiceManagementService>();

            services.AddScoped<IGarageTeamManagementRepo, GarageTeamManagementRepo>();
            services.AddScoped<IGarageTeamManagementService, GarageTeamManagementService>();

            services.AddScoped<IExpertiseRepo, ExpertiseRepo>();
            services.AddScoped<IExpertiseService, ExpertiseService>();

            services.AddScoped<IGarageExpertiseManagementRepo, GarageExpertiseManagementRepo>();
            services.AddScoped<IGarageExpertiseManagementService, GarageExpertiseManagementService>();

            services.AddScoped<IGarageExpertiseRepo, GarageExpertiseRepo>();
            services.AddScoped<IGarageExpertiseService, GarageExpertiseService>();

            services.AddScoped<IGarageTestimonialsRepo, GarageTestimonialsRepo>();
            services.AddScoped<IGarageTestimonialsService, GarageTestimonialsService>();

            services.AddScoped<IGarageBlogRepo, GarageBlogRepo>();
            services.AddScoped<IGarageBlogService, GarageBlogService>();

            services.AddScoped<IGarageSubscribersRepo, GarageSubscribersRepo>();
            services.AddScoped<IGarageSubscribersService, GarageSubscribersService>();

            services.AddScoped<IGaragePartnersManagementRepo, GaragePartnersManagementRepo>();
            services.AddScoped<IGaragePartnersManagementService, GaragePartnersManagementService>();

            services.AddScoped<IGarageAppointmentManagementRepo, GarageAppointmentManagementRepo>();
            services.AddScoped<IGarageAppointmentManagementService, GarageAppointmentManagementService>();

            services.AddScoped<IGarageFAQRepo, GarageFAQRepo>();
            services.AddScoped<IGarageFAQService, GarageFAQService>();

            services.AddScoped<IGarageCustomerAppointmentRepo, GarageCustomerAppointmentRepo>();
            services.AddScoped<IGarageCustomerAppointmentService, GarageCustomerAppointmentService>();

            services.AddScoped<IGarageCareersRepo, GarageCareersRepo>();
            services.AddScoped<IGarageCareersService, GarageCareersService>();

            services.AddScoped<IGarageCustomerFeedbackRepo, GarageCustomerFeedbackRepo>();
            services.AddScoped<IGarageCustomerFeedbackService, GarageCustomerFeedbackService>();

            services.AddScoped<IGarageCustomerRepo, GarageCustomerRepo>();
            services.AddScoped<IGarageCustomerService, GarageCustomerService>();

            services.AddScoped<IRestaurantWaiterRepo, RestaurantWaiterRepo>();
            services.AddScoped<IRestaurantWaiterService, RestaurantWaiterService>();

            services.AddScoped<ISparePartTransactionHistoryRepo, SparePartTransactionHistoryRepo>();
            services.AddScoped<ISparePartTransactionHistoryService, SparePartTransactionHistoryService>();
            services.AddScoped<IGarageProjectRepo, GarageProjectRepo>();
            services.AddScoped<IGarageProjectService, GarageProjectService>();

            services.AddScoped<IGarageProjectImageRepo, GarageProjectImageRepo>();
            services.AddScoped<IGarageProjectImageService, GarageProjectImageService>();



            services.AddScoped<IGarageBusinessSettingRepo, GarageBusinessSettingRepo>();
            services.AddScoped<IGarageBusinessSettingService, GarageBusinessSettingService>();

            services.AddScoped<IGarageBranchBusinessSettingRepo, GarageBranchBusinessSettingRepo>();
            services.AddScoped<IGarageBranchBusinessSettingService, GarageBranchBusinessSettingService>();

            services.AddScoped<ISparePartTransactionHistoryRepo, SparePartTransactionHistoryRepo>();
            services.AddScoped<ISparePartTransactionHistoryService, SparePartTransactionHistoryService>();

            //Garage END

            //SpareParts Starts
            services.AddScoped<ISparePartAppointmentManagementRepo, SparePartAppointmentManagementRepo>();
            services.AddScoped<ISparePartAppointmentManagementService, SparePartAppointmentManagementService>();

            services.AddScoped<ISparePartBannerSettingRepo, SparePartBannerSettingRepo>();
            services.AddScoped<ISparePartBannerSettingService, SparePartBannerSettingService>();

            services.AddScoped<ISparePartBlogRepo, SparePartBlogRepo>();
            services.AddScoped<ISparePartBlogService, SparePartBlogService>();

            services.AddScoped<ISparePartCareerRepo, SparePartCareerRepo>();
            services.AddScoped<ISparePartCareerService, SparePartCareerService>();

            services.AddScoped<ISparePartBusinessSettingRepo, SparePartBusinessSettingRepo>();
            services.AddScoped<ISparePartBusinessSettingService, SparePartBusinessSettingService>();

            services.AddScoped<IGarageBranchBusinessSettingRepo, GarageBranchBusinessSettingRepo>();
            services.AddScoped<IGarageBranchBusinessSettingService, GarageBranchBusinessSettingService>();

            services.AddScoped<ISparePartContentManagementRepo, SparePartContentManagementRepo>();
            services.AddScoped<ISparePartContentManagementService, SparePartContentManagementService>();

            services.AddScoped<ISparePartCustomerAppointmentRepo, SparePartCustomerAppointmentRepo>();
            services.AddScoped<ISparePartCustomerAppointmentService, SparePartCustomerAppointmentService>();

            services.AddScoped<ISparePartCustomerFeedbackRepo, SparePartCustomerFeedbackRepo>();
            services.AddScoped<ISparePartCustomerFeedbackService, SparePartCustomerFeedbackService>();

            services.AddScoped<ISparePartExpertiseRepo, SparePartExpertiseRepo>();
            services.AddScoped<ISparePartExpertiseService, SparePartExpertiseService>();

            services.AddScoped<ISparePartExpertiseManagementRepo, SparePartExpertiseManagementRepo>();
            services.AddScoped<ISparePartExpertiseManagementService, SparePartExpertiseManagementService>();

            services.AddScoped<ISparePartMenuRepo, SparePartMenuRepo>();
            services.AddScoped<ISparePartMenuService, SparePartMenuService>();

            services.AddScoped<ISparePartPartnersManagementRepo, SparePartPartnersManagementRepo>();
            services.AddScoped<ISparePartPartnersManagementService, SparePartPartnersManagementService>();

            services.AddScoped<ISparePartServiceManagementRepo, SparePartServiceManagementRepo>();
            services.AddScoped<ISparePartServiceManagement, SparePartServiceManagementService>();

            services.AddScoped<ISparePartSubscriberRepo, SparePartSubscriberRepo>();
            services.AddScoped<ISparePartSubscriberService, SparePartSubscriberService>();

            services.AddScoped<ISparePartTeamManagementRepo, SparePartTeamManagementRepo>();
            services.AddScoped<ISparePartTeamManagementService, SparePartTeamManagementService>();

            services.AddScoped<ISparePartTestimonialRepo, SparePartTestimonialRepo>();
            services.AddScoped<ISparePartTestimonialService, SparePartTestimonialService>();

            services.AddScoped<ISparePartMenuManagementRepo, SparePartMenuManagementRepo>();
            services.AddScoped<ISparePartMenuManagementService, SparePartMenuManagementService>();

            services.AddScoped<ISparePartServiceManagementRepo, SparePartServiceManagementRepo>();
            services.AddScoped<ISparePartServiceManagement, SparePartServiceManagementService>();

            services.AddScoped<ISparePartTeamManagementRepo, SparePartTeamManagementRepo>();
            services.AddScoped<ISparePartTeamManagementService, SparePartTeamManagementService>();

            services.AddScoped<ISparePartBranchBusinessSettingRepo, SparePartBranchBusinessSettingRepo>();
            services.AddScoped<ISparePartBranchBusinessSettingService, SparePartBranchBusinessSettingService>();

            services.AddScoped<ISparePartFAQRepo, SparePartFAQRepo>();
            services.AddScoped<ISparePartFAQService, SparePartFAQService>();


            services.AddScoped<IGarageProjectImageRepo, GarageProjectImageRepo>();
            services.AddScoped<IGarageProjectImageService, GarageProjectImageService>();

            services.AddScoped<IClientIndustriesRepo, ClientIndustriesRepo>();
            services.AddScoped<IClientIndustriesService, ClientIndustriesService>();

            services.AddScoped<IClientSectionsRepo, ClientSectionsRepo>();
            services.AddScoped<IClientSectionsService, ClientSectionsService>();

            services.AddScoped<IClientTypesRepo, ClientTypesRepo>();
            services.AddScoped<IClientTypesService, ClientTypesService>();

            //SpareParts END

            //Vendor Start
            services.AddScoped<IVendorRepo, VendorRepo>();
            services.AddScoped<IVendorService, VendorService>();
            services.AddScoped<IVendorDocumentRepo, VendorDocumentRepo>();
            services.AddScoped<IVendorDocumentService, VendorDocumentService>();
            services.AddScoped<IVendorOTPVerificationRepo, VendorOTPVerificationRepo>();
            services.AddScoped<IVendorOTPVerificationService, VendorOTPVerificationService>();
            //Vendor END

            //Module Start
            services.AddScoped<IModuleRepo, ModuleRepo>();
            services.AddScoped<IModuleService, ModuleService>();
            //Module End

            //Client Module Purchases Start
            services.AddScoped<IClientModulePurchasesRepo, ClientModulePurchasesRepo>();
            services.AddScoped<IClientModulePurchasesService, ClientModulePurchasesService>();
            services.AddScoped<IModulePurchaseDetailsRepo, ModulePurchaseDetailsRepo>();
            services.AddScoped<IModulePurchaseDetailsService, ModulePurchaseDetailsService>();
            services.AddScoped<IClientModulesRepo, ClientModulesRepo>();
            services.AddScoped<IClientModulesService, ClientModulesService>();
            services.AddScoped<IClientModulePurchaseTransactionsRepo, ClientModulePurchaseTransactionsRepo>();
            services.AddScoped<IClientModulePurchaseTransactionsService, ClientModulePurchaseTransactionsService>();
            //Client Module Purchases End

            //Client Content Start
            services.AddScoped<IClientContentMediaRepo, ClientContentMediaRepo>();
            services.AddScoped<IClientContentMediaService, ClientContentMediaService>();
            services.AddScoped<IClientDomainSuggestionsRepo, ClientDomainSuggestionsRepo>();
            services.AddScoped<IClientDomainSuggestionsService, ClientDomainSuggestionsService>();
            services.AddScoped<IClientEmailsRepo, ClientEmailsRepo>();
            services.AddScoped<IClientEmailsService, ClientEmailsService>();
            //Client Content End


            //To avoid the MultiPartBodyLength error
            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else if (env.IsProduction())
            {
                //Swagger authorization setup
                app.UseMiddleware<SwaggerAuthMiddleware>();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));

            app.ConfigureExceptionHandler();

            app.UseCors(builder =>
            {
                builder.WithOrigins("https://localhost:4200", "http://localhost:4200", "http://localhost:3000", "http://*.localhost:4200",
                    "https://localhost:44378","http://localhost:3000"
                    , "https://www.consulttous.webbeesite.com", "https://consulttous.webbeesite.com"
                    , "https://www.dwt.webbeesite.com", "https://dwt.webbeesite.com"
                    , "https://www.demo.webbeesite.com", "https://demo.webbeesite.com"
                    , "https://www.dwtads.webbeesite.com", "https://dwtads.webbeesite.com"
                    , "https://www.dwtprinters.webbeesite.com", "https://dwtprinters.webbeesite.com"
                    , "https://www.wesetupbusiness.webbeesite.com", "https://wesetupbusiness.webbeesite.com"
                    , "https://www.transfirstuae.com", "https://transfirstuae.com" 
                    , "https://transfirstuae.azurewebsites.net", "https://www.transfirstuae.azurewebsites.net"
                    , "https://dwt-webbeesite.azurewebsites.net" , "https://www.dwt-webbeesite.azurewebsites.net"
                    , "https://shameerhussain.com" , "https://www.shameerhussain.com", "https://webbeesite.com", "https://www.webbeesite.com", "https://demo.demowbs.com", "https://www.demo.demowbs.com")
                       //.SetIsOriginAllowedToAllowWildcardSubdomains()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
