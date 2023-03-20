using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Garage;
using HelperClasses.DTOs.Garage.Filter;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageService : IGarageService
    {
        private readonly IGarageRepo _repo;
        public GarageService(IGarageRepo repo)
        {
            _repo = repo;
        }

        public async Task<Garage> AddGarageAsync(Garage Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<Garage> ArchiveGarageAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<Garage>> GetAllGaragesAsync(GarageFilter Filter)
        {
            if (Filter.IsRecentRequired)
                return await _repo.GetAllAsync(x => (x.Status != Enum.GetName(typeof(Status), Status.Active) &&
                x.Status != Enum.GetName(typeof(Status), Status.Inactive)) &&
                (EF.Functions.Like(x.NameAsPerTradeLicense, "%" + Filter.Paging.Search + "%") || EF.Functions.Like(x.ReferenceCode, "%" + Filter.Paging.Search + "%")),
                Pagination: Filter.Paging, OrderExp: x => x.Id, ChildObjects: "GarageRepairSpecifications, GarageImages, GarageSchedules, GarageRatings, GarageDocuments", IsOrderDescending: true);
            else
                return await _repo.GetAllAsync(x => (x.Status == Enum.GetName(typeof(Status), Status.Active) ||
                x.Status == Enum.GetName(typeof(Status), Status.Inactive)) &&
                (EF.Functions.Like(x.NameAsPerTradeLicense, "%" + Filter.Paging.Search + "%") || EF.Functions.Like(x.ReferenceCode, "%" + Filter.Paging.Search + "%")),
                    Pagination: Filter.Paging, OrderExp: x => x.Id, ChildObjects: "GarageRepairSpecifications, GarageImages, GarageSchedules, GarageRatings, GarageDocuments");
        }

        public async Task<long> GetAllGaragesCountAsync()
        {
            return await _repo.GetCount();
        }

        public async Task<long> GetAllGaragesCountByUserIdAsync(long VendorId)
        {
            return await _repo.GetCount(x => x.VendorId == VendorId);
        }

        public async Task<IEnumerable<Garage>> GetAllGaragesAsync()
        {
            return await _repo.GetAllAsync(x => x.Status != Enum.GetName(typeof(Status), Status.Rejected));
        }

        public IEnumerable<GarageCardResponseDTO> GetAllNearMe(GarageFilter Filter)
        {
            return _repo.GetAllNearMe(Filter);
        }

        public async Task<IEnumerable<Garage>> GetGarageByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "GarageRepairSpecifications,GarageRepairSpecifications.CarMake, User, GarageImages, GarageSchedules, GarageRatings, GarageRatings.User, GarageDocuments,ClientType,ClientIndustry,GarageContentManagement,GarageBusinessSetting,Country,City");
        }

        public async Task<IEnumerable<Garage>> GetGarageByLogoAsync(string Path)
        {
            return await _repo.GetByIdAsync(x => x.Logo == Path);
        }

        public async Task<IEnumerable<Garage>> GetGarageBySlugAsync(string Slug)
        {
            return await _repo.GetByIdAsync(x => x.Slug == Slug, ChildObjects: "GarageRepairSpecifications , GarageRepairSpecifications.CarMake, GarageImages, GarageSchedules, GarageRatings, GarageRatings.User");
        }

        public async Task<IEnumerable<Garage>> GetGarageByUserAsync(string UserId)
        {
            return await _repo.GetByIdAsync(x => x.UserId == UserId, ChildObjects: "GarageRepairSpecifications,GarageRepairSpecifications.CarMake, User, GarageImages, GarageSchedules, GarageRatings, GarageRatings.User, GarageDocuments");
        }
        public async Task<IEnumerable<Garage>> GetGarageByVendorAsync(long VendorId)
        {
            return await _repo.GetByIdAsync(x => x.VendorId == VendorId, ChildObjects: "GarageRepairSpecifications,GarageRepairSpecifications.CarMake, User, GarageImages, GarageSchedules, GarageRatings, GarageRatings.User, GarageDocuments");
        }

        public async Task<Garage> UpdateGarageAsync(Garage Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }

        public async Task<double> ActiveIactiveCount()
        {
            return await _repo.GetCount(x => (x.Status == Enum.GetName(typeof(Status), Status.Active) ||
                x.Status == Enum.GetName(typeof(Status), Status.Inactive)));
        }

        public async Task<IEnumerable<Garage>> GetGarageByVideoAsync(string Path)
        {
            return await _repo.GetByIdAsync(x => x.Video == Path);
        }

        public async Task<IEnumerable<Garage>> GetGarageByOrigin(string Origin, string Section = "Default")
        {
            Origin = Origin.Replace("www.", "");
            IEnumerable<Garage> List = Section switch
            {
                "Header" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "GarageBannerSettings, GarageBusinessSetting, GarageMenuManagement, GarageMenuManagement.GarageMenu , GarageContentManagement , User"),
                "Promo" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "GarageBannerSettings, GarageContentManagement"),
                "FAQ" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "GarageFAQs"),
                "Garage" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "Garage"),
                "BusinessSetting" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "GarageBusinessSetting"),
                "Footer" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "GarageBannerSettings, GarageBusinessSetting, GarageMenuManagement, GarageContentManagement, GarageMenuManagement.GarageMenu , User"),
                "Content" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "GarageContentManagement"),
                "Services" => await _repo.GetByIdAsync(x => x.Website == Origin && x.IsServicesAllowed == true, ChildObjects: "GarageServiceManagement"),
                "Team" => await _repo.GetByIdAsync(x => x.Website == Origin && x.IsTeamsAllowed == true, ChildObjects: "GarageTeamManagement"),
                "Expertise" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "GarageExpertiseManagements, GarageExpertiseManagements.GarageExpertise, GarageExpertiseManagements.GarageExpertise.Expertise"),
                "Testimonials" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "GarageTestimonials"),
                "Blogs" => await _repo.GetByIdAsync(x => x.Website == Origin && x.IsBlogsAllowed == true, ChildObjects: "GarageBlogs"),
                "Partner" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "GaragePartnersManagement"),
                "Appointment" => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "GarageAppointmentManagement"),
                _ => await _repo.GetByIdAsync(x => x.Website == Origin, ChildObjects: "GarageBannerSettings, GarageContentManagement, GarageServiceManagement, " +
                                "GarageBusinessSetting, GarageTeamManagement, GarageExpertiseManagements, GarageExpertiseManagements.GarageExpertise, " +
                                "GarageExpertiseManagements.GarageExpertise.Expertise, " +
                                "GarageTestimonials, GarageBlogs, GaragePartnersManagement, GarageMenuManagement, GarageMenuManagement.GarageMenu"),
            };

            return List;
        }
    }
}
