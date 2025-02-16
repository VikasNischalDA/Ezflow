using LeadManagementSystem.Comman.Helpers;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Xml.Serialization;


namespace LeadManagementSystem.Providers
{
    public abstract class BaseHttpProvider
    {
        public virtual string _hostWebApiUrl { get; set; }

        public virtual HttpClient GetClient()
        {
            string _baseAddress = _hostWebApiUrl;
            var client = new HttpClient
            {
                BaseAddress = new Uri(_baseAddress)
            };
            return client;
        }

        public async Task<TResult> GetAsync<TResult>(HttpClient client, string url)
        {
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = default(TResult);
            try
            {
                HttpResponseMessage response = null;
                try
                {
                    response = await client.GetAsync(url);
                }
                catch (Exception e)
                {

                    throw;
                }

                response.EnsureSuccessStatusCode();
                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<TResult>(x.Result);
                });
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public async Task<string> GetStringAsync(HttpClient client, string url)
        {
            try
            {
                var response = await client.GetStringAsync(url);
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual async Task<TResult> PostAsync<TRequest, TResult>(HttpClient client, TRequest t, string url)
        {
            var result = default(TResult);
            var json = JsonConvert.SerializeObject(t);
            HttpContent httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(url, httpContent).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
            {
                if (x.IsFaulted)
                    throw x.Exception;

                result = JsonConvert.DeserializeObject<TResult>(x.Result);
            });
            return result;

        }

        public virtual async Task<TResult> PostXMLAsync<TRequest, TResult>(HttpClient client, TRequest t, string url)
        {
            var result = default(TResult);
            var json = JsonConvert.SerializeObject(t);
            HttpContent httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("text/json");

            var response = await client.PostAsync(url, httpContent).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
            {
                if (x.IsFaulted)
                    throw x.Exception;

                result = JsonConvert.DeserializeObject<TResult>(x.Result);
            });
            return result;

        }

        public virtual async Task<TResult> PostSoapXmlAsync<TRequest, TResult>(HttpClient client, TRequest t, string url)
        {
            try
            {
                var result = default(TResult);
                var xmlFormat = XmlJsonConverter.SerializeToXml(t);

                HttpContent httpContent = new StringContent(xmlFormat);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("text/xml");

                var response = await client.PostAsync(url, httpContent).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!string.IsNullOrEmpty(responseString))
                {
                    var serializer = new XmlSerializer(typeof(TResult));
                    using (var reader = new StringReader(responseString))
                    {
                        return (TResult)serializer.Deserialize(reader);
                    }
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        protected string DecodeBase64(string encodedValue)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(encodedValue);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
