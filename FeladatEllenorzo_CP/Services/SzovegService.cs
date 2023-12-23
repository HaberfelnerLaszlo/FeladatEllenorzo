using FeladatEllenorzo_CP.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeladatEllenorzo_CP.Services
{
	public class SzovegService : ISzovegService
	{
		private string _baseUrl = "https://fapi.haberfelner.eu";
//#if ANDROID
//		private string _baseUrl = "http://10.0.2.2:7130";
//#else
//		private string _baseUrl = "http://localhost:7130";
//#endif
		public async Task<MainResponse> Add(DataSzoveg szoveg)
		{
			var returnResponse = new MainResponse();
			try
			{
				using (var client = new HttpClient())
				{
					string url = $"{_baseUrl}/szoveg";

					var serializeContent = JsonConvert.SerializeObject(szoveg);

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

		public async Task<List<DataSzoveg>> GetSzovegek()
		{
			var returnResponse = new List<DataSzoveg>();
			try
			{
				using (var client = new HttpClient())
				{
					string url = $"{_baseUrl}/szoveg";
					var apiResponse = await client.GetAsync(url);

					if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
					{
						var response = await apiResponse.Content.ReadAsStringAsync();
						var deserilizeResponse = JsonConvert.DeserializeObject<MainResponse>(response);

						if (deserilizeResponse.IsSuccess)
						{
							returnResponse = JsonConvert.DeserializeObject<List<DataSzoveg>>(deserilizeResponse.Content.ToString());
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

		public async Task<MainResponse> Remove(DataSzoveg szoveg)
		{
			var returnResponse = new MainResponse();
			try
			{
				using (var client = new HttpClient())
				{
					string url = $"{_baseUrl}/szoveg/{szoveg.Id}";

					var serializeContent = JsonConvert.SerializeObject(szoveg);

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
				string msg = ex.Message;
			}
			return returnResponse;
		}

		public async Task<MainResponse> Update(DataSzoveg szoveg)
		{
			var returnResponse = new MainResponse();
			try
			{
				using (var client = new HttpClient())
				{
					string url = $"{_baseUrl}/szoveg/{szoveg.Id}";

					var serializeContent = JsonConvert.SerializeObject(szoveg);

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
				string msg = ex.Message;
			}
			return returnResponse;
		}
	}
}
