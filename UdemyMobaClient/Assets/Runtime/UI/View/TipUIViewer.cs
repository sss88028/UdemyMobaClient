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
		public static async Task<bool> Open(CancellationToken ct)
		{
			var instance = await GetInstance(_sceneName);
			instance.OpenInternal();

			var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
			try
			{
				var linkedCt = linkedCts.Token;
				var buttonTask = SelectButton(linkedCt, instance._buttonAccept, instance._buttonCancel, instance._buttonClose);
				var finishedTask = await Task.WhenAny(buttonTask);

				await finishedTask;
				linkedCts.Cancel();

				return finishedTask == buttonTask && buttonTask.Result == instance._buttonAccept;
			}
			finally
			{
				linkedCts.Dispose();
			}
		}
		
		public static async void SetText(string textContent) 
		{
			var instance = await GetInstance(_sceneName);

			instance?.SetTextInternal(textContent);
		}
		#endregion public-method
		
		#region private-method
		private static async Task<Button> SelectButton(CancellationToken ct, params Button[] buttons)
		{
			var tasks = buttons.Select(PressButton);
			var finishedTasks = await Task.WhenAny(tasks); ;
			return finishedTasks.Result;
		}

		private static async Task<Button> PressButton(Button button)
		{
			bool isPress = false;
			button.onClick.AddListener(() => isPress = true);

			while (!isPress)
			{
				await Task.Yield();
			}
			return button;
		}

		private void OpenInternal()
		{
			gameObject.SetActive(true);
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