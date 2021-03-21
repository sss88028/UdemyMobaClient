using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICountDonwText : MonoBehaviour
{
	#region public-method
	public event Action OnCountDownFinish;
	#endregion public-method

	#region private-field
	[SerializeField]
	private Text _text;
	[SerializeField]
	private string _format;

	[SerializeField]
	private float _countDownTime = 10;
	private float _remainTime;

	private bool _isStartCountDown = false;
	#endregion private-field

	#region public-property
	public float CountDownTime 
	{
		get 
		{
			return _countDownTime;
		}
		set 
		{
			_countDownTime = value;
		}
	}
	#endregion public-property

	#region public-method
	public void PlayCountDown()
	{
		_isStartCountDown = true;
	}

	public void PauseCountDown()
	{
		_isStartCountDown = false;
	}

	public void StopCountDown()
	{
		_isStartCountDown = false;
		_remainTime = _countDownTime;
	}
	#endregion public-method

	#region MonoBehaviour-method
	private void Update()
	{
		if (_isStartCountDown) 
		{
			if (_remainTime > 0) 
			{
				_remainTime -= Time.deltaTime;
				_remainTime = Mathf.Max(0, _remainTime);

				if (_text != null) 
				{
					_text.text = string.Format(_format, _remainTime);
				}
			}
		}
	}
	#endregion MonoBehaviour-method
}
