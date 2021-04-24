using ProtoMsg;
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
		private Dictionary<int, UITeamUnit> _rolesDict = new Dictionary<int, UITeamUnit>();
		#endregion private-field

		#region public-method
		public void CreateRoles(IEnumerable<RolesInfo> rolesInfos) 
		{
			foreach (var role in rolesInfos) 
			{
				var item = GetItem();
				item.gameObject.SetActive(true);

				_rolesDict.Add(role.ID, item);

				item.SetName(role.NickName);
			}
		}

		public void SetHeroInfo(int rolesId, int heroId) 
		{
			if (_rolesDict.TryGetValue(rolesId, out var unit)) 
			{
				unit.SetHeroInfo(heroId);
			}
		}
		public void SetHeroSkill(int rolesId, int gridId, int skillId)
		{
			if (_rolesDict.TryGetValue(rolesId, out var unit))
			{
				unit.SetHeroSkill(gridId, skillId);
			}
		}
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
				result = Instantiate(_prefab, transform, false);
			}
			return result;
		}
		#endregion private-method
	}
}