using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageTestimonialsService
    {
        Task<IEnumerable<GarageTestimonials>> GetAllAsync();
        Task<IEnumerable<GarageTestimonials>> GetGarageTestimonialsByIdAsync(long Id);
        Task<IEnumerable<GarageTestimonials>> GetGarageTestimonialsByGarageIdAsync(long GaragedId);
        Task<GarageTestimonials> AddGarageTestimonialsAsync(GarageTestimonials Model);
        Task<GarageTestimonials> UpdateGarageTestimonialsAsync(GarageTestimonials Model);
        Task<GarageTestimonials> ArchiveGarageTestimonialsAsync(long Id);
        Task<long> GetCountByGarageIdAsync(long garageId);
    }
}
