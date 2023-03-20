using AutoMapper;
using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebAPI.ErrorHandling;
using WebAPI.Interfaces.IRepositories;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.Repositories;
using WebAPI.ResponseWrapper;
using WebAPI.Services;

namespace WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [Authorize]
    public class FileController : ControllerBase
	{
		private readonly IFTPUpload _ftpUpload;
		private readonly ICityService _cityService;
		private readonly ICarMakeService _carMakeService;
		private readonly IRequestService _requestService;
		private readonly ISparePartRequestImagesService _sparePartRequestImagesService;
		private readonly IGarageService _garageService;
		private readonly IGarageImageService _garageImageService;
		private readonly IDealerImageService _dealerImageService;
		private readonly ISparePartsDealerService _sparePartsDealerService;
		private readonly IUserService _userService;
		private readonly IGarageDocumentService _garageDocumentService;
		private readonly ISparePartsDealerDocumentService _sparePartsDealerDocumentService;
		private readonly IRestaurantImageService _restaurantImageService;
		private readonly ISupplierDocumentService _supplierDocumentService;
		private readonly ISupplierItemImageService _supplierItemImageService;
		private readonly IGarageProjectImageService _garageProjectImageService;
		private readonly UserManager<AppUser> _userManager;
        public FileController(IFTPUpload ftpUpload, ICityService cityService, ICarMakeService carMakeService, IRequestService requestService,
            ISparePartRequestImagesService sparePartRequestImagesService, IGarageService garageService, IGarageImageService garageImageService,
            IDealerImageService dealerImageService, ISparePartsDealerService sparePartsDealerService, IUserService userService, UserManager<AppUser> userManager,
            IGarageDocumentService garageDocumentService, ISparePartsDealerDocumentService sparePartsDealerDocumentService,
            IRestaurantImageService restaurantImageService, ISupplierDocumentService supplierDocumentService, ISupplierItemImageService supplierItemImageService, 
			IGarageProjectImageService garageProjectImageService)
        {
            _ftpUpload = ftpUpload;
            _cityService = cityService;
            _carMakeService = carMakeService;
            _requestService = requestService;

            _sparePartRequestImagesService = sparePartRequestImagesService;
            _garageService = garageService;
            _garageImageService = garageImageService;
            _dealerImageService = dealerImageService;
            _sparePartsDealerService = sparePartsDealerService;
            _userService = userService;
            _userManager = userManager;
            _garageDocumentService = garageDocumentService;
            _sparePartsDealerDocumentService = sparePartsDealerDocumentService;
            _restaurantImageService = restaurantImageService;
            _supplierDocumentService = supplierDocumentService;
            _supplierItemImageService = supplierItemImageService;
            _garageProjectImageService = garageProjectImageService;
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload(IFormFile file)
        {
            string AbsoluteUri = string.Empty;

            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (_ftpUpload.UploadToDraft(file, ref AbsoluteUri))
                return Ok(AbsoluteUri);

            return Conflict();
        }

        [HttpPost("Mobile"), DisableRequestSizeLimit, AllowAnonymous]
        public IActionResult MobileUpload(IFormFile file)
        {
            string AbsoluteUri = string.Empty;

            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (_ftpUpload.UploadToDraft(file, ref AbsoluteUri))
                return Ok(new SuccessResponse<string>("", AbsoluteUri));

            return Conflict(new ErrorDetails(409, "Unable to upload image. Try again", null));
        }

        [HttpDelete()]
        public async Task<IActionResult> Delete([FromQuery] string AbsoluteUri, [FromQuery] string Table)
        {
            if (string.IsNullOrEmpty(AbsoluteUri))
                throw new ArgumentNullException(nameof(AbsoluteUri));

            await DeleteFromDB(Uri.UnescapeDataString(AbsoluteUri).Replace("wwwroot/", ""), Table);

            _ftpUpload.DeleteFile(Uri.UnescapeDataString(AbsoluteUri));

            return Ok(AbsoluteUri.Replace("wwwroot/", ""));
        }

        [HttpDelete("Mobile")]
        public async Task<IActionResult> MobileDelete([FromQuery] string AbsoluteUri, string Table = "optional")
        {
            if (string.IsNullOrEmpty(AbsoluteUri))
            {
                throw new ArgumentNullException(nameof(AbsoluteUri));
            }

            await DeleteFromDB(Uri.UnescapeDataString(AbsoluteUri), Table);

            if (_ftpUpload.DeleteFile(Uri.UnescapeDataString(AbsoluteUri)))
                return Ok(new SuccessResponse<string>("", Uri.UnescapeDataString(AbsoluteUri)));

            return Conflict(new ErrorDetails(409, "Unable to delete image. Try again", null));
        }

        private async Task<bool> DeleteFromDB(string AbsoluteUri, string Table)
        {
            bool isDeleted = false;
            string[] Split = AbsoluteUri.Split("/");

            string SwitchSearch = AbsoluteUri.Contains("Draft") ? Table : Split[4];

            switch (SwitchSearch)
            {
                case "City":
                    IEnumerable<City> cities = await _cityService.GetCityByLogoAsync(AbsoluteUri);

                    if (cities.Any())
                    {
                        City city = cities.FirstOrDefault();

                        city.Logo = null;

                        await _cityService.UpdateCityAsync(city);

                        isDeleted = true;
                    }

                    break;

                case "CarMake":
                    IEnumerable<CarMake> carMakes = await _carMakeService.GetCarMakeByImageAsync(AbsoluteUri);

                    if (carMakes.Any())
                    {
                        CarMake carMake = carMakes.FirstOrDefault();

                        carMake.Logo = null;

                        await _carMakeService.UpdateCarMakeAsync(carMake);

                        isDeleted = true;
                    }

                    break;

                case "RestaurantImages":
                    IEnumerable<RestaurantImages> restaurantImages = await _restaurantImageService.GetImageByPath(AbsoluteUri);

                    if (restaurantImages.Any())
                    {
                        RestaurantImages restaurantImage = restaurantImages.FirstOrDefault();

                        restaurantImage.Image = null;

                        await _restaurantImageService.ArchiveRestaurantImageAsync(restaurantImage.Id);

                        isDeleted = true;
                    }

                    break;

                case "SparePartRequest":
                    IEnumerable<SparePartRequest> sparePartRequests = await _requestService.GetRequestByMulkiyaAsync(AbsoluteUri);

                    if (sparePartRequests.Any())
                    {
                        SparePartRequest sparePartRequest = sparePartRequests.FirstOrDefault();

                        if (sparePartRequest.MulkiyaImageFront.Equals(AbsoluteUri))
                            sparePartRequest.MulkiyaImageFront = null;
                        else
                            sparePartRequest.MulkiyaImageBack = null;

                        await _requestService.UpdateRequestAsync(sparePartRequest);

                        isDeleted = true;
                    }

                    break;


                case "SparePartRequestImages":
                    IEnumerable<SparePartRequestImage> sparePartRequestImages = await _sparePartRequestImagesService.GetRequestByImageAsync(AbsoluteUri);

                    if (sparePartRequestImages.Any())
                    {
                        SparePartRequestImage sparePartRequestImage = sparePartRequestImages.FirstOrDefault();

                        sparePartRequestImage.Image = null;

                        await _sparePartRequestImagesService.DeleteAsync(sparePartRequestImage.Id);

                        isDeleted = true;
                    }

                    break;

                case "Garage":
                    IEnumerable<Garage> garages = await _garageService.GetGarageByLogoAsync(AbsoluteUri);

                    if (garages.Any())
                    {
                        Garage garage = garages.FirstOrDefault();

                        garage.Logo = null;

                        await _garageService.UpdateGarageAsync(garage);

                        isDeleted = true;

                        break;
                    }

                    IEnumerable<Garage> garagesVideo = await _garageService.GetGarageByVideoAsync(AbsoluteUri);

                    if (garagesVideo.Any())
                    {
                        Garage garage = garagesVideo.FirstOrDefault();

                        garage.Video = null;

                        await _garageService.UpdateGarageAsync(garage);

                        isDeleted = true;

                        break;
                    }

                    IEnumerable<GarageDocument> garageDocuments = await _garageDocumentService.GetByPath(AbsoluteUri);

                    if (garageDocuments.Any())
                    {
                        GarageDocument document = garageDocuments.FirstOrDefault();

                        await _garageDocumentService.DeleteRecord(document.Id);

                        break;
                    }

                    IEnumerable<GarageProjectImages> GarageProjectImages = await _garageProjectImageService.GetByPath(AbsoluteUri);
                    if (GarageProjectImages.Any())
                    {
                        GarageProjectImages GarageProjectImage = GarageProjectImages.FirstOrDefault();

                        await _garageProjectImageService.DeleteImage(GarageProjectImage.Id);

                        isDeleted = true;
                        break;

                    }

                    break;

                case "GarageImages":
                    IEnumerable<GarageImage> garageImages = await _garageImageService.GetGarageImagesByImagePath(AbsoluteUri);

                    if (garageImages.Any())
                    {
                        GarageImage garage = garageImages.FirstOrDefault();

                        await _garageImageService.DeleteGarageImage(garage.Id);

                        isDeleted = true;
                    }

                    break;

                
                case "SparePartsDealer":
                    IEnumerable<Models.SparePartsDealer> SparePartsDealerList = await _sparePartsDealerService.GetSparePartsDealerByLogoAsync(AbsoluteUri);

                    if (SparePartsDealerList.Any())
                    {
                        Models.SparePartsDealer sparePartsDealer = SparePartsDealerList.FirstOrDefault();

                        sparePartsDealer.Logo = null;

                        await _sparePartsDealerService.UpdateSparePartsDealerAsync(sparePartsDealer);

                        isDeleted = true;

                        break;
                    }

                    IEnumerable<Models.SparePartsDealer> SparePartsDealerVideoList = await _sparePartsDealerService.GetSparePartsDealerByVideoAsync(AbsoluteUri);

                    if (SparePartsDealerVideoList.Any())
                    {
                        Models.SparePartsDealer sparePartsDealer = SparePartsDealerVideoList.FirstOrDefault();

                        sparePartsDealer.Video = null;

                        await _sparePartsDealerService.UpdateSparePartsDealerAsync(sparePartsDealer);

                        isDeleted = true;

                        break;
                    }

                    IEnumerable<Models.SparePartsDealerDocument> dealerDocuments = await _sparePartsDealerDocumentService.GetByPath(AbsoluteUri);

                    if (dealerDocuments.Any())
                    {
                        SparePartsDealerDocument document = dealerDocuments.FirstOrDefault();

                        await _sparePartsDealerDocumentService.DeleteRecord(document.Id);

                        break;
                    }


                    break;

                case "SparePartsDealerImages":
                    IEnumerable<DealerImage> DealerImageList = await _dealerImageService.GetDealerImageByImagePathAsync(AbsoluteUri);

                    if (DealerImageList.Any())
                    {
                        DealerImage dealerImage = DealerImageList.FirstOrDefault();

                        await _dealerImageService.DeleteDealerImageAsync(dealerImage.Id);

                        isDeleted = true;
                    }

                    break;

                case "Customer":
                    IEnumerable<AppUser> List = await _userService.GetUsersByLogoAsync(AbsoluteUri);

                    if (List.Any())
                    {
                        AppUser user = List.FirstOrDefault();
                        user.Logo = null;

                        await _userManager.UpdateAsync(user);

                        isDeleted = true;
                    }

                    break;

                case "Supplier":
                    IEnumerable<SupplierDocument> SupplierDocuments = await _supplierDocumentService.GetAllByDocumentPath(AbsoluteUri);

                    if (SupplierDocuments.Any())
                    {
                        SupplierDocument document = SupplierDocuments.FirstOrDefault();

                        await _supplierDocumentService.DeleteSupplierDocumentAsync(document.Id);

                        isDeleted = true;
                    }

					IEnumerable<SupplierItemImage> SupplierImage = await _supplierItemImageService.GetByImagePathAsync(AbsoluteUri);

					if (SupplierImage.Any())
					{
						SupplierItemImage image = SupplierImage.FirstOrDefault();

						await _supplierItemImageService.DeleteSupplierItemImageAsync(image.Id);

						isDeleted = true;
					}

					break;

                default:
                    break;
            }

            return isDeleted;
        }

        [HttpGet("Gallery")]
        public async Task<IActionResult> Gallery()
        {
            var Gallery = _ftpUpload.FetchGallery();

            return Ok(new SuccessResponse<object>("", new { data = Gallery.ToList() }));
        }
    }
}
