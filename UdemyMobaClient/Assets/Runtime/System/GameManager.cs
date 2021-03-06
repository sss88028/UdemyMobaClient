using CCTU.GameDevTools.MonoSingleton;
using Game.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.MainSystem
{
	public class GameManager : MonoSingleton<GameManager>
	{
		#region private-field
		private static USocket _uSocket;
		#endregion private-field

		#region MonoBehaviour-method
		private void Start()
		{
			_uSocket = new USocket(DispatcNetHandler);
		}

		private void Update()
		{
			_uSocket?.Handle();
		}
		#endregion MonoBehaviour-method

		#region private-method
		private void DispatcNetHandler(BufferEntity buffer)
		{
			NetEvent.Instance.Dispatch(buffer.MessageId, buffer);
		}
		#endregion private-method
	}
}
