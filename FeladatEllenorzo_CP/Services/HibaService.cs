using FeladatEllenorzo_CP.Models;
using FeladatEllenorzo_CP.Pages;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeladatEllenorzo_CP.Services
{
	public class HibaService : IHibaService
	{
//#if ANDROID
//		private string _baseUrl = "http://10.0.2.2:7130";
//#else
//		private string _baseUrl = "http://localhost:7130";
//#endif
		private string _baseUrl = "https://fapi.haberfelner.eu";

		public async Task<MainResponse> Add(HibasFeladat hiba)
		{
			var returnResponse = new MainResponse();
			try
			{
				using (var client = new HttpClient())
				{
					string url = $"{_baseUrl}/hiba";

					var serializeContent = JsonConvert.SerializeObject(hiba);

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
				returnResponse.IsSuccess = false;
                returnResponse.ErrorMessage = ex.Message;
			}
			return returnResponse;
		}

		public async Task<List<Tanulo>> GetHibak(string datum)
		{
			var returnResponse = new List<Tanulo>();
			try
			{
				using (var client = new HttpClient())
				{
					string url = $"{_baseUrl}/hibak/{datum}";
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
	public async Task<List<Tanulo>> GetMaiHibak()
		{
			var returnResponse = new List<Tanulo>();
			try
			{
				using (var client = new HttpClient())
				{
					string url = $"{_baseUrl}/maihibak";
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
		public async Task<MainResponse> Remove(HibasFeladat hiba)
		{
			var returnResponse = new MainResponse();
			try
			{
				using (var client = new HttpClient())
				{
					string url = $"{_baseUrl}/hiba/{hiba.Id}";

					var serializeContent = JsonConvert.SerializeObject(hiba);

					var request = new HttpRequestMessage();
					request.Method = HttpMethod.Delete;
					request.RequestUri = new Uri(url);
					request.Content = new StringContent(serializeContent, Encoding.UTF8, "application/json");
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
                returnResponse.IsSuccess = false;
                returnResponse.ErrorMessage = ex.Message;
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
					string url = $"{_baseUrl}/allhiba";

					var request = new HttpRequestMessage();
					request.Method = HttpMethod.Delete;
					request.RequestUri = new Uri(url);
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
                returnResponse.IsSuccess = false;
                returnResponse.ErrorMessage = ex.Message;
            }
            return returnResponse;
		}

        public async Task<MainResponse> RemoveClass(string osztaly, string date)
        {
            var returnResponse = new MainResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{_baseUrl}/classhiba/{osztaly}/{date}";


                    var request = new HttpRequestMessage();
                    request.Method = HttpMethod.Delete;
                    request.RequestUri = new Uri(url);
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
                returnResponse.IsSuccess = false;
                returnResponse.ErrorMessage = ex.Message;
            }
            return returnResponse;
        }

        public async Task<MainResponse> Update(HibasFeladat hiba)
		{
			var returnResponse = new MainResponse();
			try
			{
				using (var client = new HttpClient())
				{
					string url = $"{_baseUrl}/hiba/{hiba.Id}";

					var serializeContent = JsonConvert.SerializeObject(hiba);

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
                returnResponse.IsSuccess = false;
                returnResponse.ErrorMessage = ex.Message;
            }
            return returnResponse;
		}
	}
}
