using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Text;

static class Debug
{
	#region public-method
	public static void Log(string log)
	{
		var tempColor = Console.ForegroundColor;
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine(log);
		Console.ForegroundColor = tempColor;
	}

	public static void LogError(string log)
	{
		var tempColor = Console.ForegroundColor;
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine(log);
		Console.ForegroundColor = tempColor;
	}
	public static void Log(int messageId, IMessage message)
	{
		var tempColor = Console.ForegroundColor;
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine($"MessageId : {messageId} \n Package {JsonHelper.SerializeObject(message)}");
		Console.ForegroundColor = tempColor;
	}
	#endregion public-method
}