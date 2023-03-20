using HelperClasses.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepo _repo;

        public TicketService(ITicketRepo repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            return await _repo.GetAllAsync(OrderExp: x => x.Id, ChildObjects: "user , Restaurant ,Supplier");
        }
        public async Task<IEnumerable<Ticket>> GetTicketsByOpenStatus()
        {
            var tickets =  await _repo.GetAllAsync(i => i.Status == "OPEN");
            return tickets;
        }
        public async Task<Ticket> GetTicketsByUser(string userID)
        {
            var tickets = await _repo.GetAllAsync(x => x.UserId == userID);
            return tickets.FirstOrDefault();
        }
		public async Task<IEnumerable<Ticket>> GetTicketsByModule(string Module)
		{
            if (Module == "Supplier")
            {
				return await _repo.GetAllAsync(ChildObjects: "user , Supplier , Restaurant");
			}
            else
            {
				return await _repo.GetAllAsync(ChildObjects: "user , Supplier , Restaurant");
			}
		}
		public async Task<long> GetAllTicketsCountAsync()
		{
			return await _repo.GetCount(x =>x.Status == "Active");
		}
		public async Task<IEnumerable<Ticket>> GetTicketsByRestaurant(long restaurantID)
        {
            return await _repo.GetAllAsync(i => i.RestaurantId == restaurantID, ChildObjects: "user , Restaurant");
        }
		public async Task<IEnumerable<Ticket>> GetTicketsBySupplier(long supplierId)
		{
			return await _repo.GetAllAsync(i => i.SupplierId == supplierId, ChildObjects: "user , Supplier , Restaurant");
		}
		public async Task<Ticket> GetTicket(long Id)
        {
           var tickets =  await _repo.GetByIdAsync(x => x.Id == Id);
			return tickets.FirstOrDefault();

		}
		public async Task<Ticket> Create(Ticket Model)
		{
			return await _repo.InsertAsync(Model);
		}
		public async Task<Ticket> Update(Ticket Model)
		{
			return await _repo.UpdateAsync(Model);
		}
		//public bool CreateTicket(Ticket Ticket, ref string message)
		//{
		//	try
		//	{

		//		Ticket.IsDeleted = false;
		//		Ticket.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
		//		_ticketRepository.Add(Ticket);
		//		if (SaveTicket())
		//		{
		//			message = "Ticket added successfully ...";
		//			return true;

		//		}
		//		else
		//		{
		//			message = "Oops! Something went wrong. Please try later.";
		//			return false;
		//		}


		//	}
		//	catch (Exception ex)
		//	{
		//		message = "Oops! Something went wrong. Please try later.";
		//		return false;
		//	}
		//}

		//public bool UpdateTicket(ref Ticket ticket, ref string message)
		//{
		//	try
		//	{
		//		Ticket CurrentTicket = _ticketRepository.GetById(ticket.ID);

		//		CurrentTicket.UserID = ticket.UserID;
		//		CurrentTicket.Status = ticket.Status;

		//		ticket = null;

		//		_ticketRepository.Update(CurrentTicket);
		//		if (SaveTicket())
		//		{
		//			ticket = CurrentTicket;
		//			message = "Ticket updated successfully ...";
		//			return true;
		//		}
		//		else
		//		{
		//			message = "Oops! Something went wrong. Please try later.";
		//			return false;
		//		}

		//	}
		//	catch (Exception ex)
		//	{
		//		message = "Oops! Something went wrong. Please try later.";
		//		return false;
		//	}
		//}

		//public bool DeleteTicket(long id, ref string message, bool softDelete = true)
		//{
		//	try
		//	{
		//		Ticket ticket = _.GetByIdAsync(id);

		//		if (softDelete)
		//		{
		//			ticket.IsDeleted = true;
		//			_ticketRepository.Update(ticket);
		//		}
		//		else
		//		{
		//			_ticketRepository.Delete(ticket);
		//		}
		//		if (SaveTicket())
		//		{
		//			message = "Ticket deleted successfully ...";
		//			return true;
		//		}
		//		else
		//		{
		//			message = "Oops! Something went wrong. Please try later.";
		//			return false;
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		message = "Oops! Something went wrong. Please try later.";
		//		return false;
		//	}
		//}
	}

}
