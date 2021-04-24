using Game.MainSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public class UIHeroSkillDisplay : UIButton
	{
		#region public-field
		public event Action<int> OnSelectGrid;
		#endregion public-field

		#region private-field
		[SerializeField]
		private int _gridId = -1;
		[SerializeField]
		private Image _skillIcon;
		#endregion private-field

		#region public-property
		public int GridId 
		{
			get 
			{
				return _gridId;
			}
		}
		#endregion public-property

		#region public-method
		public void SetSkill(int skillId) 
		{
			ResourceManager.Instance.Load<Sprite>($"Image/GeneralSkill/{skillId}", OnHeroSkillSpriteLoaded);
		}
		#endregion public-method

		#region protected-method
		protected override void OnButtonClick()
		{
			OnSelectGrid?.Invoke(_gridId);
		}
		#endregion private-method

		#region protected-method
		private void OnHeroSkillSpriteLoaded(Sprite sprite, object[] param)
		{
			_skillIcon.sprite = sprite;
		}
		#endregion private-method
	}
}