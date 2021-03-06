using System;
using System.Collections.Generic;
using System.Text;

namespace MobaServer.GameModule
{
	public abstract class GameModuleBase<T> where T : new()
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

		#region public-method		
		public virtual void Init() 
		{
			AddListener();
		}

		public virtual void Release() 
		{
			RemoveListener();
		}

		public abstract void AddListener();

		public abstract void RemoveListener();
		#endregion public-method
	}
}
