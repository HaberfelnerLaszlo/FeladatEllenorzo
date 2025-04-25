using FeladatEllenorzo_CP.Data;
using FeladatEllenorzo_CP.Models;

using Newtonsoft.Json;

using System.Text;

using static Microsoft.Maui.Controls.Internals.Profile;

namespace FeladatEllenorzo_CP.Services
{
    public class TanuloService : ITanuloService
    {
        //#if ANDROID
        //		private string _baseUrl = "http://10.0.2.2:7130";
        //#else
        //		private string _baseUrl = "http://localhost:7130";
        //#endif
        private string _baseUrl = "https://fapi.haberfelner.eu";
        public string msg = "";

        public async Task<MainResponse> CreateTanulo(Tanulo tanulo)
        {
            var returnResponse = new MainResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{_baseUrl}/tanulo";

                    var serializeContent = JsonConvert.SerializeObject(tanulo);

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
        public async Task<Tanulo?> GetTanulo(Guid id)
        {
            var returnResponse = new Tanulo();
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{_baseUrl}/tanulo/{id}";
                    var apiResponse = await client.GetAsync(url);

                    if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var response = await apiResponse.Content.ReadAsStringAsync();
                        var deserilizeResponse = JsonConvert.DeserializeObject<MainResponse>(response);

                        if (deserilizeResponse.IsSuccess)
                        {
                            returnResponse = JsonConvert.DeserializeObject<Tanulo>(deserilizeResponse.Content.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
            return returnResponse;
        }

        public async Task<List<Tanulo>> GetTanulok()
        {
            var returnResponse = new List<Tanulo>();
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{_baseUrl}/tanulo";
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
                msg = ex.Message;
                return null;
            }
            return returnResponse;
        }

        public async Task<List<TanuloSync>> GetTanulokToIds()
        {
            var returnResponse = new List<TanuloSync>();
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{_baseUrl}/tanulokids";
                    var apiResponse = await client.GetAsync(url);

                    if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var response = await apiResponse.Content.ReadAsStringAsync();
                        var deserilizeResponse = JsonConvert.DeserializeObject<MainResponse>(response);

                        if (deserilizeResponse.IsSuccess)
                        {
                            returnResponse = JsonConvert.DeserializeObject<List<TanuloSync>>(deserilizeResponse.Content.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return returnResponse;

        }

        public async Task<List<TanuloSync>> GetTanulokToIds(string osztaly)
        {
            var returnResponse = new List<TanuloSync>();
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{_baseUrl}/tanuloids/{osztaly}";
                    var apiResponse = await client.GetAsync(url);

                    if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var response = await apiResponse.Content.ReadAsStringAsync();
                        var deserilizeResponse = JsonConvert.DeserializeObject<MainResponse>(response);

                        if (deserilizeResponse.IsSuccess)
                        {
                            returnResponse = JsonConvert.DeserializeObject<List<TanuloSync>>(deserilizeResponse.Content.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return returnResponse;

        }

        public Task<MainResponse> UpdateTanulo(Guid id, Tanulo tanulo)
        {
            throw new NotImplementedException();
        }
        public Task<MainResponse> DeleteTanulo(Guid id)
        {
            throw new NotImplementedException();
        }

    }
}