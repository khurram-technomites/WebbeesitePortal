using HelperClasses.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
	public class RestaurantTableReservationService : IRestaurantTableReservationService
	{
		private readonly IRestaurantTableReservationRepo _repo;
		private readonly IRestaurantTableService _tableService;
		public RestaurantTableReservationService(IRestaurantTableReservationRepo repo, IRestaurantTableService tableService)
		{
			_repo = repo;
			_tableService = tableService;
		}

		public async Task<IEnumerable<RestaurantTableReservation>> GetAllAsync()
		{
			return await _repo.GetAllAsync();
		}

		public async Task<RestaurantTableReservation> GetByIdAsync(long Id)
		{
			var list = await _repo.GetByIdAsync(x => x.Id == Id);
			return list.FirstOrDefault();
		}

		public async Task<IEnumerable<RestaurantTableReservation>> GetByRestaurentTableId(long restaurantTableId)
		{
			return await _repo.GetAllAsync(x => x.RestaurantTableId == restaurantTableId, ChildObjects: "RestaurantTable");
		}

		public async Task<IEnumerable<RestaurantTableReservation>> GetReservedByRestaurentTableId(long restaurantTableId)
		{
			return await _repo.GetAllAsync(x => x.RestaurantTableId == restaurantTableId && x.Status == Enum.GetName(typeof(Status), Status.Reserved), ChildObjects: "RestaurantTable");
		}

		public async Task<RestaurantTableReservation> AddRestaurantTableReservationAsync(RestaurantTableReservation Model)
		{
			return await _repo.InsertAsync(Model);
		}

		public async Task<RestaurantTableReservation> AddRestaurantTableReservationByOrder(RestaurantTableReservation Model)
		{
			List<RestaurantTable> listTable = new List<RestaurantTable>();
			string Status = string.Empty;

			if (Model.Status == Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Reserved))
			{
				Status = Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Reserved);
			}
			else if (Model.Status == Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Completed) || Model.Status == Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Canceled))
			{
				Status = Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Active);
			}

			if (!/*string.IsNullOrEmpty(Model.MergeTableIds)*/true)
			{
				for (int i = 0; i <= Model.MergeTableIds.Length; i++)
				{
					RestaurantTable table = await _tableService.GetByIdAsync(Model.MergeTableIds[i]);
					listTable.Add(table);
				}
			}
			else
			{
				var table = _tableService.GetByIdAsync(Model.RestaurantTableId).Result;
				listTable.Add(table);
			}

			foreach (var item in listTable)
			{
				item.Status = Status;
				item.RestaurantTableReservations = null;
				item.Restaurant = null;
				item.RestaurantBranch = null;
				await _tableService.UpdateRestaurantTableAsync(item);
			}

			if (Model.Status == Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Reserved))
			{
				Model.RestaurantTable = null;
				Model.Order = null;
				return await _repo.InsertAsync(Model);
			}
			else
			{
				var reserve = await _repo.GetAllAsync(x => x.OrderId == (long)Model.OrderId);
				Model.Id = reserve.FirstOrDefault().Id;
				Model.RestaurantTable = null;
				Model.Order = null;
				return await _repo.UpdateAsync(Model);
			}
		}

		public async Task<bool> UpdateRestaurantTableReservationByOrder(RestaurantTable Model, long OrderId)
		{
			string Status = string.Empty;
			var reservations = await _repo.GetAllAsync(x => x.OrderId == OrderId && x.Status == Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Reserved));

			if (Model.Status == Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Active))
			{
				Status = Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Completed);
			}
			else if (Model.Status == Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Canceled))
			{
				Status = Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Canceled);
				Model.Status = Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Active);
			}
			else if (Model.Status == Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Merged))
			{
				Status = Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Merged);
				Model.Status = Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Active);
			}

			Model.Restaurant = null;
			Model.RestaurantBranch = null;
			Model.RestaurantTableReservations = new List<RestaurantTableReservation>();
			await _tableService.UpdateRestaurantTableAsync(Model);
	
			foreach (var item in reservations)
			{
				item.Status = Status;
				item.RestaurantTable = null;
				item.Order = null;
				await _repo.UpdateAsync(item);
			}

			return true;
		}

		public async Task<RestaurantTableReservation> UpdateRestaurantTableReservationAsync(RestaurantTableReservation Model)
		{
			return await _repo.UpdateAsync(Model);
		}

		public async Task<RestaurantTableReservation> ArchiveRestaurantTableReservationAsync(long id)
		{
			return await _repo.ArchiveAsync(id);
		}
	}
}
