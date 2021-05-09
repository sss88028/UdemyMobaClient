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
		public static async Task<UITaskButton> SelectButton(CancellationToken ct, params UITaskButton[] buttons)
		{
			var tasks = buttons.Select(PressButton);
			var finishedTasks = await Task.WhenAny(tasks); ;
			return finishedTasks.Result;
		}
		#endregion public-method

		#region private-method

		private static async Task<UITaskButton> PressButton(UITaskButton button)
		{
			button.Clear();

			while (!button.IsSelected)
			{
				await Task.Yield();
			}
			return button;
		}
		#endregion private-method
	}
}