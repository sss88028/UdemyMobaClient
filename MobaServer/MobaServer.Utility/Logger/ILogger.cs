using Google.Protobuf;
using Moba.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moba.Utility
{
	internal class DefaultLogger : ILogger
	{
		#region public-method
		public void Log(string log)
		{
			var tempColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(log);
			Console.ForegroundColor = tempColor;
		}

		public void Log(int messageId, IMessage message)
		{
			var tempColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine($"MessageId : {messageId} \n Package {JsonHelper.SerializeObject(message)}");
			Console.ForegroundColor = tempColor;
		}

		public void LogError(string log)
		{
			var tempColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(log);
			Console.ForegroundColor = tempColor;
		}
		#endregion public-method
	}

	public interface ILogger
	{
		void Log(string log);
		void LogError(string log);
		void Log(int messageId, IMessage message);
	}
}
