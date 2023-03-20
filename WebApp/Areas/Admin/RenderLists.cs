using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Admin
{
    public class RenderLists : Microsoft.AspNetCore.Mvc.ViewComponent
    {
        private readonly IGarageClient _garageClient;
        private readonly IMapper _mapper;

        public RenderLists(IGarageClient garageClient, IMapper mapper)
        {
            _garageClient = garageClient;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var info = _mapper.Map<IEnumerable<GarageViewModel>>(await _garageClient.GetGarages());
            return View(info);
        }
    }
}
