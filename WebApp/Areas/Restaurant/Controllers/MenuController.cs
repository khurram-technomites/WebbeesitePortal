using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Menu;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Authorization;
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
    public class MenuController : Controller
    {
        private readonly IMenuClient _menuClient;
        private readonly IRestaurantBranchClient _branchClient;
        private readonly ICategoryClient _categoryClient;
        private readonly IMenuItemClient _menuItemClient;
        private readonly IMenuItemOptionClient _menuItemOptionClient;
        private readonly IMenuItemOptionValueClient _menuItemOptionValueClient;
        private readonly IItemOptionValueClient _itemOptionValueClient;
        private readonly IItemOptionClient _itemOptionClient;
        private readonly IItemClient _itemClient;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSessionManager;

        public MenuController(IMenuClient menuClient, IMenuItemClient menuItemClient, IMenuItemOptionClient menuItemOptionClient, IMenuItemOptionValueClient menuItemOptionValueClient, IItemOptionValueClient itemOptionValueClient, IItemOptionClient itemOptionClient, IItemClient itemClient, IMapper mapper, IUserSessionManager userSessionManager, ICategoryClient categoryClient, IRestaurantBranchClient restaurantBranchClient)
        {
            _menuClient = menuClient;
            _branchClient = restaurantBranchClient;
            _categoryClient = categoryClient;
            _menuItemClient = menuItemClient;
            _menuItemOptionClient = menuItemOptionClient;
            _menuItemOptionValueClient = menuItemOptionValueClient;
            _itemOptionValueClient = itemOptionValueClient;
            _itemOptionClient = itemOptionClient;
            _itemClient = itemClient;
            _mapper = mapper;
            _userSessionManager = userSessionManager;
        }

        public async Task<IActionResult> Index()
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            IEnumerable<MenuViewModel> menu = _mapper.Map<IEnumerable<MenuViewModel>>(await _menuClient.GetAllMenusAsync(restaurantId)).OrderBy(x => x.Position);
            return View(menu);
        }

        public async Task<ActionResult> GetBranchDropDown(long id)
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            var result = await _branchClient.GetAllRestaurantBranchsAsync(restaurantId);

            return Json(new
            {

                success = true,
                data = result.Select(i => new
                {

                    id = i.Id,
                    name = i.NameAsPerTradeLicense,
                    selectedId = id,


                })

            });
        }
        public IActionResult Create()
        {
            return View();
        }

        public async Task<ActionResult> SaveImagePositions(List<MenuDTO> positions)
        {
            try
            {
                MenuViewModel model = new MenuViewModel();
                foreach (var item in positions)
                {
                    model.Position = item.Position;
                    model.Id = item.Id;
                    await _menuClient.SavePosition(_mapper.Map<MenuDTO>(model));
                }

                return Json(new { success = true, message = "Position successfully updated..." });
            }
            catch
            {
                return Json(new { success = false, message = "Something went wrong!" });
            }

        }

        public async Task<ActionResult> SaveItemPositions(List<MenuItemDTO> positions)
        {
            try
            {
                MenuItemViewModel model = new MenuItemViewModel();
                foreach (var item in positions)
                {
                    model.Position = item.Position;
                    model.Id = item.Id;
                    await _menuItemClient.SavePosition(_mapper.Map<MenuItemDTO>(model));
                }

                return Json(new { success = true, message = "Item Position successfully updated..." });
            }
            catch
            {
                return Json(new { success = false, message = "Something went wrong!" });
            }

        }

        public async Task<ActionResult> SaveCategoryPositions(List<MenuItemDTO> positions)
        {
            try
            {
                MenuItemViewModel model = new MenuItemViewModel();
                foreach (var item in positions)
                {
                    model.CategoryPosition = item.CategoryPosition;
                    model.MenuId = item.MenuId;
                    model.CategoryId = item.CategoryId;
                    await _menuItemClient.SaveCategoryPosition(_mapper.Map<MenuItemDTO>(model));
                }

                return Json(new { success = true, message = "Position successfully updated..." });
            }
            catch
            {
                return Json(new { success = false, message = "Something went wrong!" });
            }

        }

        [HttpPost]
        public async Task<IActionResult> Create(string Name, string Description)
        {
            try
            {

                MenuViewModel model = new MenuViewModel();
                long restaurantId = _userSessionManager.GetUserStore().Id;
                model.Status = Enum.GetName(typeof(Status), Status.Active);
                model.RestaurantId = restaurantId;
                model.Name = Name;
                model.Description = Description;

                MenuDTO Result = await _menuClient.AddMenuAsync(_mapper.Map<MenuDTO>(model));

                return Json(new
                {
                    success = true,
                    message = "Menu Created Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        MenuItemCount = Result.MenuItemCount,
                        Name = Result.Name,
                        Description = Result.Description,
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

        public async Task<IActionResult> Duplicate(long id)
        {
            DuplicateMenuViewModel menu = _mapper.Map<DuplicateMenuViewModel>(await _menuClient.GetMenuByIdAsync(id));
            menu.Id = 0;
            menu.Name = menu.Name + " - Copy";
            menu.Status = Enum.GetName(typeof(Status), Status.Inactive);
            var Result = await _menuClient.AddMenuAsync(_mapper.Map<MenuDTO>(menu));


            return Json(new
            {
                success = true,
                message = "Menu duplication Successfully",
                data = new
                {
                    ID = Result.Id,
                    MenuItemCount = Result.MenuItemCount,
                    Name = Result.Name,
                    Description = Result.Description,
                }
            });

        }

        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(long Id, string Name, string Description, long RestaurantBranchId, string Status, bool IsPeriodic, DateTime? BreakFastStartTime, DateTime? BreakFastEndTime, DateTime? LunchStartTime, DateTime? LunchEndTime,
            DateTime? DinnerStartTime, DateTime? DinnerEndTime)
        {
            try
            {
                MenuViewModel model = new MenuViewModel();
                model.Status = Status;
                model.Id = Id;
                model.Name = Name;
                model.Description = Description;
                model.RestaurantBranchId = RestaurantBranchId;
                model.IsPeriodic = IsPeriodic;
                model.BreakFastStartTime = BreakFastStartTime;
                model.BreakFastEndTime = BreakFastEndTime;
                model.LunchStartTime = LunchStartTime;
                model.LunchEndTime = LunchEndTime;
                model.DinnerStartTime = DinnerStartTime;
                model.DinnerEndTime = DinnerEndTime;
                long restaurantId = _userSessionManager.GetUserStore().Id;
                model.RestaurantId = restaurantId;

                if (IsPeriodic == false)
                {
                    var isExist = await _menuClient.GetAllMenuByBranchIdAsync(RestaurantBranchId, Id);
                    if (isExist.Any())
                    {
                        model.Status = "Inactive";
                    }
                }

                MenuDTO Result = await _menuClient.UpdateMenuAsync(_mapper.Map<MenuDTO>(model));

                return Json(new
                {
                    success = true,
                    message = "Menu Edited Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        MenuItemCount = Result.MenuItemCount,
                        Name = Result.Name,
                        Description = Result.Description,
                        Status = Result.Status,
                        IsPeriodic = Result.IsPeriodic,
                        BreakFastStartTime = Result.BreakFastStartTime,
                        BreakFastEndTime = Result.BreakFastEndTime,
                        LunchStartTime = Result.LunchStartTime,
                        LunchEndTime = Result.LunchEndTime,
                        DinnerStartTime = Result.DinnerStartTime,
                        DinnerEndTime = Result.DinnerEndTime,
                        RestaurantBranchId = Result.RestaurantBranchId,
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

        public async Task<IActionResult> GetMenuItem(long Id)
        {
            if (Id != 0)
            {
                ViewBag.MenuId = Id;
                var menuName = await _menuClient.GetMenuByIdAsync(Id);
                ViewBag.MenuName = menuName.Name;
            }

            IEnumerable<MenuItemByMenuViewModel> result = _mapper.Map<IEnumerable<MenuItemByMenuViewModel>>(await _menuItemClient.GetAllMenuItemsByMenuAsync(Id));
            return View(result);
        }


        [HttpPost]
        public async Task<IActionResult> EditCategory(string Name, string Description, string Status, string Url, long Id)
        {
            CategoryViewModel category = _mapper.Map<CategoryViewModel>(await _categoryClient.GetCategoryByIdAsync(Id));
            category.Name = Name;
            category.Description = Description;
            category.Status = Status;
            category.Image = Url;

            CategoryDTO Result = await _categoryClient.UpdateCategoryAsync(_mapper.Map<CategoryDTO>(category));

            return Json(new
            {
                success = true,
                message = "Category Updated Successfully",
                data = new
                {
                    ID = Result.Id,
                    Name = Result.Name,
                    Description = Result.Description,
                    Status = Result.Status,
                    Url = Result.Image
                }
            }); ;
        }

        [HttpPost]
        public async Task<JsonResult> MenuItemOptions(long Id, long ItemId)
        {
            var model = await _menuItemOptionClient.GetAllMenuItemOptionsAsync(Id);
            IEnumerable<ItemOptionViewModel> modelItem = _mapper.Map<IEnumerable<ItemOptionViewModel>>(await _itemOptionClient.GetAllItemOptionsAsync(ItemId));

            return Json(new
            {
                success = true,
                dropdown = modelItem.Select(i => new
                {

                    text = i.Title,
                    value = i.Id,

                }),
                data = model.Select(i => new
                {

                    id = i.Id,
                    name = i.Title,
                    isRequired = i.IsRequired,
                    isPriceMain = i.IsPriceMain,
                    isRadio = i.IsRadioButton,
                    minimum = i.Minimum,
                    maximum = i.Maximum,
                    optionValues = i.MenuItemOptionValues.Select(i => new
                    {

                        id = i.Id,
                        name = i.Value,
                        price = i.Price


                    })


                }),


            });

        }

        [HttpPost]
        public async Task<IActionResult> EditMenuItem(MenuItemViewModel model)
        {

            MenuItemDTO menuItem = await _menuItemClient.GetMenuItemByIdAsync(model.Id);
            model.ItemId = menuItem.ItemId;
            MenuItemDTO Result = await _menuItemClient.UpdateMenuItemAsync(_mapper.Map<MenuItemDTO>(model));

            return Json(new
            {
                success = true,
                message = "Menu Edited Successfully",
                data = new
                {
                    ID = Result.Id,
                    Name = Result.Name,
                    Description = Result.Description,
                    Status = Result.Status,
                }
            });
        }

        #region Status Change

        public async Task<ActionResult> MenuToggleActiveStatus(long id)
        {
            try
            {
                var menu = await _menuClient.GetMenuByIdAsync(id);
                long branchId = menu.RestaurantBranchId != null ? (long)menu.RestaurantBranchId : 0;
                var isExist = await _menuClient.GetAllMenuByBranchIdAsync(branchId, id);
                if (isExist.Any())
                {
                    return Json(new
                    {
                        success = false,
                        message = "Menu is already assigned on this branch...",

                    });
                }

                else
                {
                    MenuViewModel Result = _mapper.Map<MenuViewModel>(await _menuClient.ToggleActiveStatus(id));

                    return Json(new
                    {
                        success = true,
                        message = "Status Updated Successfully",

                    });
                }

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

        public async Task<ActionResult> MenuItemToggleActiveStatus(long id)
        {
            try
            {
                MenuItemViewModel item = _mapper.Map<MenuItemViewModel>(await _menuItemClient.GetMenuItemByIdAsync(id));
                if (item.Price > 0)
                {
                    if (await _menuItemClient.CheckMainPrice(id))
                    {
                        return Json(new
                        {
                            success = false,
                            message = "An item containing an option marked as Main should have zero price!",

                        });
                    }
                    else
                    {
                        MenuItemViewModel Result = _mapper.Map<MenuItemViewModel>(await _menuItemClient.ToggleActiveStatus(id));
                        return Json(new
                        {
                            success = true,
                            message = "Status Updated Successfully",

                        });
                    }

                }
                else
                {
                    MenuItemViewModel Result = _mapper.Map<MenuItemViewModel>(await _menuItemClient.ToggleActiveStatus(id));
                    return Json(new
                    {
                        success = true,
                        message = "Status Updated Successfully",

                    });
                }


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

        public async Task<ActionResult> CategoryToggleActiveStatus(long id)
        {
            try
            {
                CategoryViewModel Result = _mapper.Map<CategoryViewModel>(await _categoryClient.ToggleActiveStatus(id));

                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",

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
        #endregion
        public async Task<ActionResult> CategoryItems(long id, long menuId)
        {

            var restaurantId = _userSessionManager.GetUserStore().Id;
            var items = await _itemClient.GetItemsByCategoryIdAsync(id, restaurantId, menuId);
            var itemModel = items.Select(i => new
            {
                id = i.Id.ToString(),
                value = i.Name.Trim()

            }).ToList();


            if (itemModel != null)
            {

                return Json(new
                {
                    success = true,
                    message = "Data recieved successfully!",
                    items = itemModel
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Data not found",
                });
            }
        }

        public async Task<ActionResult> CreateItem(long id)
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            ViewBag.MenuId = id;
            var parent = _mapper.Map<IEnumerable<CategoryViewModel>>(await _categoryClient.GetAllCategoriesDropDownByMenuIdAsync(id, restaurantId));
            ViewBag.Category = new SelectList(parent, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateItem(long categoryId, long menuId, List<ItemViewModel> items)
        {

            if (items.Any())
            {
                CategoryViewModel category = _mapper.Map<CategoryViewModel>(await _categoryClient.GetCategoryByIdAsync(categoryId));
                List<MenuItemViewModel> listOfMenuItems = new List<MenuItemViewModel>();
                foreach (var item in items)
                {
                    MenuItemViewModel menu = new MenuItemViewModel();
                    ItemViewModel model = _mapper.Map<ItemViewModel>(await _itemClient.GetItemByIdAsync(item.Id));
                    menu.Name = model.Name;
                    menu.Price = model.Price;
                    menu.CategoryId = categoryId;
                    menu.MenuId = menuId;
                    menu.ItemId = model.Id;
                    menu.Description = model.Description;
                    menu.Image = model.Image;
                    menu.Status = Enum.GetName(Status.Active);

                    var result = await _menuItemClient.AddMenuItemAsync(_mapper.Map<MenuItemDTO>(menu));
                    if (model.ItemOptions.Any())
                    {
                        foreach (var itemOption in model.ItemOptions)
                        {
                            MenuItemOptionViewModel modelOption = new MenuItemOptionViewModel();
                            modelOption.Title = itemOption.Title;
                            modelOption.IsRequired = itemOption.IsRequired;
                            modelOption.IsPriceMain = itemOption.IsPriceMain;
                            modelOption.IsRadioButton = itemOption.IsRadioButton;
                            modelOption.Maximum = itemOption.Maximum;
                            modelOption.Minimum = itemOption.Minimum;
                            modelOption.MenuItemId = result.Id;

                            var resultOption = await _menuItemOptionClient.AddMenuItemOptionAsync(_mapper.Map<MenuItemOptionDTO>(modelOption));

                            if (itemOption.ItemOptionValues.Any())
                            {
                                foreach (var itemValue in itemOption.ItemOptionValues)
                                {

                                    MenuItemOptionValueViewModel valueViewModel = new MenuItemOptionValueViewModel();
                                    valueViewModel.Value = itemValue.Value;
                                    valueViewModel.Price = itemValue.Price;
                                    valueViewModel.IsPriceMain = false;
                                    valueViewModel.MenuItemOptionId = resultOption.Id;

                                    var resultValue = await _menuItemOptionValueClient.AddMenuItemOptionValueAsync(_mapper.Map<MenuItemOptionValueDTO>(valueViewModel));

                                }
                            }

                        }
                    }

                    listOfMenuItems.Add(_mapper.Map<MenuItemViewModel>(result));


                }
                return Json(new
                {

                    success = true,
                    message = "Item Added Successfully...",
                    menuItems = listOfMenuItems.Select(i => new
                    {

                        id = i.Id,
                        itemId = i.ItemId,
                        name = i.Name,
                        description = i.Description,
                        status = i.Status,
                        price = i.Price,
                        image = i.Image != null ? i.Image : ""


                    }),

                    category = new
                    {

                        id = category.Id,
                        name = category.Name,
                        status = category.Status,
                        description = category.Description,
                        Image = category.Image != null ? category.Image : "",
                    }


                });
            }

            else
            {
                return Json(new
                {
                    success = false,
                    message = "Something went wrong ...!"

                });
            }

        }

        [HttpPost]
        public async Task<ActionResult> CreateItemCategory(long categoryId, long menuId, long itemId, decimal price)
        {
            if (itemId > 0)
            {
                CategoryViewModel category = _mapper.Map<CategoryViewModel>(await _categoryClient.GetCategoryByIdAsync(categoryId));
                List<MenuItemViewModel> listOfMenuItems = new List<MenuItemViewModel>();

                MenuItemViewModel menu = new MenuItemViewModel();
                ItemViewModel model = _mapper.Map<ItemViewModel>(await _itemClient.GetItemByIdAsync(itemId));
                menu.Name = model.Name;
                menu.Price = price == 0 ? model.Price : price;
                menu.CategoryId = categoryId;
                menu.MenuId = menuId;
                menu.ItemId = itemId;
                menu.Description = model.Description;
                menu.Status = Enum.GetName(Status.Active);

                var result = await _menuItemClient.AddMenuItemAsync(_mapper.Map<MenuItemDTO>(menu));
                if (model.ItemOptions.Any())
                {
                    foreach (var itemOption in model.ItemOptions)
                    {
                        MenuItemOptionViewModel modelOption = new MenuItemOptionViewModel();
                        modelOption.Title = itemOption.Title;
                        modelOption.IsRequired = itemOption.IsRequired;
                        modelOption.IsRadioButton = itemOption.IsRadioButton;
                        modelOption.Maximum = itemOption.Maximum;
                        modelOption.Minimum = itemOption.Minimum;
                        modelOption.IsPriceMain = itemOption.IsPriceMain;
                        modelOption.MenuItemId = result.Id;

                        var resultOption = await _menuItemOptionClient.AddMenuItemOptionAsync(_mapper.Map<MenuItemOptionDTO>(modelOption));

                        if (itemOption.ItemOptionValues.Any())
                        {
                            foreach (var itemValue in itemOption.ItemOptionValues)
                            {

                                MenuItemOptionValueViewModel valueViewModel = new MenuItemOptionValueViewModel();
                                valueViewModel.Value = itemValue.Value;
                                valueViewModel.Price = itemValue.Price;
                                valueViewModel.IsPriceMain = false;
                                valueViewModel.MenuItemOptionId = resultOption.Id;

                                var resultValue = await _menuItemOptionValueClient.AddMenuItemOptionValueAsync(_mapper.Map<MenuItemOptionValueDTO>(valueViewModel));

                            }
                        }

                    }
                }




                return Json(new
                {

                    success = true,
                    message = "Item Added Successfully...",
                    item = new
                    {
                        id = result.Id,
                        name = result.Name,
                        description = result.Description,
                        price = result.Price,
                        status = result.Status
                    }


                });

            }
            else
            {
                return Json(new
                {

                    success = false,
                    message = "Select Item first ...!",



                });
            }





        }

        [HttpPost]
        public async Task<ActionResult> ChangePrice(long id, decimal price)
        {
            bool main = false;
            string message = string.Empty;
            bool success = false;
            MenuItemViewModel model = _mapper.Map<MenuItemViewModel>(await _menuItemClient.GetMenuItemByIdAsync(id));
            model.Price = price;
            if (price > 0)
            {
                if (await _menuItemClient.CheckMainPrice(id))
                {
                    model.Status = Enum.GetName(typeof(Status), Status.Inactive);
                    main = true;
                    success = false;
                    message = "Item is inactive due to missing main option";
                }
                else
                {
                    await _menuItemClient.UpdateMenuItemAsync(_mapper.Map<MenuItemDTO>(model));
                    success = true;
                    message = "Price Updated Successfully ...!";
                }
            }

            else
            {
                await _menuItemClient.UpdateMenuItemAsync(_mapper.Map<MenuItemDTO>(model));
                success = true;
                message = "Price Updated Successfully ...!";
            }

            return Json(new
            {

                success = success,
                main = main,
                message = message

            });


        }

        public async Task<ActionResult> GetItems(long id, long menuId)
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            IEnumerable<ItemViewModel> items = _mapper.Map<IEnumerable<ItemViewModel>>(await _itemClient.GetItemsByCategoryIdAsync(id, restaurantId, menuId));

            return Json(new
            {
                success = true,
                items = items.Select(i => new
                {
                    text = i.Name,
                    value = i.Id

                }),
            });

        }

        public async Task<ActionResult> DeleteOption(long id)
        {

            await _menuItemOptionClient.DeleteMenuItemOptionAsync(id);
            return Json(new
            {

                success = true,
                message = "Item option deleted successfully ...!"

            });
        }


        public async Task<ActionResult> DeleteOptionValue(long id)
        {
            await _menuItemOptionValueClient.Delete(id);
            return Json(new
            {

                success = true,
                message = "Value deleted successfully ...!"

            });
        }

        [HttpPost]
        public async Task<ActionResult> UpdateValueOption(long Id, decimal Price, long OptionId, string Name)
        {
            MenuItemOptionValueViewModel model = new MenuItemOptionValueViewModel();
            model.Id = Id > 0 ? Id : 0;
            model.Value = Name;
            model.MenuItemOptionId = OptionId;
            model.Price = Price;

            await _menuItemOptionValueClient.UpdateMenuItemOptionValueAsync(_mapper.Map<MenuItemOptionValueDTO>(model));

            return Json(new
            {
                success = true,
                message = "Value updated successfully...!"
            });
        }

        [HttpPost]
        public async Task<ActionResult> ChangeRequiredItemOption(long Id, int Min, int Max, bool IsRequired, bool IsMain, bool IsRadio)
        {
            MenuItemOptionViewModel model = _mapper.Map<MenuItemOptionViewModel>(await _menuItemOptionClient.GetMenuItemOptionByIdAsync(Id));

            model.Id = Id;
            model.IsRequired = IsRequired;
            model.Minimum = Min;
            model.Maximum = Max;
            model.IsPriceMain = IsMain;
            if (IsMain == true)
            {
                await _menuItemOptionClient.GetMainPrice(model.MenuItemId, model.Id);
            }

            model.IsRadioButton = IsRadio;
            var result = await _menuItemOptionClient.UpdateMenuItemOptionAsync(_mapper.Map<MenuItemOptionDTO>(model));

            return Json(new
            {
                success = true,
                message = "Value updated successfully...!"

            });


        }

        public async Task<ActionResult> AddItemOptionValue(long menuItemOptionId)
        {
            MenuItemOptionValueViewModel model = new MenuItemOptionValueViewModel();
            model.MenuItemOptionId = menuItemOptionId;
            model.Value = "";

            MenuItemOptionValueDTO result = await _menuItemOptionValueClient.AddMenuItemOptionValueAsync(_mapper.Map<MenuItemOptionValueDTO>(model));

            return Json(new
            {

                success = true,
                id = result.Id,
                message = "Value created successfully...!"

            });
        }

        [HttpPost]
        public async Task<ActionResult> AddItemOption(long Id, long MenuItemId)
        {
            List<MenuItemOptionValueViewModel> valueList = new List<MenuItemOptionValueViewModel>();
            ItemOptionViewModel model = _mapper.Map<ItemOptionViewModel>(await _itemOptionClient.GetItemOptionByIdAsync(Id));
            MenuItemOptionViewModel itemOption = new MenuItemOptionViewModel();
            itemOption.Title = model.Title;
            itemOption.MenuItemId = MenuItemId;
            itemOption.IsRequired = model.IsRequired;
            itemOption.IsPriceMain = model.IsPriceMain;
            itemOption.IsRadioButton = model.IsRadioButton;
            itemOption.Maximum = model.Maximum;
            itemOption.Minimum = model.Minimum;
            var result = await _menuItemOptionClient.AddMenuItemOptionAsync(_mapper.Map<MenuItemOptionDTO>(itemOption));
            if (model.ItemOptionValues.Any())
            {
                foreach (var item in model.ItemOptionValues)
                {
                    MenuItemOptionValueViewModel value = new MenuItemOptionValueViewModel();
                    value.Value = item.Value;
                    value.Price = item.Price;
                    value.MenuItemOptionId = result.Id;

                    MenuItemOptionValueViewModel valueresult = _mapper.Map<MenuItemOptionValueViewModel>(await _menuItemOptionValueClient.AddMenuItemOptionValueAsync(_mapper.Map<MenuItemOptionValueDTO>(value)));
                    valueList.Add(valueresult);

                }
            }

            return Json(new
            {

                success = true,
                message = "Option added successfully...!",
                menuItemOption = new
                {
                    id = result.Id,
                    title = result.Title,
                    menuItemId = result.MenuItemId,
                    isRequired = result.IsRequired,
                    isMainPrice = result.IsPriceMain,
                    isRadioButton = result.IsRadioButton,
                    maximum = result.Maximum,
                    minimum = result.Minimum,
                },
                menuItemOptionValue = valueList.Select(i => new
                {
                    id = i.Id,
                    value = i.Value,
                    price = i.Price,
                    menuItemOptionId = i.MenuItemOptionId,

                })

            });
        }

        public async Task<ActionResult> DeleteMenuItem(long id)
        {
            await _menuItemClient.DeleteMenuItemAsync(id);


            return Json(new
            {

                success = true,
                message = "Menu item deleted successfully..."

            });
        }

        public async Task<ActionResult> DeleteMenu(long id)
        {
            await _menuClient.DeleteMenuAsync(id);

            return Json(new
            {
                success = true,
                message = "Menu deleted successfully..."

            });
        }
    }
}
