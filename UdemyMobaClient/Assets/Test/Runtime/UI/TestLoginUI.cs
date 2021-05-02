using Game.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TestLoginUI : MonoBehaviour
{
	#region private-field
	private CancellationTokenSource _cts;
	#endregion private-field

	#region public-method
	public async void Open()
	{
		var viewer = await LoginUIViewer.GetInstance();
		viewer.Open();

		TaskUtil.RefreshToken(ref _cts);
		while (!_cts.IsCancellationRequested)
		{
			var result = await viewer.GetLoginType();
			Debug.Log($"[TestLoginUI.Open] {result}");
		}
	}

	public async void Close()
	{
		_cts.Cancel();
		var viewer = await LoginUIViewer.GetInstance();
		viewer.Close();
	}
	#endregion public-method

}
