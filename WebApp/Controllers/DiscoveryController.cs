using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DynamicAuthorization.Mvc.Core;
using Microsoft.AspNetCore.Mvc;
using WebApp.Helpers;
using WebApp.ViewModels;

namespace WebApp.Controllers
{

    public class DiscoveryController : Controller
    {
        public IActionResult Index()
        {
            List<ControllerActionViewModel> IgnoreRoutes = IgnoredRoutes.GetAllIgnoredRoutes();
            List<ControllerActionViewModel> IgnoreControllers = IgnoreRoutes.Where(x => x.Action == "*").ToList();

            Assembly asm = Assembly.GetExecutingAssembly();

            List<ControllerActionViewModel> controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                    .Select(x => new ControllerActionViewModel()
                    {
                        Controller = x.DeclaringType.Name.Replace("Controller", ""),
                        Action = x.Name
                    })
                    .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();
            var list = new List<ControllerActionViewModel>();
            foreach (var item in controlleractionlist)
            {
                if (!IgnoreControllers.Any(x => x.Controller == item.Controller))
                {
                    if (!IgnoreRoutes.Any(x=>x.Controller == item.Controller && x.Action == item.Action))
                    {
                        if (!list.Any(x => x.Controller == item.Controller && x.Action == item.Action))
                        {
                            list.Add(new ControllerActionViewModel()
                            {
                                Controller = item.Controller,
                                Action = item.Action,
                            });
                        }
                    }
                    
                }
                    
            }
            return View(list);
        }
    }
}
