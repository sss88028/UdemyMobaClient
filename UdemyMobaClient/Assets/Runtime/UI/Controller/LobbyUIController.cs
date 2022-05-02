using CCTU.UIFramework;
using Game.Model;
using Game.Net;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.UI
{
    public class LobbyUIController : MobaUIControllerBase
	{
		#region private-field
		private LobbyUIViewer _viewer;

		private const string _sceneName = "UI/LobbyUI.unity";
		#endregion private-field

		#region public-method
		public LobbyUIController()
		{
			_handlerDict[typeof(LobbyUIShowEvent)] = HandleShowEvent;
		}

		public override async Task OnEnter()
		{
			_viewer = await GetViewer<LobbyUIViewer>(_sceneName);
			await _viewer.OnEnter();
			AddEventListener();
			RegisterUIEvent();
		}

		public override Task OnResume()
		{
			SetRolesInfo();
			return base.OnResume();
		}

		public override async Task OnExit()
		{
			UnregisterUIEvent();
			RemoveEventListener();

			_viewer = await GetViewer<LobbyUIViewer>(_sceneName);
			await _viewer.OnExit();
		}

		public override void RegisterEvent(Dictionary<Type, UIControllerBase> dict)
		{
			dict[typeof(LobbyUIShowEvent)] = this;
		}
		#endregion public-method

		#region private-method
		private void AddEventListener()
		{
			NetEvent.Instance.AddEventListener(1300, OnGetLobbyToMatchS2C);
			NetEvent.Instance.AddEventListener(1301, OnGetLobbyUpdateMatchStateS2C);
			NetEvent.Instance.AddEventListener(1302, OnGetLobbyCancelMatchS2C);
		}

		private void RegisterUIEvent() 
		{
			_viewer.OnClickMatchingNormalEvent += OnClickMatchingNormalHandler;
			_viewer.OnClickCancelMatchingNormalEvent += OnClickCancelMatchingNormalHandler;
		}

		private void RemoveEventListener()
		{
			NetEvent.Instance.RemoveEventListener(1300, OnGetLobbyToMatchS2C);
			NetEvent.Instance.RemoveEventListener(1301, OnGetLobbyUpdateMatchStateS2C);
			NetEvent.Instance.RemoveEventListener(1302, OnGetLobbyCancelMatchS2C);
		}

		private void UnregisterUIEvent()
		{
			_viewer.OnClickMatchingNormalEvent -= OnClickMatchingNormalHandler;
			_viewer.OnClickCancelMatchingNormalEvent -= OnClickCancelMatchingNormalHandler;
		}

		private async Task HandleShowEvent(IUIEvent uiEvent) 
		{
			if (uiEvent is LobbyUIShowEvent showEvent)
			{
				if (showEvent.IsShow)
				{
					await UIManager.Instance.PushUI((int)UIType.MainUI, this);
				}
				else
				{
					await UIManager.Instance.PopUI((int)UIType.MainUI, this);
				}
			}
		}

		private async void OnGetLobbyToMatchS2C(BufferEntity response)
		{
			var msg = ProtobufHelper.FromBytes<LobbyToMatchS2C>(response.Protocal);
			if (msg.Result == 0)
			{
				_viewer.SetState(LobbyUIViewer.LobbyState.Matching);
			}
			else 
			{
				var evt = new TipUIMessageEvent() 
				{
					Message = "Can't matching",
				};
				await UIManager.Instance.TriggerUIEvent(evt);
			}
		}

		private void OnGetLobbyUpdateMatchStateS2C(BufferEntity response)
		{
			var msg = ProtobufHelper.FromBytes<LobbyUpdateMatchStateS2C>(response.Protocal);

			if (msg.Result == 0)
			{
				PlayerModel.Instance.RoomInfo = msg.RoomInfo;
				UIManager.Instance.PopUI(0, this);
				_viewer.SetState(LobbyUIViewer.LobbyState.Entered);
			}
		}

		private void OnGetLobbyCancelMatchS2C(BufferEntity response)
		{
			var msg = ProtobufHelper.FromBytes<LobbyQuitMatchS2C>(response.Protocal);

			if (msg.Result == 0)
			{
				_viewer.SetState(LobbyUIViewer.LobbyState.Idle);
			}
			else
			{
			}
		}

		private void OnClickMatchingNormalHandler()
		{
			BufferFactory.CreateAndSendPackage(1300, new LobbyToMatchC2S());
		}

		private void OnClickCancelMatchingNormalHandler()
		{
			BufferFactory.CreateAndSendPackage(1302, new LobbyQuitMatchC2S());
		}

		private void SetRolesInfo()
		{
			_viewer.NickNameText.text = PlayerModel.Instance.RolesInfo.NickName;
			_viewer.RankText.text = PlayerModel.Instance.RolesInfo.VictoryPoint.ToString();
			_viewer.CoinText.SetNumber(PlayerModel.Instance.RolesInfo.GoldCoin);
			_viewer.DaimondText.SetNumber(PlayerModel.Instance.RolesInfo.Diamonds);
		}
		#endregion private-method
	}
}