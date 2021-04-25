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
			var tasks = buttons.Select(PressButton);
			var finishedTasks = await Task.WhenAny(tasks); ;
			return finishedTasks.Result;
		}
		#endregion public-method

		#region private-method

		private static async Task<Button> PressButton(Button button)
		{
			var isPress = false;
			if (button != null)
			{
				button.onClick.AddListener(() => isPress = true);
			}

			while (!isPress)
			{
				await Task.Yield();
			}
			return button;
		}
		#endregion private-method
	}
}