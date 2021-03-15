using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer.Match
{
	class MatchManager
	{
		#region public-field
		public const int TeamSize = 1;
		public const int MatchSize = TeamSize * 2;
		#endregion public-field

		#region private-field
		private ConcurrentDictionary<int, MatchEntity> _pool = new ConcurrentDictionary<int, MatchEntity>();
		#endregion private-field

		#region public-method
		public void Add(MatchEntity matchEntity) 
		{
			if (_pool.TryAdd(matchEntity.TeamID, matchEntity))
			{
				if (_pool.Count >= MatchSize)
				{

				}
			}
			else 
			{ 
			
			}
		}
		#endregion public-method
	}
}
