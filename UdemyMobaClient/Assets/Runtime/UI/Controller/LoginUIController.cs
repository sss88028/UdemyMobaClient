using Game.Model;
using Game.Net;
using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Game.UI
{
	public class LoginUIController : Singleton<LoginUIController>
	{
		#region public-method
		public LoginUIController()
		{
		}
		
		public void OpenUI() 
		{
			AddEventListener();
			LoginUIViewer.Open();
		}

		public void CloseUI() 
		{
			RemoveEventListener();
			LoginUIViewer.Close();
		}

		public void SaveRolesInfo(RolesInfo rolesInfo) 
		{
			PlayerModel.Instance.RolesInfo = rolesInfo;
		}
		#endregion public-method

		#region private-method
		private void AddEventListener()
		{
			NetEvent.Instance.AddEventListener(1000, OnGetUserRegisterS2C);
			NetEvent.Instance.AddEventListener(1001, OnGetUserLoginS2C);
		}

		private void RemoveEventListener()
		{
			NetEvent.Instance.RemoveEventListener(1000, OnGetUserRegisterS2C);
			NetEvent.Instance.RemoveEventListener(1001, OnGetUserLoginS2C);
		}

		private async void OnGetUserRegisterS2C(BufferEntity buffer) 
		{
			var s2cMSG = ProtobufHelper.FromBytes<UserRegisterS2C>(buffer.Protocal);
			Debug.Log($"[LoginUIController.OnGetUserLoginS2C] Result {s2cMSG.Result}");
			switch (s2cMSG.Result) 
			{
				//success
				case 0:
					{
						TipUIViewer.SetText("Register Success!!");
						await TipUIViewer.Open(new CancellationToken());
						GoToCreateRole();
					}
					break;
				//Already register
				case 3:
					{
						TipUIViewer.SetText("Account exist!!");
						await TipUIViewer.Open(new CancellationToken());
					}
					break;
			}
		}

		private async void OnGetUserLoginS2C(BufferEntity buffer)
		{
			var s2cMSG = ProtobufHelper.FromBytes<UserLoginS2C>(buffer.Protocal);
			Debug.Log($"[LoginUIController.OnGetUserLoginS2C] Result {s2cMSG.Result}");
			switch (s2cMSG.Result)
			{
				//success
				case 0:
					{
						CloseUI();

						if (s2cMSG.RolesInfo != null)
						{
							SaveRolesInfo(s2cMSG.RolesInfo);
							GoToLobby();
						}
						else 
						{
							GoToCreateRole();
						}
					}
					break;
				//No match
				case 2:
					{
						TipUIViewer.SetText("Account not exist!!");
						await TipUIViewer.Open(new CancellationToken());
					}
					break;
			}
		}

		private void GoToCreateRole()
		{
			CloseUI();
			GameFlow.Instance.Flow.SetTrigger("Next");
		}

		private void GoToLobby()
		{
			CloseUI();
			GameFlow.Instance.Flow.SetBool("HasRole", true);
			GameFlow.Instance.Flow.SetTrigger("Next");
		}
		#endregion private-method
	}
}