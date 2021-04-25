using Game.Model;
using Game.Net;
using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
		private Button _matchingNormalBtn;
		[SerializeField]
		private Button _matchingRankBtn;
		[SerializeField]
		private Button _matchingCancelBtn;

		private LobbyState _currentState;
		private CancellationTokenSource _cts;
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
		private void Start() 
		{
			_cts = new CancellationTokenSource();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			_cts?.Cancel();
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
					{
						_matchingCancelBtn.gameObject.SetActive(false);
						_matchingTip.SetActive(false);
						_matchingNormalBtn.gameObject.SetActive(true);
						_matchingRankBtn.gameObject.SetActive(true);
						WaitClickMatching(_cts.Token);
					}
					break;
				case LobbyState.Entered:
					{
						_matchingCancelBtn.gameObject.SetActive(false);
						_matchingTip.SetActive(false);
						_matchingNormalBtn.gameObject.SetActive(true);
						_matchingRankBtn.gameObject.SetActive(true);
					}
					break;
				case LobbyState.Matching:
					{
						_matchingCancelBtn.gameObject.SetActive(true);
						_matchingTip.SetActive(true);
						_matchingNormalBtn.gameObject.SetActive(false);
						_matchingRankBtn.gameObject.SetActive(false);

						WaitClickCancel(_cts.Token);
					}
					break;
			}
		}

		private async void WaitClickMatching(CancellationToken ct)
		{
			var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
			try
			{
				var linkedCt = linkedCts.Token;
				var buttonTask = UIUtility.SelectButton(linkedCt, _matchingNormalBtn);
				var finishedTask = await Task.WhenAny(buttonTask);

				await finishedTask;
				linkedCts.Cancel();

				if (buttonTask.Result == _matchingNormalBtn)
				{
					BufferFactory.CreateAndSendPackage(1300, new LobbyToMatchC2S());
				}
			}
			finally
			{
				linkedCts.Dispose();
			}
		}


		private async void WaitClickCancel(CancellationToken ct)
		{
			var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
			try
			{
				var linkedCt = linkedCts.Token;
				var buttonTask = UIUtility.SelectButton(linkedCt, _matchingCancelBtn);
				var finishedTask = await Task.WhenAny(buttonTask);

				await finishedTask;
				linkedCts.Cancel();

				if (buttonTask.Result == _matchingCancelBtn)
				{
					BufferFactory.CreateAndSendPackage(1302, new LobbyQuitMatchC2S());
				}
			}
			finally
			{
				linkedCts.Dispose();
			}
		}
		#endregion private-method
	}
}