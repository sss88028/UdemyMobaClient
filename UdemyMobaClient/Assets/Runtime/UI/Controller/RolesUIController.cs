using Game.Model;
using Game.Net;
using Moba.Utility;
using Moba.Utility.Net;
using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Game.UI
{
    public class RolesUIController : Singleton<RolesUIController>
	{
		#region private-field
		private CancellationTokenSource _cancelToken;
		#endregion private-field

		#region public-method
		public RolesUIController()
		{
		}
		
		public async void OpenUI()
		{
			if (_cancelToken != null)
			{
				_cancelToken.Cancel();
				_cancelToken.Dispose();
			}
			_cancelToken = new CancellationTokenSource();

			NetEvent.Instance.AddEventListener(1201, OnGetRolesCreateS2C);

			string rolesName = string.Empty;

			while (string.IsNullOrEmpty(rolesName))
			{
				rolesName = await RolesUIViewer.Open(_cancelToken.Token);
			}

			var msg = new RolesCreateC2S();
			msg.NickName = rolesName;

			Net.BufferFactory.CreateAndSendPackage(1201, msg);
		}

		public void CloseUI()
		{
			if (_cancelToken != null)
			{
				_cancelToken.Cancel();
				_cancelToken.Dispose();
			}

			NetEvent.Instance.RemoveEventListener(1201, OnGetRolesCreateS2C);
			RolesUIViewer.Close();
		}

		public void SaveRolesInfo(RolesInfo rolesInfo)
		{
			PlayerModel.Instance.RolesInfo = rolesInfo;
		}

		public RoomInfo GetRoomInfo() 
		{
			return PlayerModel.Instance.RoomInfo;
		}
		#endregion public-method

		#region private-method
		private void OnGetRolesCreateS2C(BufferEntity buffer)
		{
			var msg = ProtobufHelper.FromBytes<RolesCreateS2C>(buffer.Protocal);
			Debug.Log($"[LoginUIController.OnGetUserLoginS2C] Result {msg.Result}");
			switch (msg.Result)
			{
				//success
				case 0:
					{
						PlayerModel.Instance.RolesInfo = msg.RolesInfo;
						CloseUI();
						GoToLobby();
					}
					break;
			}
		}

		private void GoToLobby()
		{
			GameFlow.Instance.Flow.SetTrigger("Next");
		}
		#endregion private-method
	}
}