using FeladatEllenorzo_CP.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeladatEllenorzo_CP.Services
{
    public class PontService : IPontService
    {
//#if ANDROID
//        		private string _baseUrl = "http://10.0.2.2:7130";
//#else
//        private string _baseUrl = "http://localhost:7130";
//#endif
        private string _baseUrl = "https://fapi.haberfelner.eu";
        public string ErrorMessage { get; set; } = string.Empty;
        public async Task<MainResponse> Add(Pont pont)
        {
            var returnResponse = new MainResponse();
            try
            {
                using var client = new HttpClient();
                string url = $"{_baseUrl}/pont";

                var serializeContent = JsonConvert.SerializeObject(pont);

                var apiResponse = await client.PostAsync(url, new StringContent(serializeContent, Encoding.UTF8, "application/json"));

                if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK ||apiResponse.StatusCode==System.Net.HttpStatusCode.Created)
                {
                    var response = await apiResponse.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<MainResponse>(response);
                }
            }
            catch (Exception ex)
            {
                returnResponse.IsSuccess = false;
                returnResponse.ErrorMessage = ex.Message;
                ErrorMessage = ex.Message;
            }
            return returnResponse;
        }

        public async Task<List<Pont>> GetPontById(int id)
        {
            var returnResponse = new List<Pont>();
            try
            {
                using var client = new HttpClient();
                string url = $"{_baseUrl}/pont/{id}";
                var apiResponse = await client.GetAsync(url);

                if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var response = await apiResponse.Content.ReadAsStringAsync();
                    var deserilizeResponse = JsonConvert.DeserializeObject<MainResponse>(response);

                    if (deserilizeResponse.IsSuccess)
                    {
                        returnResponse = JsonConvert.DeserializeObject<List<Pont>>(deserilizeResponse.Content.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return returnResponse;
        }

        public async Task<List<Pont>> GetPontByTanulo(Guid tId)//tanuloId
        {
            var returnResponse = new List<Pont>();
            try
            {
                using var client = new HttpClient();
                string url = $"{_baseUrl}/pont/tanulo/{tId}";
                var apiResponse = await client.GetAsync(url);
                if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var response = await apiResponse.Content.ReadAsStringAsync();
                    var deserilizeResponse = JsonConvert.DeserializeObject<MainResponse>(response);
                    if (deserilizeResponse.IsSuccess)
                    {
                        returnResponse = JsonConvert.DeserializeObject<List<Pont>>(deserilizeResponse.Content.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return returnResponse;
        }
        public async Task<List<Tanulo>> GetPontByOsztaly(string osztaly)
        {
            var returnResponse = new List<Tanulo>();
            try
            {
                using var client = new HttpClient();
                string url = $"{_baseUrl}/pont/osztaly/{osztaly}";
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
                ErrorMessage = ex.Message;
            }
            return returnResponse;
        }
        //public Task<MainResponse> Remove(Pont pont)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<MainResponse> Update(Pont pont)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
