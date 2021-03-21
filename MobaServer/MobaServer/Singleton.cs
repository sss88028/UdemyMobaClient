using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer
{
	abstract class Singleton<T> where T : new()
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
}
