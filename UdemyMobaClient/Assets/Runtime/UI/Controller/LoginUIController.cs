using CCTU.UIFramework;
using Game.Model;
using Game.Net;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.UI
{
	public class LoginUIController : MobaUIControllerBase
	{
		public enum InputType
		{
			Login,
			Register,
		}

		public struct LoginInfo
		{
			public InputType InputType;
			public string Account;
			public string Password;
		}

		#region private-field
		private CancellationTokenSource _cancelToken;
		private bool _isEventAdded = false;

		private const string _sceneName = "UI/LoginUI.unity";
		#endregion private-field

		#region public-method
		public LoginUIController()
		{
			_handlerDict[typeof(LoginUIShowEvent)] = ShowHandler;
			_handlerDict[typeof(LoginUIGetInputEvent)] = GetInputHandler;
		}

		public override async Task OnEnter()
		{
			AddEventListener();
			var viewer = await GetViewer<LoginUIViewer>(_sceneName);
			await viewer.OnEnter();
		}

		public override async Task OnExit()
		{
			TaskUtility.CancelToken(ref _cancelToken);
			RemoveEventListener();
			var viewer = await GetViewer<LoginUIViewer>(_sceneName);
			await viewer.OnExit();
		}

		public override void RegisterEvent(Dictionary<Type, UIControllerBase> dict)
		{
			dict[typeof(LoginUIShowEvent)] = this;
			dict[typeof(LoginUIGetInputEvent)] = this;
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

		private async Task ShowHandler(IUIEvent uiEvent)
		{
			if (uiEvent is LoginUIShowEvent showEvent)
			{
				if (showEvent.IsShow)
				{
					await UIManager.Instance.PushUI((int)UIType.MainUI, this);
				}
				else
				{
					await UIManager.Instance.PopUI((int)UIType.MainUI, this);
				}
			}
		}

		private async Task GetInputHandler(IUIEvent uiEvent)
		{
			if (uiEvent is LoginUIGetInputEvent showEvent)
			{
				var viewer = await GetViewer<LoginUIViewer>(_sceneName);
				var buttonEvent = await viewer.GetButtonEvent(TaskUtility.RefreshToken(ref _cancelToken));
				while (!HandleButtonEvent(viewer, buttonEvent))
				{
					buttonEvent = await viewer.GetButtonEvent(TaskUtility.RefreshToken(ref _cancelToken));
				}
			}
		}

		private bool HandleButtonEvent(ILoginInfoProvider loginviwer, LoginUIViewer.ButtonEvent buttonEvent) 
		{
			switch (buttonEvent)
			{
				case LoginUIViewer.ButtonEvent.Login:
					return HandleLogin(loginviwer);
				case LoginUIViewer.ButtonEvent.Register:
					return HandleRegister(loginviwer);
			}
			return false;
		}

		private bool HandleLogin(ILoginInfoProvider loginviwer)
		{
			var account = loginviwer.Account;
			if (string.IsNullOrEmpty(account))
			{
				Debug.Log($"[LoginUIController.HandleLogin] Account is Empty");
				return false;
			}

			var password = loginviwer.Password;
			if (string.IsNullOrEmpty(loginviwer.Password))
			{
				Debug.Log($"[LoginUIController.HandleLogin] Password is Empty");
				return false;
			}

			var c2sMSG = new UserLoginC2S();
			var userInfo = new UserInfo();
			userInfo.Account = account;
			userInfo.Password = password;
			c2sMSG.UserInfo = userInfo;

			BufferFactory.CreateAndSendPackage(1001, c2sMSG);
			return true;
		}

		private bool HandleRegister(ILoginInfoProvider loginviwer)
		{
			var account = loginviwer.Account;
			if (string.IsNullOrEmpty(account))
			{
				Debug.Log($"[LoginUIController.HandleRegister] Account is Empty");
				return false;
			}

			var password = loginviwer.Password;
			if (string.IsNullOrEmpty(loginviwer.Password))
			{
				Debug.Log($"[LoginUIController.HandleRegister] Password is Empty");
				return false;
			}

			var c2sMSG = new UserRegisterC2S();
			var userInfo = new UserInfo();
			userInfo.Account = account;
			userInfo.Password = password;
			c2sMSG.UserInfo = userInfo;

			BufferFactory.CreateAndSendPackage(1000, c2sMSG);
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
						var evt = new TipUIMessageEvent()
						{
							Message = "Register Success!!",
						};
						await UIManager.Instance.TriggerUIEvent(evt);
						GoToCreateRole();
					}
					break;
				//Already register
				case 3:
					{
						var evt = new TipUIMessageEvent()
						{
							Message = "Account exist!!",
						};
						await UIManager.Instance.TriggerUIEvent(evt);
						await UIManager.Instance.TriggerUIEvent(new LoginUIGetInputEvent());
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
						UIManager.Instance.TriggerUIEvent(new LoginUIShowEvent()
						{
							IsShow = false
						});
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
						var evt = new TipUIMessageEvent()
						{
							Message = "Account not exist!!",
						};
						await UIManager.Instance.TriggerUIEvent(evt);
						await UIManager.Instance.TriggerUIEvent(new LoginUIGetInputEvent());
					}
					break;
			}
		}

		private void GoToCreateRole()
		{
			UIManager.Instance.TriggerUIEvent(new LoginUIShowEvent()
			{
				IsShow = false
			});
			GameFlow.Instance.Flow.SetTrigger("Next");
		}

		private void GoToLobby()
		{
			UIManager.Instance.TriggerUIEvent(new LoginUIShowEvent()
			{
				IsShow = false
			});
			GameFlow.Instance.Flow.SetBool("HasRole", true);
			GameFlow.Instance.Flow.SetTrigger("Next");
		}
		#endregion private-method
	}
}