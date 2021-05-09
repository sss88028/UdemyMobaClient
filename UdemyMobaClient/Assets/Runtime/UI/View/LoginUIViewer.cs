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
		public enum ButtonEvent 
		{ 
			ForceCancel = -2,
			Unknown = -1,
			Login,
			Register,
		}

		#region private-field
		private static string _sceneName = "UI/LoginUI.unity";

		[SerializeField]
		private InputField _accountInput = null;
		[SerializeField]
		private InputField _passwordInput = null;

		[SerializeField]
		private UITaskButton _loginButton;
		[SerializeField]
		private UITaskButton _registerButton;

		private CancellationTokenSource _buttonCts;
		#endregion private-field

		#region public-method
		public async static Task<LoginUIViewer> GetInstance()
		{
			var instance = await GetInstance(_sceneName);
			return instance;
		}

		public void Open()
		{
			gameObject.SetActive(true);
		}

		public async Task<ButtonEvent> GetButtonEvent(CancellationToken ct)
		{
			if (_buttonCts != null)
			{
				TaskUtility.CancelToken(ref _buttonCts);
			}

			_buttonCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
			try
			{
				var linkedCt = _buttonCts.Token;
				var buttonTask = UIUtility.SelectButton(linkedCt, _loginButton, _registerButton);
				var finishedTask = await Task.WhenAny(buttonTask);

				await finishedTask;
				_buttonCts.Cancel();

				if (buttonTask.Result == _loginButton)
				{
					return ButtonEvent.Login;
				}
				else if (buttonTask.Result == _registerButton)
				{
					return ButtonEvent.Register;
				}

				return ButtonEvent.Unknown;
			}
			catch (ArgumentException e) 
			{
				return ButtonEvent.ForceCancel;
			}
			finally
			{
				_buttonCts.Dispose();
				_buttonCts = null;
			}
		}

		public string GetAccount() 
		{
			return _accountInput.text;
		}

		public string GetPassword()
		{
			return _passwordInput.text;
		}

		public void Close()
		{
			gameObject.SetActive(false);
		}
		#endregion public-method

		#region MonoBehaviour-method
		protected override void OnDestroy()
		{
			if (_instance == null || _instance != this)
			{
				return;
			}
			_instance = null;
			TaskUtility.CancelToken(ref _buttonCts);
		}
		#endregion MonoBehaviour-method
	}
}