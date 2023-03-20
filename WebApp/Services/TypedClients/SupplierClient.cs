using HelperClasses.DTOs;
using HelperClasses.DTOs.Supplier;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
    public class SupplierClient : ISupplierClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SupplierClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public SupplierClient(HttpClient client, ILogger<SupplierClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }


        public async Task<IEnumerable<SupplierCardDTO>> GetAllSuppliersAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Supplier");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierCardDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<SupplierDTO>> GetAllSuppliersListAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Supplier/GetAll");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<SupplierDTO>> GetSupplierByIDAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Supplier/{Id}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<SupplierDTO>> GetAllForApproval()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Supplier/GetAllForapprovalStatus");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<SupplierDTO>> GetSupplierForDropDwonAsync()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.PostAsync("Supplier/GetUnAssignedSupplierCodeSupplier", null);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<SupplierDTO>> GetSupplierForDropDwonAssignAsync()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.PostAsync("Supplier/GetAssignedSupplierCodeSupplier", null);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierDTO>>(HttpResponse);
        }

        public async Task<SupplierDTO> ToggleApproveAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.PutAsync($"Supplier/{Id}/ToggleApprovalStatus", null);

            return await _clientHelper.ParseResponseAsync<SupplierDTO>(HttpResponse);
        }

        public async Task<SupplierDTO> UpdateSupplierAsync(SupplierDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PutAsync($"Supplier/Account/CompleteProfile", content);

            return await _clientHelper.ParseResponseAsync<SupplierDTO>(HttpResponse);
        }
        public async Task<SupplierDTO> Reject(SupplierDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PutAsync($"Supplier/Reject", content);

            return await _clientHelper.ParseResponseAsync<SupplierDTO>(HttpResponse);
        }
        public async Task<SupplierDTO> Approve(SupplierDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PutAsync($"Supplier/Approve", content);

            return await _clientHelper.ParseResponseAsync<SupplierDTO>(HttpResponse);
        }

        public async Task<SupplierDTO> AddSupplierAsync(SupplierDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PostAsync("Supplier", content);

            return await _clientHelper.ParseResponseAsync<SupplierDTO>(HttpResponse);
        }
        public async Task<SupplierDTO> DeleteSupplier(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.DeleteAsync($"Supplier/{Id}");

            return await _clientHelper.ParseResponseAsync<SupplierDTO>(HttpResponse);
        }

        public async Task<IEnumerable<SupplierDTO>> GetSupplierById(long SupplierId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Supplier/{SupplierId}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierDTO>>(HttpResponse);
        }


        public async Task<IEnumerable<SupplierItemDTO>> GetAllSupplierItems(long SupplierId , PagingParameters paging)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(paging);
            var HttpResponse = await _client.PostAsync($"Supplier/{SupplierId}/Items", content);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierItemDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SupplierItemDTO>> GetAllSupplierItemsByCategory(long SupplierId , long CategoryId , PagingParameters paging)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(paging);
            var HttpResponse = await _client.PostAsync($"Supplier/{SupplierId}/Category/{CategoryId}" , content);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierItemDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SupplierItemCategoryDTO>> GetAllCategory(long SupplierId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Supplier/{SupplierId}/Category");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierItemCategoryDTO>>(HttpResponse);
        }

        public async Task<SupplierItemDTO> GetItemById(long SupplierItemId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Supplier/Item/{SupplierItemId}");

            return await _clientHelper.ParseResponseAsync<SupplierItemDTO>(HttpResponse);
        }
        public async Task<SupplierDTO> ToggleActiveStatusAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.PutAsync($"Supplier/{Id}/ToggleActiveStatus", null);

            return await _clientHelper.ParseResponseAsync<SupplierDTO>(HttpResponse);
        }






    }
}
