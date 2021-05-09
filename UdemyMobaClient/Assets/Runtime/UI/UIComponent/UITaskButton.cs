using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITaskButton : MonoBehaviour
{
	#region public-property
	public bool IsSelected 
	{
		get;
		private set;
	}
	#endregion public-property

	#region public-method
	public void Clear() 
	{
		IsSelected = false;
	}

	public void Select()
	{
		IsSelected = true;
	}
	#endregion public-method
}
