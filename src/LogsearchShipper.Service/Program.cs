using log4net;
using log4net.Config;
using LogsearchShipper.Core;
using Topshelf;

namespace LogsearchShipper.Service
{
	internal class MainClass
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof (MainClass));

		public static void Main(string[] args)
		{
			XmlConfigurator.Configure();

			HostFactory.Run(x =>
			{
				x.Service<LogsearchShipperService>();
				x.RunAsNetworkService();
				x.StartAutomatically();

				x.SetDescription("Logsearch Shipper.NET - forwards (Windows) log files to Logsearch cluster");
				x.SetDisplayName("Logsearch Shipper.NET");
				x.SetServiceName("logsearch_shipper_net");

				x.EnableServiceRecovery(rc => { rc.RestartService(1); // restart the service after 1 minute
				});

				x.UseLog4Net();
				x.UseLinuxIfAvailable();
			});
		}
	}

	public class LogsearchShipperService : ServiceControl
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof (LogsearchShipperService));
		private LogsearchShipperProcessManager _LogsearchShipperProcessManager;

		public bool Start(HostControl hostControl)
		{
			_LogsearchShipperProcessManager = new LogsearchShipperProcessManager();
			_LogsearchShipperProcessManager.Start();

			return true;
		}

		public bool Stop(HostControl hostControl)
		{
			_log.Debug("Stop: Calling LogsearchShipperProcessManager.Stop()");
			_LogsearchShipperProcessManager.Stop();
			_log.Debug("Stop: LogsearchShipperProcessManager.Stop() completed");

			return true;
		}
	}
}