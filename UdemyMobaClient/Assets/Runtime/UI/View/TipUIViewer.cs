using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TipUIViewer : BaseUIViewer<TipUIViewer>
	{
		#region private-field
		private static string _sceneName = "UI/TipUI.unity";

		[SerializeField]
		private Text _hintText;
		[SerializeField]
		private Button _buttonAccept;
		[SerializeField]
		private Button _buttonCancel;
		[SerializeField]
		private Button _buttonClose;
		#endregion private-field

		#region public-method
		public static async Task<bool> Open(CancellationToken ct = default)
		{
			var instance = await GetInstance(_sceneName);
			return await instance.OpenInternal(ct);
		}
		
		public static async void SetText(string textContent) 
		{
			var instance = await GetInstance(_sceneName);

			instance?.SetTextInternal(textContent);
		}
		#endregion public-method
		
		#region private-method
		private async Task<bool> OpenInternal(CancellationToken ct)
		{
			gameObject.SetActive(true);

			var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
			try
			{
				var linkedCt = linkedCts.Token;
				var buttonTask = UIUtility.SelectButton(linkedCt, _buttonAccept, _buttonCancel, _buttonClose);
				var finishedTask = await Task.WhenAny(buttonTask);

				await finishedTask;
				linkedCts.Cancel();

				return finishedTask == buttonTask && buttonTask.Result == _buttonAccept;
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

		private void SetTextInternal(string textContent) 
		{
			_hintText.text = textContent;
		}
		#endregion private-method
	}
}