using Game.Net;
using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class RolesUIViewer : BaseUIViewer<RolesUIViewer>
	{
		#region private-field
		private static string _sceneName = "UI/RolesUI.unity";

		[SerializeField]
		private InputField _nameField = null;
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