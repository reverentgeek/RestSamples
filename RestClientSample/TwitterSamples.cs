using System.Collections.Generic;
using System.Configuration;
using System.Net;
using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;
using Newtonsoft.Json;

namespace RestClientSample
{
    public class TwitterSamples
    {
        public RestResponse GetFavorites(string user, int page, int pageSize)
        {
            // Documentation for GET /favorites
            // https://dev.twitter.com/docs/api/1/get/favorites

            // Create the REST Client
            var client = new RestClient {Authority = "http://api.twitter.com/1"};

            // Create the REST Request
            var request = new RestRequest {Path = "favorites.json", Method = WebMethod.Get};
            request.AddParameter("id", user);
            request.AddParameter("page", page.ToString());
            request.AddParameter("count", pageSize.ToString());

            // Set API authentication tokens
            var appSettings = ConfigurationManager.AppSettings;
            request.Credentials = OAuthCredentials.ForProtectedResource(
                appSettings["ConsumerKey"], appSettings["ConsumerSecret"], appSettings["Token"],
                appSettings["TokenSecret"]);

            // Make request
            var response = client.Request(request);

            return response;
        }

        public List<Tweet> GetFavorites2(string user, int page, int pageSize)
        {
            var response = GetFavorites(user, page, pageSize);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var tweets = JsonConvert.DeserializeObject<List<Tweet>>(response.Content);
                return tweets;
            }

            return null;
        }
    }
}
