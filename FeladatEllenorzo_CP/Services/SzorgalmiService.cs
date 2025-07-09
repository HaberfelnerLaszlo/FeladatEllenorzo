using FeladatEllenorzo_CP.Data;
using FeladatEllenorzo_CP.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Microsoft.Maui.Controls.Internals.Profile;

namespace FeladatEllenorzo_CP.Services
{
    public class SzorgalmiService(GlobalData data) : ISzorgalmiService
    {
//#if ANDROID
//        		private string _baseUrl = "http://10.0.2.2:7130";
//#else
//        private string _baseUrl = "http://localhost:7130";
//#endif
        //private string _baseUrl = "https://fapi.haberfelner.eu";
        private string _baseUrl = data.ApiUrl;

        public async Task<MainResponse> Add(Szorgalmi szorgalmi)
        {
            var returnResponse = new MainResponse();
            try
            {
                using var client = new HttpClient();
                string url = $"{_baseUrl}/szorgalmi";

                var serializeContent = JsonConvert.SerializeObject(szorgalmi);

                var apiResponse = await client.PostAsync(url, new StringContent(serializeContent, Encoding.UTF8, "application/json"));

                if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var response = await apiResponse.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<MainResponse>(response);
                }
            }
            catch (Exception ex)
            {
                returnResponse.IsSuccess = false;
                returnResponse.ErrorMessage = ex.Message;
            }
            return returnResponse;

        }

        public async Task<List<Tanulo>> GetSzorgalmiByNev(string nev)
        {
            var returnResponse = new List<Tanulo>();
            try
            {
                using var client = new HttpClient();
                string url = $"{_baseUrl}/szorgalmi_nev/{nev}";
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
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return returnResponse;
        }

        public async Task<List<Tanulo>> GetSzorgalmiByOsztaly(string osztaly)
        {
            var returnResponse = new List<Tanulo>();
            try
            {
                using var client = new HttpClient();
                string url = $"{_baseUrl}/szorgalmik/{osztaly}";
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
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return returnResponse;
        }

        public async Task<MainResponse> Remove(Szorgalmi szorgalmi)
        {
            var returnResponse = new MainResponse();
            try
            {
                using var client = new HttpClient();
                string url = $"{_baseUrl}/szorgalmi/{szorgalmi.Id}";

                var serializeContent = JsonConvert.SerializeObject(szorgalmi);

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
            catch (Exception ex)
            {
                returnResponse.IsSuccess = false;
                returnResponse.ErrorMessage = ex.Message;
            }
            return returnResponse;
        }

        //Task<MainResponse> ISzorgalmiService.RemoveAll()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<MainResponse> Update(Szorgalmi szorgalmi)
        {
            var returnResponse = new MainResponse();
            try
            {
                using var client = new HttpClient();
                string url = $"{_baseUrl}/szorgalmi/{szorgalmi.Id}";

                var serializeContent = JsonConvert.SerializeObject(szorgalmi);

                var apiResponse = await client.PutAsync(url, new StringContent(serializeContent, Encoding.UTF8, "application/json"));

                if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var response = await apiResponse.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<MainResponse>(response);
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