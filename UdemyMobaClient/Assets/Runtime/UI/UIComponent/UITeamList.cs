using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
	public class UITeamList : MonoBehaviour
	{
		#region private-field
		[SerializeField]
		private UITeamUnit _prefab;

		private Stack<UITeamUnit> _pool = new Stack<UITeamUnit>();
		#endregion private-field

		#region public-method
		#endregion public-method

		#region private-method
		private UITeamUnit GetItem() 
		{
			UITeamUnit result = null;
			if (_pool.Count > 0)
			{
				result = _pool.Pop();
			}
			else 
			{
				result = Instantiate(_prefab);
			}
			return result;
		}
		#endregion private-method
	}
}