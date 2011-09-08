using System;
using System.Diagnostics;
using System.Net;

namespace RestClientSample
{
	class Program
	{
		static void Main(string[] args)
		{
            // Sample using WebRequest
            var webRequestSamples = new WebRequestSamples();
			Console.WriteLine(webRequestSamples.MakeYahooPostSample());

            // Sample using Hammock
            //var hammockSamples = new HammockSamples();
            //Console.WriteLine(hammockSamples.MakeYahooPostSample());

            // Sample using RestSharp
            //var restSharpSamples = new RestSharpSamples();
            //Console.WriteLine(restSharpSamples.MakeYahooPostSample());

            // Performance Comparison
            //RunPerfComparison(50);

            // Twitter Sample 1
            //var twitterSample = new TwitterSamples();
            //var response = twitterSample.GetFavorites("reverentgeek", 1, 5);
            //if (response.StatusCode == HttpStatusCode.OK)
            //    Console.WriteLine(response.Content);
            //else
            //    Console.WriteLine("Status: {0}, Content: {1}, Exception: {2}", response.StatusCode, response.Content,
            //                     response.InnerException != null ? response.InnerException.Message : "");

            // Twitter Sample 2
            //var twitterSample = new TwitterSamples();
            //var tweets = twitterSample.GetFavorites2("reverentgeek", 1, 5);
            //foreach (var t in tweets)
            //    Console.WriteLine("{0}{1}{2}", t.Id, Environment.NewLine, t.Text);

            Console.Read();
		}

		static void RunPerfComparison(int iterations)
		{
			var webRequestSamples = new WebRequestSamples();
			var hammockSamples = new HammockSamples();
			var restSharpSamples = new RestSharpSamples();

			var elapsed = GetElapsedTime(webRequestSamples, iterations);
			Console.WriteLine("WebRequest Total Time: {0:#,###}", elapsed);
			Console.WriteLine("WebRequest Avg Time: {0:#,###}", elapsed / iterations);
			Console.WriteLine();

			elapsed = GetElapsedTime(hammockSamples, iterations);
			Console.WriteLine("Hammock Total Time: {0:#,###}", elapsed);
			Console.WriteLine("Hammock Avg Time: {0:#,###}", elapsed / iterations);
			Console.WriteLine();

			elapsed = GetElapsedTime(restSharpSamples, iterations);
			Console.WriteLine("RestSharp Total Time: {0:#,###}", elapsed);
			Console.WriteLine("RestSharp Avg Time: {0:#,###}", elapsed / iterations);
			Console.WriteLine();

		}

		private static long GetElapsedTime(IRestSample restSample, int iterations)
		{
			// Warm-up
			restSample.MakeYahooPostSample();

			var stopWatch = Stopwatch.StartNew();
			for (var i = 0; i < iterations; i++)
				restSample.MakeYahooPostSample();
			stopWatch.Stop();
			return stopWatch.ElapsedMilliseconds;
		}
	}
}
