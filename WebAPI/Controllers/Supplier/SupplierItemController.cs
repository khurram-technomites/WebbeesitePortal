using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Supplier
{
    [Route("api/Supplier")]
    [ApiController]
    public class SupplierItemController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISupplierItemService _supplierItemService;
        private readonly IFTPUpload _fTPUpload;

        public SupplierItemController(IMapper mapper,
            ISupplierItemService supplierItemService,
            IFTPUpload fTPUpload)
        {
            _mapper = mapper;
            _supplierItemService = supplierItemService;
            _fTPUpload = fTPUpload;
        }


        [HttpPost("{Id}/Items")]
        public async Task<IActionResult> GetAllSupplierItems(long Id, PagingParameters Model)
        {
            return Ok(_mapper.Map<IEnumerable<SupplierItemDTO>>(await _supplierItemService.GetAllBySupplierId(Id, Model)));
        }


        [HttpPost("{Id}/Category/{CategoryId}")]
        public async Task<IActionResult> GetItemsbySupplierAndcategoryId(long Id, long CategoryId, PagingParameters Model)
        {
            return Ok(_mapper.Map<IEnumerable<SupplierItemDTO>>(await _supplierItemService.GetItemBySupplierAndCategoryID(Id, CategoryId , Model)));
        }

        [HttpPost("Item")]
        public async Task<IActionResult> AddItem(SupplierItemDTO Model)
        {
            string Path = "/Images/Supplier/" + Model.SupplierId + "/";

            if (!string.IsNullOrEmpty(Model.Thumbnail))
                if (_fTPUpload.MoveFile(Model.Thumbnail, ref Path))
                {
                    Model.Thumbnail = Path;
                }

            foreach (var images in Model.SupplierItemImages)
            {
                string imagePath = "/Images/Supplier/" + Model.SupplierId + "/";
                if (_fTPUpload.MoveFile(images.Path, ref imagePath))
                {
                    images.Path = imagePath;
                }
            }

            Model.Supplier = null;
            Model.Category = null;
            Model.SupplierOrderDetails = null;

            return Ok(_mapper.Map<SupplierItemDTO>(await _supplierItemService.AddSupplierItemAsync(_mapper.Map<SupplierItem>(Model))));
        }

        [HttpPut("Item")]
        public async Task<IActionResult> UpdateItem(SupplierItemDTO Model)
        {
            string Path = "/Images/Supplier/" + Model.SupplierId + "/";

            if (!string.IsNullOrEmpty(Model.Thumbnail) && Model.Thumbnail.Contains("Draft"))
                if (_fTPUpload.MoveFile(Model.Thumbnail, ref Path))
                {
                    Model.Thumbnail = Path;
                }

            foreach (var images in Model.SupplierItemImages)
            {
                string imagePath = "/Images/Supplier/" + Model.SupplierId + "/";
                if (_fTPUpload.MoveFile(images.Path, ref imagePath))
                {
                    images.Path = imagePath;
                }
            }

            return Ok(_mapper.Map<SupplierItemDTO>(await _supplierItemService.UpdateSupplierItemAsync(_mapper.Map<SupplierItem>(Model))));
        }

        [HttpDelete("Item/{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<SupplierItemDTO>(await _supplierItemService.ArchiveSupplierItemAsync(Id)));
        }

        [HttpPut("Item/{Id}/ToggleStatus")]
        public async Task<IActionResult> ToggleApprovalStatus(long Id)
        {
            IEnumerable<Models.SupplierItem> list = await _supplierItemService.GetByIdAsync(Id);
            Models.SupplierItem supplieritem = list.FirstOrDefault();

            supplieritem.StockStatus = supplieritem.StockStatus == Enum.GetName(typeof(ItemStock), ItemStock.InStock) ?
                Enum.GetName(typeof(ItemStock), ItemStock.OutOfStock) : Enum.GetName(typeof(ItemStock), ItemStock.InStock);

            return Ok(_mapper.Map<SupplierItemDTO>(await _supplierItemService.UpdateSupplierItemAsync(supplieritem)));
        }

        [HttpGet("{Id}/Item")]
        public async Task<IActionResult> GetAllSupplierItems(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SupplierItemDTO>>(await _supplierItemService.GetAllBySupplierId(Id)));
        }

        [HttpGet("Item/{Id}")]
        public async Task<IActionResult> GetSupplierItemById(long Id)
        {
            var item = _mapper.Map<IEnumerable<SupplierItemDTO>>(await _supplierItemService.GetByIdAsync(Id));
            return Ok(item.FirstOrDefault());
        }

    }
}
