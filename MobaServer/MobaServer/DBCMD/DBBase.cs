using System;
using System.Collections.Generic;
using System.Text;

public abstract class DBBase<T> where T : new()
{
	#region private-field
	private static T _instance;
	#endregion private-field

	#region public-property
	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new T();
			}
			return _instance;
		}
	}
	#endregion public-property
}