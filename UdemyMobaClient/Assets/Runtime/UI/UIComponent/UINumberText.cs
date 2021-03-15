using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public class UINumberText : MonoBehaviour
	{
		#region private-field
		[SerializeField]
		private Text _text;
		[SerializeField]
		private string _format = "{0:n0}";
		#endregion private-field

		#region private-property
		private Text Text 
		{
			get 
			{
				if (_text == null) 
				{
					_text = GetComponent<Text>();
				}
				return _text;
			}
		}
		#endregion private-property

		#region public-method
		public void SetNumber(int number) 
		{
			Text.text = string.Format(_format, number);
		}
		#endregion public-method
	}
}