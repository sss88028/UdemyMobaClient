using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TipUIViewer : BaseUIViewer<TipUIViewer>
	{
		#region private-field
		private static string _sceneName = "TipUI";
		private static bool _isOpen = false;

		[SerializeField]
		private Text _hintText;

		private static string _textContent;
		private static Action _onClickEnterEvent;
		private static Action _onClickCloseEvent;
		#endregion private-field

		#region public-method
		public static void Open(Action onClickEnterEvent = null, Action onClickCloseEvent = null)
		{
			_isOpen = true;
			_onClickEnterEvent = onClickEnterEvent;
			_onClickCloseEvent = onClickCloseEvent;
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
			_onClickEnterEvent = null;
			_onClickCloseEvent = null;
		}

		public static void SetText(string textContent) 
		{
			_textContent = textContent;

			_instance?.SetTextInternal();
		}

		public static void LoadScene()
		{
			LoadScene(_sceneName);
		}

		public void OnClickEnterHanlder() 
		{
			if (_onClickEnterEvent != null)
			{
				_onClickEnterEvent.Invoke();
				_onClickEnterEvent = null;
			}
			else 
			{
				Close();
			}
		}

		public void OnClickCloseHanlder()
		{
			if (_onClickCloseEvent != null)
			{
				_onClickCloseEvent.Invoke();
				_onClickCloseEvent = null;
			}
			else
			{
				Close();
			}
		}
		#endregion public-method

		#region MonoBehaviour-method
		private void Start()
		{
			if (_isOpen)
			{
				OpenInternal();
				SetTextInternal();
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

		private void SetTextInternal() 
		{
			_hintText.text = _textContent;
		}
		#endregion private-method
	}
}