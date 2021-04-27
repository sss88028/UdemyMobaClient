using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moba.Utility
{
	public static class MobaLogger
	{
		#region private-field
		private static ILogger _logger = null;
		private static ILogger _defaultLogger = new DefaultLogger();
		#endregion private-field

		#region public-field
		public static ILogger Logger
		{
			get
			{
				return _logger ?? _defaultLogger;
			}
			set
			{
				_logger = value;
			}
		}
		#endregion public-field

		#region public-method
		public static void Log(string log)
		{
			Logger.Log(log);
		}

		public static void LogError(string log)
		{
			Logger.LogError(log);
		}

		public static void Log(int messageId, IMessage message)
		{
			Logger.Log(messageId, message);
		}
		#endregion public-method
	}
}