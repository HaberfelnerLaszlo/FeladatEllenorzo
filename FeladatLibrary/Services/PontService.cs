using FeladatLibrary.Data;
using FeladatLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace FeladatLibrary.Services
{
    public class PontService : IPontService
    {
        private readonly GlobalData _data;
        private readonly string _baseUrl;
        public string ErrorMessage { get; set; } = string.Empty;

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public PontService(GlobalData data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _baseUrl = _data.ApiUrl;
        }

        public async Task<MainResponse> Add(Pont pont)
        {
            var returnResponse = new MainResponse();
            try
            {
                using var client = new HttpClient();
                string url = $"{_baseUrl}/pont";

                var serializeContent = JsonSerializer.Serialize(pont, _jsonOptions);

                var apiResponse = await client.PostAsync(url, new StringContent(serializeContent, Encoding.UTF8, "application/json"));

                if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK || apiResponse.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var response = await apiResponse.Content.ReadAsStringAsync();
                    returnResponse = JsonSerializer.Deserialize<MainResponse>(response, _jsonOptions) ?? new MainResponse();
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
                returnResponse = new MainResponse();
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
                    var deserilizeResponse = JsonSerializer.Deserialize<MainResponse>(response, _jsonOptions) ?? new MainResponse();
                    if (deserilizeResponse.IsSuccess)
                    {
                        returnResponse = JsonSerializer.Deserialize<List<Pont>>(deserilizeResponse.Content?.ToString() ?? string.Empty, _jsonOptions) ?? new List<Pont>();
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
            return returnResponse ?? new List<Pont>();
        }

        public async Task<List<Pont>> GetPontByTanulo(Guid tId)
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
                    var deserilizeResponse = JsonSerializer.Deserialize<MainResponse>(response, _jsonOptions) ?? new MainResponse();
                    if (deserilizeResponse.IsSuccess)
                    {
                        returnResponse = JsonSerializer.Deserialize<List<Pont>>(deserilizeResponse.Content?.ToString() ?? string.Empty, _jsonOptions) ?? new List<Pont>();
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
            return returnResponse ?? new List<Pont>();
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
                    var deserilizeResponse = JsonSerializer.Deserialize<MainResponse>(response, _jsonOptions) ?? new MainResponse();
                    if (deserilizeResponse.IsSuccess)
                    {
                        returnResponse = JsonSerializer.Deserialize<List<Tanulo>>(deserilizeResponse.Content?.ToString() ?? string.Empty, _jsonOptions) ?? new List<Tanulo>();
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
            return returnResponse ?? new List<Tanulo>();
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
                    var deserilizeResponse = JsonSerializer.Deserialize<MainResponse>(response, _jsonOptions) ?? new MainResponse();
                    if (deserilizeResponse.IsSuccess)
                    {
                        returnResponse = JsonSerializer.Deserialize<TanuloData>(deserilizeResponse.Content?.ToString() ?? string.Empty, _jsonOptions) ?? new TanuloData();
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
                    returnResponse = true;
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
                var apiResponse = await client.PutAsync(url, new StringContent(JsonSerializer.Serialize(pont, _jsonOptions), Encoding.UTF8, "application/json"));
                if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var response = await apiResponse.Content.ReadAsStringAsync();
                    var deserilizeResponse = JsonSerializer.Deserialize<MainResponse>(response, _jsonOptions) ?? new MainResponse();
                    if (deserilizeResponse.IsSuccess)
                    {
                        returnResponse = JsonSerializer.Deserialize<Pont>(deserilizeResponse.Content?.ToString() ?? string.Empty, _jsonOptions);
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
