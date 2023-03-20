using AutoMapper;
using HelperClasses.DTOs.Supplier;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Mapper.SupplierMapper
{
    public class SupplierOrderMapper : Profile
    {
        public SupplierOrderMapper()
        {
            CreateMap<SupplierOrder, SupplierOrderDTO>();
            CreateMap<SupplierOrderDTO, SupplierOrder>();

            CreateMap<SupplierOrderItemsDTO, SupplierOrderDetail>();

            CreateMap<SupplierOrderPlacementDTO, SupplierOrder>()
                .AfterMap<SetRemainingColumns>();
        }

        private class SetRemainingColumns : IMappingAction<SupplierOrderPlacementDTO, SupplierOrder>
        {
            private readonly ISupplierItemService _supplierItem;
            public SetRemainingColumns(ISupplierItemService supplierItem)
            {
                _supplierItem = supplierItem;
            }

            public void Process(SupplierOrderPlacementDTO source, SupplierOrder destination, ResolutionContext context)
            {
                foreach (var item in destination.SupplierOrderDetails)
                {
                    IEnumerable<SupplierItem> items = _supplierItem.GetByIdAsync(item.SupplierItemId).Result;
                    SupplierItem item1 = items.FirstOrDefault();

                    item.Price = item1.SalePrice > 0 ? item1.SalePrice : item1.RegularPrice ;
                    item.SupplierItemName = item1.Title;

                    item.UnitPrice = item.Price;
                    item.TotalPrice = item.Price * item.Quantity;
                }
            }
        }
    }
}
