using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer.Player
{
	public static class PlayerManager
	{
		#region private-field
		private static ConcurrentDictionary<int, PlayerEntity> _id2PlayerDict = new ConcurrentDictionary<int, PlayerEntity>();
		private static ConcurrentDictionary<int, PlayerEntity> _session2PlayerDict = new ConcurrentDictionary<int, PlayerEntity>();
		#endregion private-field

		#region public-method
		public static void Add(int session, int userID, PlayerEntity player) 
		{
			_id2PlayerDict.TryAdd(userID, player);
			_session2PlayerDict.TryAdd(session, player);
		}

		public static bool RemoveBySession(int session)
		{
			var result = _session2PlayerDict.TryRemove(session, out var player);
			return result;
		}

		public static bool RemoveByRolesID(int rolesID)
		{
			var result = _id2PlayerDict.TryRemove(rolesID, out var player);
			return result;
		}

		public static bool TryGetPlayerEntityByUserId(int rolesID, out PlayerEntity player) 
		{
			return _id2PlayerDict.TryGetValue(rolesID, out player);
		}

		public static bool TryGetPlayerEntityBySessionId(int session, out PlayerEntity player)
		{
			return _session2PlayerDict.TryGetValue(session, out player);
		}
		#endregion public-method

		#region private-method
		#endregion private-method
	}
}
