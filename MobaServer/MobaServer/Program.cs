using MobaServer.Net;
using System;

namespace MobaServer
{
	class Program
	{

		#region private-field
		private static USocket _uSocket;
		#endregion private-field

		#region private-field
		#endregion private-field

		#region private-method
		static void Main(string[] args)
		{
			NetSystemInit();

			Console.ReadLine();
		}

		private static void NetSystemInit()
		{
			_uSocket = new USocket(DispatchEvent);
			Debug.Log("[Program.NetSystemInit] 0");
		}

		private static void DispatchEvent(BufferEntity buffer) 
		{ 
		
		}
		#endregion private-method
	}
}
