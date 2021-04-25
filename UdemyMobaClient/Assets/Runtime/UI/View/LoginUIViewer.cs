using Game.Net;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static Game.UI.LoginUIController;

namespace Game.UI
{
    public class LoginUIViewer : BaseUIViewer<LoginUIViewer>
	{
		#region private-field
		private static string _sceneName = "LoginUI";

		[SerializeField]
		private InputField _accountInput = null;
		[SerializeField]
		private InputField _passwordInput = null;

		[SerializeField]
		private Button _loginButton;
		[SerializeField]
		private Button _registerButton;
		#endregion private-field

		#region public-method
		public static async Task<Func<bool>> Open(CancellationToken ct) 
		{
			var instance = await GetInstance(_sceneName);
			return await instance.OpenInternal(ct);
		}

		public static async void Close()
		{
			var instance = await GetInstance(_sceneName);
			instance.CloseInternal();
		}
		#endregion public-method

		#region MonoBehaviour-method
		#endregion MonoBehaviour-method

		#region private-method
		private async Task<Func<bool>> OpenInternal(CancellationToken ct) 
		{
			gameObject.SetActive(true);

			var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
			try
			{
				var linkedCt = linkedCts.Token;
				var buttonTask = UIUtility.SelectButton(linkedCt, _loginButton, _registerButton);
				var finishedTask = await Task.WhenAny(buttonTask);

				await finishedTask;
				linkedCts.Cancel();

				Func<bool> act = null;
				if (buttonTask.Result == _loginButton)
				{
					act = OnLoginButtonClick;
				}
				else if (buttonTask.Result == _registerButton)
				{
					act = OnRegisterButtonClick;
				}

				return act;
			}
			finally
			{
				linkedCts.Dispose();
			}
		}

		private void CloseInternal()
		{
			gameObject.SetActive(false);
		}

		private bool OnRegisterButtonClick()
		{
			if (string.IsNullOrEmpty(_accountInput.text))
			{
				Debug.Log($"[LoginUIViewer.OnRegisterButtonClick] Account is Empty");
				return false;
			}

			if (string.IsNullOrEmpty(_passwordInput.text))
			{
				Debug.Log($"[LoginUIViewer.OnRegisterButtonClick] Password is Empty");
				return false;
			}

			var c2sMSG = new UserRegisterC2S();
			var userInfo = new UserInfo();
			userInfo.Account = _accountInput.text;
			userInfo.Password = _passwordInput.text;
			c2sMSG.UserInfo = userInfo;

			BufferFactory.CreateAndSendPackage(1000, c2sMSG);
			return true;
		}

		private bool OnLoginButtonClick()
		{
			if (string.IsNullOrEmpty(_accountInput.text))
			{
				Debug.Log($"[LoginUIViewer.OnRegisterButtonClick] Account is Empty");
				return false;
			}

			if (string.IsNullOrEmpty(_passwordInput.text))
			{
				Debug.Log($"[LoginUIViewer.OnRegisterButtonClick] Password is Empty");
				return false;
			}


			var c2sMSG = new UserLoginC2S();
			var userInfo = new UserInfo();
			userInfo.Account = _accountInput.text;
			userInfo.Password = _passwordInput.text;
			c2sMSG.UserInfo = userInfo;

			BufferFactory.CreateAndSendPackage(1001, c2sMSG);
			return true;
		}
		#endregion private-method
	}
}