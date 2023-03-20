using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Supplier.Controllers
{
    [Area("Supplier")]
    [Authorize(Roles = "Supplier")]
    public class DocumentController : Controller
    {
        private readonly ISupplierDocumentClient _supplierDocumentClient;
        private readonly IMapper _mapper;

        public DocumentController(ISupplierDocumentClient supplierDocumentClient, IMapper mapper)
        {
            _supplierDocumentClient = supplierDocumentClient;
            _mapper = mapper;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDocument(SupplierDocumentViewModel Model)
        {
            if (Model.Path == "undefined")
            {
                return Json(
               new
               {
                   success = false,
                   message = "Please Upload Document!",
               });
            }
            else if (Model.DocumentType == null)
            {
                return Json(
               new
               {
                   success = false,
                   message = "Name Field Rquired...!",
               });
            }
            else if (Model.ExpiryDateTime.ToString() == "1/1/0001 12:00:00 AM")
            {
                return Json(
               new
               {
                   success = false,
                   message = "Date Field Rquired...!",
               });
            }
            else
            {
                var result = await _supplierDocumentClient.AddSupplierDocument(_mapper.Map<SupplierDocumentDTO>(Model));

                return Json(
                    new
                    {
                        success = true,
                        message = "Document Added Successfully",
                        data = new
                        {
                            result.Id,
                            result.Path,
                            ExpiryDateTime = result.ExpiryDateTime.ToShortDateString(),
                            result.DocumentType
                        }
                    });
            }
           
        }

        public async Task<IActionResult> DeleteDocument(long Id)
        {
            await _supplierDocumentClient.DeleteSupplierDocument(Id);

            return Json(
                new
                {
                    success = true,
                    message = "Document Deleted Successfully",                   
                });
        }
    }
}
