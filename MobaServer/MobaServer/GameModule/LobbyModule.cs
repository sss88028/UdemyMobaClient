using MobaServer.Match;
using MobaServer.Net;
using MobaServer.Player;
using ProtoMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer.GameModule
{
	public class LobbyModule : GameModuleBase<LobbyModule>
	{
		#region public-method
		public override void AddListener()
		{
			NetEvent.Instance.AddEventListener(1300, OnGetLobbyToMatchC2S);
			NetEvent.Instance.AddEventListener(1302, OnGetLobbyQuitMatchC2S);
		}

		public override void RemoveListener()
		{
			NetEvent.Instance.RemoveEventListener(1300, OnGetLobbyToMatchC2S);
			NetEvent.Instance.RemoveEventListener(1302, OnGetLobbyQuitMatchC2S);
		}
		#endregion public-method

		#region private-method
		private void OnGetLobbyToMatchC2S(BufferEntity request)
		{
			var c2sMSG = ProtobufHelper.FromBytes<LobbyToMatchC2S>(request.Protocal);
			
			var s2cMSG = new LobbyToMatchS2C();
			s2cMSG.Result = 0;

			if (!PlayerManager.TryGetPlayerEntityBySessionId(request.SessionId, out var playerEntity)) 
			{
				Debug.LogError($"[LobbyModule.OnGetLobbyToMatchC2S] Can't get player by session : {request.SessionId}");
				return;
			}

			var matchEntity = new MatchEntity();
			matchEntity.TeamID = playerEntity.RolesInfo.RolesID;
			matchEntity.PlayerEntity.Add(playerEntity);
			playerEntity.MatchEntity = matchEntity;

			BufferFactory.CreateAndSendPackage(request, s2cMSG);
		}

		private void OnGetLobbyQuitMatchC2S(BufferEntity request)
		{
			var c2sMSG = ProtobufHelper.FromBytes<LobbyQuitMatchC2S>(request.Protocal);

			if (!PlayerManager.TryGetPlayerEntityBySessionId(request.SessionId, out var playerEntity))
			{
				Debug.Log($"[LobbyModule.OnGetLobbyQuitMatchC2S] Can't get player by session : {request.SessionId}");
				return;
			}

			var s2cMSG = new LobbyQuitMatchS2C();
			var isRemove = MatchManager.Instance.Remove(playerEntity.MatchEntity);
			if (isRemove)
			{
				playerEntity.MatchEntity = null;
				s2cMSG.Result = 0;
			}
			else
			{
				s2cMSG.Result = 1;
			}
			BufferFactory.CreateAndSendPackage(request, s2cMSG);
		}
		#endregion private-method
	}
}
