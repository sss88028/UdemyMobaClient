using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UISelectHero : MonoBehaviour
	{
		#region public-field
		public event Action<int> OnSelectEvent;
		#endregion public-field

		#region private-field
		[SerializeField]
		private int _heroId;
		#endregion private-field

		#region public-method
		public void OnSelectHero() 
		{
			OnSelectEvent?.Invoke(_heroId);
		}
		#endregion public-method
	}
}