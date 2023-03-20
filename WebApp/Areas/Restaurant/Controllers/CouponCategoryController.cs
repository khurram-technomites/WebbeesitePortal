using AutoMapper;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    [Authorize(Roles = "RestaurantOwner")]
    public class CouponCategoryController : Controller
    {
        private readonly ICouponCategoryClient _client;
        private readonly ICouponClient _couponClient;
        private readonly ICategoryClient _categoryClient;
        private readonly IUserSessionManager _userSessionManager;
        private readonly IMapper _mapper;
        public CouponCategoryController(ICouponCategoryClient client, ICouponClient couponClient, ICategoryClient categoryClient, IUserSessionManager userSessionManager, IMapper mapper)
        {
            _client = client;
            _couponClient = couponClient;
            _categoryClient = categoryClient;
            _userSessionManager = userSessionManager;
            _mapper = mapper;
        }
        public IActionResult Index(long id)
        {
            ViewBag.CouponID = id;
            return View();
        }

        public async Task<IActionResult> GetAll(long id)
        {
            var restaurantId = _userSessionManager.GetUserStore().Id;
            var categories = await _categoryClient.GetAllCategorysAsync(restaurantId);
            var categoriesModel = categories.Select(i => new
            {
                id = i.Id.ToString(),
                value = i.Name
            }).ToList();

            var couponCategory = await _client.GetCouponCategoriesByCoupon(id);

            if (couponCategory != null)
            {
                var couponCategoryModel = couponCategory.Select(i => new
                {
                    id = i.CategoryId.ToString(),
                    value = i.Category.Name,
                    couponCategoryId = i.Id
                });

                return Json(new
                {
                    success = true,
                    message = "Data recieved successfully!",
                    coupons = categoriesModel,
                    couponCategories = couponCategoryModel
                });
            }
            else
            {
                return Json(new
                {
                    success = true,
                    message = "Data recieved successfully!",
                    coupons = categoriesModel
                });
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CouponCategoryViewModel model)
        {

            CouponCategoryViewModel result = _mapper.Map<CouponCategoryViewModel>(await _client.AddCouponCategoryAsync(_mapper.Map<CouponCategoryDTO>(model)));
            if (result != null)
            {
                return Json(new
                {
                    success = true,
                    data = result.Id,
                    message = "Coupon Category assigned ...",
                });
            }

            else
            {
                return Json(new
                {
                    success = false,
                    message = "Something went wrong ...",
                });
            }


        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(CouponCategoryViewModel couponCategory)
        {
            CouponCategoryViewModel model = _mapper.Map<CouponCategoryViewModel>(await _client.GetCouponCategoryByCouponAndCategory(couponCategory.CouponId, couponCategory.CategoryId));
            await _client.DeleteCouponCategoryAsync(model.Id);


            return Json(new
            {
                success = false,
                message = "Category deleted successfully"
            });
        }
    }
}
