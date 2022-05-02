using CCTU.UIFramework;
using Game.Model;
using Game.Net;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TipUIViewer : UIViewerBase<TipUIViewer>
	{
		#region private-field

		[SerializeField]
		private Text _hintText;
		[SerializeField]
		private UITaskButton _buttonAccept;
		[SerializeField]
		private UITaskButton _buttonClose;

		private CancellationTokenSource _buttonCts;
		#endregion private-field

		#region public-property
		public string HintText
		{
			set 
			{
				_hintText.text = value;
			}
		}
		#endregion public-property

		#region public-method
		public override Task OnEnter()
		{
			gameObject.SetActive(true);
			return base.OnEnter();
		}

		public override Task OnExit()
		{
			gameObject.SetActive(false);
			return base.OnExit();
		}

		public async Task<bool> GetConfirm(CancellationToken ct)
		{
			TaskUtility.CancelToken(ref _buttonCts);
			_buttonCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
			try
			{
				var linkedCt = _buttonCts.Token;
				var buttonTask = UIUtility.SelectButton(linkedCt, _buttonAccept, _buttonClose);
				var finishedTask = await Task.WhenAny(buttonTask);

				await finishedTask;

				return finishedTask == buttonTask && buttonTask.Result == _buttonAccept;
			}
			finally
			{
				TaskUtility.CancelToken(ref _buttonCts);
			}
		}
		#endregion public-method
	}
}