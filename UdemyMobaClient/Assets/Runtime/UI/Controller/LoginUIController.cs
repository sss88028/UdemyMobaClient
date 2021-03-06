using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
	public class LoginUIController : Singleton<LoginUIController>
	{
		#region public-method
		public LoginUIController()
		{

		}

		public void Load() 
		{
			LoginUIViewer.LoadScene();
		}

		public void OpenUI() 
		{
			LoginUIViewer.Open();
		}

		public void CloseUI() 
		{
			LoginUIViewer.Close();
		}
		#endregion public-method

		#region private-method
		#endregion private-method
	}
}