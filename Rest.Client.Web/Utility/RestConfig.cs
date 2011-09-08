using System;
using System.Configuration;

namespace Rest.Client.Web.Utility
{
	public class RestConfig
	{
		private RestConfig() { }
		private static readonly Lazy<RestConfig> LazyConfig = 
            new Lazy<RestConfig>(() => new RestConfig
		    {
                BaseUrl = ConfigurationManager.AppSettings["Rest.Api.BaseUrl"],
                VersionPath = ConfigurationManager.AppSettings["Rest.Api.VersionPath"],
                OAuthKey = ConfigurationManager.AppSettings["Rest.Api.OAuth.Key"],
                OAuthSharedSecret = ConfigurationManager.AppSettings["Rest.Api.OAuth.SharedSecret"],
                OAuthRequestTokenPath = ConfigurationManager.AppSettings["Rest.Api.OAuth.Path.RequestToken"],
                OAuthAuthorizePath = ConfigurationManager.AppSettings["Rest.Api.OAuth.Path.Authorize"],
                OAuthAccessTokenPath = ConfigurationManager.AppSettings["Rest.Api.OAuth.Path.AccessToken"],
                OAuthCallbackUrl = ConfigurationManager.AppSettings["Rest.Api.OAuth.CallbackUrl"],
                OAuthUseAuthorizationHeader = bool.Parse(ConfigurationManager.AppSettings["Rest.Api.OAuth.UseAuthHeader"])
		    });

		public static RestConfig Current { get { return LazyConfig.Value; } }
		
		public string BaseUrl { get; set; }
		public string VersionPath { get; set; }
		public string OAuthKey { get; set; }
		public string OAuthSharedSecret { get; set; }
		public string OAuthRequestTokenPath { get; set; }
		public string OAuthAuthorizePath { get; set; }
		public string OAuthAccessTokenPath { get; set; }
		public string OAuthCallbackUrl { get; set; }
		public bool OAuthUseAuthorizationHeader { get; set; }

        // These are for demo purposes only
		public string OAuthToken { get; set; }
        public string OAuthTokenSecret { get; set; }
        public string ApplicationName { get; set; }
        public string LoginUrl { get; set; }
        public string UserId { get; set; }
	}
}