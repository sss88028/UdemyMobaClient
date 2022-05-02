using CCTU.UIFramework;
using Game.Model;
using Game.Net;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class LobbyUIViewer : UIViewerBase<LobbyUIViewer>
	{
		public enum LobbyState 
		{ 
			Idle,
			Matching,
			Entered,
		}

		#region public-field
		public Action OnClickMatchingNormalEvent;
		public Action OnClickCancelMatchingNormalEvent;
		#endregion public-field

		#region private-field
		private static string _sceneName = "UI/LobbyUI.unity";

		[SerializeField]
		private Text _nickNameText;
		[SerializeField]
		private Text _rankText;
		[SerializeField]
		private UINumberText _coinText;
		[SerializeField]
		private UINumberText _daimondText;
		[SerializeField]
		private GameObject _matchingTip;
		[SerializeField]
		private GameObject _matchingNormalBtn;
		[SerializeField]
		private GameObject _matchingRankBtn;
		[SerializeField]
		private GameObject _matchingCancelBtn;

		private LobbyState _currentState;
		#endregion private-field

		#region public-property
		public Text NickNameText
		{
			get
			{
				return _nickNameText;
			}
		}

		public Text RankText
		{
			get
			{
				return _rankText;
			}
		}

		public UINumberText CoinText 
		{
			get 
			{
				return _coinText;
			}
		}

		public UINumberText DaimondText
		{
			get
			{
				return _daimondText;
			}
		}
		#endregion public-property

		#region public-method
		public override Task OnEnter()
		{
			gameObject.SetActive(true);
			return base.OnEnter();
		}

		public override Task OnExit()
		{
			gameObject.SetActive(false);
			return base.OnExit();
		}

		public void SetState(LobbyState state) 
		{
			UpdateState(state);
		}

		public void OnClickMatchingNormal() 
		{
			OnClickMatchingNormalEvent?.Invoke();
		}

		public void OnClickCancelMatchingNormal()
		{
			OnClickCancelMatchingNormalEvent?.Invoke();
		}

		public void OnClickMatchingRank()
		{
		}
		#endregion public-method

		#region private-method
		private void UpdateState(LobbyState state) 
		{
			if (_currentState == state)
			{
				return;
			}

			_currentState = state;
			switch (_currentState)
			{
				case LobbyState.Idle:
				case LobbyState.Entered:
					{
						_matchingCancelBtn.SetActive(false);
						_matchingTip.SetActive(false);
						_matchingNormalBtn.SetActive(true);
						_matchingRankBtn.SetActive(true);
					}
					break;
				case LobbyState.Matching:
					{
						_matchingCancelBtn.SetActive(true);
						_matchingTip.SetActive(true);
						_matchingNormalBtn.SetActive(false);
						_matchingRankBtn.SetActive(false);
					}
					break;
			}
		}
		#endregion private-method
	}
}