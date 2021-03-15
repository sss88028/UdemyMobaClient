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
		private static string _sceneName = "LobbyUI";
		private static bool _isOpen = false;
		private static LobbyState _state;

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

		public static void SetState(LobbyState state) 
		{
			_state = state;
			_instance?.UpdateState();
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
			UpdateState();
		}

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

		private void UpdateState() 
		{
			switch (_state)
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