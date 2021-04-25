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
		private static string _sceneName = "RolesUI";

		[SerializeField]
		private InputField _nameField = null;
		[SerializeField]
		private Button _submitButton;
		#endregion private-field

		#region public-method
		public static async Task<string> Open(CancellationToken ct = default)
		{
			var instance = await GetInstance(_sceneName);
			return await instance.OpenInternal(ct);
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
		private async Task<string> OpenInternal(CancellationToken ct)
		{
			gameObject.SetActive(true);

			var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
			try
			{
				var linkedCt = linkedCts.Token;
				var buttonTask = UIUtility.SelectButton(linkedCt, _submitButton);
				var finishedTask = await Task.WhenAny(buttonTask);

				await finishedTask;
				linkedCts.Cancel();

				return _nameField.text;
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
		#endregion private-method
	}
}