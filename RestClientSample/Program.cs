using System;
using System.Diagnostics;

namespace RestClientSample
{
	class Program
	{
		static void Main(string[] args)
		{

			var webRequestSamples = new WebRequestSamples();
			Console.WriteLine(webRequestSamples.MakeYahooPostSample());
			Console.WriteLine();

			var hammockSamples = new HammockSamples();
			Console.WriteLine(hammockSamples.MakeYahooPostSample());
			Console.WriteLine();

			var restSharpSamples = new RestSharpSamples();
			Console.WriteLine(restSharpSamples.MakeYahooPostSample());
			Console.WriteLine();

			// Console.Read();

			RunPerfComparison(50);
			Console.Read();
			//return;
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
