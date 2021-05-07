using System;
using System.Collections.Generic;
using System.Text;

namespace Moba.Utility
{
	public interface ILogger
	{
		void Log(string text);
		void LogWarning(string text);
		void LogError(string text);
	}
}
