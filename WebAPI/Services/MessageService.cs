using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;

namespace WebAPI.Services
{
    public class MessageService : IMessageService
    {
        private readonly IIntegrationSettingService _integrationSettingService;
        private readonly IMapper _mapper;
        private string _key = string.Empty, _sender = string.Empty, _url = string.Empty, _username = string.Empty, _password = string.Empty, _type = string.Empty, _source = string.Empty;
        private bool _dlr=false;
        public MessageService(IIntegrationSettingService integrationSettingService, IMapper mapper)
        {
            _integrationSettingService = integrationSettingService;
            _mapper = mapper;

            IEnumerable<IntegrationSettingDTO> integrationSettingDTOList = _mapper.Map<IEnumerable<IntegrationSettingDTO>>(_integrationSettingService.GetAllAsync().Result);
            IntegrationSettingDTO integrationSettingDTO = integrationSettingDTOList.FirstOrDefault();

            if(integrationSettingDTO != null)
            {
                _key = integrationSettingDTO.SMSApiKey;
                _sender = integrationSettingDTO.SMSSenderId;
                _url = integrationSettingDTO.SMSUrl;
                _username = integrationSettingDTO.SMSUsername;
                _password = integrationSettingDTO.SMSPassword;
                _type = integrationSettingDTO.Type;
                _dlr = integrationSettingDTO.DLR;
                _source = integrationSettingDTO.Source;
            }            
        }
        public async Task<bool> SendMessage(string PhoneNumber, string Text)
        {
            //if (PhoneNumber.Equals("971507567600"))
            //    return true;

            //var uri = new Uri(_url + "?phonenumbers=" + PhoneNumber + "&sms.sender=" + _sender + "&sms.text=" + Text + "&apiKey=" + _key);
            var uri = new Uri(_url + "?username=" + _username + "&password=" + _password + "&type=" + _type + "&dlr=" + _dlr+ "&destination="+PhoneNumber+ "&source="+_source+ "&message="+Text);

            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            var client = new HttpClient();

            //client.DefaultRequestHeaders.Authorization =
            //                      new AuthenticationHeaderValue(
            //                          "Bearer", _key);

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}
