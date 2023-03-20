using HelperClasses.DTOs;
using HelperClasses.DTOs.Fatoorah;
using HelperClasses.DTOs.Fatoorah.WebHook;
using HelperClasses.DTOs.Garage;
using HelperClasses.DTOs.Order;
using HelperClasses.DTOs.Supplier;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services
{
	public class FatoorahService : IFatoorahService
	{
		private readonly IIntegrationSettingService _integrationSetting;
		private readonly IUrlHelperService _urlHelper;
		private readonly IConfiguration _config;
		private string _Key, _Endpoint;
		private string _InitUrl, _ExecutionUrl, _HMACKey;
		private int PaymentMethodId;
		private long RecordId = 0;

		public FatoorahService(IIntegrationSettingService integrationSetting, IUrlHelperService urlHelper, IConfiguration config)
		{
			_integrationSetting = integrationSetting;
			_urlHelper = urlHelper;
			_config = config;

			IEnumerable<IntegrationSetting> settings = _integrationSetting.GetAllAsync().Result;
			IntegrationSetting setting = settings.FirstOrDefault();

			_Key = setting.FatoorahAPIKey;
			_Endpoint = setting.GoLive ? setting.LiveEndpoint : setting.SandboxEndpoint;
			_Endpoint = setting.GoLive ? setting.LiveEndpoint : setting.SandboxEndpoint;
			_InitUrl = _Endpoint + "/v2/InitiatePayment";
			_ExecutionUrl = _Endpoint + "v2/ExecutePayment";
			_HMACKey = _config.GetValue<string>("HMAC_Key");
		}

		private async Task<string> ExecutePayment(ExecutePaymentRequestDTO executePaymentRequestDTO)
		{

			using var client = new HttpClient();
			client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + _Key);
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			var json = JsonConvert.SerializeObject(executePaymentRequestDTO);

			var content = new StringContent(json, Encoding.UTF8, "application/json");
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			var response = client.PostAsync(_ExecutionUrl, content).Result;
			if (response.IsSuccessStatusCode)
			{
				var orderResponse = JsonConvert.DeserializeObject<ExecutePaymentResponseDTO>(await response.Content.ReadAsStringAsync());

				return orderResponse.Data.PaymentURL;
			}
			else
			{
				throw new Exception(response.ReasonPhrase);
			}
		}

		public async Task<string> InitiatePayment(OrderDTO order, string restaurantPath, string supplierCode)
		{
			RecordId = order.Id;
			using var client = new HttpClient();

			client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + _Key);
			client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

			var orderBody = new
			{
				InvoiceAmount = order.TotalAmount,
				CurrencyIso = "AED"
			};

			var json = JsonConvert.SerializeObject(orderBody);

			var content = new StringContent(json, Encoding.UTF8, "application/json");
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			var response = client.PostAsync(_InitUrl, content).Result;
			if (response.IsSuccessStatusCode)
			{
				InitiatePaymentResponseDTO initiatePaymentResponse = JsonConvert.DeserializeObject<InitiatePaymentResponseDTO>(await response.Content.ReadAsStringAsync());

				if (initiatePaymentResponse != null)
				{
					var PaymentMethod = initiatePaymentResponse.Data.PaymentMethods.Where(i => i.PaymentMethodCode == "uaecc").FirstOrDefault();
					if (initiatePaymentResponse != null)
					{
						PaymentMethodId = PaymentMethod.PaymentMethodId;
					}
					//					string[] countryCodes = {
					//	"+93","+355","+213","+1","+376","+244","+1","+1","+54","+374","+297","+61","+43","+994","+1","+973","+880","+1","+375","+32","+501","+229","+1","+975","+591","+387","+267","+55","+246","+1","+673","+359","+226","+257","+855","+237","+1","+238","+599","+1","+236","+235","+56","+86","+61","+61","+57","+269","+243","+242","+682","+506","+225","+385","+53","+599","+357","+420","+45","+253","+1767","+1","+593","+20","+503","+240","+291","+372","+251","+500","+298","+679","+358","+33","+594","+689","+241","+220","+995","+49","+233","+350","+30","+299","+1473","+590","+1","+502","+44","+224","+245","+592","+509","+504","+852","+36","+354","+91","+62","+98","+964","+353","+44","+972","+39","+1","+81","+44","+962","+7","+254","+686","+383","+965","+996","+856","+371","+961","+266","+231","+218","+423","+370","+352","+853","+389","+261","+265","+60","+960","+223","+356","+692","+596","+222","+230","+262","+52","+691","+373","+377","+976","+382","+1","+212","+258","+95","+264","+674","+977","+31","+687","+64","+505","+227","+234","+683","+672","+850","+1670","+47","+968","+92","+680","+970","+507","+675","+595","+51","+63","+48","+351","+1","+974","+262","+40","+7","+250","+590","+290","+1869","+1","+590","+508","+1","+685","+378","+239","+966","+221","+381","+248","+232","+65","+1","+421","+386","+677","+252","+27","+82","+211","+34","+94","+249","+597","+47","+268","+46","+41","+963","+886","+992","+255","+66","+670","+228","+690","+676","+1","+216","+90","+993","+1649","+688","+1","+256","+380","+971","+44","+1","+598","+998","+678","+39","+58","+84","+681","+212","+967","+260","+263","+358"
					//};

					var countryCode = order.CustomerContact.Contains(" ") ? order.CustomerContact.Split(" ")[0] : "+971";
					ExecutePaymentRequestDTO executePaymentRequestDTO = new()
					{
						PaymentMethodId = PaymentMethodId,
						CustomerName = order.CustomerName,
						CustomerMobile = order.CustomerContact.Replace(countryCode, "").Replace(" ", ""),
						CustomerEmail = order.CustomerEmail,
						UserDefinedField = order.Id.ToString(),

						DisplayCurrencyIso = "AED",
						MobileCountryCode = countryCode.Replace("+", ""),
						InvoiceValue = order.TotalAmount,
						Language = "EN",
						CallBackUrl = restaurantPath,
						ErrorUrl = restaurantPath
					};

					if (!string.IsNullOrEmpty(supplierCode))
					{
						executePaymentRequestDTO.Suppliers = new List<InvoiceSupplierDTO>()
						{
							new InvoiceSupplierDTO()
							{
								SupplierCode = supplierCode,
								InvoiceShare = order.TotalAmount
							}
						};
					}

					return await ExecutePayment(executePaymentRequestDTO);
				}

			}

			throw new Exception(response.ReasonPhrase);
		}

		public async Task<string> InitiateGaragePayment(GarageCustomerInvoiceDTO order, string garagePath)
		{
			RecordId = order.Id;
			using var client = new HttpClient();

			client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + _Key);
			client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

			var orderBody = new
			{
				InvoiceAmount = order.Total,
				CurrencyIso = "AED"
			};

			var json = JsonConvert.SerializeObject(orderBody);

			var content = new StringContent(json, Encoding.UTF8, "application/json");
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			var response = client.PostAsync(_InitUrl, content).Result;
			if (response.IsSuccessStatusCode)
			{
				InitiatePaymentResponseDTO initiatePaymentResponse = JsonConvert.DeserializeObject<InitiatePaymentResponseDTO>(await response.Content.ReadAsStringAsync());

				if (initiatePaymentResponse != null)
				{
					var PaymentMethod = initiatePaymentResponse.Data.PaymentMethods.Where(i => i.PaymentMethodCode == "uaecc").FirstOrDefault();
					if (initiatePaymentResponse != null)
					{
						PaymentMethodId = PaymentMethod.PaymentMethodId;
					}
					//					string[] countryCodes = {
					//	"+93","+355","+213","+1","+376","+244","+1","+1","+54","+374","+297","+61","+43","+994","+1","+973","+880","+1","+375","+32","+501","+229","+1","+975","+591","+387","+267","+55","+246","+1","+673","+359","+226","+257","+855","+237","+1","+238","+599","+1","+236","+235","+56","+86","+61","+61","+57","+269","+243","+242","+682","+506","+225","+385","+53","+599","+357","+420","+45","+253","+1767","+1","+593","+20","+503","+240","+291","+372","+251","+500","+298","+679","+358","+33","+594","+689","+241","+220","+995","+49","+233","+350","+30","+299","+1473","+590","+1","+502","+44","+224","+245","+592","+509","+504","+852","+36","+354","+91","+62","+98","+964","+353","+44","+972","+39","+1","+81","+44","+962","+7","+254","+686","+383","+965","+996","+856","+371","+961","+266","+231","+218","+423","+370","+352","+853","+389","+261","+265","+60","+960","+223","+356","+692","+596","+222","+230","+262","+52","+691","+373","+377","+976","+382","+1","+212","+258","+95","+264","+674","+977","+31","+687","+64","+505","+227","+234","+683","+672","+850","+1670","+47","+968","+92","+680","+970","+507","+675","+595","+51","+63","+48","+351","+1","+974","+262","+40","+7","+250","+590","+290","+1869","+1","+590","+508","+1","+685","+378","+239","+966","+221","+381","+248","+232","+65","+1","+421","+386","+677","+252","+27","+82","+211","+34","+94","+249","+597","+47","+268","+46","+41","+963","+886","+992","+255","+66","+670","+228","+690","+676","+1","+216","+90","+993","+1649","+688","+1","+256","+380","+971","+44","+1","+598","+998","+678","+39","+58","+84","+681","+212","+967","+260","+263","+358"
					//};

					var countryCode = order.CustomerPhoneNumber.Contains(" ") ? order.CustomerPhoneNumber.Split(" ")[0] : "+971";
					ExecutePaymentRequestDTO executePaymentRequestDTO = new()
					{
						PaymentMethodId = PaymentMethodId,
						CustomerName = order.CustomerName,
						CustomerMobile = order.CustomerPhoneNumber.Replace(countryCode, "").Replace(" ", ""),
						CustomerEmail = order.CustomerEmail,
						UserDefinedField = order.Id.ToString(),

						DisplayCurrencyIso = "AED",
						MobileCountryCode = countryCode.Replace("+", ""),
						InvoiceValue = order.Total,
						Language = "EN",
						CallBackUrl = garagePath,
						ErrorUrl = garagePath
					};


					return await ExecutePayment(executePaymentRequestDTO);
				}

			}

			throw new Exception(response.ReasonPhrase);
		}
		public async Task<PaymentInquiryResponseDTO> GetPaymentResponse(string PaymentId)
		{
			using var client = new HttpClient();
			/*Fetch  Access Token From N-Genius*/
			var body = new
			{
				Key = PaymentId,
				KeyType = "PaymentId"
			};

			client.BaseAddress = new Uri(_Endpoint);
			client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _Key);

			var json = JsonConvert.SerializeObject(body);

			var content = new StringContent(json, Encoding.UTF8, "application/json");
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			var response = client.PostAsync("/v2/GetPaymentStatus", content).Result;

			if (response.IsSuccessStatusCode)
			{
				return JsonConvert.DeserializeObject<PaymentInquiryResponseDTO>(await response.Content.ReadAsStringAsync());
			}

			throw new Exception(response.ReasonPhrase);
		}

		public bool ValidateSignature<T>(WebHookResponseDTO<T> webHookResponse, string headerSignature)
		{
			//1- Order all properties alphabetic           
			var properties = typeof(T).GetProperties().Select(p => p.Name).OrderBy(p => p).ToList();
			var type = webHookResponse.Data.GetType();
			var parameters = new List<SignaturePrintDTO>();
			for (int i = 0; i < properties.Count; i++)
			{
				var value = type.GetProperty(properties[i]).GetValue(webHookResponse.Data);
				value = value == null ? "" : value.ToString();
				parameters.Add(new SignaturePrintDTO { Text = properties[i], Value = value.ToString() });
			}

			//2-Encrypt the data with the secret key
			var signature = Decode(parameters, _HMACKey);

			//3-Compare the signature
			return signature == headerSignature;
		}

		private static string Decode(List<SignaturePrintDTO> paramsArray, string secretKey)
		{
			var dataToSign = paramsArray.Select(p => p.Text + "=" + p.Value).ToList();
			var data = string.Join(",", dataToSign);
			var encoding = new UTF8Encoding();
			var keyByte = encoding.GetBytes(secretKey);
			var hmacsha256 = new HMACSHA256(keyByte);
			var messageBytes = encoding.GetBytes(data);
			return Convert.ToBase64String(hmacsha256.ComputeHash(messageBytes));
		}

		public async Task<string> InitiatePayment(SupplierOrder order, string restaurantPath, string supplierCode)
		{
			RecordId = order.Id;
			using var client = new HttpClient();

			client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + _Key);
			client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

			var orderBody = new
			{
				InvoiceAmount = order.TotalAmount,
				CurrencyIso = "KWD"
			};

			var json = JsonConvert.SerializeObject(orderBody);

			var content = new StringContent(json, Encoding.UTF8, "application/json");
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			var response = client.PostAsync(_InitUrl, content).Result;
			if (response.IsSuccessStatusCode)
			{
				InitiatePaymentResponseDTO initiatePaymentResponse = JsonConvert.DeserializeObject<InitiatePaymentResponseDTO>(await response.Content.ReadAsStringAsync());

				if (initiatePaymentResponse != null)
				{
					var PaymentMethod = initiatePaymentResponse.Data.PaymentMethods.Where(i => i.PaymentMethodCode == "uaecc").FirstOrDefault();
					if (initiatePaymentResponse != null)
					{
						PaymentMethodId = PaymentMethod.PaymentMethodId;
					}

					ExecutePaymentRequestDTO executePaymentRequestDTO = new()
					{
						PaymentMethodId = PaymentMethodId,
						CustomerName = order.RestaurantName,
						CustomerMobile = order.RestauantContact.Replace("971", ""),
						CustomerEmail = order.RestaurantEmail,

						DisplayCurrencyIso = "KWD",
						MobileCountryCode = "971",
						InvoiceValue = order.TotalAmount,
						Language = "EN",
						CallBackUrl = restaurantPath,
						ErrorUrl = restaurantPath
					};

					if (!string.IsNullOrEmpty(supplierCode))
					{
						executePaymentRequestDTO.Suppliers = new List<InvoiceSupplierDTO>()
						{
							new InvoiceSupplierDTO()
							{
								SupplierCode = supplierCode,
								InvoiceShare = order.TotalAmount
							}
						};
					}

					return await ExecutePayment(executePaymentRequestDTO);
				}

			}

			throw new Exception(response.ReasonPhrase);
		}
		public async Task<string> InitiatePayment(ClientModulePurchasesDTO order, string clientPath)
		{
			RecordId = order.Id;
			using var client = new HttpClient();

			client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + _Key);
			client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

			var orderBody = new
			{
				InvoiceAmount = order.AmountToBePaid,
				CurrencyIso = "AED"
			};

			var json = JsonConvert.SerializeObject(orderBody);

			var content = new StringContent(json, Encoding.UTF8, "application/json");
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			var response = client.PostAsync(_InitUrl, content).Result;
			if (response.IsSuccessStatusCode)
			{
				InitiatePaymentResponseDTO initiatePaymentResponse = JsonConvert.DeserializeObject<InitiatePaymentResponseDTO>(await response.Content.ReadAsStringAsync());

				if (initiatePaymentResponse != null)
				{
					var PaymentMethod = initiatePaymentResponse.Data.PaymentMethods.Where(i => i.PaymentMethodCode == "uaecc").FirstOrDefault();
					if (initiatePaymentResponse != null)
					{
						PaymentMethodId = PaymentMethod.PaymentMethodId;
					}

					ExecutePaymentRequestDTO executePaymentRequestDTO = new()
					{
						PaymentMethodId = PaymentMethodId,
						CustomerName = order.Garage.ContactPersonName,
						CustomerMobile = order.Garage.ContactPersonNumber.Replace("971-", "").Replace(" ",""),
						CustomerEmail = order.Garage.ContactPersonEmail,

						DisplayCurrencyIso = "AED",
						MobileCountryCode = "971",
						InvoiceValue = order.AmountToBePaid,
						Language = "EN",
						CallBackUrl = clientPath,
						ErrorUrl = clientPath
					};


					return await ExecutePayment(executePaymentRequestDTO);
				}

			}

			throw new Exception(response.ReasonPhrase);
		}
	}
}
