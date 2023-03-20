using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Interfaces.IServices
{
    public interface ITemplateService
    {
        Task<string> RenderTemplateAsync<TViewModel>(string filename, TViewModel viewModel);
    }
}
