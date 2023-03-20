
using AutoMapper;
using ExcelDataReader;
using Fingers10.ExcelExport.ActionResults;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Interfaces.IServices.Domains;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    [Authorize(Roles = "RestaurantOwner")]
    public class CategoryController : Controller
    {
        private readonly ICategoryClient _client;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSessionManager;
        private readonly IFileUpload _fileUpload;
        public CategoryController(ICategoryClient client, IMapper mapper, IUserSessionManager userSessionManager, IFileUpload fileUpload)
        {
            _client = client;
            _mapper = mapper;
            _userSessionManager = userSessionManager;
            _fileUpload = fileUpload;
        }
        public async Task<IActionResult> Index()
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            var Info = _mapper.Map<IEnumerable<CategoryViewModel>>(await _client.GetAllCategorysAsync(restaurantId));
            return View(Info);
        }

        public async Task<IActionResult> GeneralCategories()
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            var Info = _mapper.Map<IEnumerable<CategoryViewModel>>(await _client.GetAllGeneralCategoriesAsync(restaurantId));
            return View(Info.OrderBy(x=>x.Name));
        }

        public async Task<IActionResult> ImportCategories(List<ImportViewModel> categories)
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            if (categories.Any())
            {
                foreach (var item in categories)
                {
                    CategoryViewModel category =  _mapper.Map<CategoryViewModel>(await _client.GetCategoryByIdAsync(item.id));
                    category.Id = 0;
                    category.RestaurantId = restaurantId;

                    await _client.AddCategoryAsync(_mapper.Map<CategoryDTO>(category));
                }

                return Json(new
                {
                    success = true,
                    message = "Categories imported successfully"

                });
            }

            else
            {
                return Json(new
                {
                    success = false,
                    message = "Please select categories first !"
                });
            }
        }

        public async Task<IActionResult> Create()
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            var parent = _mapper.Map<IEnumerable<CategoryViewModel>>(await _client.GetParentCategoriesAsync(restaurantId));
            ViewBag.ParentCategory = new SelectList(parent, "Id", "Name");
            return View();
        }

        public IActionResult BulkUpload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetExcel(IFormFile File)
        {
            bool SkipHeader = true;
            if (File is null)
                return Json(new
                {
                    success = false,
                    message = "Cannot upload empty file!",
                });
            long restaurantId = _userSessionManager.GetUserStore().Id;
            long positionCount = await _client.MaxPosition(restaurantId) + 1;
            List<CategoryViewModel> Categories = new List<CategoryViewModel>();
            // For .net core, the next line requires the NuGet package, 
            // System.Text.Encoding.CodePages
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = File.OpenReadStream())
            //System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using var reader = ExcelReaderFactory.CreateReader(stream);
                if (reader.RowCount == 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "File contains no data",
                    });
                }

                while (reader.Read()) //Each row of the file
                {
                    if (!SkipHeader)
                    {
                        if (reader.GetValue(0) != null)
                            Categories.Add(new CategoryViewModel
                            {
                                Name = reader.GetValue(0).ToString(),
                                Description = reader.GetValue(1) == null ? "" : reader.GetValue(1).ToString(),
                                IsDefault = true,
                                RestaurantId = restaurantId,
                                Status = Enum.GetName(typeof(Status), Status.Active),
                                Slug = Slugify.GenerateSlug(string.IsNullOrEmpty(reader.GetValue(0).ToString()) ? "" : reader.GetValue(0).ToString()),
                                Image = reader.GetValue(2) == null ? "" : reader.GetValue(2).ToString(),
                                Position = (int)positionCount
                            });
                    }
                    else
                        SkipHeader = false;

                    positionCount++;

                }
            }
            IEnumerable<CategoryDTO> result = await _client.AddCategoryRangeAsync(_mapper.Map<IEnumerable<CategoryDTO>>(Categories));

            if (result.Any())
                return Json(new
                {
                    success = true,
                    message = "File uploaded successfully",
                    data = result.Select(x => new
                    {
                        firstRow = x.CreationDate,
                        secondRow = x.Image + "|" + x.Name,
                        forthRow = x.Status == Enum.GetName(typeof(Status), Status.Active),
                        fifthRow = x.Id
                    })
                });
            else
                return Json(new
                {
                    success = false,
                    message = "Error while uploading file!",
                });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            try
            {
                long restaurantId = _userSessionManager.GetUserStore().Id;
                model.Status = Enum.GetName(typeof(Status), Status.Active);
                model.RestaurantId = restaurantId;

                CategoryDTO Result = await _client.AddCategoryAsync(_mapper.Map<CategoryDTO>(model));

                //var Parent = category.ParentCategoryID.HasValue ? _categoryService.GetCategory((long)category.ParentCategoryID) : null;
                return Json(new
                {
                    success = true,
                    url = "/Restaurant/Category/Index",
                    message = "Record Added Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, h:mm:tt") : "-",
                        Category = Result.Image + "|" + Result.Name,
                        IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false


                    }
                });


                //}
                //else
                //{
                //	message = "Please fill the form properly ...";
                //}
                //return Json(new { success = false, message = message });
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
            CategoryViewModel CategoryDTO = _mapper.Map<CategoryViewModel>(await _client.GetCategoryByIdAsync(id));
            var parent = _mapper.Map<IEnumerable<CategoryViewModel>>(await _client.GetParentCategoriesAsync(restaurantId));
            ViewBag.ParentCategory = new SelectList(parent, "Id", "Name", CategoryDTO.ParentCategoryId);
            return View(CategoryDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CategoryViewModel cat = _mapper.Map<CategoryViewModel>(await _client.GetCategoryByIdAsync(model.Id));
                    if (string.IsNullOrEmpty(model.Image))
                    {
                        model.Image = cat.Image;

                    }
                    model.RestaurantId = _userSessionManager.GetUserStore().Id;
                    CategoryDTO Result = await _client.UpdateCategoryAsync(_mapper.Map<CategoryDTO>(model));

                    Result.Id = model.Id;

                    return Json(new
                    {
                        success = true,
                        message = "Record Updated Successfully",
                        data = new
                        {
                            ID = Result.Id,
                            Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, h:mm:tt") : "-",
                            Category = Result.Image + "|" + Result.Name,
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

        public async Task<ActionResult> ToggleActiveStatus(long id)
        {
            try
            {
                CategoryViewModel Result = _mapper.Map<CategoryViewModel>(await _client.ToggleActiveStatus(id));

                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, h:mm:tt") : "-",
                        Category = Result.Image + "|" + Result.Name,
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await _client.DeleteCategoryAsync(id);

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

        public async Task<IActionResult> Details(long id)
        {
            return View(_mapper.Map<CategoryViewModel>(await _client.GetCategoryByIdAsync(id)));
        }

        public async Task<IActionResult> CategoryReport()
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            var Info = _mapper.Map<IEnumerable<CategoryViewModel>>(await _client.GetAllCategorysAsync(restaurantId));
            return new CSVResult<CategoryViewModel>(Info, "Category");
        }
    }
}
