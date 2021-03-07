using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer.Net
{
	public class UClient
	{
		#region private-field
		private const int _timeOutMS = 150;
		private const int _disconnectCount = 10;

		private int _sendSN = 0;
		private int _handledSN = 0;
		private Action<BufferEntity> _dispatchNetEvent;

		private ConcurrentDictionary<int, BufferEntity> _sendPackages = new ConcurrentDictionary<int, BufferEntity>();
		private ConcurrentDictionary<int, BufferEntity> _awaitHandlePackages = new ConcurrentDictionary<int, BufferEntity>();

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

		public bool IsConnect
		{
			get;
			private set;
		} = true;
		#endregion public-property

		#region public-method
		public UClient(USocket uSocket, IPEndPoint endPoint, int sendSN, int handleSN, int sessionId, Action<BufferEntity> dispatchNetEvent)
		{
			USocket = uSocket;
			EndPoint = endPoint;
			_sendSN = sendSN;
			_handledSN = handleSN;
			SessionId = sessionId;

			_dispatchNetEvent = dispatchNetEvent;
			
			CheckTimeOut();
		}

		public void Handle(BufferEntity buffer)
		{
			switch (buffer.MessageType)
			{
				case 0:
					{
						if (_sendPackages.TryRemove(buffer.SN, out var entity))
						{
							Debug.Log($"[UClient.Handle] get ack SN : {buffer.SN}");
						}
					}
					break;
				case 1:
					{
						var ack = new BufferEntity(buffer);
						USocket.SendAck(ack, EndPoint);
						Debug.Log($"[UClient.Handle] get SN : {buffer.SN}");
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
			if (!IsConnect) 
			{
				return;
			}

			pacakge.Time = TimeHelper.Now;
			_sendSN++;
			pacakge.SN = _sendSN;

			pacakge.Encode(false);
			USocket.Send(pacakge.Buffer, EndPoint);
			if (SessionId != 0)
			{
				_sendPackages.TryAdd(_sendSN, pacakge);
			}
		}

		public void Close()
		{
			IsConnect = false;
		}
		#endregion public-method

		#region private-method
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
			Debug.Log($"[UClient.HandleLoginPackage] handle Sn : {buffer.SN}");
			_dispatchNetEvent?.Invoke(buffer);

			if (_awaitHandlePackages.TryRemove(_handledSN + 1, out var nextBuffer))
			{
				HandleLoginPackage(nextBuffer);
			}
		}

		private async void CheckTimeOut()
		{
			await Task.Delay(_timeOutMS);

			foreach (var package in _sendPackages.Values)
			{
				if (package.RetryCount >= _disconnectCount)
				{
					Debug.LogError($"[UClient.CheckTimeOut] {SessionId} timeout");
					USocket.RemoveClient(SessionId);
					return;
				}

				var dT = TimeHelper.Now - package.Time;
				if (dT >= (package.RetryCount + 1) * _timeOutMS)
				{
					package.RetryCount++;
					USocket.Send(package.Buffer, EndPoint);
				}
			}
			CheckTimeOut();
		}
		#endregion private-method

	}
}
