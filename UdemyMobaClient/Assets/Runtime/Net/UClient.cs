﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Net
{
	public class UClient
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

		public USocket USocket
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
		public UClient(USocket uSocket, IPEndPoint endPoint, int sendSN, int handleSN, int sessionId, Action<BufferEntity> dispatchNetEvent) 
		{
			USocket = uSocket;
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
				SessionId = buffer.SessionId;
			}

			switch (buffer.MessageType) 
			{
				case 0:
					{
						if (_sendPackages.TryRemove(buffer.SN, out var entity)) 
						{
							Debug.Log($"[UClient.Handle] SN : {buffer.SN}") ;
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
			pacakge.Time = Utility.Utility.Now;
			_sendSN++;
			pacakge.SN = _sendSN;
			
			pacakge.Encode(false);
			if (SessionId != 0)
			{
				_sendPackages.TryAdd(_sendSN, pacakge);
			}
			else 
			{
			
			}
			
			USocket.Send(pacakge.Buffer);
			//_uSocket.Send(pacakge.Buffer, _endPoint);
		}
		#endregion public-method

		#region private-method
		private async void CheckTimeOut()
		{
			await Task.Delay(_timeOutMS);

			var disconnectTime = _timeOutMS * _disconnectCount;
			foreach (var package in _sendPackages.Values)
			{
				var dT = Utility.Utility.Now - package.Time;
				if (dT >= disconnectTime)
				{
					OnDisconnect();
					return;
				}
				if (dT >= (package.RetryCount + 1) * _timeOutMS)
				{
					package.RetryCount++;
					USocket.Send(package.Buffer);
					//_uSocket.Send(pacakge.Buffer, _endPoint);
				}
			}
			CheckTimeOut();
		}

		private void OnDisconnect()
		{
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