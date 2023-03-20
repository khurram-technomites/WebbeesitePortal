using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.SparePartsDealer.Filter;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartDealersService : ISparePartsDealerService
    {
        private readonly ISparePartDealRepo _repo;
        public SparePartDealersService(ISparePartDealRepo repo)
        {
            _repo = repo;
        }

        public async Task<SparePartsDealer> AddSparePartsDealerAsync(SparePartsDealer Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<SparePartsDealer> ArchiveSparePartsDealerAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<SparePartsDealer>> GetAllSparePartsDealerAsync()
        {
            return await _repo.GetAllAsync(x => x.Status != Enum.GetName(typeof(Status), Status.Rejected));
        }
        public async Task<IEnumerable<SparePartsDealer>> GetAllSparePartsDealerAsync(SparePartFilterDTO Filter)
        {
            if (Filter.IsRecentRequired)
                return await _repo.GetAllAsync(x => (x.Status != Enum.GetName(typeof(Status), Status.Active) &&
                x.Status != Enum.GetName(typeof(Status), Status.Inactive)) &&
                (EF.Functions.Like(x.NameAsPerTradeLicense, "%" + Filter.Paging.Search + "%") || EF.Functions.Like(x.ReferenceCode, "%" + Filter.Paging.Search + "%")),
                Pagination: Filter.Paging, OrderExp: x => x.Id, ChildObjects: "DealerInventorySpecifications, DealerImages, DealerSchedules, User, SparePartsDealerDocuments", IsOrderDescending: true);
            else
                return await _repo.GetAllAsync(x => (x.Status == Enum.GetName(typeof(Status), Status.Active) ||
                x.Status == Enum.GetName(typeof(Status), Status.Inactive)) &&
                (EF.Functions.Like(x.NameAsPerTradeLicense, "%" + Filter.Paging.Search + "%") || EF.Functions.Like(x.ReferenceCode, "%" + Filter.Paging.Search + "%")),
                    Pagination: Filter.Paging, OrderExp: x => x.Id, ChildObjects: "DealerInventorySpecifications, DealerImages, DealerSchedules, User, SparePartsDealerDocuments");
        }
        public async Task<long> GetAllSparePartsDealersCountAsync()
        {
            return await _repo.GetCount();
        }
        public async Task<IEnumerable<SparePartsDealer>> GetSparePartsDealerByIdAsync(long Id)
        {
            //return await _repo.GetByIdAsync(x => x.Id == Id);
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "DealerInventorySpecifications, User, DealerImages, DealerSchedules, SparePartsDealerDocuments, DealerInventorySpecifications.CarMake");
        }

        public async Task<IEnumerable<SparePartsDealer>> GetSparePartsDealerByLogoAsync(string Path)
        {
            return await _repo.GetByIdAsync(x => x.Logo == Path);
        }

        public async Task<IEnumerable<SparePartsDealer>> GetSparePartsDealerBySlugAsync(string Slug)
        {
            return await _repo.GetByIdAsync(x => x.Slug == Slug);
        }

        public async Task<IEnumerable<SparePartsDealer>> GetSparePartsDealerByUserAsync(string UserId)
        {
            return await _repo.GetByIdAsync(x => x.UserId == UserId);
        }

        public async Task<SparePartsDealer> UpdateSparePartsDealerAsync(SparePartsDealer Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }

        public async Task<double> ActiveIactiveCount()
        {
            return await _repo.GetCount(x => (x.Status == Enum.GetName(typeof(Status), Status.Active) ||
                x.Status == Enum.GetName(typeof(Status), Status.Inactive)));
        }

        public async Task<IEnumerable<SparePartsDealer>> GetSparePartsDealerByVideoAsync(string Path)
        {
            return await _repo.GetByIdAsync(x => x.Video == Path);
        }

        public async Task<IEnumerable<SparePartsDealer>> GetDealerByOrigin(string Origin, string Section = "Default")
        {
            Origin = Origin.Replace("www.", "");
            IEnumerable<SparePartsDealer> List = Section switch
            {
                "Header" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "SparePartBannerSettings, SparePartBusinessSetting, SparePartMenuManagement, SparePartMenuManagement.SparePartMenu"),
                "Promo" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "SparePartBannerSettings, SparePartContentManagement"),
                "FAQ" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "SparePartFAQ"),
                "Footer" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "SparePartBannerSettings, SparePartBusinessSetting, SparePartMenuManagement, SparePartContentManagement, SparePartMenuManagement.SparePartMenu"),
                "Content" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "SparePartContentManagement"),
                "Services" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "SparePartServiceManagement"),
                "Team" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "SparePartTeamManagement"),
                "Expertise" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "SparePartExpertiseManagements, SparePartExpertiseManagements.SparePartExpertise, SparePartExpertiseManagements.SparePartExpertise.Expertise"),
                "Testimonials" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "SparePartTestimonials"),
                "Blogs" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "SparePartBlogs"),
                "Partner" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "SparePartPartnersManagement"),
                "Appointment" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "SparePartCustomerAppointment"),
                _ => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "SparePartBannerSettings, SparePartContentManagement, SparePartServiceManagement, " +
                                "SparePartBusinessSetting, SparePartTeamManagement, SparePartExpertiseManagements, SparePartExpertiseManagements.SparePartExpertise, " +
                                "SparePartExpertiseManagements.SparePartExpertise.Expertise, " +
                                "SparePartTestimonials, SparePartBlogs, SparePartPartnersManagement, SparePartMenuManagement, SparePartMenuManagement.SparePartMenu, SparePartFAQ"),
            };

            return List;
        }
    }
}
