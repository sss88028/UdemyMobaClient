using MobaServer.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobaServer.GameModule
{
	public class UserModule : GameModuleBase<UserModule>
	{
		#region public-method
		public override void AddListener()
		{
			NetEvent.Instance.AddEventListener(1000, OnGetUserRegisterCSS);
			NetEvent.Instance.AddEventListener(1001, OnGetUserLoginCSS);
		}


		public override void RemoveListener()
		{
			NetEvent.Instance.RemoveEventListener(1000, OnGetUserRegisterCSS);
			NetEvent.Instance.RemoveEventListener(1001, OnGetUserLoginCSS);
		}
		#endregion public-method

		#region private-method
		private void OnGetUserRegisterCSS(BufferEntity request)
		{
		}

		private void OnGetUserLoginCSS(BufferEntity request)
		{
		}
		#endregion private-method
	}
}
