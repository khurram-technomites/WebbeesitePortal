using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepo _repo;
        public FeedbackService(IFeedbackRepo repo)
        {
            _repo = repo;
        }

        public async Task<Feedback> AddFeedbackAsync(Feedback Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<Feedback> ArchiveFeedbackAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync(PagingParameters Pagination)
        {
            return await _repo.GetAllAsync(Pagination: Pagination, OrderExp: x => x.Id, ChildObjects: "User");
        }

        public async Task<IEnumerable<Feedback>> GetFeedbackByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<Feedback> UpdateFeedbackAsync(Feedback Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }

    }
}
