using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Configurations
{
	public class RolesConfiguration : IEntityTypeConfiguration<IdentityRole>
	{
		public void Configure(EntityTypeBuilder<IdentityRole> builder)
		{
            builder.HasData(
                    new IdentityRole
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    }
                //    new IdentityRole
                //    {
                //        Name = "AdminUser",
                //        NormalizedName = "ADMINUSER"
                //    },
                //    new IdentityRole
                //    {
                //        Name = "Customer",
                //        NormalizedName = "CUSTOMER"
                //    },
                //    new IdentityRole
                //    {
                //        Name = "ServiceStaff",
                //        NormalizedName = "SERVICESTAFF"
                //    },
                //    new IdentityRole
                //    {
                //        Name = "DeliveryStaff",
                //        NormalizedName = "DELIVERYSTAFF"
                //    },
                //    new IdentityRole
                //    {
                //        Name = "GarageOwner",
                //        NormalizedName = "GARAGEOWNER"
                //    },
                //    new IdentityRole
                //    {
                //        Name = "SparePartDealer",
                //        NormalizedName = "SPAREPARTDEALER"
                //    },
                //    new IdentityRole
                //    {
                //        Name = "RestaurantOwner",
                //        NormalizedName = "RESTAURANTOWNER"
                //    },
                //    new IdentityRole
                //    {
                //        Name = "RestaurantServiceStaff",
                //        NormalizedName = "RESTAURANTSERVICESTAFF"
                //    },
                //    new IdentityRole
                //    {
                //        Name = "RestaurantCashierStaff",
                //        NormalizedName = "RESTAURANTCASHIERSTAFF"
                //    },
                //new IdentityRole
                //{
                //    Name = "RestaurantKitchenManager",
                //    NormalizedName = "RESTAURANTKITCHENMANAGER"
                //}
            );
        }
	}
}
