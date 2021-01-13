using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DoorMonitor.WPF.Services
{
    /// <summary>
    /// Provides CRUD HTTP methods with serialisation and deserialisation
    /// </summary>
    public class RequestProvider
    {
        /// <summary>
        /// JSON serializer settings
        /// </summary>
        private readonly JsonSerializerSettings _serializerSettings;
        public RequestProvider()
        {
            _serializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

        /// <summary>
        /// Makes HTTP GET call
        /// </summary>
        /// <typeparam name="TResult">The type the response needs to resolve to</typeparam>
        /// <param name="uri">End point</param>
        /// <returns></returns>
        public async Task<TResult> GetAsync<TResult>(string uri)
        {
            HttpClient httpClient = GetHttpClient;
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            string serialized = await response.Content.ReadAsStringAsync();
            TResult result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));
            return result;
        }

        /// <summary>
        /// Makes HTTP POST call
        /// </summary>
        /// <typeparam name="TResult">The type the response needs to resolve to</typeparam>
        /// <param name="uri">End point</param>
        /// <returns></returns>
        public async Task<TResult> PostAsync<TResult>(string uri, TResult data)
        {
            HttpClient httpClient = GetHttpClient;
            var temp = JsonConvert.SerializeObject(data);
            StringContent content = new StringContent(temp);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);
            string serialized = await response.Content.ReadAsStringAsync();
            TResult result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));
            return result;
        }

        /// <summary>
        /// Makes HTTP PUT call
        /// </summary>
        /// <typeparam name="TResult">The type the response needs to resolve to</typeparam>
        /// <param name="uri">End point</param>
        /// <returns></returns>
        public async Task<TResult> PutAsync<TResult>(string uri, TResult data)
        {
            HttpClient httpClient = GetHttpClient;
            var temp = JsonConvert.SerializeObject(data);
            StringContent content = new StringContent(temp);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PutAsync(uri, content);
            string serialized = await response.Content.ReadAsStringAsync();
            TResult result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));
            return result;
        }

        /// <summary>
        /// Makes HTTP DELETE call
        /// </summary>
        /// <typeparam name="TResult">The type the response needs to resolve to</typeparam>
        /// <param name="uri">End point</param>
        /// <returns></returns>
        public async Task<TResult> DeleteAsync<TResult>(string uri)
        {
            HttpClient httpClient = GetHttpClient;
            HttpResponseMessage response = await httpClient.DeleteAsync(uri);
            string serialized = await response.Content.ReadAsStringAsync();
            TResult result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));
            return result;
        }
        /// <summary>
        /// Gets a new HttpClient with a workaround for SSL Certificate validation.
        /// </summary>
        private HttpClient GetHttpClient
        {
            get
            {
                var handler = new HttpClientHandler
                {
                    UseDefaultCredentials = true,
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, certChain, policyErrors) =>
                    { return true; }
                };
                return new HttpClient(handler);
            }
        }

    }
}
