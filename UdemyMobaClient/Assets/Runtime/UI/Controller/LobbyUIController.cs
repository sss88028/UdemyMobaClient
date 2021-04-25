using Game.Model;
using Game.Net;
using Game.State;
using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Game.UI
{
    public partial class LobbyUIController : Singleton<LobbyUIController>
	{
		#region private-field
		private bool _isEventAdded = false;
		#endregion private-field

		#region public-method
		public LobbyUIController()
		{
		}

		public void Load()
		{
			LobbyUIViewer.LoadScene();
		}

		public void OpenUI()
		{
			AddEventListener();
			LobbyUIViewer.Open();
		}

		public void CloseUI()
		{
			RemoveEventListener();
			LobbyUIViewer.Close();
		}
		#endregion public-method

		#region private-method
		private void AddEventListener()
		{
			if (_isEventAdded) 
			{
				return;
			}
			_isEventAdded = true;
			NetEvent.Instance.AddEventListener(1300, OnGetLobbyToMatchS2C);
			NetEvent.Instance.AddEventListener(1301, OnGetLobbyUpdateMatchStateS2C);
			NetEvent.Instance.AddEventListener(1302, OnGetLobbyCancelMatchS2C);
		}

		private void RemoveEventListener()
		{
			if (!_isEventAdded)
			{
				return;
			}
			_isEventAdded = false;
			NetEvent.Instance.RemoveEventListener(1300, OnGetLobbyToMatchS2C);
			NetEvent.Instance.RemoveEventListener(1301, OnGetLobbyUpdateMatchStateS2C);
			NetEvent.Instance.RemoveEventListener(1302, OnGetLobbyCancelMatchS2C);
		}

		private async void OnGetLobbyToMatchS2C(BufferEntity response)
		{
			var msg = ProtobufHelper.FromBytes<LobbyToMatchS2C>(response.Protocal);
			if (msg.Result == 0)
			{
				LobbyUIViewer.SetState(LobbyUIViewer.LobbyState.Matching);
			}
			else 
			{
				TipUIViewer.SetText("Can't matching");
				await TipUIViewer.Open();
			}
		}

		private void OnGetLobbyUpdateMatchStateS2C(BufferEntity response)
		{
			var msg = ProtobufHelper.FromBytes<LobbyUpdateMatchStateS2C>(response.Protocal);

			if (msg.Result == 0)
			{
				PlayerModel.Instance.RoomInfo = msg.RoomInfo;
				CloseUI();
				LobbyUIViewer.SetState(LobbyUIViewer.LobbyState.Entered);
			}
		}

		private void OnGetLobbyCancelMatchS2C(BufferEntity response)
		{
			var msg = ProtobufHelper.FromBytes<LobbyQuitMatchS2C>(response.Protocal);

			if (msg.Result == 0)
			{
				LobbyUIViewer.SetState(LobbyUIViewer.LobbyState.Idle);
			}
			else
			{
			}
		}
		#endregion private-method
	}
}