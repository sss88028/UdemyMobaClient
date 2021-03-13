using Game.Net;
using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class RolesUIViewer : BaseUIViewer<RolesUIViewer>
	{
		#region private-field
		private static string _sceneName = "RolesUI";
		private static bool _isOpen = false;

		[SerializeField]
		private InputField _nameField = null;
		#endregion private-field

		#region public-method
		public static void Open()
		{
			_isOpen = true;
			if (_instance == null)
			{
				LoadScene(_sceneName);
			}
			else
			{
				_instance.OpenInternal();
			}
		}

		public static void Close()
		{
			_isOpen = false;

			_instance?.CloseInternal();
		}

		public static void LoadScene()
		{
			LoadScene(_sceneName);
		}

		public void OnCreateButtonClick()
		{
			if (string.IsNullOrEmpty(_nameField.text))
			{
				Debug.Log($"[LoginUIViewer.OnRegisterButtonClick] Nickname is Empty");
				return;
			}

			var msg = new RolesCreateC2S();
			msg.NickName = _nameField.text;

			BufferFactory.CreateAndSendPackage(1201, msg);
		}
		#endregion public-method

		#region MonoBehaviour-method
		private void Start()
		{
			if (_isOpen)
			{
				OpenInternal();
			}
			else
			{
				CloseInternal();
			}
		}
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