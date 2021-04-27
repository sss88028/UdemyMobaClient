using MobaServer.Match;
using ProtoMsg;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer.Room
{
	class RoomManager : Singleton<RoomManager>
	{
		#region public-method
		public int RoomId = 0;
		#endregion public-method

		#region private-field
		private ConcurrentDictionary<int, RoomEntity> _roomDict = new ConcurrentDictionary<int, RoomEntity>();
		#endregion private-field

		#region public-method
		public void Close() 
		{
			foreach (var room in _roomDict.Values) 
			{
				room.Close();
			}
		}

		public void Add(List<MatchEntity> teamA, List<MatchEntity> teamB) 
		{
			RoomId  += 1;

			var roomInfo = new RoomInfo();
			roomInfo.ID = RoomId;
			AddTeam(ref roomInfo, teamA);
			AddTeam(ref roomInfo, teamB);

			roomInfo.StartTime = TimeHelper.Now;

			var roomEntity = new RoomEntity(roomInfo);
			if (!_roomDict.TryAdd(roomEntity.RoomID, roomEntity)) 
			{
				MobaLogger.LogError($"[RoomManager.Add] {roomEntity.RoomID}");
			}
			var s2cMsg = new LobbyUpdateMatchStateS2C();
			s2cMsg.Result = 0;
			s2cMsg.RoomInfo = roomInfo;
			roomEntity.Broadcast(1301, s2cMsg);

			SetPlayerTeam(teamA, 0, roomEntity);
			SetPlayerTeam(teamB, 1, roomEntity);
		}

		public void Remove(int roomId)
		{
			if (_roomDict.TryRemove(roomId, out var roomEntity)) 
			{
				roomEntity.Close();
			}
		}

		public bool TryGetRoomEntity(int roomId, out RoomEntity entity) 
		{
			return _roomDict.TryGetValue(roomId, out entity);
		}
		#endregion public-method

		#region private-method
		private void AddTeam(ref RoomInfo roomInfo, List<MatchEntity> team)
		{
			foreach (var matchEntity in team)
			{
				MatchManager.Instance.Remove(matchEntity);
				foreach (var player in matchEntity.PlayerEntity)
				{
					roomInfo.TeamA.Add(player.RolesInfo);
				}
			}
		}

		private void SetPlayerTeam(List<MatchEntity> team, int teamId, RoomEntity roomEntity)
		{
			foreach (var matchEntity in team)
			{
				foreach (var player in matchEntity.PlayerEntity)
				{
					player.MatchEntity = null;
					player.RoomEntity = roomEntity;
					player.TeamId = teamId;
				}
			}
		}
		#endregion private-method
	}
}
