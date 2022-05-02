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
using UnityEngine.UI;

namespace Game.UI
{
	public interface ILoginInfoProvider 
	{
		string Account 
		{
			get;
		}

		string Password
		{
			get;
		}
	}

    public class LoginUIViewer : UIViewerBase<LoginUIViewer>, ILoginInfoProvider
	{
		public enum ButtonEvent 
		{ 
			ForceCancel = -2,
			Unknown = -1,
			Login,
			Register,
		}

		#region private-field

		[SerializeField]
		private InputField _accountInput = null;
		[SerializeField]
		private InputField _passwordInput = null;

		[SerializeField]
		private UITaskButton _loginButton;
		[SerializeField]
		private UITaskButton _registerButton;

		private CancellationTokenSource _buttonCts;

		public string Account => _accountInput.text;

		public string Password => _passwordInput.text;
		#endregion private-field

		#region public-method
		public override Task OnEnter()
		{
			gameObject.SetActive(true);
			return base.OnEnter();
		}

		public override Task OnExit()
		{
			gameObject.SetActive(false);
			return base.OnExit();
		}

		public async Task<ButtonEvent> GetButtonEvent(CancellationToken ct)
		{
			TaskUtility.CancelToken(ref _buttonCts);

			_buttonCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
			try
			{
				var linkedCt = _buttonCts.Token;
				var buttonTask = UIUtility.SelectButton(linkedCt, _loginButton, _registerButton);
				var finishedTask = await Task.WhenAny(buttonTask);

				await finishedTask;

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
				TaskUtility.CancelToken(ref _buttonCts);
			}
		}
		#endregion public-method

		#region protected-method
		protected override void OnDestroyEventHandler()
		{
			TaskUtility.CancelToken(ref _buttonCts);
		}
		#endregion protected-method
	}
}