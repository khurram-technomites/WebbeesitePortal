using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Restaurant
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly IRestaurantService _restaurantService;
		private readonly ICategoryService _service;
		private readonly IMapper _mapper;
		private readonly IFTPUpload _fTPUpload;
		private readonly IImageService _imageService;

		public CategoryController(IRestaurantService restaurantService, ICategoryService service, IMapper mapper, IFTPUpload fTPUpload, IImageService imageService)
		{
			_restaurantService = restaurantService;
			_service = service;
			_mapper = mapper;
			_fTPUpload = fTPUpload;
			_imageService = imageService;
		}

		[HttpGet("GetAll/{restaurantId}")]
		public async Task<IActionResult> GetAll(long restaurantId)
		{
			return Ok(_mapper.Map<IEnumerable<CategoryDTO>>(await _service.GetAllAsync(restaurantId)));
		}

		[HttpGet("GetAll/General/Restaurants/{restaurantId}")]
		public async Task<IActionResult> GetAllGeneralCategories(long restaurantId)
		{
			return Ok(_mapper.Map<IEnumerable<CategoryDTO>>(await _service.GetGeneralCategoriesAsync(restaurantId)));
		}


		[HttpGet("GetByName/{Name}/Restaurants/{restaurantId}")]
		public async Task<IActionResult> GetByName(string Name, long restaurantId)
		{
			return Ok(_mapper.Map<CategoryDTO>(await _service.GetIdbyNameAsync(restaurantId, Name)));
		}

		[HttpGet("GetAll/Menus/{MenuId}/Restaurants/{RestaurantId}")]
		public async Task<IActionResult> GetAllByMenu(long MenuId, long RestaurantId)
		{
			return Ok(_mapper.Map<IEnumerable<CategoryDTO>>(await _service.GetAllByMenuID(MenuId, RestaurantId)));
		}

		[HttpGet("MaxPosition/{RestaurantId}")]
		public async Task<IActionResult> MaxPosition(long RestaurantId)
		{
			return Ok(await _service.GetPositionCount(RestaurantId));
		}

		[HttpGet("{Id}")]
		public async Task<IActionResult> GetById(long Id)
		{
			IEnumerable<CategoryDTO> List = _mapper.Map<IEnumerable<CategoryDTO>>(await _service.GetByIdAsync(Id));
			return Ok(List.FirstOrDefault());
		}

		[HttpGet("ByParent/{Id}")]
		public async Task<IActionResult> GetByCategory(long parentId, long restaurantId)
		{
			IEnumerable<CategoryDTO> List = _mapper.Map<IEnumerable<CategoryDTO>>(await _service.GetByParentIdAsync(parentId, restaurantId));
			return Ok(List.FirstOrDefault());
		}

		[HttpGet("ParentCategories/{restaurantId}")]
		public async Task<IActionResult> GetParentCategories(long restaurantId)
		{
			IEnumerable<CategoryDTO> List = _mapper.Map<IEnumerable<CategoryDTO>>(await _service.GetParentCategoriesAsync(restaurantId));
			return Ok(List);
		}

		[HttpPost]
		public async Task<IActionResult> Add(CategoryDTO Model)
		{

			string LogoPath = "/Images/Restaurant/Category/";
			if (!string.IsNullOrEmpty(Model.Image))
			{
				if (_fTPUpload.MoveFile(Model.Image, ref LogoPath))
				{
					Model.Image = LogoPath;
				}
			}


			return Ok(_mapper.Map<CategoryDTO>(await _service.AddCategoryAsync(_mapper.Map<Category>(Model))));
		}

		[HttpPost("BulkUpload")]
		public async Task<IActionResult> BulkUpload(IEnumerable<CategoryDTO> List)
		{
			string LogoPath = "/Images/Restaurant/Category/";
			foreach (var category in List)
			{
				if (!string.IsNullOrEmpty(category.Image))
				{
					Uri uri = new(category.Image);
					byte[] image = await _imageService.DownloadImageAsync(uri);

					if (image is not null)
					{
						string uriWithoutQuery = uri.GetLeftPart(UriPartial.Path);
						if (_fTPUpload.UploadToDirectory(image, ref LogoPath, Path.GetExtension(uriWithoutQuery)))
							category.Image = LogoPath;
					}

				}
				else
					category.Image = "https://cdn.fougito.com/Images/Restaurant/Category/default.png";
			}

			return Ok(_mapper.Map<IEnumerable<CategoryDTO>>(await _service.AddRangeAsync(_mapper.Map<IEnumerable<Category>>(List))));
		}

		[HttpPut]
		public async Task<IActionResult> Update(CategoryDTO Model)
		{
			string LogoPath = "/Images/Restaurant/Category/";
			if (!string.IsNullOrEmpty(Model.Image))
			{
				if (_fTPUpload.MoveFile(Model.Image, ref LogoPath))
				{
					Model.Image = LogoPath;
				}
			}
			return Ok(_mapper.Map<CategoryDTO>(await _service.UpdateCategoryAsync(_mapper.Map<Category>(Model))));
		}

		[HttpDelete("{Id}")]
		public async Task<IActionResult> Archive(long Id)
		{
			return Ok(_mapper.Map<CategoryDTO>(await _service.ArchiveCategoryAsync(Id)));
		}

		[HttpGet("ToggleStatus/{Id}")]
		public async Task<IActionResult> ToggleStatus(long Id)
		{
			IEnumerable<Category> Category = await _service.GetByIdAsync(Id);
			Category make = Category.FirstOrDefault();

			if (make.Status == Enum.GetName(typeof(Status), Status.Active))
				make.Status = Enum.GetName(typeof(Status), Status.Inactive);
			else
				make.Status = Enum.GetName(typeof(Status), Status.Active);

			return Ok(await _service.UpdateCategoryAsync(make));
		}


		#region Status Repsonse Apis

		[HttpGet("Restaurants/{restaurantId}")]
		public async Task<IActionResult> GetAllCategories(long restaurantId)
		{
			return Ok(new SuccessResponse<IEnumerable<CategoryDTO>>("Data received successfully", _mapper.Map<IEnumerable<CategoryDTO>>(await _service.GetAllAsync(restaurantId))));
		}

		[HttpGet("ByRestaurantBranchId/{restaurantBranchId}")]
		public async Task<IActionResult> GetAllCategoriesByBranchId(long restaurantBranchId)
		{
			var categories = _mapper.Map<List<CategoryDTO>>(await _service.GetAllAsyncByBranchId(restaurantBranchId));

			object result = categories.Select(x => new
			{
				Id = x.Id,
				Name = x.Name,
				NameAR = x.NameAR,
				Description = x.Description,
				DescriptionAR = x.DescriptionAR,
				Image = x.Image,
				Slug = x.Slug,
				ParentCategoryId = x.ParentCategoryId,
				Position = x.Position,
				IsParentCategoryDeactivate = x.IsParentCategoryDeactivate,
				IsDefault = x.IsDefault,
				Status = x.Status,
				RestaurantId = x.RestaurantId,
				CreationDate = x.CreationDate,
				MenuItems = x.MenuItems.Select(i => new
				{
					Id = i.Id,
					ItemId = i.ItemId,
					MenuId = i.MenuId,
					Name = i.Name,
					NameAr = i.NameAr,
					Description = i.Description,
					DescriptionAr = i.DescriptionAr,
					CategoryId = i.CategoryId,
					Price = i.Price,
					Image = i.Image,
					Status = i.Status,
					Position = i.Position,
					Quantity = i.Quantity,
					CategoryPosition = i.CategoryPosition,
					Menu = new
					{
						i.Menu.Id,
						i.Menu.Name,
						i.Menu.NameAr,
						i.Menu.Description,
						i.Menu.DescriptionAr,
						i.Menu.MenuItemCount,
						i.Menu.StartTime,
						i.Menu.EndTime,
						i.Menu.BreakFastStartTime,
						i.Menu.BreakFastEndTime,
						i.Menu.LunchStartTime,
						i.Menu.LunchEndTime,
						i.Menu.DinnerStartTime,
						i.Menu.DinnerEndTime,
						i.Menu.Status,
						i.Menu.IsPeriodic,
						i.Menu.Position,
						//i.Menu.RestaurantBranchId,
						//i.Menu.RestaurantId,
						//i.Menu.RestaurantBranch,
						//i.Menu.MenuItem,
					},
					//Category = i.Category,
					MenuItemOptions = i.MenuItemOptions,
					//Item = i.Item,
				}),
				//Items = x.Items,
				//CouponCategories = x.CouponCategories,
			});

			return Ok(new SuccessResponse<object>("Data received successfully", result));
		}

		//[HttpGet("GetByRestaurantBranchId/{restaurantbranchId}")]
		[HttpGet("GetByRestaurantBranchId/{restaurantbranchId}")]

		[HttpGet("GetCategory/{Id}")]
		public async Task<IActionResult> GetCategoryById(long Id)
		{
			IEnumerable<CategoryDTO> List = _mapper.Map<IEnumerable<CategoryDTO>>(await _service.GetByIdAsync(Id));
			return Ok(new SuccessResponse<CategoryDTO>("Data received successfully", _mapper.Map<CategoryDTO>(List.FirstOrDefault())));
		}

		#endregion

	}
}
