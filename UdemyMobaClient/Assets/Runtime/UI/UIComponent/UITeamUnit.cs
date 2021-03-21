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
		#endregion private-field

		#region public-method
		public void SetName(string name) 
		{
			_nameText.text = name;
		}

		public void SetHeroInfo(int heroId)
		{
			ResourceManager.Instance.Load<Sprite>($"Image/Round/{heroId}", OnSpriteLoaded);
		}
		#endregion public-method

		#region private-method
		private void OnSpriteLoaded(Sprite sprite, object[] param) 
		{
			_heroIcon.sprite = sprite;
		}
		#endregion private-method
	}
}