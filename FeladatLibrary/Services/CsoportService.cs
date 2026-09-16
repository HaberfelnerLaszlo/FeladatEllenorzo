using FeladatLibrary.Data;
using FeladatLibrary.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace FeladatLibrary.Services
{
    public class CsoportService(GlobalData data) : ICsoportService
    {
        private string _baseUrl = data.ApiUrl;

        public async Task<MainResponse> CreateCsoport(Csoport csoport)
        {
            var returnResponse = new MainResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{_baseUrl}/csoport";

                    var serializeContent = JsonConvert.SerializeObject(csoport);

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

        public async Task<MainResponse> DeleteCsoport(Guid id)
        {
            var returnResponse = new MainResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{_baseUrl}/csoport/{id}";

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
                returnResponse.IsSuccess = false;
                returnResponse.ErrorMessage = ex.Message;
            }
            return returnResponse;
        }

        public async Task<Csoport?> GetCsoport(Guid id)
        {
            var returnResponse = new MainResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{_baseUrl}/csoport/{id}";

                    var apiResponse = await client.GetAsync(url);

                    if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var response = await apiResponse.Content.ReadAsStringAsync();
                        returnResponse = JsonConvert.DeserializeObject<MainResponse>(response);
                        return JsonConvert.DeserializeObject<Csoport>(returnResponse.Content?.ToString() ?? string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                returnResponse.IsSuccess = false;
                returnResponse.ErrorMessage = ex.Message;
            }
            return null;
        }

        public async Task<List<Csoport>?> GetCsoportok()
        {
            var returnResponse = new MainResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{_baseUrl}/csoport";

                    var apiResponse = await client.GetAsync(url);

                    if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var response = await apiResponse.Content.ReadAsStringAsync();
                        returnResponse = JsonConvert.DeserializeObject<MainResponse>(response);
                    }
                }
                return JsonConvert.DeserializeObject<List<Csoport>>(returnResponse.Content?.ToString() ?? string.Empty) ?? new List<Csoport>();
            }
            catch (Exception ex)
            {
                returnResponse.IsSuccess = false;
                returnResponse.ErrorMessage = ex.Message;
            }
            return null;
        }

        public async Task<MainResponse> UpdateCsoport(Guid id, Csoport csoport)
        {
            var returnResponse = new MainResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{_baseUrl}/csoport/{id}";

                    var serializeContent = JsonConvert.SerializeObject(csoport);
                    var content = new StringContent(serializeContent, Encoding.UTF8, "application/json");

                    var apiResponse = await client.PutAsync(url, content);

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
