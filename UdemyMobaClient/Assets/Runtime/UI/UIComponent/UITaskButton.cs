using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
	public class UITaskButton : UIButton
	{
		#region public-property
		public bool IsSelected
		{
			get;
			private set;
		} = false;
		#endregion public-property

		#region public-method
		public void Clear() 
		{
			IsSelected = false;
		}
		#endregion public-method

		#region protected-method
		protected override void OnButtonClick()
		{
			IsSelected = true;
		}
		#endregion protected-method
	}
}