using Microsoft.Owin.Hosting;
using System;
using System.Threading;

namespace OwinApp
{
	public class Program
	{
		private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);

		static void Main(string[] args)
		{
			var port = 5000;
			if (args.Length > 0)
			{
				int.TryParse(args[0], out port);
			}

			Console.CancelKeyPress += (sender, eArgs) =>
			{
				QuitEvent.Set();
				eArgs.Cancel = true;
			};

			using (WebApp.Start<Startup>(string.Format("http://*:{0}", port)))
			{
				Console.WriteLine("Started");
				QuitEvent.WaitOne();
			}
		}
	}
}
