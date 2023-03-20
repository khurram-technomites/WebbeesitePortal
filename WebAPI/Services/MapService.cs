using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;

namespace WebAPI.Services
{
    public class MapService : IMapService
    {
        private readonly HttpClient _client;
        private readonly IIntegrationSettingService _integrationSettingService;
        protected string _key { get; set; }

        public MapService(HttpClient client, IIntegrationSettingService integrationSettingService)
        {
            _client = client;
            _integrationSettingService = integrationSettingService;

            IEnumerable<Models.IntegrationSetting> settings = _integrationSettingService.GetAllAsync().Result;
            _key = settings.FirstOrDefault().GoogleMapKey;
        }

        public async Task<object> GetPlaces(string Place)
        {
            HttpResponseMessage httpResponse = await _client.GetAsync($"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={Place}&inputtype=textquery&key={_key}&components=country:ae");
            //HttpResponseMessage httpResponse = await _client.GetAsync($"https://maps.googleapis.com/maps/api/place/textsearch/json?query={Place}&key={_key}");


            var data = await httpResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<object>(data);
        }

        public async Task<object> GetPlaceDetails(string PlaceId)
        {
            HttpResponseMessage httpResponse = await _client.GetAsync($"https://maps.googleapis.com/maps/api/place/details/json?key={_key}&place_id={PlaceId}&fields=name%2Cformatted_address%2Cgeometry");

            var data = await httpResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<object>(data);
        }
    }
}
