using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public class UISelectHeroSkillButton : UIButton
	{
		#region public-field
		public event Action<int> OnSelectSkillEvent;
		#endregion public-field

		#region private-field
		[SerializeField]
		private int _skillId = -1;
		#endregion private-field

		#region private-method
		protected override void OnButtonClick()
		{
			OnSelectSkillEvent?.Invoke(_skillId);
		}
		#endregion private-method
	}
}