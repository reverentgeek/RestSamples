using Hammock;
using Hammock.Web;

namespace Rest.Client.Sample
{
	public class HammockSamples : IRestSample
	{
		public string MakeYahooPostSample()
		{
			var client = new RestClient
			             	{
			             		Authority = "http://api.search.yahoo.com/ContentAnalysisService",
			             		VersionPath = "V1"
			             	};
			var request = new RestRequest
			              	{
			              		Path = "termExtraction",
								Method = WebMethod.Post
			              	};

			var appId = "YahooDemo";
			var context = "Italian sculptors and painters of the renaissance favored the Virgin Mary for inspiration";
			var query = "madonna";

			request.AddField("appid", appId);
			request.AddField("context", context);
			request.AddField("query", query);

			var response = client.Request(request);

			return response.Content;
		}

	}
}
