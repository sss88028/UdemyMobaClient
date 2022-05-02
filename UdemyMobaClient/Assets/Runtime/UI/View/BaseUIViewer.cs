
using CCTU.GameDevTools.MonoSingleton;
using CCTU.UIFramework;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Game.UI
{
    public abstract class BaseUIViewer<T> : MonoBehaviour where T : BaseUIViewer<T>
	{
		#region private-method
		protected static T _instance;
		#endregion private-method

		#region protected-method
		protected static async Task<T> GetInstance(string sceneName)
		{
			if (_instance != null)
			{
				return _instance;
			}

			Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			while (_instance == null)
			{
				await Task.Yield();
			}
			return _instance;
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
	}
}