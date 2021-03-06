﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;

namespace LogSearchShipper.Core
{
	static class EdbDataFormatter
	{
		public static void ReportData(IEnumerable<EDBEnvironment> environments)
		{
			var log = LogManager.GetLogger("EdbExpectedEventSourcesLogger");

			foreach (var environment in environments)
			{
				var envName = environment.Name;

				foreach (var serverGroup in environment.ServerGroups)
				{
					foreach (var server in serverGroup.Servers)
					{
						var cluster = server.NetworkArea;
						var host = server.Name;

						foreach (var serviceData in server.Services)
						{
							var service = new EdbService(serviceData);

							foreach (var eventSource in service.EventSources)
							{
								log.Info(
									new
									{
										environment = envName,
										cluster = cluster,
										host = host,
										serverDescription = server.Description,
										service = service.Name,
										serviceName = service.ServiceName,
										serviceDescription = service.Description,
										event_source = eventSource,
										expected_state = service.State,
										serviceType = service.ServiceType,
										binaryPath = service.BinaryPath,
										systemArea = service.SystemArea,
										tags = service.Tags,
										bundlePath = service.BundlePath,
										website = service.Website,
										applicationUri = service.ApplicationUri,
										logPath = service.LogPath,
										logPathType = service.LogPathType,
										logPath1 = service.LogPath1,
										logPath1Type = service.LogPath1Type,
										logPath2 = service.LogPath2,
										logPath2Type = service.LogPath2Type,
									});
							}
						}

						if (server.Services.Count == 0)
						{
							log.Info(
								new
								{
									environment = envName,
									cluster = cluster,
									host = host,
									serverDescription = server.Description,
									service = "None",
									serviceName = "None",
									serviceDescription = "No services to display",
									serviceType = "NoService",
								});
						}
					}
				}
			}
		}
	}
}
