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
	public static class UIUtility
	{
		#region public-method
		public static async Task<Button> SelectButton(CancellationToken ct, params Button[] buttons)
		{
			var tasks = buttons.Select(PressButton).Append(CancelButton(ct));
			var finishedTasks = await Task.WhenAny(tasks);
			return finishedTasks.Result;
		}

		public static async Task<Button> SelectButton(CancellationToken ct, params UITaskButton[] buttons)
		{
			var tasks = buttons.Select(PressButton).Append(CancelButton(ct));
			var finishedTasks = await Task.WhenAny(tasks);
			return finishedTasks.Result;
		}
		#endregion public-method

		#region private-method
		private static async Task<Button> PressButton(Button button)
		{
			var isPress = false;
			if (button != null)
			{
				button.onClick.AddListener(() => 
				{ 
					isPress = true;
				});
			}

			while (!isPress)
			{
				await Task.Yield();
			}
			return button;
		}

		private static async Task<Button> PressButton(UITaskButton button)
		{
			button.Clear();
			while (!button.IsSelected)
			{
				await Task.Yield();
			}
			return button.Button;
		}

		private static async Task<Button> CancelButton(CancellationToken ct) 
		{
			while (!ct.IsCancellationRequested)
			{
				await Task.Yield();
			}
			throw new OperationCanceledException();
		}
		#endregion private-method
	}
}