using Moba.Utility;
using Moba.Utility.Net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Moba.Utility
{
	public class ServerSocket : ISocket
	{
		#region public-field
		public const string Ip = "127.0.0.1";
		public const int Port = 8899;
		#endregion public-field

		#region private-field
		private UdpClient _socket;
		private Action<BufferEntity> _dispatchNetEvent;
		private ConcurrentQueue<UdpReceiveResult> _awaitHandle = new ConcurrentQueue<UdpReceiveResult>();
		private ConcurrentDictionary<int, UClient> _clients = new ConcurrentDictionary<int, UClient>();

		private int _sessionId = 1000;
		private CancellationTokenSource _cancelToken = new CancellationTokenSource();
		#endregion private-field

		#region public-method
		public ServerSocket(Action<BufferEntity> dispatchNetEvent)
		{
			_dispatchNetEvent = dispatchNetEvent;

			_socket = new UdpClient(Port);
			Recevice();

			Task.Run((Action)Handle, _cancelToken.Token);
		}

		public async void Send(byte[] data, IPEndPoint endPoint)
		{
			if (_socket != null)
			{
				try
				{
					var dataLength = data.Length;
					var length = await _socket.SendAsync(data, dataLength, endPoint);
					if (dataLength == length)
					{

					}
				}
				catch (Exception e)
				{
					MobaLogger.LogError($"[USocket.Send] {e.Message}");
					Close();
				}
			}
		}

		public void SendAck(BufferEntity ackPackage, IPEndPoint endPoint)
		{
			MobaLogger.Log($"[USocket.SendAck] target sessionId : {ackPackage.SessionId}");
			Send(ackPackage.Buffer, endPoint);
		}

		public void RemoveClient(int sessionId)
		{
			if (_clients.TryRemove(sessionId, out var client))
			{
				client.Close();
				client = null;
			}
		}

		public UClient GetClient(int sessionId)
		{
			if (!_clients.TryGetValue(sessionId, out var client))
			{
				return null;
			}
			return client;
		}

		public void OnClientDisconnect(UClient client)
		{
			_clients.TryRemove(client.SessionId, out var _);
			client.Close();
		}
		#endregion public-method

		#region private-method
		private async void Recevice()
		{
			if (_socket != null)
			{
				try
				{
<<<<<<< HEAD:MobaServer/MobaServer.Utility/Net/USocket.cs
					var result = await Socket.ReceiveAsync();
=======
					var result = await _socket.ReceiveAsync();
>>>>>>> e621aef ([Server] Change server hierarchy):MobaServer/MobaServer/Net/ServerSocket.cs
					MobaLogger.Log("[USocket.Recevice] Get client message");
					_awaitHandle.Enqueue(result);
					Recevice();
				}
				catch (Exception e)
				{
					MobaLogger.LogError($"[USocket.Recevice] {e.Message}");
					Close();
				}
			}
		}

		private void Handle()
		{
			while (!_cancelToken.IsCancellationRequested)
			{
				if (_awaitHandle.Count <= 0)
				{
					continue;
				}
				if (_awaitHandle.TryDequeue(out var data))
				{
					var bufferEntity = new BufferEntity(data.RemoteEndPoint, data.Buffer);
					//Not connect yet,
					if (bufferEntity.SessionId == 0)
					{
						_sessionId += 1;
						bufferEntity.SessionId = _sessionId;

						CreateUClient(bufferEntity);
						MobaLogger.Log($"[USocket.Handle] Create client sessionId : {_sessionId}");
					}

					if (_clients.TryGetValue(bufferEntity.SessionId, out var targetClient))
					{
						targetClient.Handle(bufferEntity);
					}
				}
			}
		}

		private void CreateUClient(BufferEntity buffer)
		{
			if (!_clients.TryGetValue(buffer.SessionId, out var client))
			{
				client = new UClient(this, buffer.EndPoint, 0, 0, buffer.SessionId, _dispatchNetEvent);

				_clients.TryAdd(buffer.SessionId, client);
			}
		}

		private void Close()
		{
			_cancelToken.Cancel();

			foreach (var client in _clients.Values)
			{
				client.Close();
			}

			_socket?.Close();
			_socket = null;

			_dispatchNetEvent = null;
		}
		#endregion private-method
	}

}
