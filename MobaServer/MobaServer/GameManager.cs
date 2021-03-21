using MobaServer.GameModule;
using MobaServer.Net;
using System;

namespace MobaServer
{
	class GameManager
	{
		#region private-field
		private static USocket _uSocket;
		#endregion private-field

		#region public-property
		public static USocket USocket 
		{
			get 
			{
				return _uSocket;
			}
		}
		#endregion public-property

		#region private-method
		private static void Main(string[] args)
		{
			GameModuleInit();
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
			NetEvent.Instance.Dispatch(buffer.MessageId, buffer);
		}

		private static void GameModuleInit() 
		{
			UserModule.Instance.Init();
			RolesModule.Instance.Init();
			LobbyModule.Instance.Init();
			RoomModule.Instance.Init();
		}
		#endregion private-method
	}
}
