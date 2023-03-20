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
using System.Linq;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    [Authorize(Roles = "RestaurantOwner")]
    public class ItemController : Controller
    {
        private readonly IItemClient _client;
        private readonly IItemOptionClient _itemOptionClient;
        private readonly IItemOptionValueClient _itemOptionValueClient;
        private readonly IUserSessionManager _userSessionManager;
        private readonly IMapper _mapper;
        private readonly ICategoryClient _categoryclient;
        private readonly IFileUpload _fileUpload;
        public ItemController(IItemClient client, IMapper mapper, IUserSessionManager userSessionManager, ICategoryClient categoryClient, IFileUpload fileUpload, IItemOptionClient itemOptionClient, IItemOptionValueClient itemOptionValueClient)
        {
            _client = client;
            _mapper = mapper;
            _userSessionManager = userSessionManager;
            _categoryclient = categoryClient;
            _fileUpload = fileUpload;
            _itemOptionClient = itemOptionClient;
            _itemOptionValueClient = itemOptionValueClient;
        }

        public async Task<IActionResult> Index()
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            var Info = _mapper.Map<IEnumerable<ItemViewModel>>(await _client.GetAllItemsAsync(restaurantId));
            return View(Info);
        }

        public async Task<IActionResult> GeneralItems()
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            var Info = _mapper.Map<IEnumerable<ItemViewModel>>(await _client.GetAllGeneralAsync(restaurantId));
            return View(Info.OrderBy(x=>x.Name));
        }

        public async Task<IActionResult> ImportItems(List<ImportViewModel> items)
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            if (items.Any())
            {
                foreach (var data in items)
                {
                    ItemViewModel item = _mapper.Map<ItemViewModel>(await _client.GetItemByIdAsync(data.id));

                    ItemViewModel currentItem = new ItemViewModel();
                    currentItem.RestaurantId = restaurantId;
                    currentItem.CategoryId = item.CategoryId;
                    currentItem.Description = item.Description;
                    currentItem.Image = item.Image;
                    currentItem.Name = item.Name;
                    currentItem.Price = item.Price;
                    currentItem.Status = "Active";

                    var result = await _client.AddItemAsync(_mapper.Map<ItemDTO>(currentItem));

                    if (item.ItemOptions.Any())
                    {
                        foreach (var options in item.ItemOptions)
                        {
                            ItemOptionViewModel option = _mapper.Map<ItemOptionViewModel>(await _itemOptionClient.GetItemOptionByIdAsync(options.Id));

                            ItemOptionViewModel currentOption = new ItemOptionViewModel();
                            currentOption.ItemId = result.Id;
                            currentOption.IsPriceMain = option.IsPriceMain;
                            currentOption.IsRadioButton = option.IsRadioButton;
                            currentOption.IsRequired = option.IsRequired;
                            currentOption.Title = option.Title;
                            currentOption.Maximum = option.Maximum;
                            currentOption.Minimum = option.Minimum;
                            currentOption.RestaurantId = restaurantId;
                            

                            var optionResult = await _itemOptionClient.AddItemOptionAsync(_mapper.Map<ItemOptionDTO>(currentOption));
                            if (option.ItemOptionValues.Any())
                            {
                                foreach (var values in option.ItemOptionValues)
                                {
                                    ItemOptionValueViewModel value = _mapper.Map<ItemOptionValueViewModel>(await _itemOptionValueClient.GetItemOptionValueByIdAsync(values.Id));
                                    ItemOptionValueViewModel currentValue = new ItemOptionValueViewModel();

                                    currentValue.ItemOptionId = optionResult.Id;
                                    currentValue.Value = value.Value;
                                    currentValue.Price = value.Price;

                                    await _itemOptionValueClient.AddItemOptionValueAsync(_mapper.Map<ItemOptionValueDTO>(currentValue));
                                }
                            }
                        }
                    }
                }

                return Json(new
                {
                    success = true,
                    message = "Items imported successfully!"

                });
            }

            else
            {
                return Json(new
                {
                    success = false,
                    message = "Please select items first !"
                });
            }
        }

        public async Task<IActionResult> Create()
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            var parent = _mapper.Map<IEnumerable<CategoryViewModel>>(await _categoryclient.GetAllCategorysAsync(restaurantId));
            ViewBag.Category = new SelectList(parent, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ItemViewModel model)
        {
            try
            {
                long restaurantId = _userSessionManager.GetUserStore().Id;
                model.Status = Enum.GetName(typeof(Status), Status.Active);
                model.RestaurantId = restaurantId;
                model.CreationDate = DateTime.Now;


                ItemDTO Result = await _client.AddItemAsync(_mapper.Map<ItemDTO>(model));

                if (Result != null)
                {
                    Result.Category = await _categoryclient.GetCategoryByIdAsync(Result.CategoryId);
                }

                //var Parent = Item.ParentItemID.HasValue ? _ItemService.GetItem((long)Item.ParentItemID) : null;
                return Json(new
                {
                    success = true,
                    url = "/Restaurant/Item/Index",
                    message = "Record Added Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm:tt") : "-",
                        Category = Result.Category.Name,
                        Item = Result.Image + "|" + Result.Name,
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

        [HttpPost]
        public async Task<IActionResult> AddItemOptionValue(long ItemOptionId, string Name, decimal Price)
        {
            try
            {

                ItemOptionValueViewModel model = new ItemOptionValueViewModel();
                model.ItemOptionId = ItemOptionId;
                model.Value = Name;
                model.Price = Price;


                var result = await _itemOptionValueClient.AddItemOptionValueAsync(_mapper.Map<ItemOptionValueDTO>(model));
                return Json(new
                {
                    success = true,
                    message = "Record Added Successfully",
                    data = new
                    {

                        id = result.Id,
                        value = result.Value,
                        itemOptionId = result.ItemOptionId,
                        price = result.Price,
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

        [HttpPost]
        public async Task<IActionResult> AddItemOption(long ItemId, string Title, bool IsRequired, bool IsPriceMain, bool IsRadio, int Maximum)
        {
            try
            {
                long restaurantId = _userSessionManager.GetUserStore().Id;
                ItemOptionViewModel model = new ItemOptionViewModel();
                model.ItemId = ItemId;
                model.Title = Title;
                model.IsRequired = IsRequired;
                model.IsPriceMain = IsPriceMain;
                model.IsRadioButton = IsRadio;
                model.Minimum = 0;
                model.Maximum = 0;
                if (IsRequired)
                {
                    model.Minimum = 1;
                }
                if (!IsRadio)
                {
                    model.Maximum = Maximum;
                }
                else
                {
                    model.Minimum = 1;
                    model.Maximum = 1;
                }

                if (IsPriceMain)
                {
                    await _itemOptionClient.GetMainPrice(ItemId, 0);
                }

                //if (IsRequired)
                //{
                //    model.Minimum = 1;
                //}

                model.RestaurantId = restaurantId;

                var result = await _itemOptionClient.AddItemOptionAsync(_mapper.Map<ItemOptionDTO>(model));
                return Json(new
                {
                    success = true,
                    message = "Record Added Successfully",
                    data = new
                    {

                        id = result.Id,
                        name = result.Title,
                        isRequired = result.IsRequired,
                        isMain = result.IsPriceMain,
                        isRadio = result.IsRadioButton,

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
            ItemViewModel item = _mapper.Map<ItemViewModel>(await _client.GetItemByIdAsync(id));
            var parent = _mapper.Map<IEnumerable<CategoryViewModel>>(await _categoryclient.GetAllCategorysAsync(restaurantId));
            ViewBag.Category = new SelectList(parent, "Id", "Name", item.CategoryId);
            var option = _mapper.Map<IEnumerable<ItemOptionViewModel>>(await _itemOptionClient.GetAllItemOptionsAsync(id));
            ViewBag.Option = new SelectList(option, "Id", "Title");
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ItemViewModel cat = _mapper.Map<ItemViewModel>(await _client.GetItemByIdAsync(model.Id));
                    if (string.IsNullOrEmpty(model.Image))
                    {
                        model.Image = cat.Image;

                    }
                    model.RestaurantId = _userSessionManager.GetUserStore().Id;
                    ItemDTO Result = await _client.UpdateItemAsync(_mapper.Map<ItemDTO>(model));



                    if (Result != null)
                    {
                        Result.Category = await _categoryclient.GetCategoryByIdAsync(Result.CategoryId);
                        Result.Id = model.Id;
                    }

                    return Json(new
                    {
                        success = true,
                        message = "Record Updated Successfully",
                        data = new
                        {
                            ID = Result.Id,
                            Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm:tt") : "-",
                            Category = Result.Category.Name,
                            Item = Result.Image + "|" + Result.Name,
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
                ItemViewModel Result = _mapper.Map<ItemViewModel>(await _client.ToggleActiveStatus(id));

                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm:tt") : "-",
                        Category = Result.Category.Name,
                        Item = Result.Image + "|" + Result.Name,
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
                await _client.DeleteItemAsync(id);

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

        public async Task<ActionResult> DeleteOption(long id)
        {
            try
            {
                await _itemOptionClient.DeleteItemOptionAsync(id);

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

        public async Task<ActionResult> DeleteValue(long id)
        {
            try
            {
                await _itemOptionValueClient.DeleteItemOptionValueAsync(id);

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
            return View(_mapper.Map<ItemViewModel>(await _client.GetItemByIdAsync(id)));
        }

        public async Task<IActionResult> ItemReport()
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            var Info = _mapper.Map<IEnumerable<ItemViewModel>>(await _client.GetAllItemsAsync(restaurantId));
            return new CSVResult<ItemViewModel>(Info, "Item");
        }

        public async Task<IActionResult> OptionDropDown(long Id)
        {
            var option = await _itemOptionClient.GetAllItemOptionsAsync(Id);

            return Json(new
            {
                success = true,
                data = option.Select(i => new
                {

                    id = i.Id,
                    name = i.Title

                })
            });
        }

        public IActionResult BulkUpload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetExcel(IFormFile File)
        {
            bool SkipHeader = true;
            if (File is null)
                return Json(new
                {
                    success = false,
                    message = "Cannot upload empty file!",
                });
            long restaurantId = _userSessionManager.GetUserStore().Id;
            CategoryViewModel category = new CategoryViewModel();
            ItemViewModel Items = new ItemViewModel();
            ItemOptionViewModel ItemOption = new ItemOptionViewModel();
            ItemOptionValueViewModel ItemOptionValue = new ItemOptionValueViewModel();
            List<ItemViewModel> result = new List<ItemViewModel>();
            IDictionary<long, string> categoryNames = new Dictionary<long, string>();
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
                        {
                            category = _mapper.Map<CategoryViewModel>(_categoryclient.GetByName(restaurantId, reader.GetValue(3).ToString()).Result);
                            if (category == null)
                            {
                                return Json(new
                                {
                                    success = false,
                                    message = "Category name is incorrect!",
                                });
                            }
                            if (!categoryNames.ContainsKey(category.Id))
                            {
                                categoryNames.Add(category.Id, category.Name);
                            }

                            if (reader.GetValue(0).ToString() == "Mawaean special tea (Black/Green)")
                            {

                            }

                            var itemObj =  _client.GetByNameAsync(restaurantId, reader.GetValue(0).ToString()).Result;
                            if (itemObj != null)
                            {
                                Items.Id = itemObj.Id;
                            }

                            Items.Name = reader.GetValue(0).ToString();
                            Items.Description = reader.GetValue(1) == null ? "" : reader.GetValue(1).ToString();
                            Items.RestaurantId = restaurantId;
                            Items.Status = Enum.GetName(typeof(Status), Status.Active);
                            Items.Image = reader.GetValue(2) == null ? "" : reader.GetValue(2).ToString();
                            Items.CategoryId = category != null ? category.Id : 0;
                            Items.Price = reader.GetValue(4) == null ? 0 : Convert.ToDecimal(reader.GetValue(4));



                            ItemDTO itemResult = _client.UpdateItemAsync(_mapper.Map<ItemDTO>(Items)).Result;
                            result.Add(_mapper.Map<ItemViewModel>(itemResult));
                            Items = new ItemViewModel();

                            if (reader.GetValue(5) != null)
                            {
                                var itemOptionObj = _itemOptionClient.GetByName(itemResult.Id, reader.GetValue(5).ToString()).Result;
                                if (itemOptionObj != null)
                                {
                                    ItemOption.Id = itemOptionObj.Id;
                                }

                                ItemOption.Title = reader.GetValue(5).ToString();
                                
                                ItemOption.IsPriceMain = reader.GetValue(6).ToString() == "TRUE" ? true : false;
                                ItemOption.IsRadioButton = reader.GetValue(7).ToString() == "TRUE" ? true : false;
                                ItemOption.IsRequired = reader.GetValue(8).ToString() == "TRUE" ? true : false;
                                ItemOption.Maximum = reader.GetValue(9) != null ? Convert.ToInt32(reader.GetValue(9)) : 0;
                                ItemOption.Minimum = 0;
                                if (ItemOption.IsRequired)
                                {
                                    ItemOption.Minimum = 1;
                                }

                                if (!ItemOption.IsRadioButton)
                                {
                                    ItemOption.Minimum = 1;
                                    
                                }

                                if (ItemOption.IsPriceMain)
                                {
                                    ItemOption.IsRequired = true;
                                    ItemOption.Minimum = 1;
                                }

                                

                                ItemOption.ItemId = itemResult.Id;
                                ItemOption.RestaurantId = restaurantId;

                                var itemOptionResult =  _itemOptionClient.UpdateItemOptionAsync(_mapper.Map<ItemOptionDTO>(ItemOption)).Result;
                                ItemOption = new ItemOptionViewModel();

                                if (reader.GetValue(10) != null)
                                {
                                    var itemOptionValuesObj = _mapper.Map<ItemOptionValueViewModel>( _itemOptionValueClient.GetByNameAsync(itemOptionResult.Id, reader.GetValue(10).ToString()).Result);
                                    if (itemOptionValuesObj != null)
                                    {
                                        ItemOptionValue.Id = itemOptionValuesObj.Id; 
                                    }

                                    ItemOptionValue.Value = reader.GetValue(10).ToString();
                                    ItemOptionValue.Price = reader.GetValue(11) != null ? Convert.ToDecimal(reader.GetValue(11)) : 0;
                                    ItemOptionValue.ItemOptionId = itemOptionResult.Id;

                                    var valueResult = _itemOptionValueClient.UpdateItemOptionValueAsync(_mapper.Map<ItemOptionValueDTO>(ItemOptionValue)).Result;
                                    ItemOptionValue = new ItemOptionValueViewModel();
                                }
                            }

                        }
                    }
                    else
                    {
                        SkipHeader = false;
                    }

                }
            }





            if (result.Any())
                return Json(new
                {
                    success = true,
                    message = "File uploaded successfully",
                    data = result.Select(x => new
                    {

                        firstRow = x.CreationDate.ToString("dd MMM yyyy, h:mm:ss"),
                        secondRow = x.Image + "|" + x.Name,
                        thirdRow = categoryNames[x.CategoryId],
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
    }
}
