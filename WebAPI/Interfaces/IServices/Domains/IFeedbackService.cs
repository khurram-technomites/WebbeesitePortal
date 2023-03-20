using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IFeedbackService 
    {
        Task<IEnumerable<Feedback>> GetAllFeedbacksAsync(PagingParameters Pagination);
        Task<IEnumerable<Feedback>> GetFeedbackByIdAsync(long Id);
        Task<Feedback> AddFeedbackAsync(Feedback Entity);
        Task<Feedback> UpdateFeedbackAsync(Feedback Entity);
        Task<Feedback> ArchiveFeedbackAsync(long Id);
    }
}
