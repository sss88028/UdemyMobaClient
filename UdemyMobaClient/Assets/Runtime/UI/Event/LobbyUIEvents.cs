using CCTU.UIFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
	public class LobbyUIShowEvent : IUIEvent
	{
		#region public-field
		public bool IsShow = true;
		#endregion public-field

		#region public-property
		public int Priority => 0;
		#endregion public-property
	}
}