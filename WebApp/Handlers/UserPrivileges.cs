using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApp.Handlers
{
    public class UserPrivileges : ActionFilterAttribute
    {

        //public override void OnActionExecuting(AuthorizationFilterContext context)
        //{
        //    //RouteValueDictionary RouteValues = context.HttpContext.Request.RouteValues;

        //    //string EndPoint = RouteValues.GetValueOrDefault("controller") + "/" + RouteValues.GetValueOrDefault("action");

        //    //bool IsAdmin = context.HttpContext.User.Claims
        //    //    .FirstOrDefault(c => c.Type == "Admin") == null ? false : true;

        //    //bool IsAssign = context.HttpContext.User.Claims
        //    //    .FirstOrDefault(c => c.Type == EndPoint) == null ? false : true;

        //    //if (IsAdmin || IsAssign)
        //    //    return;

        //    //context.Result = new ForbidResult(); 
        //    var asf = context.Result;
        //}

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }
    }
}
