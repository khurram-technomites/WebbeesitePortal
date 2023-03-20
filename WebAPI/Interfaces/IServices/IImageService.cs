using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Interfaces.IServices
{
    public interface IImageService
    {
        Task<byte[]> DownloadImageAsync(Uri uri);
    }
}
