using MobaServer.Room;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer.Match
{
	class MatchManager : Singleton<MatchManager>
	{
		#region public-field
		public const int TeamSize = 1;
		public const int MatchSize = TeamSize * 2;
		#endregion public-field

		#region private-field
		private ConcurrentDictionary<int, MatchEntity> _pool = new ConcurrentDictionary<int, MatchEntity>();
		#endregion private-field

		#region public-method
		public bool Add(MatchEntity matchEntity) 
		{
			if (_pool.TryAdd(matchEntity.TeamID, matchEntity))
			{
				if (_pool.Count >= MatchSize)
				{
					MatchCompleteEvent();
				}
				return true;
			}
			Debug.LogError($"[MatchManager.Add] Add pool failed, {matchEntity.TeamID}");
			return false;
		}
		public bool Remove(MatchEntity matchEntity)
		{
			return _pool.TryRemove(matchEntity.TeamID, out var entity);
		}
		#endregion public-method

		#region private-method
		private void MatchCompleteEvent() 
		{
			Debug.Log($"[MatchManager.MatchCompleteEvent]");
			var teamA = new List<MatchEntity>();
			var teamB = new List<MatchEntity>();
			for (var i = 0; i < MatchSize; i++) 
			{
				var entity = _pool.ElementAt(i).Value;
				if (teamA.Count < TeamSize)
				{
					teamA.Add(entity);
				}
				else
				{
					teamB.Add(entity);
				}
			}
			RoomManager.Instance.Add(teamA, teamB);
		}
		#endregion private-method
	}
}
