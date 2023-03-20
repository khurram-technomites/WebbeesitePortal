using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
    public class FileUpload : IFileUpload
    {
        private readonly HttpClient _client;
        private readonly ILogger<FileUpload> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public FileUpload(HttpClient client, ILogger<FileUpload> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }
        public async Task<string> DeleteFile(string Path, string table)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.DeleteAsync($"File?AbsoluteUri={Path}&Table={table}");

            return await _clientHelper.ParseResponseAsync<string>(HttpResponse);
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

                using MultipartFormDataContent content = new MultipartFormDataContent();

                await using MemoryStream memoryStream = new MemoryStream();
                file.CopyTo(memoryStream);
                byte[] Bytes = memoryStream.ToArray();

                var fileContent = new ByteArrayContent(Bytes);
                fileContent.Headers.Add("Content-Type", "multipart/form-data");
                content.Add(fileContent, "file", file.FileName);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                var response = await _client.PostAsync("File", content);

                return await response.Content.ReadAsStringAsync();
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
           
        }

        public async Task<object> Gallery()
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());


                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                var response = await _client.GetAsync("File/Gallery");

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
