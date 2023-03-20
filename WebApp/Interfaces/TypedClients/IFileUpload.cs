using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IFileUpload
    {
        Task<string> UploadFile(IFormFile file);
        Task<string> DeleteFile(string Path, string table);
        Task<object> Gallery();
    }
}
