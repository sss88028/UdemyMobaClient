using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Moba.Utility
{
	public class EventBase<T, Param, KEY> where T : new()
	{
		#region private-field
		private static T instance;
		private ConcurrentDictionary<KEY, Action<Param>> _dic = new ConcurrentDictionary<KEY, Action<Param>>();
		#endregion private-field

		#region public-property
		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new T();
				}
				return instance;
			}
		}
		#endregion public-property

		#region public-method
		public void AddEventListener(KEY key, Action<Param> handle)
		{
			if (!_dic.TryGetValue(key, out var actions))
			{
				_dic.TryAdd(key, null);
			}
			_dic[key] += handle;
		}
		public void RemoveEventListener(KEY key, Action<Param> handle)
		{
			if (_dic.ContainsKey(key))
			{
				_dic[key] -= handle;

				if (_dic[key] == null)
				{
					_dic.TryRemove(key, out var removeActions);
				}
			}
		}

		public void Dispatch(KEY key, Param p)
		{
			if (!_dic.TryGetValue(key, out var actions))
			{
				return;
			}
			actions?.Invoke(p);
		}

		public void Dispatch(KEY key)
		{
			Dispatch(key, default(Param));
		}
		#endregion public-method
	}
}