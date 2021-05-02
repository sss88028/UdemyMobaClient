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
		private UITaskButton _loginButton;
		[SerializeField]
		private UITaskButton _registerButton;

		private CancellationTokenSource _buttonCancellation;
		#endregion private-field

		#region public-property
		public string Account 
		{
			get 
			{
				if (_accountInput == null) 
				{
					return string.Empty;
				}
				return _accountInput.text;
			}
		}
		
		public string Password
		{
			get
			{
				if (_passwordInput == null)
				{
					return string.Empty;
				}
				return _passwordInput.text;
			}
		}
		#endregion public-property

		#region public-method
		public static async Task<LoginUIViewer> GetInstance()
		{
			var instance = await GetInstance(_sceneName);
			return instance;
		}

		public async void Open()
		{
			gameObject.SetActive(true);
		}

		public async void Close()
		{
			gameObject.SetActive(false);
			TaskUtil.CancelToken(ref _buttonCancellation);
		}

		public async Task<InputType> GetLoginType()
		{
			var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(TaskUtil.RefreshToken(ref _buttonCancellation));
			try
			{
				var linkedCt = linkedCts.Token;
				var buttonTask = UIUtility.SelectButton(linkedCt, _loginButton, _registerButton);
				var finishedTask = await Task.WhenAny(buttonTask);

				await finishedTask;
				linkedCts.Cancel();
				var selectedButton = buttonTask.Result;

				var result = default(InputType);
				if (selectedButton == _loginButton.Button)
				{
					result = InputType.Login;
				}
				else if (selectedButton == _registerButton.Button)
				{
					result = InputType.Register;
				}

				return result;
			}
			catch (AggregateException e)
			{
				return InputType.Cancel;
			}
			finally
			{
				linkedCts.Dispose();
			}
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

			TaskUtil.CancelToken(ref _buttonCancellation);
		}
		#endregion MonoBehaviour-method
	}
}