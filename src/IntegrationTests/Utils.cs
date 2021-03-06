﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;

namespace IntegrationTests
{
	static class Utils
	{
		public static void ShutdownProcess(Process process)
		{
			if (process == null)
				return;

			Trace.WriteLine(string.Format("    Trying to close the process {0}", process.ProcessName));

			// send Ctrl-C to the process so it can clean up
			process.StandardInput.WriteLine("q");
			process.StandardInput.Close();

			process.CancelOutputRead();
			process.WaitForExit(30 * 1000);

			if (!process.HasExited)
			{
				Trace.WriteLine(string.Format("    Terminating the process {0} forcibly", process.ProcessName));
				KillProcessAndChildren(process.Id);
			}
		}

		private static void KillProcessAndChildren(int pid)
		{
			var searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid);
			var moc = searcher.Get();
			foreach (var cur in moc)
			{
				var mo = (ManagementObject)cur;
				KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
			}

			try
			{
				var proc = Process.GetProcessById(pid);
				proc.Kill();
			}
			catch (ArgumentException)
			{
				// Process has already exited
			}
		}

		public static void Cleanup(string path)
		{
			foreach (var file in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
			{
				File.Delete(file);
			}
		}

		public static void WriteDelimiter()
		{
			Trace.WriteLine(Delimiter);
		}

		private static readonly string Delimiter = new string('=', 60);
	}
}
