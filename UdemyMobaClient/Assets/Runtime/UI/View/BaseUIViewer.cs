using CCTU.GameDevTools.MonoSingleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UI
{
    public abstract class BaseUIViewer<T> : MonoBehaviour where T : BaseUIViewer<T>
	{
		#region protected-field
		protected static T _instance;
		#endregion protected-field

		#region private-method
		private static bool _isLoding = false;
		#endregion private-method

		#region protected-method
		protected static void LoadScene(string sceneName)
		{
			if (_isLoding) 
			{
				return;
			}
			_isLoding = true;
			GameSystem.Instance.StartCoroutine(LoadSceneAsync(sceneName));
		}
		#endregion protected-method

		#region MonoBehaviour-method
		protected virtual void Awake() 
		{
			if (_instance != null && _instance != this) 
			{
				Destroy(this);
				return;
			}
			_instance = (T)this;
		}

		protected virtual void OnDestroy()
		{
			if (_instance == null || _instance != this)
			{
				return;
			}
			_instance = null;
		}
		#endregion MonoBehaviour-method

		#region private-method
		private static IEnumerator LoadSceneAsync(string sceneName)
		{
			var asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			while (!asyncLoad.isDone)
			{
				yield return null;
			}
			_isLoding = false;
		}
		#endregion private-method
	}
}