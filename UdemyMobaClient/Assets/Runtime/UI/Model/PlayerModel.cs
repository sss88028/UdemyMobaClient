using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Model
{
	public class PlayerModel : Singleton<PlayerModel>
	{
		#region public-property
		public RolesInfo RolesInfo 
		{ 
			get; 
			internal set; 
		}
		#endregion public-property
	}
}