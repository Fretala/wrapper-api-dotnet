using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;
namespace Fretala
{
    public class Api
    {
        private const string sandboxUrl = "https://sandbox.freta.la/";
        private const string productionUrl = "https://api.freta.la/";
        private string token;
        private string environment;
        private string postfields;
        private string getfield;
        private string clientId;
        private string clientSecret;
        private string username;
        private string password;
        private string url;

        public Api(string environment, Dictionary<string, string> settings)
        {
            string[] array = { "sandbox", "production" };
            if (!array.Contains(environment))
            {
                throw new Exception("environment must be production or sandbox");
            }

            this.environment = environment;
            if (environment == "production")
            {
                this.url = productionUrl;
            }
            else
            {
                this.url = sandboxUrl;
            }

            this.clientId = settings["clientId"];
            this.clientSecret = settings["clientSecret"];
            this.username = settings["username"];
            this.password = settings["password"];

            this.token = "";
        }

        public void authenticate()
        {
            var data = new
            {
                grant_type = "password",
                username = this.username,
                password = this.password
            };
            var s1 = new JavaScriptSerializer().Serialize(data);
            Task<Dictionary<string, object>> res = this.performRequest(HttpMethod.Post, "authenticate", s1, true);
            res.Wait();
            this.token = (String)res.Result["access_token"];
        }

        public Dictionary<string, object> insertFrete(object data)
        {
            this.authenticate();
            var s1 = new JavaScriptSerializer().Serialize(data);
            Task<Dictionary<string, object>> res = this.performRequest(HttpMethod.Post, "fretes", s1);
            res.Wait();
            return res.Result;
        }

        public Int32 cost(object data)
        {
            this.authenticate();
            var s1 = new JavaScriptSerializer().Serialize(data);
            Task<Dictionary<string, object>> res = this.performRequest(HttpMethod.Post, "fretes/cost", s1);
            res.Wait();
            return (Int32)res.Result["price"];
        }

        private Dictionary<string, string> buildHeaders(bool auth = false)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            return headers;
        }

        private async Task<Dictionary<string, object>> performRequest(HttpMethod type, string path, string data = "", bool auth = false)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (auth)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", this.clientId, this.clientSecret))
                    ));
                }
                else if (this.token != "")
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.token);
                }

                //do the request
                HttpRequestMessage requestMessage = new HttpRequestMessage(type, path);
                requestMessage.Content = new StringContent(data);
                requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response =  await client.SendAsync(requestMessage);
                var resultObj = new JavaScriptSerializer().DeserializeObject(response.Content.ReadAsStringAsync().Result) as Dictionary<string, object>;
                if (!response.IsSuccessStatusCode)
                {
                    if (resultObj.ContainsKey("error_description"))
                    {
                        throw new Exception((String)resultObj["error_description"]);
                    }
                    else if (resultObj.ContainsKey("message"))
                    {
                        throw new Exception((String)resultObj["message"]);
                    }
                    else
                    {
                        throw new Exception("Error making request, no message returned from server.");
                    }
                }
                this.token = "";
                return resultObj;
            }
        }
    }
}
