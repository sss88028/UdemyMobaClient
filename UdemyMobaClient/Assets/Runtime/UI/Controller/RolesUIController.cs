using Game.Model;
using Game.Net;
using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class RolesUIController : Singleton<RolesUIController>
	{
		#region public-method
		public RolesUIController()
		{
		}

		public void Load()
		{
			RolesUIViewer.LoadScene();
		}

		public void OpenUI()
		{
			RolesUIViewer.Open();
			NetEvent.Instance.AddEventListener(1201, OnGetRolesCreateS2C);
		}

		public void CloseUI()
		{
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