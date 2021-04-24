using Game.MainSystem;
using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UITeamUnit : MonoBehaviour
	{
		#region private-field
		[SerializeField]
		private Text _nameText;
		[SerializeField]
		private Image _heroIcon;

		private UIHeroSkillDisplay[] _displays;
		#endregion private-field

		#region private-property
		private UIHeroSkillDisplay[] Displays
		{
			get
			{
				if (_displays == null)
				{
					_displays = GetComponentsInChildren<UIHeroSkillDisplay>(true);
				}
				return _displays;
			}
		}
		#endregion private-property

		#region public-method
		public void SetName(string name) 
		{
			_nameText.text = name;
		}

		public void SetHeroInfo(int heroId)
		{
			ResourceManager.Instance.Load<Sprite>($"Image/Round/{heroId}", OnHeroSpriteLoaded);
		}

		public void SetHeroSkill(int gridId, int skillId)
		{
			foreach (var display in Displays)
			{
				if (display.GridId == gridId)
				{
					display.SetSkill(skillId);
					break;
				}
			}
		}
		#endregion public-method

		#region private-method
		private void OnHeroSpriteLoaded(Sprite sprite, object[] param) 
		{
			_heroIcon.sprite = sprite;
		}
		#endregion private-method
	}
}