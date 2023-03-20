using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;

namespace WebAPI.Services
{
    public class ImageService : IImageService
    {
        public async Task<byte[]> DownloadImageAsync(Uri uri)
        {
            using var httpClient = new HttpClient();

            // Get the file extension
            var uriWithoutQuery = uri.GetLeftPart(UriPartial.Path);

            // Download the image and write to the file
            return await httpClient.GetByteArrayAsync(uri);
        }
    }
}
