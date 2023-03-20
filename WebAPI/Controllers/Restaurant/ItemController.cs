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
    public class ItemController : ControllerBase
    {
        private readonly IItemService _service;
        private readonly IMapper _mapper;
        private readonly IFTPUpload _fTPUpload;
        private readonly IImageService _imageService;
        private readonly IMenuItemService _menuItemService;

        public ItemController(IItemService service, IMapper mapper, IFTPUpload fTPUpload, IImageService imageService, IMenuItemService menuItemService)
        {
            _service = service;
            _mapper = mapper;
            _fTPUpload = fTPUpload;
            _imageService = imageService;
            _menuItemService = menuItemService;
        }

        [HttpGet("GetAll/{restaurantId}")]
        public async Task<IActionResult> GetAll(long restaurantId)
        {
            return Ok(_mapper.Map<IEnumerable<ItemDTO>>(await _service.GetAllAsync(restaurantId)));
        }

        [HttpGet("GetAll/General/Restaurants/{restaurantId}")]
        public async Task<IActionResult> GetAllGeneralItem(long restaurantId)
        {
            return Ok(_mapper.Map<IEnumerable<ItemDTO>>(await _service.GetAllGeneralItemAsync(restaurantId)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<ItemDTO> List = _mapper.Map<IEnumerable<ItemDTO>>(await _service.GetByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpGet("GetByName/Restaurants/{restaurantId}")]
        public async Task<IActionResult> GetByName(long restaurantId, [FromQuery] string Name = "")
        {
            ItemDTO List = _mapper.Map<ItemDTO>(await _service.GetByName(restaurantId, Name));
            return Ok(List);
        }

        //[HttpGet("ByCategory/{Id}")]
        //public async Task<IActionResult> GetByItem(long categoryId, long restaurantId)
        //{
        //    IEnumerable<ItemDTO> List = _mapper.Map<IEnumerable<ItemDTO>>(await _service.GetByCategoryIdAsync(categoryId, restaurantId));
        //    return Ok(List.FirstOrDefault());
        //}


        [HttpGet("Category/{categoryId}/Restaurants/{restaurantId}/Menus/{menuId}")]
        public async Task<IActionResult> GetByCategory(long categoryId, long restaurantId, long menuId)
        {
            IEnumerable<ItemDTO> List = _mapper.Map<IEnumerable<ItemDTO>>(await _service.GetByCategoryIdAsync(categoryId, restaurantId, menuId));
            return Ok(List);
        }


        [HttpPost]
        public async Task<IActionResult> Add(ItemDTO Model)
        {

            string LogoPath = "/Images/Restaurant/Item/";
            if (!string.IsNullOrEmpty(Model.Image))
            {
                if (_fTPUpload.MoveFile(Model.Image, ref LogoPath))
                {
                    Model.Image = LogoPath;
                }
            }


            return Ok(_mapper.Map<ItemDTO>(await _service.AddItemAsync(_mapper.Map<Item>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ItemDTO Model)
        {
            string LogoPath = "/Images/Restaurant/Item/";
            if (!string.IsNullOrEmpty(Model.Image))
            {
                if (_fTPUpload.MoveFile(Model.Image, ref LogoPath))
                {
                    Model.Image = LogoPath;
                }
            }
            return Ok(_mapper.Map<ItemDTO>(await _service.UpdateItemAsync(_mapper.Map<Item>(Model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            IEnumerable<MenuItem> menuItems = await _menuItemService.GetByItemAsync(Id);

            foreach (var menu in menuItems)
            {
                await _menuItemService.ArchiveMenuItemAsync(menu.Id);
            }

            return Ok(_mapper.Map<ItemDTO>(await _service.ArchiveItemAsync(Id)));
        }

        [HttpGet("ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<Item> item = await _service.GetByIdAsync(Id);
            Item make = item.FirstOrDefault();

            if (make.Status == Enum.GetName(typeof(Status), Status.Active))
                make.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                make.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(await _service.UpdateItemAsync(make));
        }

        [HttpPost("BulkUpload")]
        public async Task<IActionResult> BulkUpload(IEnumerable<ItemDTO> List)
        {
            string LogoPath = "/Images/Restaurant/Item/";
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
                    category.Image = "https://cdn.fougito.com/Images/Restaurant/Item/default.png";
            }

            return Ok(_mapper.Map<IEnumerable<ItemDTO>>(await _service.AddRangeAsync(_mapper.Map<IEnumerable<Item>>(List))));
        }

        #region Status Repsonse Apis
        
        [HttpGet("Restaurants/{restaurantId}")]
        public async Task<IActionResult> GetAllItems(long restaurantId)
        {
            return Ok(new SuccessResponse<IEnumerable<ItemDTO>>("Data received successfully", _mapper.Map<IEnumerable<ItemDTO>>(await _service.GetAllAsync(restaurantId))));
        }

        [HttpGet("GetItem/{Id}")]
        public async Task<IActionResult> GetItemById(long Id)
        {
            IEnumerable<ItemDTO> List = _mapper.Map<IEnumerable<ItemDTO>>(await _service.GetByIdAsync(Id));
            return Ok(new SuccessResponse<ItemDTO>("Data received successfully", _mapper.Map<ItemDTO>(List.FirstOrDefault())));
        }

        #endregion

    }
}
