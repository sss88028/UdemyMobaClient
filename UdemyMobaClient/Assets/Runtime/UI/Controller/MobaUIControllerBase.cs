using CCTU.UIFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Game.UI
{
	public abstract class MobaUIControllerBase : UIControllerBase
	{
		#region protected-field
		protected Dictionary<Type, Func<IUIEvent, Task>> _handlerDict = new Dictionary<Type, Func<IUIEvent, Task>>();
		#endregion protected-field

		#region private-field
		private SemaphoreSlim _getViewSemaphore = new SemaphoreSlim(1);
		private AsyncOperationHandle<SceneInstance> _handler;
		#endregion private-field

		#region public-method
		public override Task HandleUIEvent(IUIEvent uiEvent)
		{
			if (!_handlerDict.TryGetValue(uiEvent.GetType(), out var evt))
			{
				return Task.CompletedTask;
			}
			return evt(uiEvent);
		}
		#endregion public-method

		#region protected-method
		protected async Task<T> GetViewer<T>(string sceneName) where T : UIViewerBase<T> 
		{
			await _getViewSemaphore.WaitAsync();
			if (UIViewerBase<T>.Instance != null)
			{
				_getViewSemaphore.Release();
				return UIViewerBase<T>.Instance;
			}

			ReleaseHandler();
			_handler = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			await _handler.Task;
			while (UIViewerBase<T>.Instance == null)
			{
				await Task.Yield();
			}
			_getViewSemaphore.Release();
			return UIViewerBase<T>.Instance;
		}

		protected void ReleaseHandler()
		{
			if (_handler.IsValid())
			{
				Addressables.Release(_handler);
			}
		}
		#endregion protected-method
	}
}