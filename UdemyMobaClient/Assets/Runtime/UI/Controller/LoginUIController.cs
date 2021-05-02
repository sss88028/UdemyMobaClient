using Game.Model;
using Game.Net;
using Google.Protobuf;
using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.UI
{
	public class LoginUIController : Singleton<LoginUIController>
	{
		public enum InputType
		{
			Undefine,
			Cancel,
			Login,
			Register,
		}

		#region private-field
		private CancellationTokenSource _cancelToken;
		private bool _isEventAdded = false;
		#endregion private-field

		#region public-method
		public LoginUIController()
		{
		}

		public async void OpenUI()
		{
			AddEventListener();
			var viewer = await LoginUIViewer.GetInstance();
			viewer.Open();

			while (!(await HandleLogin(viewer)))
			{ 
			}
		}

		public async void CloseUI() 
		{
			RemoveEventListener();

			TaskUtil.CancelToken(ref _cancelToken);
			var viewer = await LoginUIViewer.GetInstance();
			viewer.Close();
		}

		public void SaveRolesInfo(RolesInfo rolesInfo) 
		{
			PlayerModel.Instance.RolesInfo = rolesInfo;
		}
		#endregion public-method

		#region private-method
		private void AddEventListener()
		{
			if (_isEventAdded)
			{
				return;
			}
			_isEventAdded = true;
			NetEvent.Instance.AddEventListener(1000, OnGetUserRegisterS2C);
			NetEvent.Instance.AddEventListener(1001, OnGetUserLoginS2C);
		}

		private void RemoveEventListener()
		{
			if (!_isEventAdded)
			{
				return;
			}
			_isEventAdded = true;
			NetEvent.Instance.RemoveEventListener(1000, OnGetUserRegisterS2C);
			NetEvent.Instance.RemoveEventListener(1001, OnGetUserLoginS2C);
		}

		private async Task<bool> HandleLogin(LoginUIViewer viewer)
		{
			var loginType = await viewer.GetLoginType();

			if (string.IsNullOrEmpty(viewer.Account))
			{
				Debug.Log($"[LoginUIViewer.HandleLogin] Account is Empty");
				return false;
			}

			if (string.IsNullOrEmpty(viewer.Password))
			{
				Debug.Log($"[LoginUIViewer.HandleLogin] Password is Empty");
				return false;
			}

			var c2sMSG = default(IMessage);
			var messageId = default(int);
			switch (loginType)
			{
				case InputType.Login:
					{
						c2sMSG = new UserLoginC2S();
						var userInfo = new UserInfo();
						userInfo.Account = viewer.Account;
						userInfo.Password = viewer.Password;
						((UserLoginC2S)c2sMSG).UserInfo = userInfo;
						messageId = 1001;
					}
					break;
				case InputType.Register:
					{
						c2sMSG = new UserRegisterC2S();
						var userInfo = new UserInfo();
						userInfo.Account = viewer.Account;
						userInfo.Password = viewer.Password;
						((UserRegisterC2S)c2sMSG).UserInfo = userInfo;
						messageId = 1002;
					}
					break;
				default:
					{
						return true;
					}
			}

			BufferFactory.CreateAndSendPackage(messageId, c2sMSG);
			return true;
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
						await TipUIViewer.Open();
						GoToCreateRole();
					}
					break;
				//Already register
				case 3:
					{
						TipUIViewer.SetText("Account exist!!");
						await TipUIViewer.Open();
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
						await TipUIViewer.Open();
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