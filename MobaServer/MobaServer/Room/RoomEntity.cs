using Google.Protobuf;
using MobaServer.Net;
using MobaServer.Player;
using ProtoMsg;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer.Room
{
	public class RoomEntity
	{
		#region public-field
		public int SelectHeroTime = 10000;//10s
		public RoomInfo RoomInfo;
		public ConcurrentDictionary<int, PlayerInfo> PlayerList = new ConcurrentDictionary<int, PlayerInfo>();
		public ConcurrentDictionary<int, UClient> ClientList = new ConcurrentDictionary<int, UClient>();
		public int LockCount = 0;
		public ConcurrentDictionary<int, int> LoadingProgress = new ConcurrentDictionary<int, int>();
		public bool IsLoaded = false;
		#endregion public-field

		#region public-property
		public int RoomID 
		{
			get 
			{
				return RoomInfo.ID;
			}
		}
		#endregion public-property

		#region public-method
		public RoomEntity(RoomInfo roomInfo)
		{
			RoomInfo = roomInfo;
			
			Init();
		}

		public void Close() 
		{ 
		
		}

		public void LockHero(int rolesId, int heroId) 
		{
			LockCount++;
			PlayerList[rolesId].HeroID = heroId;
		}

		public void UpdateSkill(int rolesId, int skillId, int slotId) 
		{
			if (slotId == 0)
			{
				PlayerList[rolesId].SkillA = skillId;
			}
			else
			{
				PlayerList[rolesId].SkillB = skillId;
			}
		}

		public bool UpdateLoadProgress(int rolesId, int progress) 
		{
			if (!LoadingProgress.ContainsKey(rolesId)) 
			{
				MobaLogger.LogError($"[RoomEntity.UpdateLoadProgress]");
				return true;
			}
			
			LoadingProgress[rolesId] = progress;

			if (IsLoaded)
			{
				return IsLoaded;
			}

			foreach (var progressPair in LoadingProgress)
			{
				if (progressPair.Value < 100)
				{
					IsLoaded = false;
					return IsLoaded;
				}
			}

			IsLoaded = true;

			var s2cMsg = new RoomLoadProgressS2C();
			s2cMsg.IsBattleStart = true;
			GetLoadingProgress(ref s2cMsg);

			Broadcast(1406, s2cMsg);
			return IsLoaded;
		}

		public void GetLoadingProgress(ref RoomLoadProgressS2C s2cMsg)
		{
			foreach (var progressPair in LoadingProgress)
			{
				s2cMsg.RolesID.Add(progressPair.Key);
				s2cMsg.LoadProgress.Add(progressPair.Value);
			}
		}

		public void Broadcast(int messageId, IMessage s2cMsg)
		{
			foreach (var client in ClientList.Values)
			{
				BufferFactory.CreateAndSendPackage(client, messageId, s2cMsg);
			}
		}

		public void Broadcast(int teamId, int messageId, IMessage s2cMsg)
		{
			foreach (var pair in ClientList)
			{
				if (!PlayerList.TryGetValue(pair.Key, out var player))
				{
					continue;
				}
				if (player.TeamID != teamId)
				{
					continue;
				}
				BufferFactory.CreateAndSendPackage(pair.Value, messageId, s2cMsg);
			}
		}
		#endregion public-method

		#region private-method
		private async void Init()
		{
			PlayerInit();
			await Task.Delay(SelectHeroTime);

			IMessage s2cMsg = null;
			if (LockCount == RoomInfo.TeamA.Count + RoomInfo.TeamB.Count)
			{
				s2cMsg = new RoomToBattleS2C();
				
				((RoomToBattleS2C)s2cMsg).PlayerList.AddRange(PlayerList.Values);

				Broadcast(1407, s2cMsg);
			}
			else
			{
				s2cMsg = new RoomCloseS2C();
				Broadcast(1403, s2cMsg);

				RoomManager.Instance.Remove(RoomID);
			}
		}

		private void PlayerInit()
		{
			var index = 0;
			foreach (var rolesInfo in RoomInfo.TeamA)
			{
				SetUpPlayers(rolesInfo, 0, ref index);
			}

			index = 5;
			foreach (var rolesInfo in RoomInfo.TeamB)
			{
				SetUpPlayers(rolesInfo, 0, ref index);
			}
		}

		private void SetUpPlayers(RolesInfo rolesInfo, int teamId, ref int posId)
		{
			var playerInfo = new PlayerInfo();
			playerInfo.RolesInfo = rolesInfo;
			playerInfo.SkillA = 103;
			playerInfo.SkillB = 106;
			playerInfo.HeroID = 0;
			playerInfo.TeamID = teamId;
			playerInfo.PosID = posId;
			posId++;

			PlayerList.TryAdd(rolesInfo.RolesID, playerInfo);
			LoadingProgress.TryAdd(rolesInfo.RolesID, 0);
			AddClient(rolesInfo);
		}

		private void AddClient(RolesInfo rolesInfo)
		{
			if (!PlayerManager.TryGetPlayerEntityByRolesId(rolesInfo.RolesID, out var player))
			{
				MobaLogger.LogError("[RoomEntity.RoomEntity] get player failed.");
				return;
			}
			var client = GameManager.USocket.GetClient(player.Session);
			ClientList.TryAdd(rolesInfo.RolesID, client);
		}
		#endregion private-method
	}
}
