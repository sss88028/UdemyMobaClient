using System;
using System.Collections.Generic;
using System.Text;

namespace Moba.Utility
{
	public static class MobaLogger
	{
		private class DefaultLogger : ILogger
		{
			#region private-field
			private static DefaultLogger _instance;
			#endregion private-field

			#region public-property
			public static DefaultLogger Instance
			{
				get
				{
					if (_instance == null)
					{
						_instance = new DefaultLogger();
					}
					return _instance;
				}
			}
			#endregion public-property

			#region public-method
			public void Log(string text)
			{
				Console.WriteLine(text);
			}

			public void LogError(string text)
			{
				var oldColor = Console.ForegroundColor;

				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(text);
				Console.ForegroundColor = oldColor;
			}

			public void LogWarning(string text)
			{
				var oldColor = Console.ForegroundColor;

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine(text);
				Console.ForegroundColor = oldColor;
			}
			#endregion public-method
		}

		#region private-field
		private static ILogger _logger = null;
		#endregion private-field

		#region public-property
		public static ILogger Logger
		{
			get
			{
				if (_logger == null)
				{
					return DefaultLogger.Instance;
				}
				return _logger;
			}
		}
		#endregion public-property

		#region public-method
		public static void Log(string text)
		{
			Logger.Log(text);
		}

		public static void LogWarning(string text)
		{
			Logger.LogWarning(text);
		}

		public static void LogError(string text)
		{
			Logger.LogError(text);
		}
		#endregion public-method
	}
}
