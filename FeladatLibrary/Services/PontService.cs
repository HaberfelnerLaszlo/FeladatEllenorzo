using FeladatLibrary.Data;
using FeladatLibrary.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FeladatLibrary.Services
{
    public class PontService(GlobalData data) : IPontService
    {
//#if ANDROID
//        		private string _baseUrl = "http://10.0.2.2:7130";
//#else
//        private string _baseUrl = "http://localhost:7130";
//#endif
        //private string _baseUrl = "https://fapi.haberfelner.eu";
        private readonly string _baseUrl = data.ApiUrl;
        public string ErrorMessage { get; set; } = string.Empty;
        public async Task<MainResponse> Add(Pont pont)
        {
            var returnResponse = new MainResponse();
            try
            {
                using var client = new HttpClient();
                string url = $"{_baseUrl}/pont";

                var serializeContent = JsonConvert.SerializeObject(pont);

                var apiResponse = await client.PostAsync(url, new StringContent(serializeContent, Encoding.UTF8, "application/json")) ?? throw new NullReferenceException();

                if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK ||apiResponse.StatusCode==System.Net.HttpStatusCode.Created)
                {
                    var response = await apiResponse.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<MainResponse>(response) ?? throw new NullReferenceException();
                }
                else
                {
                    returnResponse.IsSuccess = false;
                    returnResponse.ErrorMessage = $"Hiba a pont hozzáadásakor: {apiResponse.StatusCode}";
                    ErrorMessage = returnResponse.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                returnResponse ??= new MainResponse();
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
                    var deserilizeResponse = JsonConvert.DeserializeObject<MainResponse>(response) ?? throw new NullReferenceException();
                    if (deserilizeResponse.IsSuccess)
                    {
                        returnResponse = JsonConvert.DeserializeObject<List<Pont>>(deserilizeResponse.Content?.ToString() ?? string.Empty);
                        if (returnResponse is null) throw new NullReferenceException();
                    }
                    else
                    {
                        ErrorMessage = deserilizeResponse.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return returnResponse ?? [];
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
                    var deserilizeResponse = JsonConvert.DeserializeObject<MainResponse>(response) ?? throw new NullReferenceException();
                    if (deserilizeResponse.IsSuccess)
                    {
                        returnResponse = JsonConvert.DeserializeObject<List<Pont>>(deserilizeResponse.Content?.ToString() ?? string.Empty);
                        if (returnResponse is null) throw new NullReferenceException();
                    }
                    else
                    {
                        ErrorMessage = deserilizeResponse.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return returnResponse ?? [];
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
                    var deserilizeResponse = JsonConvert.DeserializeObject<MainResponse>(response)?? throw new NullReferenceException();
                    if (deserilizeResponse.IsSuccess)
                    {
                        returnResponse = JsonConvert.DeserializeObject<List<Tanulo>>(deserilizeResponse.Content?.ToString() ?? string.Empty);
                    }
                    else
                    {
                        ErrorMessage = deserilizeResponse.ErrorMessage;
                    }
                }
                if (returnResponse is null) throw new NullReferenceException();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return returnResponse ?? [];
        }
        public async Task<TanuloData> GetTanuloData(string id)
        {
            var returnResponse = new TanuloData();
            try
            {
                using var client = new HttpClient();
                string url = $"{_baseUrl}/tanulo_data/{id}";
                var apiResponse = await client.GetAsync(url);
                if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var response = await apiResponse.Content.ReadAsStringAsync();
                    var deserilizeResponse = JsonConvert.DeserializeObject<MainResponse>(response) ?? throw new NullReferenceException();
                    if (deserilizeResponse.IsSuccess)
                    {
                        returnResponse = JsonConvert.DeserializeObject<TanuloData>(deserilizeResponse.Content?.ToString() ?? string.Empty);
                    }
                    else
                    {
                        ErrorMessage = deserilizeResponse.ErrorMessage;
                    }
                }
                if (returnResponse is null) throw new NullReferenceException();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return returnResponse ?? new TanuloData();
        }
        public async Task<bool> Remove(int pontId)
        {
            bool returnResponse = false;
            try
            {
                using var client = new HttpClient();
                string url = $"{_baseUrl}/pont/{pontId}";
                var apiResponse = await client.DeleteAsync(url);
                if (apiResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    returnResponse =true;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return returnResponse;
        }

        public async Task<Pont?> Update(Pont pont)
        {
            Pont? returnResponse = null;
            try
            {
                using var client = new HttpClient();
                string url = $"{_baseUrl}/pont";
                var apiResponse = await client.PutAsync(url, new StringContent(JsonConvert.SerializeObject(pont), Encoding.UTF8, "application/json"));
                if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var response = await apiResponse.Content.ReadAsStringAsync();
                    var deserilizeResponse = JsonConvert.DeserializeObject<MainResponse>(response) ?? throw new NullReferenceException();
                    if (deserilizeResponse.IsSuccess)
                    {
                        returnResponse = JsonConvert.DeserializeObject<Pont>(deserilizeResponse.Content?.ToString() ?? string.Empty);
                        if (returnResponse is null) throw new NullReferenceException(nameof(returnResponse));
                    }
                    else
                    {
                        ErrorMessage = deserilizeResponse.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return returnResponse;
        }
    }
}
