﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class TipUIViewer : BaseUIViewer<TipUIViewer>
	{
		#region private-field
		private static string _sceneName = "TipUI";
		private static bool _isOpen = false;
		#endregion private-field

		#region public-method
		public static void Open()
		{
			_isOpen = true;
			if (_instance == null)
			{
				LoadScene(_sceneName);
			}
			else
			{
				_instance.OpenInternal();
			}
		}

		public static void Close()
		{
			_isOpen = false;

			_instance?.CloseInternal();
		}

		public static void LoadScene()
		{
			LoadScene(_sceneName);
		}
		#endregion public-method

		#region MonoBehaviour-method
		private void Start()
		{
			if (_isOpen)
			{
				OpenInternal();
			}
			else
			{
				CloseInternal();
			}
		}
		#endregion MonoBehaviour-method

		#region private-method
		private void OpenInternal()
		{
			gameObject.SetActive(true);
		}

		private void CloseInternal()
		{
			gameObject.SetActive(false);
		}
		#endregion private-method
	}
}