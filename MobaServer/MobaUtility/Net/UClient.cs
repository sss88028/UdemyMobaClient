using Moba.Utility;
using Moba.Utility.Net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

<<<<<<< HEAD:MobaServer/MobaServer.Utility/Net/UClient.cs
namespace Moba.Utility
=======
namespace Moba.Utility.Net
>>>>>>> e621aef ([Server] Change server hierarchy):MobaServer/MobaUtility/Net/UClient.cs
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

<<<<<<< HEAD:MobaServer/MobaServer.Utility/Net/UClient.cs
		public USocket USocket
=======
		public ISocket Socket 
>>>>>>> e621aef ([Server] Change server hierarchy):MobaServer/MobaUtility/Net/UClient.cs
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
		public UClient(ISocket socket, IPEndPoint endPoint, int sendSN, int handleSN, int sessionId, Action<BufferEntity> dispatchNetEvent)
		{
			Socket = socket;
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
							MobaLogger.Log($"[UClient.Handle] get ack SN : {buffer.SN}");
						}
					}
					break;
				case 1:
					{
						var ack = new BufferEntity(buffer);
<<<<<<< HEAD:MobaServer/MobaServer.Utility/Net/UClient.cs
						USocket.SendAck(ack, EndPoint);
=======
						Socket.SendAck(ack, EndPoint);
>>>>>>> e621aef ([Server] Change server hierarchy):MobaServer/MobaUtility/Net/UClient.cs
						MobaLogger.Log($"[UClient.Handle] get SN : {buffer.SN}");
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
			Socket.Send(pacakge.Buffer, EndPoint);
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
					MobaLogger.Log($"[UClient.HandleLoginPackage] Disorder sn : {buffer.SN}");
				}
				return;
			}

			_handledSN = buffer.SN;
			MobaLogger.Log($"[UClient.HandleLoginPackage] handle Sn : {buffer.SN}");
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
					MobaLogger.LogError($"[UClient.CheckTimeOut] {SessionId} timeout");
<<<<<<< HEAD:MobaServer/MobaServer.Utility/Net/UClient.cs
					USocket.RemoveClient(SessionId);
=======
					Socket.OnClientDisconnect(this);
>>>>>>> e621aef ([Server] Change server hierarchy):MobaServer/MobaUtility/Net/UClient.cs
					return;
				}

				var dT = TimeHelper.Now - package.Time;
				if (dT >= (package.RetryCount + 1) * _timeOutMS)
				{
					package.RetryCount++;
					Socket.Send(package.Buffer, EndPoint);
				}
			}
			CheckTimeOut();
		}
		#endregion private-method

	}
}
