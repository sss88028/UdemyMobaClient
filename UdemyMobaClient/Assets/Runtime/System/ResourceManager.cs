using CCTU.GameDevTools.MonoSingleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.MainSystem
{
	public class ResourceManager : Singleton<ResourceManager>
	{
		#region private-field
		private HashSet<string> _loadingSet = new HashSet<string>();
		#endregion private-field

		#region public-method
		public T Load<T>(string path) where T : UnityEngine.Object
		{
			var obj = Resources.Load<T>(path);
			if (obj == null) 
			{
				throw new System.ArgumentException($"[ResourceManager.Load] Can't find paht : \"{path}\"");
			}

			return UnityEngine.Object.Instantiate(obj);
		}

		public void Load<T>(string path, Action<T, object[]> onLoad = null, params object[] param) where T : UnityEngine.Object
		{
			if (!_loadingSet.Add(path)) 
			{
				return;
			}
			GameSystem.Instance.StartCoroutine(LoadAsync<T>(path));
		}
		#endregion public-method

		#region private-method
		private IEnumerator LoadAsync<T>(string path, Action<T, object[]> onLoad = null, params object[] param) where T : UnityEngine.Object
		{
			var request = Resources.LoadAsync<T>(path);
			while (!request.isDone) 
			{
				yield return null;
			}

			_loadingSet.Remove(path);
			var obj = UnityEngine.Object.Instantiate(request.asset);
			
			onLoad?.Invoke((T)obj, param);
		}
		#endregion private-method
	}
}