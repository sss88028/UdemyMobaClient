using CCTU.GameDevTools.MonoSingleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
	#region public-field
	public event Action<float> OnLoading;
	#endregion public-field

	#region private-field
	private bool _isStartLoading = false;
	private List<string> _scenes = new List<string>();
	private List<AsyncOperation> _scenesToLoad = new List<AsyncOperation>();
	#endregion private-field

	#region public-method
	public void AddScene(string sceneName) 
	{
		if (_isStartLoading) 
		{
			return;
		}
		_scenes.Add(sceneName);
	}

	public void StartLoading()
	{
		if (_isStartLoading)
		{
			return;
		}
		if (_scenes.Count <= 0) 
		{
			return;
		}

		_scenesToLoad.Add(SceneManager.LoadSceneAsync(_scenes[0]));

		for (var i = 1; i < _scenes.Count; i++)
		{
			_scenesToLoad.Add(SceneManager.LoadSceneAsync(_scenes[i], LoadSceneMode.Additive));
		}

		GameSystem.Instance.StartCoroutine(LoadingScene());
	}
	#endregion public-method

	#region private-method
	private IEnumerator LoadingScene() 
	{
		var totalProgress = 0.0F;
		var loadingCount = _scenesToLoad.Count;

		foreach (var async in _scenesToLoad) 
		{
			while (!async.isDone) 
			{
				totalProgress += async.progress;
				var progress = totalProgress / loadingCount;
				OnLoading?.Invoke(progress);

				yield return null;
			}
		}
	}
	#endregion private-method
}
