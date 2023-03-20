using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Helpers
{
    public class ValidateCashier : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool access = false;

            var Model = (RestaurantBalanceSheetDTO) context.ActionArguments.FirstOrDefault().Value;

            if (Model.RestaurantId < 1)
                access = false;
            else if (Model.RestaurantBranchId < 1)
                access = false;
            else if (Model.RestaurantCashierStaffId < 1)
                access = false;
            else
                access = true;

            if (!access)
            {
                var url = "/api/Error";
                context.Result = new RedirectResult(url);
            }
        }
    }
}
