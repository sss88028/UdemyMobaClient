using CCTU.GameDevTools.MonoSingleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoSingleton<GameFlow>
{
	#region private-field
	private Animator _gameFlow;
	#endregion private-field

	#region public-property
	public Animator Flow 
	{
		get 
		{
			return _gameFlow;
		}
	}
	#endregion public-property
}
