using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Menu;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.Restaurant.Filter;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class RestaurantRepo : Repository<Restaurant>, IRestaurantRepo
    {
        private new readonly FougitoContext _context;
        private readonly IMapper _mapper;
        public RestaurantRepo(FougitoContext context, ILoggerManager logger, IMapper mapper) : base(context, logger)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<RestaurantCardResponseDTO> GetAllNearMe(RestaurantFilter Filter)
        {
            var result = _context.RestaurantBranches.Include(x => x.Restaurant).ThenInclude(x => x.RestaurantRatings)
                .Where(x => x.Restaurant.Status == Enum.GetName(typeof(Status), Status.Active) && x.Status == Enum.GetName(typeof(Status), Status.Active)).AsQueryable();

            if (!string.IsNullOrEmpty(Filter.Paging.Search))
                result = result.Where(x => EF.Functions.Like(x.Restaurant.NameAsPerTradeLicense, "%" + Filter.Paging.Search + "%"));


            var List = result.Include(x => x.BranchSchedules).AsEnumerable().Select(x => new RestaurantCardResponseDTO
            {
                Id = x.Id,
                RestaurantId = x.RestaurantId,
                DeliveryTime = x.DeliveryMinutes,
                ServiceDistance = x.ServiceDistance,
                Logo = !string.IsNullOrEmpty(x.Restaurant.Logo) ? x.Restaurant.Logo.Replace(" ", "%20") : "",
                NameAsPerTradeLicense = x.Restaurant.NameAsPerTradeLicense,
                Address = x.Address,
                Distance = DistanceHelper.DistanceTo((double)Filter.Latitude, (double)Filter.Longitude, (double)x.Latitude, (double)x.Longitude).ToString("n2") + " Km",
                OpeningTime = x.BranchSchedules.Where(x => x.Day == DateTime.Now.DayOfWeek.ToString()).Any() ?
                              (DateTime.Today + x.BranchSchedules.Where(x => x.Day == DateTime.Now.DayOfWeek.ToString()).FirstOrDefault().OpeningTime).ToShortTimeString() : null,
                ClosingTime = x.BranchSchedules.Where(x => x.Day == DateTime.Now.DayOfWeek.ToString()).Any() ?
                              (DateTime.Today + x.BranchSchedules.Where(x => x.Day == DateTime.Now.DayOfWeek.ToString()).FirstOrDefault().ClosingTime).ToShortTimeString() : null,
                AvgRating = x.Restaurant.RestaurantRatings.Any() ? x.Restaurant.RestaurantRatings.Average(x => x.Rating) : 0,
                RatingCount = x.Restaurant.RestaurantRatings.Count,
                Slug = !string.IsNullOrEmpty(x.Slug) ? x.Slug : "",
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                ThumbnailImage = !string.IsNullOrEmpty(x.Restaurant.ThumbnailImage) ? x.Restaurant.ThumbnailImage.Replace(" ", "%20") : "",
            });

            if (Filter.Latitude is 0 && Filter.Longitude is 0)
            {
                //Switch case expression
                return Filter.SortBy switch
                {
                    1 => List.OrderBy(x => Convert.ToDouble(x.Distance.Replace(" Km", ""))).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToList(),
                    2 => List.OrderByDescending(x => x.Id).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToList(),
                    3 => List.OrderByDescending(x => x.AvgRating).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToList(),
                    _ => List.Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToList(),
                };
            }
            else
            {
                //Switch case expression
                return Filter.SortBy switch
                {
                    1 => List.Where(x => Convert.ToDouble(x.Distance.Replace(" Km", "")) <= Convert.ToDouble(x.ServiceDistance)).OrderBy(x => Convert.ToDouble(x.Distance.Replace(" Km", "")))
                                                                                                            .Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToList(),
                    2 => List.Where(x => Convert.ToDouble(x.Distance.Replace(" Km", "")) <= Convert.ToDouble(x.ServiceDistance)).OrderByDescending(x => x.Id).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToList(),
                    3 => List.Where(x => Convert.ToDouble(x.Distance.Replace(" Km", "")) <= Convert.ToDouble(x.ServiceDistance)).OrderByDescending(x => x.AvgRating).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToList(),
                    _ => List.Where(x => Convert.ToDouble(x.Distance.Replace(" Km", "")) <= Convert.ToDouble(x.ServiceDistance)).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToList(),
                };
            }
        }

        public async Task<IEnumerable<PopularCategories>> GetAllPopularCategoriesByRestaurantBranch(long BranchId)
        {
            return await _context.PopularCategories.Where(x => x.RestaurantBranchId == BranchId).ToListAsync();
        }

        public IEnumerable<LandingPageResponseDTO> GetRestaurantDetailsByOrigin(string Origin)
        {

            List<LandingPageResponseDTO> result = _context.Restaurants.Where(x => x.Origin == Origin).Include(x => x.RestaurantBranches).Include(x => x.RestaurantBannerSettings).Include(x => x.RestaurantRatings)
                .ThenInclude(x => x.User).Include(x => x.RestaurantContentManagement).Include(x => x.Orders).ThenInclude(x => x.OrderDetails).Select(x => new LandingPageResponseDTO
                {
                    RestaurantId = x.Id,
                    RestaurantBranchId = x.RestaurantBranches.Where(x => x.IsMainBranch).FirstOrDefault().Id,
                    RestaurantName = x.NameAsPerTradeLicense,
                    BannerImage = x.RestaurantBannerSettings.Where(y => y.BannerType == Enum.GetName(typeof(BannerType), BannerType.Banner)).FirstOrDefault().ImagePath,
                    BannerUrl = x.RestaurantBannerSettings.Where(y => y.BannerType == Enum.GetName(typeof(BannerType), BannerType.Banner)).FirstOrDefault().Url,
                    PromotionBanners = _mapper.Map<List<LandingPromotionBannerDTO>>(x.RestaurantBannerSettings.Where(y => y.BannerType == Enum.GetName(typeof(BannerType), BannerType.PromotionBanner)).ToList()),
                    Logo = x.Logo,
                    BranchDeliveryType = x.RestaurantBranches.Where(x => x.IsMainBranch).FirstOrDefault().DeliveryType,
                    BranchDeliveryCharges = x.RestaurantBranches.Where(x => x.IsMainBranch).FirstOrDefault().DeliveryCharges,
                    AboutUs = x.RestaurantContentManagement.AboutUs,
                    AboutUsImage = string.IsNullOrEmpty(x.DescriptionImage) ? "https://cdn.fougito.com/images/restaurant/Daily%20Dubai%20Restaurant/AboutUsImage.png" : x.DescriptionImage,
                    Address = x.RestaurantBranches.Where(x => x.IsMainBranch).FirstOrDefault().Address,
                    Latitude = x.RestaurantBranches.Where(x => x.IsMainBranch).FirstOrDefault().Latitude,
                    Longitude = x.RestaurantBranches.Where(x => x.IsMainBranch).FirstOrDefault().Longitude,
                    Contact = x.PhoneNumber,
                    Email = x.Email,
                    Facebook = x.Facebook,
                    Twitter = x.Twitter,
                    Instagram = x.Instagram,
                    Favicon = x.Favicon,
                    Linkedin = x.Linkedin,
                    RestaurantRatings = _mapper.Map<List<RestaurantRatingDTO>>(x.RestaurantRatings.Where(x => x.ShowOnWebsite)),
                    ThemeColor = x.ThemeColor ?? "#F5504F",
                    VAT = x.TaxPercent,
                    Footer = x.RestaurantContentManagement.FooterImage ?? "https://cdn.fougito.com/images/restaurant/footer/footer-bg_compressed.jpg",
                    IsClose = x.RestaurantBranches.Where(x => x.IsMainBranch).FirstOrDefault().IsClose,
                    OrderPaymentType = x.OrderPaymentType,
                    SecondaryLogo = x.SecondaryLogo
                }).ToList();

            result.FirstOrDefault().TrendingItems = _context.TrendingItems.Where(y => y.RestaurantId == result.FirstOrDefault().RestaurantId).OrderByDescending(x => x.Count).Take(4).Select(x => new TrendingItemsDTO() { Name = x.Name }).ToList();

            return result;
        }

        public object GetNearestBranch(decimal lat, decimal lng, long RestaurantId)
        {
            var List = _context.RestaurantBranches.Where(x => x.RestaurantId == RestaurantId).AsEnumerable().Select(x => new RestaurantCardResponseDTO
            {
                Id = x.Id,
                ServiceDistance = x.ServiceDistance,
                Distance = DistanceHelper.DistanceTo((double)lat, (double)lng, (double)x.Latitude, (double)x.Longitude).ToString("n2") + " Km",
                BranchDeliveryCharges = x.DeliveryCharges,
                BranchDeliveryType = x.DeliveryType,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                IsClose = x.IsClose,
                NameAsPerTradeLicense = x.NameAsPerTradeLicense,
                Address = x.Address,
                ClosingTimeSpan = x.ClosingTimeSpan,
            });

            RestaurantCardResponseDTO result = List.Where(x => Convert.ToDouble(x.Distance.Replace(" Km", "")) <= Convert.ToDouble(x.ServiceDistance)).OrderBy(x => Convert.ToDouble(x.Distance.Replace(" Km", ""))).FirstOrDefault();

            if (result is not null)
            {
                double DeliveryCharges = 0;
                if (result.BranchDeliveryType == Enum.GetName(typeof(DeliveryType), DeliveryType.PerKilometer))
                    DeliveryCharges = Math.Round(Convert.ToDouble(result.Distance.Replace(" Km", "")) * (double)result.BranchDeliveryCharges, 2);
                else
                    DeliveryCharges = Math.Round((double)result.BranchDeliveryCharges, 2);

                return new
                {
                    result.Id,
                    BranchDeliveryCharges = DeliveryCharges,
                    result.BranchDeliveryType,
                    result.ServiceDistance,
                    result.Latitude,
                    result.Longitude,
                    result.IsClose,
                    result.ClosingTimeSpan,
                    branchName = result.NameAsPerTradeLicense,
                    branchAddress = result.Address,
                    result.MinOrderPrice
                };
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Restaurant>> GetRestaurantByOrigin(string Origin)
        {
            return await _context.Restaurants.Where(x => x.Origin == Origin).ToListAsync();
        }

        public async Task<IEnumerable<LandingPageResponseDTO>> GetRestaurantBranchDetails(long BranchId, long? CustomerId)
        {
            var result = _context.RestaurantBranches.Where(x => x.Id == BranchId).Include(x => x.BranchSchedules).Include(x => x.CustomerFavouriteBranches).Include(x => x.Restaurant)
                .ThenInclude(x => x.RestaurantRatings).Include(x => x.Restaurant.RestaurantRatings).Include(x=>x.Restaurant.RestaurantBannerSettings)
                .AsEnumerable().Select(x => new LandingPageResponseDTO
                {
                    RestaurantId = x.RestaurantId,
                    RestaurantBranchId = x.Id,
                    MenuBannerImage = x.Restaurant.RestaurantBannerSettings.Where(x => x.BannerType == Enum.GetName(typeof(BannerType), BannerType.MenuBanner)).Any() ?
                                      x.Restaurant.RestaurantBannerSettings.Where(x => x.BannerType == Enum.GetName(typeof(BannerType), BannerType.MenuBanner)).FirstOrDefault().ImagePath
                                      : "https://cdn.fougito.com/images/restaurant/Daily%20Dubai%20Restaurant/Menu-Banner.png",
                    Address = x.Address,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    BranchName = x.NameAsPerTradeLicense,
                    Logo = x.Restaurant.Logo,
                    AvgRating = x.Restaurant.RestaurantRatings.Any() ? x.Restaurant.RestaurantRatings.Average(x => x.Rating) : 0,
                    RatingCount = x.Restaurant.RestaurantRatings.Count,
                    DeliveryCharges = x.DeliveryCharges,
                    RestaurantDeliveryType = x.DeliveryType,
                    TaxPercent = x.Restaurant.TaxPercent,
                    Isfavourite = (CustomerId.HasValue && x.CustomerFavouriteBranches.Any(x => x.CustomerId == CustomerId)),
                    Origin = x.Restaurant.Origin,
                    //Distance = DistanceHelper.DistanceTo((double)Filter.Latitude, (double)Filter.Longitude, (double)x.Latitude, (double)x.Longitude).ToString("n2") + " Km",
                    OpeningTime = x.BranchSchedules.Where(x => x.Day == DateTime.Now.DayOfWeek.ToString()).Any() ?
                              (DateTime.Today + x.BranchSchedules.Where(x => x.Day == DateTime.Now.DayOfWeek.ToString()).FirstOrDefault().OpeningTime).ToShortTimeString() : null,
                    ClosingTime = x.BranchSchedules.Where(x => x.Day == DateTime.Now.DayOfWeek.ToString()).Any() ?
                              (DateTime.Today + x.BranchSchedules.Where(x => x.Day == DateTime.Now.DayOfWeek.ToString()).FirstOrDefault().ClosingTime).ToShortTimeString() : null,
                    ClosingTimeSpan = x.IsClose ? x.ClosingTimeSpan : null,
                    IsClose = x.IsClose
                });

            return result.ToList();
        }

        public async Task<List<BranchMenuDTO>> GetBranchMenu(long BranchId)
        {
            var result = await GetBranchMenuItems(BranchId);

            // Group at DB level was not doable
            List<BranchMenuDTO> filter = (from MT in result
                                          group MT by new
                                          {
                                              MT.CategoryId,
                                              MT.Category.Name,
                                              MT.Category.Position
                                          } into g
                                          select new BranchMenuDTO
                                          {
                                              CategoryId = g.Key.CategoryId,
                                              CategoryName = g.Key.Name,

                                              Items = _mapper.Map<List<ItemForMenuDTO>>(g.OrderBy(x => x.Position).ToList())
                                          }).ToList();

            return filter;
        }

        public async Task<IEnumerable<MenuPartnerDTO>> GetBranchMenuForPartner(long BranchId)
        {
            var result = await _context.MenuItems.Include(x => x.MenuItemOptions).ThenInclude(x => x.MenuItemOptionValues).Include(x => x.Item).Include(x => x.Menu).Include(x => x.Category)
                                                                           .Where(x => x.Menu.RestaurantBranchId == BranchId &&
                                                                           x.ArchivedDate == null && x.Menu.RestaurantBranch.ArchivedDate == null)
                                                                           .ToListAsync();


            IEnumerable<MenuPartnerDTO> filter = from MT in result
                                                 group MT by new
                                                 {
                                                     MT.MenuId,
                                                     MT.Menu.Name,
                                                     MT.Menu.Status
                                                 } into newGroup
                                                 select new MenuPartnerDTO
                                                 {
                                                     MenuId = newGroup.Key.MenuId,
                                                     MenuName = newGroup.Key.Name,
                                                     Status = newGroup.Key.Status,

                                                     Categories = (from MT in newGroup
                                                                   group MT by new
                                                                   {
                                                                       MT.CategoryId,
                                                                       MT.Category.Name
                                                                   } into newGroup1
                                                                   select new MenuCategoryPartnerDTO
                                                                   {
                                                                       Id = newGroup1.Key.CategoryId,
                                                                       Name = newGroup1.Key.Name,

                                                                       Items = _mapper.Map<List<MenuCategoryItemDTO>>(newGroup1.ToList())
                                                                   }).ToList()
                                                 };

            return filter;
        }

        public async Task<List<MenuItem>> GetBranchMenuItems(long BranchId)
        {
            var result = await _context.MenuItems.Include(x => x.MenuItemOptions).ThenInclude(x => x.MenuItemOptionValues).Include(x => x.Item).Include(x => x.Menu).Include(x => x.Category)
                                                                            .Where(x => x.Menu.RestaurantBranchId == BranchId &&
                                                                            x.ArchivedDate == null && x.Status == Enum.GetName(typeof(Status), Status.Active) &&
                                                                            x.Item.Status == Enum.GetName(typeof(Status), Status.Active) &&
                                                                            x.Menu.Status == Enum.GetName(typeof(Status), Status.Active) &&
                                                                            x.Category.Status == Enum.GetName(typeof(Status), Status.Active) &&
                                                                            x.Menu.RestaurantBranch.ArchivedDate == null)
                                                                            //&&
                                                                            //x.Menu.StartTime <= DateTime.UtcNow &&
                                                                            //(x.Menu.EndTime.HasValue == false || x.Menu.EndTime >= DateTime.UtcNow))
                                                                            .OrderBy(x => x.CategoryPosition).ToListAsync();

            return result;
        }

        public IEnumerable<RestaurantCardResponseDTO> Trending(RestaurantFilter Filter)
        {
            var result = _context.RestaurantBranches.Include(x => x.Restaurant).ThenInclude(x => x.RestaurantRatings)
                .Where(x => x.Restaurant.Status == Enum.GetName(typeof(Status), Status.Active) && x.Status == Enum.GetName(typeof(Status), Status.Active)).AsQueryable();

            if (!string.IsNullOrEmpty(Filter.Paging.Search))
                result = result.Where(x => EF.Functions.Like(x.Restaurant.NameAsPerTradeLicense, "%" + Filter.Paging.Search + "%"));


            var List = result.Include(x => x.BranchSchedules).AsEnumerable().Select(x => new RestaurantCardResponseDTO
            {
                Id = x.Id,
                RestaurantId = x.RestaurantId,
                DeliveryTime = x.DeliveryMinutes,
                ServiceDistance = x.ServiceDistance,
                Logo = !string.IsNullOrEmpty(x.Restaurant.Logo) ? x.Restaurant.Logo.Replace(" ", "%20") : "",
                NameAsPerTradeLicense = x.Restaurant.NameAsPerTradeLicense,
                Address = x.Address,
                Distance = DistanceHelper.DistanceTo((double)Filter.Latitude, (double)Filter.Longitude, (double)x.Latitude, (double)x.Longitude).ToString("n2") + " Km",
                OpeningTime = x.BranchSchedules.Where(x => x.Day == DateTime.Now.DayOfWeek.ToString()).Any() ?
                              (DateTime.Today + x.BranchSchedules.Where(x => x.Day == DateTime.Now.DayOfWeek.ToString()).FirstOrDefault().OpeningTime).ToShortTimeString() : null,
                ClosingTime = x.BranchSchedules.Where(x => x.Day == DateTime.Now.DayOfWeek.ToString()).Any() ?
                              (DateTime.Today + x.BranchSchedules.Where(x => x.Day == DateTime.Now.DayOfWeek.ToString()).FirstOrDefault().ClosingTime).ToShortTimeString() : null,
                AvgRating = x.Restaurant.RestaurantRatings.Any() ? x.Restaurant.RestaurantRatings.Average(x => x.Rating) : 0,
                RatingCount = x.Restaurant.RestaurantRatings.Count,
                Slug = !string.IsNullOrEmpty(x.Slug) ? x.Slug : "",
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                ThumbnailImage = x.Restaurant.ThumbnailImage
            });

            //Switch case expression
            return Filter.SortBy switch
            {
                1 => List.OrderBy(x => Convert.ToDouble(x.Distance.Replace(" Km", ""))).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToList(),
                2 => List.OrderByDescending(x => x.Id).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToList(),
                3 => List.OrderByDescending(x => x.AvgRating).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToList(),
                _ => List.Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToList(),
            };
        }
    }
}
