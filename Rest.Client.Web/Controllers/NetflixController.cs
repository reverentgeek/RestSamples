using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Hammock;
using Hammock.Authentication.OAuth;
using Rest.Client.Web.Models;
using Rest.Client.Web.Utility;

namespace Rest.Client.Web.Controllers
{
    public class NetflixController : Controller
    {
        public ActionResult Index()
        {
            var restConfig = RestConfig.Current;
            if (string.IsNullOrEmpty(restConfig.UserId))
                return RedirectToAction("Authorize");

            var client = new RestClient {Authority = restConfig.BaseUrl};
            var request = new RestRequest
                              {
                                  Path = string.Format("users/{0}/queues/instant", restConfig.UserId),
                                  Credentials = OAuthCredentials.ForProtectedResource(
                                      restConfig.OAuthKey, restConfig.OAuthSharedSecret,
                                      restConfig.OAuthToken, restConfig.OAuthTokenSecret)
                              };
            var response = client.Request(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var xml = XDocument.Parse(response.Content);
                var items = from i in xml.Descendants("queue_item")
                            select new Movie
                                       {
                                           Title = (string) i.Descendants("title").Attributes("regular").FirstOrDefault(),
                                           Thumbnail = (string)i.Descendants("box_art").Attributes("small").FirstOrDefault(),
                                           ReleaseYear = (string)i.Descendants("release_year").FirstOrDefault(),
                                           Link = (string)i.Descendants("link").Where(x => (string) x.Attribute("rel") == "alternate").Attributes("href").FirstOrDefault()
                                       };
                var model = new {Response = response, Items = items}.ToExpando();
                return View(model);
            }
            return View(new {Response = response, Items = (object) null}.ToExpando());
        }

        public ActionResult Authorize()
        {
            var response = RequestToken();
            var restConfig = RestConfig.Current;
            var loginUrl = BuildLoginUrl();
            var model =
                new
                {
                    LastResponse = response,
                    restConfig.OAuthToken,
                    restConfig.OAuthTokenSecret,
                    restConfig.ApplicationName,
                    LoginUrl = loginUrl
                }.ToExpando();
            return View(model);
        }

        public ActionResult Callback()
        {
            var response = RequestAuthorizedToken();
            if (response.StatusCode == HttpStatusCode.OK)
                return RedirectToAction("Index");

            var model = new {LastResponse = response, Config = RestConfig.Current}.ToExpando();
            return View(model);
        }

		private RestResponse RequestToken()
		{
		    var restConfig = RestConfig.Current;
            if (!string.IsNullOrEmpty(restConfig.OAuthToken))
                return null;

			var client = new RestClient { Authority = restConfig.BaseUrl };
            var credentials = OAuthCredentials.ForRequestToken(restConfig.OAuthKey, restConfig.OAuthSharedSecret);
		    credentials.ParameterHandling = restConfig.OAuthUseAuthorizationHeader
		                                        ? OAuthParameterHandling.HttpAuthorizationHeader
		                                        : OAuthParameterHandling.UrlOrPostParameters;
			var request = new RestRequest
			              	{
			              		Path = restConfig.OAuthRequestTokenPath,
                                Credentials = credentials
			              	};
		    var response = client.Request(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var qs = HttpUtility.ParseQueryString(response.Content);
                restConfig.OAuthToken = qs["oauth_token"];
                restConfig.OAuthTokenSecret = qs["oauth_token_secret"];
                restConfig.ApplicationName = qs["application_name"];
                restConfig.LoginUrl = qs["login_url"];
            }
		    return response;
		}

        private string BuildLoginUrl()
        {
            var sb = new StringBuilder();
            var restConfig = RestConfig.Current;
            sb.Append(restConfig.LoginUrl);
            // sb.AppendFormat("?oauth_token={0}", restConfig.OAuthToken);
            sb.AppendFormat("&oauth_consumer_key={0}", HttpUtility.UrlEncode(restConfig.OAuthKey));
            sb.AppendFormat("&application_name={0}", HttpUtility.UrlEncode(restConfig.ApplicationName));
            if (string.IsNullOrEmpty(restConfig.OAuthCallbackUrl) && Request.Url != null)
            {
                restConfig.OAuthCallbackUrl = string.Format("http://{0}{1}", Request.Url.Authority, Url.Action("Callback", "Netflix"));
            }
            sb.AppendFormat("&oauth_callback={0}", HttpUtility.UrlEncode(restConfig.OAuthCallbackUrl));
            return sb.ToString();
        }

        private RestResponse RequestAuthorizedToken()
        {
            var restConfig = RestConfig.Current;
            if (!string.IsNullOrEmpty(restConfig.UserId))
                return null;

            var client = new RestClient { Authority = restConfig.BaseUrl };
            var credentials = OAuthCredentials.ForAccessToken(restConfig.OAuthKey, restConfig.OAuthSharedSecret,
                                                              restConfig.OAuthToken, restConfig.OAuthTokenSecret);
            credentials.ParameterHandling = restConfig.OAuthUseAuthorizationHeader
                                                ? OAuthParameterHandling.HttpAuthorizationHeader
                                                : OAuthParameterHandling.UrlOrPostParameters;
            var request = new RestRequest
            {
                Path = restConfig.OAuthAccessTokenPath,
                Credentials = credentials
            };
            var response = client.Request(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var qs = HttpUtility.ParseQueryString(response.Content);
                restConfig.OAuthToken = qs["oauth_token"];
                restConfig.OAuthTokenSecret = qs["oauth_token_secret"];
                restConfig.UserId = qs["user_id"];
            }
            return response;
        }
    }
}
