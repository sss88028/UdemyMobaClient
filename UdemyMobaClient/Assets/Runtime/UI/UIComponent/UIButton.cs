using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public abstract class UIButton : MonoBehaviour
	{
		#region private-field
		[SerializeField]
		private Button _button;
		#endregion private-field

		#region public-property
		public Button Button
		{
			get
			{
				if (_button == null)
				{
					_button = GetComponent<Button>();
				}
				return _button;
			}
		}
		#endregion public-property

		#region MonoBehaviour-method
		protected virtual void Awake()
		{
			SetUpButtonEvent();
		}
		#endregion MonoBehaviour-method

		#region protected-property
		private void SetUpButtonEvent()
		{
			if (Button != null)
			{
				Button.onClick.AddListener(OnButtonClick);
			}
		}

		protected abstract void OnButtonClick();
		#endregion protected-property
	}
}