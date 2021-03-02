using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Net
{
	internal class UClient
	{
		#region private-field
		private int _sendSN = 0;
		private int _handledSN = 0;
		private Action<BufferEntity> _dispatchNetEvent;

		private ConcurrentDictionary<int, BufferEntity> _sendPackages = new ConcurrentDictionary<int, BufferEntity>();
		private ConcurrentDictionary<int, BufferEntity> _awaitHandlePackages = new ConcurrentDictionary<int, BufferEntity>();

		private const int _timeOutMS = 150;
		private const int _disconnectCount = 10;
		#endregion private-field

		#region public-property
		public IPEndPoint EndPoint
		{
			get;
			private set;
		}

		public int SessionId
		{
			get;
			private set;
		}
		#endregion public-property

		#region public-method
		public UClient(IPEndPoint endPoint, int sendSN, int handleSN, int sessionId, Action<BufferEntity> dispatchNetEvent) 
		{
			EndPoint = endPoint;
			SessionId = sessionId;
			_sendSN = sendSN;
			_handledSN = handleSN;
			_dispatchNetEvent = dispatchNetEvent;

			CheckTimeOut();
		}

		public void Handle(BufferEntity buffer) 
		{
			if (SessionId == 0 && buffer.SessionId != 0)
			{
				Debug.Log($"[UClient.Handle] Get SessionId {buffer.SessionId}");
				SessionId = buffer.SessionId;
			}

			switch (buffer.MessageType) 
			{
				case 0:
					{
						if (_sendPackages.TryRemove(buffer.SN, out var entity)) 
						{
							Debug.Log($"[UClient.Handle] get ack SN : {buffer.SN}") ;
						}
					}
					break;
				case 1:
					{
						var ack = new BufferEntity(buffer);
						USocket.SendAck(ack);
						HandleLoginPackage(buffer);
					}
					break;
				default: 
					{
					}
					break;
			}
		}

		public void Send(BufferEntity pacakge) 
		{
			pacakge.Time = TimeHelper.Now;
			_sendSN++;
			pacakge.SN = _sendSN;
			
			pacakge.Encode(false);
			if (SessionId != 0)
			{
				_sendPackages.TryAdd(_sendSN, pacakge);
			}

			USocket.Send(pacakge.Buffer);
			//_uSocket.Send(pacakge.Buffer, _endPoint);
		}
		#endregion public-method

		#region private-method
		private async void CheckTimeOut()
		{
			await Task.Delay(_timeOutMS);

			foreach (var package in _sendPackages.Values)
			{
				if (package.RetryCount >= _disconnectCount)
				{
					OnDisconnect();
					return;
				}

				var dT = TimeHelper.Now - package.Time;
				if (dT >= (package.RetryCount + 1) * _timeOutMS)
				{
					Debug.Log($"[UClient.CheckTimeOut] Retry, Count : {package.RetryCount}");
					package.RetryCount++;
					USocket.Send(package.Buffer);
					//_uSocket.Send(pacakge.Buffer, _endPoint);
				}
			}
			CheckTimeOut();
		}

		private void OnDisconnect()
		{
			Debug.Log("[UClient.OnDisconnect]");
			_dispatchNetEvent = null;
			USocket.Close();
		}

		private void HandleLoginPackage(BufferEntity buffer) 
		{
			if (buffer.SN <= _handledSN) 
			{
				return;
			}

			if (buffer.SN - _handledSN > 1) 
			{
				if (_awaitHandlePackages.TryAdd(buffer.SN, buffer))
				{
					Debug.Log($"[UClient.HandleLoginPackage] Disorder sn : {buffer.SN}");
				}
				return;
			}

			_handledSN = buffer.SN;
			_dispatchNetEvent?.Invoke(buffer);

			if (_awaitHandlePackages.TryRemove(_handledSN + 1, out var nextBuffer)) 
			{
				HandleLoginPackage(nextBuffer);
			}
		}
		#endregion private-method
	}
}