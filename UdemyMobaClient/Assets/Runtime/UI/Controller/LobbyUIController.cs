using Game.Model;
using Game.Net;
using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class LobbyUIController : Singleton<LobbyUIController>
	{
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
			NetEvent.Instance.AddEventListener(1300, OnGetLobbyToMatchS2C);
			NetEvent.Instance.AddEventListener(1301, OnGetLobbyUpdateMatchStateS2C);
			NetEvent.Instance.AddEventListener(1302, OnGetLobbyCancelMatchS2C);
		}

		private void RemoveEventListener()
		{
			NetEvent.Instance.RemoveEventListener(1300, OnGetLobbyToMatchS2C);
			NetEvent.Instance.RemoveEventListener(1301, OnGetLobbyUpdateMatchStateS2C);
			NetEvent.Instance.RemoveEventListener(1302, OnGetLobbyCancelMatchS2C);
		}

		private void OnGetLobbyToMatchS2C(BufferEntity response)
		{
			var msg = ProtobufHelper.FromBytes<LobbyToMatchS2C>(response.Protocal);
			if (msg.Result == 0)
			{
				LobbyUIViewer.SetState(LobbyUIViewer.LobbyState.Matching);
			}
			else 
			{
				TipUIViewer.Open();
				TipUIViewer.SetText("Can't matching");
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