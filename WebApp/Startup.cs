using DynamicAuthorization.Mvc.Core.Extensions;
using HelperClasses.Classes;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Handlers;
using WebApp.Helpers;
using WebApp.Interfaces;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces.TypedClients;
using WebApp.Middleware;
using WebApp.Services;
using WebApp.Services.TypedClients;

namespace WebApp
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configuration for all session cookies except for Auth cookie.
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;

                options.MinimumSameSitePolicy = SameSiteMode.Strict; // cookie is only for a single site
                options.Secure = CookieSecurePolicy.Always; // always over ssl
                options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always; // no need to access cookie in javascript                
            });

            string keysFolder = Path.Combine(_environment.ContentRootPath, "keys");

            services.AddDataProtection()
                   .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
                   .SetApplicationName("FougitoWebApp")
                   .SetDefaultKeyLifetime(TimeSpan.FromDays(90));

            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = options.LoginPath;
                options.Events.OnValidatePrincipal = CookieHelper.ValidateAsync;
                options.Cookie.Name = "FougitoAppAuthCookie";
                options.Cookie.HttpOnly = true; // no need to access cookie in javascript
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // always over ssl
                options.Cookie.SameSite = SameSiteMode.Strict; // cookie is only for a single site

                // How long the authentication ticket (containing claims etc.) will remain valid
                // options.Cookie.Expires is ignored in 3.1. On live hosting, pool recycles earlier on idle, so this value 
                // becomes meaningless on that case.
                options.ExpireTimeSpan = TimeSpan.FromDays(90);
                options.Cookie.MaxAge = options.ExpireTimeSpan;
            });

            services.AddScoped<CookieRedirectionEvents>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole(Roles.Admin.ToString()));
                options.AddPolicy("B2B Manager", policy => policy.RequireRole("B2B Manager"));
                options.AddPolicy("Automobile Manager", policy => policy.RequireRole("Automobile Manager"));
                options.AddPolicy("Restaurant Manager", policy => policy.RequireRole("Restaurant Manager"));
                options.AddPolicy("GarageOwner", policy => policy.RequireRole("GarageOwner"));
                options.AddPolicy("SparePartDealer", policy => policy.RequireRole("SparePartDealer"));
                options.AddPolicy("Vendor", policy => policy.RequireRole("Vendor"));
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddSession(options =>
            {
                options.Cookie.Name = "FougitoWebAppSession";

                // May be
                options.Cookie.IsEssential = true;

                // Cannot do this here.
                //options.Cookie.Expiration = TimeSpan.MaxValue;

                options.Cookie.MaxAge = TimeSpan.MaxValue;

                // Session should never expire due to NO interaction with website
                options.IdleTimeout = TimeSpan.MaxValue;
            });

            services.AddControllersWithViews().AddNewtonsoftJson();
            services.AddRazorPages()
                        .AddNewtonsoftJson()
                        .AddRazorPagesOptions(options =>
                        {
                            //    options.Conventions.AuthorizeFolder("/");
                            //    options.Conventions.AllowAnonymousToPage("/SignIn");
                            //    options.Conventions.AllowAnonymousToPage("/Register");
                        });




            // Important to add this after Controllers,Views, and Pages and before all the objects that might use it.
            services.AddHttpContextAccessor();

            services.AddSingleton<ITokenManager, TokenManager>();
            services.AddSingleton<IUserSessionManager, UserSessionManager>();
            services.AddSingleton<IHttpClientHelper, HttpClientHelper>();
            services.AddTransient<IJSONHelper, JSONHelper>();

            services.AddTransient<PortalDelegatingHandler>();

            services.AddHttpClient("RegularClient", client =>
            {
                client.BaseAddress = new Uri(Configuration.GetValue<string>("ApiURL"));
            })
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
                })
                .AddHttpMessageHandler<PortalDelegatingHandler>()
                .AddTypedClient<IFileUpload, FileUpload>()
                .AddTypedClient<IAuthenticateClient, AuthenticateClient>()
                .AddTypedClient<IUserClient, UserClient>()
                .AddTypedClient<ICategoryClient, CategoryClient>()
                .AddTypedClient<ICouponClient, CouponClient>()
                .AddTypedClient<ICustomerCouponClient, CustomerCouponClient>()
                .AddTypedClient<IAdminAccountClient, AdminAccountClient>()
                .AddTypedClient<ICarMakeClient, CarMakeClient>()
                .AddTypedClient<IRestaurantDeliveryStaffClient, RestaurantDeliveryStaffClient>()
                .AddTypedClient<IRestaurantCashierStaffClient, RestaurantCashierStaffClient>()
                .AddTypedClient<ICityClient, CityClient>()
                .AddTypedClient<IItemClient, ItemClient>()
                .AddTypedClient<ICountryClient, CountriesClient>()
                .AddTypedClient<ICarModelClient, CarModelClient>()
                .AddTypedClient<IAreaClient, AreaClient>()
                .AddTypedClient<IServiceStaffClient, ServiceStaffClient>()
                .AddTypedClient<IDeliveryStaffClient, DeliveryStaffClient>()
                .AddTypedClient<IBusinessSettingClient, BusinessSettingClient>()
                .AddTypedClient<IIntegrationSettingClient, IntegrationSettingClient>()
                .AddTypedClient<ISubscriberClient, SubscriberClient>()
                .AddTypedClient<IResturantClient, ResturantClient>()
                .AddTypedClient<INotificationClient, NotificationClient>()
                .AddTypedClient<INotificationClient, NotificationClient>()
                .AddTypedClient<IBlogClient, BlogClient>()
                .AddTypedClient<IBlogCategoryClient, BlogCategoryClient>()
                .AddTypedClient<IUserRoleClient, UserRoleClient>()
                .AddTypedClient<ICustomerClient, CustomerClient>()
                .AddTypedClient<ICustomerFeedbackClient, CustomerFeedbackClient>()
                .AddTypedClient<IGarageClient, GarageClient>()
                .AddTypedClient<ISparePartsDealerClient, SparePartsDealerClient>()
                .AddTypedClient<ICustomerClient, CustomerClient>()
                .AddTypedClient<ICouponCategoryClient, CouponCategoryClient>()
                .AddTypedClient<IRestaurantBannerSettingClient, RestaurantBannerSettingClient>()
                .AddTypedClient<IRestaurantBranchClient, RestaurantBranchClient>()
                .AddTypedClient<IRestaurantBranchScheduleClient, RestaurantBranchScheduleClient>()
                .AddTypedClient<IRestaurantRatingClient, RestaurantRatingClient>()
                .AddTypedClient<IGarageRatingClient, GarageRatingClient>()
                .AddTypedClient<IRestaurantClient, RestaurantClient>()
                .AddTypedClient<IGarageRatingClient, GarageRatingClient>()
                .AddTypedClient<IDashboardClient, DashboardClient>()
                .AddTypedClient<IItemOptionClient, ItemOptionClient>()
                .AddTypedClient<IItemOptionValueClient, ItemOptionValueClient>()
                .AddTypedClient<IMenuItemOptionValueClient, MenuItemOptionValueClient>()
                .AddTypedClient<IMenuItemOptionClient, MenuItemOptionClient>()
                .AddTypedClient<IMenuItemClient, MenuItemClient>()
                .AddTypedClient<IMenuClient, MenuClient>()
                .AddTypedClient<IRestaurantDocumentClient, RestaurantDocumentClient>()
                .AddTypedClient<IRestaurantOrderClient, RestaurantOrderClient>()
                .AddTypedClient<IRestaurantDashboardClient, RestaurantDashboardClient>()
                .AddTypedClient<IRestaurantContentManagementClient, RestaurantContentManagementClient>()
                .AddTypedClient<IOrderDetailClient, OrderDetailClient>()
                .AddTypedClient<IRestaurantServiceStaffClient, RestaurantServiceStaffClient>()
                .AddTypedClient<ITicketClient, TicketClient>()
                .AddTypedClient<ITicketMessageClient, TicketMessageClient>()
                .AddTypedClient<ICustomerTransactionHistoryClient, CustomerTransactionHistoryClient>()
                .AddTypedClient<ISupplierAccountClient, SupplierAccountClient>()
                .AddTypedClient<ISupplierClient, SupplierClient>()
                .AddTypedClient<ISupplierDocumentClient, SupplierDocumentClient>()
                .AddTypedClient<ISupplierOrderClient, SupplierOrderClient>()
                .AddTypedClient<ISupplierCouponClient, SupplierCouponClient>()
                .AddTypedClient<ISupplierPackageClient, SupplierPackageClient>()
                .AddTypedClient<ISupplierItemClient, SupplierItemClient>()
                .AddTypedClient<ISupplierCategoryClient, SupplierCategoryClient>()
                .AddTypedClient<ISupplierItemCategoryClient, SupplierItemCategoryClient>()
                .AddTypedClient<IRestaurantCouponClient, RestaurantCouponClient>()
                .AddTypedClient<ISupplierDashboardClient, SupplierDashboardClient>()
                .AddTypedClient<IRestaurantSubscriberClient, RestaurantSubscriberClient>()
                .AddTypedClient<IRestaurantPrinterSettingClient, RestaurantPrinterSettingClient>()
                .AddTypedClient<IRestaurantTaxSettingClient, RestaurantTaxSettingClient>()
                .AddTypedClient<IRestaurantKitchenManagerClient, RestaurantKitchenManagerClient>()
                .AddTypedClient<IRestaurantManagerClient, RestaurantManagerClient>()
                .AddTypedClient<IRestaurantTableClient, RestaurantTableClient>()
                .AddTypedClient<ISupplierCouponRedemptionClient, SupplierCouponRedemptionClient>()
                .AddTypedClient<IRestaurantSubscriberClient, RestaurantSubscriberClient>()
                .AddTypedClient<IGarageDocumentClient, GarageDocumentClient>()
                .AddTypedClient<IGarageScheduleClient, GarageScheduleClient>()
                .AddTypedClient<IGarageImageClient, GarageImageClient>()
                .AddTypedClient<IGarageRepairClient, GarageRepairClient>()
                .AddTypedClient<IRestaurantSubscriberClient, RestaurantSubscriberClient>()
                .AddTypedClient<ISparePartsDocumentClient, SparePartsDealerDocumentClient>()
                .AddTypedClient<ISparePartsDealerScheduleClient, SparePartsDealerScheduleClient>()
                .AddTypedClient<ISparePartsDealerInventorySpecificationClient, SparePartsDealersInventorySpecificationClient>()
                .AddTypedClient<ISparePartsDealerImageClient, SparePartsDealerImageClient>()
                .AddTypedClient<ICustomerRestaurantClient, CustomerRestaurantClient>()
                .AddTypedClient<IGarageAndSparePartDealerAccountClient, GarageAndSparePartDealerAccountClient>()
                .AddTypedClient<ISurveyClient, SurveyClient>()
                .AddTypedClient<ISurveyOptionClient, SurveyOptionClient>()
                .AddTypedClient<ISurveyQuestionClient, SurveyQuestionClient>()
                .AddTypedClient<IAggregatorClient, AggregatorClient>()
                .AddTypedClient<ICardSchemeClient, CardSchemeClient>()
                .AddTypedClient<IGarageBannerSettingClient, GarageBannerSettingClient>()
                .AddTypedClient<IGarageContentManagementClient, GarageContentManagementClient>()
                .AddTypedClient<IGarageMenuClient, GarageMenuClient>()
                .AddTypedClient<IGarageMenuManagementClient, GarageMenuManagementClient>()
                .AddTypedClient<IGarageServiceManagementClient, GarageServiceManagementClient>()
                .AddTypedClient<IGarageTeamManagementClient, GarageTeamManagementClient>()
                .AddTypedClient<IExpertiseClient, ExpertiseClient>()
                .AddTypedClient<IGarageExpertiseManagementClient, GarageExpertiseManagementClient>()
                .AddTypedClient<IGarageExpertiseClient, GarageExpertiseClient>()
                .AddTypedClient<IGarageTestimonialsClient, GarageTestimonialsClient>()
                .AddTypedClient<IGarageBlogClient, GarageBlogClient>()
                .AddTypedClient<IGarageSubscribersClient, GarageSubscribersClient>()
                .AddTypedClient<IGaragePartnersManagementClient, GaragePartnersManagementClient>()
                .AddTypedClient<IGarageBranchBusinessSettingClient, GarageBranchBusinessSettingClient>()
                .AddTypedClient<IGarageCustomerAppointmentClient, GarageCustomerAppointmentClient>()
                .AddTypedClient<IGarageCareersClient, GarageCareersClient>()
                .AddTypedClient<IGarageCustomerFeedbackClient, GarageCustomerFeedbackClient>()
                .AddTypedClient<ICardSchemeClient, CardSchemeClient>()
                .AddTypedClient<IRestaurantUserLogManagementClient, RestaurantUserLogManagementClient>()
                .AddTypedClient<IRestaurantBalanceSheetClient, RestaurantBalanceSheetClient>()
                .AddTypedClient<IRestaurantWaiterClient, RestaurantWaiterClient>()
                .AddTypedClient<IGarageAppointmentManagementClient, GarageAppointmentManagementClient>()
                .AddTypedClient<IGarageProjectClient, GarageProjectClient>()
                .AddTypedClient<IGarageBusinessSettingClient, GarageBusinessSettingClient>()
                .AddTypedClient<IGarageAwardClient, GarageAwardClient>()
                .AddTypedClient<IGarageProjectImageClient, GarageProjectImageClient>()
                .AddTypedClient<IGarageDashboardClient, GarageDashboardClient>()
                .AddTypedClient<IGarageCustomerClient, GarageCustomerClient>()
                .AddTypedClient<ISparePartAppointmentManagementClient, SparePartAppointmentManagementClient>()
                .AddTypedClient<ISparePartBannerSettingClient, SparePartBannerSettingClient>()
                .AddTypedClient<ISparePartBlogClient, SparePartBlogClient>()
                .AddTypedClient<ISparePartCareerClient, SparePartCareerClient>()
                .AddTypedClient<ISparePartContentManagementClient, SparePartContentManagementClient>()
                .AddTypedClient<ISparePartCustomerAppointmentClient, SparePartCustomerAppointmentClient>()
                .AddTypedClient<ISparePartCustomerFeedbackClient, SparePartCustomerFeedbackClient>()
                .AddTypedClient<ISparePartExpertiseClient, SparePartExpertiseClient>()
                .AddTypedClient<ISparePartExpertiseManagementClient, SparePartExpertiseManagementClient>()
                .AddTypedClient<ISparePartMenuClient, SparePartMenuClient>()
                .AddTypedClient<ISparePartMenuManagementClient, SparePartMenuManagementClient>()
                .AddTypedClient<ISparePartPartnersManagementClient, SparePartPartnersManagementClient>()
                .AddTypedClient<ISparePartServiceManagementClient, SparePartServiceManagementClient>()
                .AddTypedClient<ISparePartSubscriberClient, SparePartSubscriberClient>()
                .AddTypedClient<ISparePartTeamManagementClient, SparePartTeamManagementClient>()
                .AddTypedClient<ISparePartTestimonialClient, SparePartTestimonialClient>()
                .AddTypedClient<IBlogCategoryClient, BlogCategoryClient>()
                .AddTypedClient<ISparePartBusinessSettingClient, SparePartBusinessSettingClient>()
                .AddTypedClient<ISparePartBranchBusinessSettingClient, SparePartBranchBusinessSettingClient>()
                .AddTypedClient<IGarageFAQClient, GarageFAQClient>()
                .AddTypedClient<ISparePartDashboardClient, SparePartDashboardClient>()
                .AddTypedClient<ISparePartFAQClient, SparePartFAQClient>()
                .AddTypedClient<IClientTypesClient, ClientTypesClient>()
                .AddTypedClient<IClientSectionsClient, ClientSectionsClient>()
                .AddTypedClient<IClientIndustriesClient, ClientIndustriesClient>()
                .AddTypedClient<IVendorAccountClient, VendorAccountClient>()
                .AddTypedClient<IVendorClient, VendorClient>()
                .AddTypedClient<IVendorDocumentClient, VendorDocumentClient>()
                .AddTypedClient<IModuleClient, ModuleClient>()
                .AddTypedClient<IModulePurchaseDetailsClient, ModulePurchaseDetailsClient>()
                .AddTypedClient<IClientModulePurchasesClient, ClientModulePurchasesClient>()
                .AddTypedClient<IClientContentMediaClient, ClientContentMediaClient>()
                .AddTypedClient<IClientDomainSuggestionsClient, ClientDomainSuggestionsClient>()
                .AddTypedClient<IClientEmailsClient, ClientEmailsClient>()
                .AddTypedClient<IClientModulesClient, ClientModulesClient>()
                .AddTypedClient<IClientModulePurchaseTransactionsClient, ClientModulePurchaseTransactionsClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseMiddleware<RedirectingMiddleware>();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapDefaultControllerRoute();
                //name: "default",
                //pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}"
          );
                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "{area:exists}/{controller}/{action}/{id?}"
                    );

                //endpoints.MapAreaControllerRoute(
                //   name: "Restaurant",
                //   areaName: "Restaurant",
                //   pattern: "{area:exists}/{controller}/{action}/{id?}"
                //   );

                //endpoints.MapAreaControllerRoute(
                //  name: "Supplier",
                //  areaName: "Supplier",
                //  pattern: "{area:exists}/{controller}/{action}/{id?}"
                //  );

                endpoints.MapAreaControllerRoute(
                 name: "Client",
                 areaName: "Client",
                 pattern: "{area:exists}/{controller}/{action}/{id?}"
                 );
                endpoints.MapAreaControllerRoute(
                name: "Vendor",
                areaName: "Vendor",
                pattern: "{area:exists}/{controller}/{action}/{id?}"
                );

                //endpoints.MapAreaControllerRoute(
                //  name: "SparePart",
                //  areaName: "SparePart",
                //  pattern: "{area:exists}/{controller}/{action}/{id?}"
                //);

            });
        }
    }
}
