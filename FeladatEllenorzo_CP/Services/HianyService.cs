using FeladatEllenorzo_CP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeladatEllenorzo_CP.Services
{
	public class HianyService : IHianyService
	{
		public string ErrorMsg = string.Empty;
        private string _baseUrl = "https://fapi.haberfelner.eu";
//#if ANDROID
//		private string _baseUrl = "http://10.0.2.2:7130";
//#else
//		private string _baseUrl = "http://localhost:7130";
//#endif
		public async Task<MainResponse> Add(FeladatHiany hiany)
		{
			var returnResponse = new MainResponse();
			try
			{
				using (var client = new HttpClient())
				{
					string url = $"{_baseUrl}/hiany";

					var serializeContent = JsonConvert.SerializeObject(hiany);

					var apiResponse = await client.PostAsync(url, new StringContent(serializeContent, Encoding.UTF8, "application/json"));

					if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
					{
						var response = await apiResponse.Content.ReadAsStringAsync();
						returnResponse = JsonConvert.DeserializeObject<MainResponse>(response);
					}
				}
			}
			catch (Exception ex)
			{
				string msg = ex.Message;
			}
			return returnResponse;
		}

		public async Task<List<Tanulo>> GetHianyok(string datum)
		{
			var returnResponse = new List<Tanulo>();
			try
			{
				using (var client = new HttpClient())
				{
					string url = $"{_baseUrl}/hianyok/{datum}";
					var apiResponse = await client.GetAsync(url);

					if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
					{
						var response = await apiResponse.Content.ReadAsStringAsync();
						var deserilizeResponse = JsonConvert.DeserializeObject<MainResponse>(response);

						if (deserilizeResponse.IsSuccess)
						{
							returnResponse = JsonConvert.DeserializeObject<List<Tanulo>>(deserilizeResponse.Content.ToString());
						}
					}
				}
			}
			catch (Exception ex)
			{
				string msg = ex.Message;
			}
			return returnResponse;
		}

        public async Task<List<FeladatHiany>> GetHianyokByFeladat(string fId)
        {
			try
			{
                using var client = new HttpClient();
                string url = $"{_baseUrl}/hiany/{fId}";
                var apiResponse = await client.GetAsync(url);

                if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var response = await apiResponse.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<FeladatHiany>>(response);
                }
            }
			catch (Exception ex)
			{
				ErrorMsg = ex.Message;
			}
            return default;
        }

        public async Task<List<Tanulo>> GetMaiHiany()
		{
			var returnResponse = new List<Tanulo>();
			try
			{
				using (var client = new HttpClient())
				{
					string url = $"{_baseUrl}/maihianyok";
					var apiResponse = await client.GetAsync(url);

					if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
					{
						var response = await apiResponse.Content.ReadAsStringAsync();
						var deserilizeResponse = JsonConvert.DeserializeObject<MainResponse>(response);

						if (deserilizeResponse.IsSuccess)
						{
							returnResponse = JsonConvert.DeserializeObject<List<Tanulo>>(deserilizeResponse.Content.ToString());
						}
					}
				}
			}
			catch (Exception ex)
			{
				ErrorMsg = ex.Message;
			}
			return returnResponse;
		}
		public async Task<MainResponse> Remove(int hianyId)
		{
			var returnResponse = new MainResponse();
			try
			{
				using (var client = new HttpClient())
				{
					string url = $"{_baseUrl}/hiany/{hianyId}";

					//var serializeContent = JsonConvert.SerializeObject(hianyId);

					var request = new HttpRequestMessage();
					request.Method = HttpMethod.Delete;
					request.RequestUri = new Uri(url);
					//request.Content = new StringContent(serializeContent, Encoding.UTF8, "application/json");
					var apiResponse = await client.SendAsync(request);

					if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
					{
						var response = await apiResponse.Content.ReadAsStringAsync();
						returnResponse = JsonConvert.DeserializeObject<MainResponse>(response);
					}
				}
			}
			catch (Exception ex)
			{
				ErrorMsg = ex.Message;
			}
			return returnResponse;
		}
		public async Task<MainResponse> RemoveAll()
		{
			var returnResponse = new MainResponse();
			try
			{
				using (var client = new HttpClient())
				{
					string url = $"{_baseUrl}/allhiany";

					var apiResponse = await client.DeleteAsync(url);

					if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
					{
						var response = await apiResponse.Content.ReadAsStringAsync();
						returnResponse = JsonConvert.DeserializeObject<MainResponse>(response);
					}
				}
			}
			catch (Exception ex)
			{
				ErrorMsg = ex.Message;
			}
			return returnResponse;
		}

		public async Task<MainResponse> Update(FeladatHiany hiany)
		{
			var returnResponse = new MainResponse();
			try
			{
				using (var client = new HttpClient())
				{
					string url = $"{_baseUrl}/hiany/{hiany.Id}";

					var serializeContent = JsonConvert.SerializeObject(hiany);

					var apiResponse = await client.PutAsync(url, new StringContent(serializeContent, Encoding.UTF8, "application/json"));

					if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
					{
						var response = await apiResponse.Content.ReadAsStringAsync();
						returnResponse = JsonConvert.DeserializeObject<MainResponse>(response);
					}
				}
			}
			catch (Exception ex)
			{
				ErrorMsg = ex.Message;
			}
			return returnResponse;
		}
	}
}
