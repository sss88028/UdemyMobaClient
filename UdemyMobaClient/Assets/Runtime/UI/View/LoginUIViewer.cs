﻿using Game.Net;
using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class LoginUIViewer : BaseUIViewer<LoginUIViewer>
	{
		#region private-field
		private static string _sceneName = "UI/LoginUI.unity";

		[SerializeField]
		private InputField _accountInput = null;
		[SerializeField]
		private InputField _passwordInput = null;
		#endregion private-field

		#region public-method
		public static async void Open() 
		{
			var instance = await GetInstance(_sceneName);
			instance.OpenInternal();
		}

		public static async void Close()
		{
			var instance = await GetInstance(_sceneName);
			instance.CloseInternal();
		}
		
		public void OnRegisterButtonClick() 
		{
			if (string.IsNullOrEmpty(_accountInput.text)) 
			{
				Debug.Log($"[LoginUIViewer.OnRegisterButtonClick] Account is Empty");
				return;
			}

			if (string.IsNullOrEmpty(_passwordInput.text))
			{
				Debug.Log($"[LoginUIViewer.OnRegisterButtonClick] Password is Empty");
				return;
			}

			var c2sMSG = new UserRegisterC2S();
			var userInfo = new UserInfo();
			userInfo.Account = _accountInput.text;
			userInfo.Password = _passwordInput.text;
			c2sMSG.UserInfo = userInfo;

			BufferFactory.CreateAndSendPackage(1000, c2sMSG);
		}

		public void OnLoginButtonClick()
		{
			if (string.IsNullOrEmpty(_accountInput.text))
			{
				Debug.Log($"[LoginUIViewer.OnRegisterButtonClick] Account is Empty");
				return;
			}

			if (string.IsNullOrEmpty(_passwordInput.text))
			{
				Debug.Log($"[LoginUIViewer.OnRegisterButtonClick] Password is Empty");
				return;
			}
			

			var c2sMSG = new UserLoginC2S();
			var userInfo = new UserInfo();
			userInfo.Account = _accountInput.text;
			userInfo.Password = _passwordInput.text;
			c2sMSG.UserInfo = userInfo;

			BufferFactory.CreateAndSendPackage(1001, c2sMSG);
		}
		#endregion public-method

		#region MonoBehaviour-method
		#endregion MonoBehaviour-method

		#region private-method
		private void OpenInternal() 
		{
			gameObject.SetActive(true);
		}

		private void CloseInternal()
		{
			gameObject.SetActive(false);
		}
		#endregion private-method
	}
}