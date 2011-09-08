using RestSharp;

namespace Rest.Client.Sample
{
	public class RestSharpSamples : IRestSample
	{
		public string MakeYahooPostSample()
		{
			var baseUrl = "http://api.search.yahoo.com/ContentAnalysisService/V1";
			var client = new RestClient(baseUrl);
			var request = new RestRequest("termExtraction", Method.POST);
			var appId = "YahooDemo";
			var context = "Italian sculptors and painters of the renaissance favored the Virgin Mary for inspiration";
			var query = "madonna";

			request.AddParameter("appid", appId);
			request.AddParameter("context", context);
			request.AddParameter("query", query);

			var response = client.Execute(request);

			return response.Content;
		}
	}
}
