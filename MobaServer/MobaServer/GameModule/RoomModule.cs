using MobaServer.MySql;
using MobaServer.Net;
using MobaServer.Player;
using ProtoMsg;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobaServer.GameModule
{
	class RoomModule : GameModuleBase<RoomModule>
	{
		#region public-method
		public override void AddListener()
		{
			NetEvent.Instance.AddEventListener(1400, OnGetRoomSelectHeroC2S);
			NetEvent.Instance.AddEventListener(1401, OnGetRoomSelectHeroSkillC2S);
			NetEvent.Instance.AddEventListener(1404, OnGetRoomSendMsgC2S);
			NetEvent.Instance.AddEventListener(1405, OnGetRoomLockHeroC2S);
			NetEvent.Instance.AddEventListener(1406, OnGetRoomLoadingProgressC2S);
		}

		public override void RemoveListener()
		{
			NetEvent.Instance.RemoveEventListener(1400, OnGetRoomSelectHeroC2S);
			NetEvent.Instance.RemoveEventListener(1401, OnGetRoomSelectHeroSkillC2S);
			NetEvent.Instance.RemoveEventListener(1404, OnGetRoomSendMsgC2S);
			NetEvent.Instance.RemoveEventListener(1405, OnGetRoomLockHeroC2S);
			NetEvent.Instance.RemoveEventListener(1406, OnGetRoomLoadingProgressC2S);
		}
		#endregion public-method

		#region private-method
		private void OnGetRoomSelectHeroC2S(BufferEntity request)
		{
			var c2sMsg = ProtobufHelper.FromBytes<RoomSelectHeroC2S>(request.Protocal);

			var s2cMsg = new RoomSelectHeroS2C();
			s2cMsg.HeroID = c2sMsg.HeroID;
			if (!PlayerManager.TryGetPlayerEntityBySessionId(request.SessionId, out var playerEntity))
			{
				Debug.LogError("[RoomModule.OnGetRoomSelectHeroC2S]");
			}
			s2cMsg.RolesID = playerEntity.RolesInfo.RolesID;

			playerEntity.RoomEntity.Broadcast(request.MessageId, s2cMsg);
		}

		private void OnGetRoomSelectHeroSkillC2S(BufferEntity request)
		{
			var c2sMsg = ProtobufHelper.FromBytes<RoomSelectHeroSkillC2S>(request.Protocal);

			var s2cMsg = new RoomSelectHeroSkillS2C();
			s2cMsg.SkillID = c2sMsg.SkillID;
			s2cMsg.GridID = c2sMsg.GridID;
			if (!PlayerManager.TryGetPlayerEntityBySessionId(request.SessionId, out var playerEntity))
			{
				Debug.LogError("[RoomModule.OnGetRoomSelectHeroSkillC2S]");
			}
			s2cMsg.RolesID = playerEntity.RolesInfo.RolesID;

			playerEntity.RoomEntity.UpdateSkill(s2cMsg.RolesID, s2cMsg.SkillID, s2cMsg.GridID);
			playerEntity.RoomEntity.Broadcast(request.MessageId, s2cMsg);
		}

		private void OnGetRoomSendMsgC2S(BufferEntity request)
		{
			var c2sMsg = ProtobufHelper.FromBytes<RoomSendMsgC2S>(request.Protocal);

			var s2cMsg = new RoomSendMsgS2C();
			s2cMsg.Text = c2sMsg.Text;

			if (!PlayerManager.TryGetPlayerEntityBySessionId(request.SessionId, out var playerEntity))
			{
				Debug.LogError("[RoomModule.OnGetRoomSelectHeroSkillC2S]");
			}
			s2cMsg.RolesID = playerEntity.RolesInfo.RolesID;

			//playerEntity.RoomEntity.Broadcast(playerEntity.TeamId, request.MessageId, s2cMsg);
			playerEntity.RoomEntity.Broadcast(request.MessageId, s2cMsg);
		}

		private void OnGetRoomLockHeroC2S(BufferEntity request)
		{
			var c2sMsg = ProtobufHelper.FromBytes<RoomLockHeroC2S>(request.Protocal);

			var s2cMsg = new RoomLockHeroS2C();
			s2cMsg.HeroID = c2sMsg.HeroID;
			if (!PlayerManager.TryGetPlayerEntityBySessionId(request.SessionId, out var playerEntity))
			{
				Debug.LogError("[RoomModule.OnGetRoomLockHeroC2S]");
			}
			s2cMsg.RolesID = playerEntity.RolesInfo.RolesID;

			playerEntity.RoomEntity.LockHero(s2cMsg.RolesID, s2cMsg.HeroID);
			playerEntity.RoomEntity.Broadcast(request.MessageId, s2cMsg);
		}

		private void OnGetRoomLoadingProgressC2S(BufferEntity request)
		{
			var c2sMsg = ProtobufHelper.FromBytes<RoomLoadProgressC2S>(request.Protocal);

			var s2cMsg = new RoomLoadProgressS2C();
			s2cMsg.IsBattleStart = false;
			if (!PlayerManager.TryGetPlayerEntityBySessionId(request.SessionId, out var playerEntity))
			{
				Debug.LogError("[RoomModule.OnGetRoomLockHeroC2S]");
			}

			var isLoaded = playerEntity.RoomEntity.UpdateLoadProgress(playerEntity.RolesInfo.RolesID, c2sMsg.LoadProgress);
			if (!isLoaded) 
			{
				playerEntity.RoomEntity.GetLoadingProgress(ref s2cMsg);
				BufferFactory.CreateAndSendPackage(request, s2cMsg);
			}
		}
		#endregion private-method
	}
}
