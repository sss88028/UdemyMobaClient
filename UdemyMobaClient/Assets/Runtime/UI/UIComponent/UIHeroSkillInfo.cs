using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.UI
{
	public class UIHeroSkillInfo : MonoBehaviour
	{
		#region private-field
		[SerializeField]
		private UnityEventIntInt _onSelectSkillEvent;

		private int _gridId = -1;
		private int _skillId = -1;
		#endregion private-field

		#region public-method
		public void ShowSelectInfo(int gridId, int skillId) 
		{
			_gridId = gridId;
			_skillId = skillId;
			gameObject.SetActive(true);
		}
		#endregion public-method

		#region private-method
		private void Awake()
		{
			SetUpUIEvent();
		}
		#endregion private-method

		#region private-method
		private void SetUpUIEvent() 
		{
			var list = GetComponentsInChildren<UISelectHeroSkillButton>();
			foreach (var l in list) 
			{
				l.OnSelectSkillEvent += OnSelectSkill;
			}
		}

		private void OnSelectSkill(int skillId)
		{
			_skillId = skillId;
			_onSelectSkillEvent?.Invoke(_gridId, _skillId);
			gameObject.SetActive(false);
		}
		#endregion private-method
	}
}