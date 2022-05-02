using CCTU.UIFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.UI
{
	public class TipUIController : MobaUIControllerBase
	{
		#region private-field
		private const string _sceneName = "UI/TipUI.unity";

		private TipUIViewer _viewer;
		private SemaphoreSlim _messageSemaphore = new SemaphoreSlim(1);
		#endregion private-field

		#region public-method
		public TipUIController() 
		{
			_handlerDict[typeof(TipUIMessageEvent)] = HandleMessageEvent;
		}

		public override async Task OnEnter()
		{
			_viewer = await GetViewer<TipUIViewer>(_sceneName);
			await _viewer.OnEnter();
		}

		public override async Task OnExit()
		{
			_viewer = await GetViewer<TipUIViewer>(_sceneName);
			await _viewer.OnExit();
		}

		public override void RegisterEvent(Dictionary<Type, UIControllerBase> dict)
		{
			dict[typeof(TipUIMessageEvent)] = this;
		}

		public override Task HandleUIEvent(IUIEvent uiEvent)
		{
			return base.HandleUIEvent(uiEvent);
		}
		#endregion public-method

		#region private-method
		private async Task HandleMessageEvent(IUIEvent uiEvent)
		{
			if (uiEvent is TipUIMessageEvent messageEvent)
			{
				await _messageSemaphore.WaitAsync();
				await UIManager.Instance.PushUI((int)UIType.MainUI, this);
				_viewer.HintText = messageEvent.Message;
				await _viewer.GetConfirm(default);
				await UIManager.Instance.PopUI((int)UIType.MainUI, this);
				_messageSemaphore.Release();
			}
		}
		#endregion private-method
	}
}