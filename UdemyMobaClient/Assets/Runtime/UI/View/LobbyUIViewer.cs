using Game.Model;
using Game.Net;
using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Game.UI
{
    public class LobbyUIViewer : BaseUIViewer<LobbyUIViewer>
	{
		public enum LobbyState 
		{ 
			Idle,
			Matching,
			Entered,
		}

		#region private-field
		private static string _sceneName = "UI/LobbyUI.unity";

		[SerializeField]
		private Text _nickNameText;
		[SerializeField]
		private Text _rankText;
		[SerializeField]
		private UINumberText _cointText;
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

		#region public-method
		public static async void Open()
		{
			var instance = await GetInstance(_sceneName);
			instance.OpenInternal();
		}

		public static async void Close()
		{
			var instance = await GetInstance(_sceneName);
			instance?.CloseInternal();
		}

		public static async void LoadScene()
		{
			await GetInstance(_sceneName);
		}

		public static async void SetState(LobbyState state) 
		{
			var instance = await GetInstance(_sceneName);

			instance?.UpdateState(state);
		}

		public void OnClickMatchingNormal() 
		{
			BufferFactory.CreateAndSendPackage(1300, new LobbyToMatchC2S());
		}

		public void OnClickCancelMatchingNormal()
		{
			BufferFactory.CreateAndSendPackage(1302, new LobbyQuitMatchC2S());
		}

		public void OnClickMatchingRank()
		{
		}

		public void SetRolesInfo()
		{
			_nickNameText.text = PlayerModel.Instance.RolesInfo.NickName;
			_rankText.text = PlayerModel.Instance.RolesInfo.VictoryPoint.ToString();
			_cointText.SetNumber(PlayerModel.Instance.RolesInfo.GoldCoin);
			_daimondText.SetNumber(PlayerModel.Instance.RolesInfo.Diamonds);
		}
		#endregion public-method

		#region MonoBehaviour-method
		private void OnEnable()
		{
			SetRolesInfo();
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