using MobaServer.Player;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer.Match
{
	public class MatchEntity
	{
		#region private-field
		public HashSet<PlayerEntity> PlayerEntity = new HashSet<PlayerEntity>();
		public int TeamID;
		#endregion private-field

		#region public-property
		public int TeamCount 
		{
			get 
			{
				return PlayerEntity.Count;
			}
		}
		#endregion public-property
	}
}
