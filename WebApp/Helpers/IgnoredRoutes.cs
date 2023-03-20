using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Helpers
{
    public static class IgnoredRoutes
    {
        public static List<ControllerActionViewModel> GetAllIgnoredRoutes()
        {
            List<ControllerActionViewModel> List = new();

            List.Add(new ControllerActionViewModel { Controller = "Account", Action = "*" });
            List.Add(new ControllerActionViewModel { Controller = "Home", Action = "Error" });
            List.Add(new ControllerActionViewModel { Controller = "Home", Action = "Privacy" });

            return List;
        }
    }
}
