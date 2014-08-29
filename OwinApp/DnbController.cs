using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace OwinApp
{
    [RoutePrefix("api/dnb")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DnbController : ApiController
    {
        public static string Authorization { get; set; }

        public static object AuthLock = new object();
        public void RefreshAuth()
        {
            var authorization = Authorization;
            lock (AuthLock)
            {
                if (authorization != Authorization)
                {
                    return;
                }
                var client = new RestClient("https://maxcvservices.dnb.com/");
                var request = new RestRequest("rest/Authentication", Method.POST);
                request.AddHeader("x-dnb-user", "P20000024544D84196E427191027057D");
                request.AddHeader("x-dnb-pwd", "dnbtest");
                var response = client.Execute(request);
                Authorization = response.Headers.Single(header => header.Name == "Authorization").Value.ToString();
            }
        }

        // GET dnb/dnbcall
        public HttpResponseMessage Get(string resource = null)
        {
            if (string.IsNullOrEmpty(Authorization))
            {
                RefreshAuth();
            }
            var client = new RestClient("https://maxcvservices.dnb.com/V4.0/" + resource + Request.RequestUri.Query);
            var request = new RestRequest("", Method.GET);
            request.AddHeader("Authorization", Authorization);
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                RefreshAuth();
                response = client.Execute(request);
            }
            return Request.CreateResponse(response.StatusCode, JObject.Parse(response.Content));
        }
    }
}