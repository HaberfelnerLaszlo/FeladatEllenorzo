using FeladatLibrary.Models;

using Newtonsoft.Json;

using System.Net.Http.Headers;

namespace FeladatManagment.Services
{
    public class TeszterApiService()
    {
        public string ErrorMessage { get; set; } = string.Empty;
        private static readonly HttpClient client = new();
        private static readonly string[] scopes = ["api://6091ad40-f274-4e3a-813b-a9498817fd69/access_as_user"];

        protected static string URI = "https://teszter-api.haberfelner.hu/";
        //protected static string URI = "http://localhost:7025/";
        //protected static string URI = "https://localhost:44308/"; //ISS

        public async Task<Tv?> Get<Tv>(string uri)
        {
            try
            {
                client.BaseAddress ??= new Uri(URI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                using HttpResponseMessage response = await client.GetAsync(uri);
                string valasz;
                if (response.IsSuccessStatusCode)
                {
                    valasz = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(valasz) && valasz.Contains("errorMessage"))
                    {
                        var deserilizeResponse = JsonConvert.DeserializeObject<MainResponse>(valasz);
                        if (deserilizeResponse != null)
                        {
                            if (deserilizeResponse.IsSuccess)
                            {
                                return JsonConvert.DeserializeObject<Tv>(deserilizeResponse.Content.ToString());
                            }
                        }
                    }
                    return JsonConvert.DeserializeObject<Tv>(valasz);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    valasz = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(valasz) && valasz.Contains("errorMessage"))
                    {
                        var r = JsonConvert.DeserializeObject<MainResponse>(valasz);
                        if (r?.IsSuccess ?? false)
                        {
                            ErrorMessage = r.ErrorMessage;
                        }
                        else ErrorMessage = r?.ErrorMessage ?? "Null eredmény lett a hiba üzenet";
                    }
                    else ErrorMessage = "Nem elérhető a szerver!";
                    return default;
                }
                else
                {
                    ErrorMessage = response.StatusCode.ToString() + " : " + response.RequestMessage;
                    return default;
                }
            }
            catch (HttpRequestException e)
            {
                ErrorMessage = "Nem elérhető a szerver! Hibaüzenet: " + e.Message;
                return default;
            }
            catch (Exception e)
            {
                client.CancelPendingRequests();
                ErrorMessage = "Hiba történt! Az üzenet: " + e.Message;
                return default;
            }
        }

        public async Task<T?> Post<T>(string uri, T t)
        {
            var valasz = string.Empty;
            //var token = await authorizationHeaderProvider.CreateAuthorizationHeaderForUserAsync(scopes);

            //if (string.IsNullOrEmpty(_token) && !uri.Contains("Math"))
            //{
            //    await CreateAuthorizationHeaderForUserAsync();
            //}
            client.BaseAddress ??= new Uri(URI);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.PostAsJsonAsync(uri, t);
            if (response.IsSuccessStatusCode)
            {
                valasz = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(valasz) && valasz.Contains("errorMessage"))
                {
                    var deserilizeResponse = JsonConvert.DeserializeObject<MainResponse>(valasz);
                    if (deserilizeResponse != null)
                    {
                        if (deserilizeResponse.IsSuccess)
                        {
                            return JsonConvert.DeserializeObject<T>(deserilizeResponse.Content.ToString());
                        }
                    }
                }
                return JsonConvert.DeserializeObject<T?>(valasz);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                valasz = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(valasz) && valasz.Contains("errorMessage"))
                {
                    var r = JsonConvert.DeserializeObject<MainResponse>(valasz);
                    ErrorMessage = r?.ErrorMessage ?? "Null eredmény lett a hiba üzenet";
                }
                else ErrorMessage += "Nem elérhető a szerver!";
                return default;
            }
            else
            {
                ErrorMessage = response.StatusCode.ToString() + " : " + response.RequestMessage;
                var message = await response.Content.ReadAsStringAsync();
                ErrorMessage += message;
            }
            return default;
        }

        public async Task<T?> Put<T>(string uri, T t)
        {
            //var token = await authorizationHeaderProvider.CreateAuthorizationHeaderForUserAsync(scopes);
            //if (string.IsNullOrEmpty(_token) && !uri.Contains("Math"))
            //{
            //    await CreateAuthorizationHeaderForUserAsync();
            //}
            client.BaseAddress ??= new Uri(URI);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.PutAsJsonAsync(uri, t);
            string valasz;
            if (response.IsSuccessStatusCode)
            {
                valasz = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(valasz) && valasz.Contains("errorMessage"))
                {
                    var deserilizeResponse = JsonConvert.DeserializeObject<MainResponse>(valasz);
                    if (deserilizeResponse != null)
                    {
                        if (deserilizeResponse.IsSuccess)
                        {
                            return JsonConvert.DeserializeObject<T>(deserilizeResponse.Content.ToString());
                        }
                    }
                }
                return JsonConvert.DeserializeObject<T>(valasz);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                valasz = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(valasz) && valasz.Contains("errorMessage"))
                {
                    var r = JsonConvert.DeserializeObject<MainResponse>(valasz);
                    ErrorMessage = r?.ErrorMessage ?? "Null eredmény lett a hiba üzenet";
                }
                else ErrorMessage += "Nem elérhető a szerver!";
                return default;
            }
            else
            {
                ErrorMessage = response.StatusCode.ToString() + " : " + response.RequestMessage;
                var message = await response.Content.ReadAsStringAsync();
                ErrorMessage += message;
            }
            return default;
        }

        public async Task<bool> Delete(string uri)
        {
            bool valasz = false;
            //var token = await authorizationHeaderProvider.CreateAuthorizationHeaderForUserAsync(scopes);
            //if (string.IsNullOrEmpty(_token) && !uri.Contains("Math"))
            //{
            //    await CreateAuthorizationHeaderForUserAsync();
            //}
            client.BaseAddress ??= new Uri(URI);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.DeleteAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                valasz = true;
            }
            else ErrorMessage = response.StatusCode.ToString() + " : " + response.RequestMessage;
            return valasz;
        }
    }
    public class TokenResponse
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }

}