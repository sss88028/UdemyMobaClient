using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MobaServer.Net
{
	public class USocket
	{
		#region public-field
		public const string Ip = "127.0.0.1";
		public const int Port = 8899;

		public UdpClient Socket;
		#endregion public-field

		#region private-field
		private Action<BufferEntity> _dispatchNetEvent;
		private ConcurrentQueue<UdpReceiveResult> _awaitHandle = new ConcurrentQueue<UdpReceiveResult>();
		private ConcurrentDictionary<int, UClient> _clients = new ConcurrentDictionary<int, UClient>();

		private int _sessionId = 1000;
		private CancellationTokenSource _cancelToken = new CancellationTokenSource();
		#endregion private-field

		#region public-method
		public USocket(Action<BufferEntity> dispatchNetEvent) 
		{
			_dispatchNetEvent = dispatchNetEvent;

			Socket = new UdpClient(Port);
			Recevice();

			Task.Run(Handle, _cancelToken.Token);
		}

		public async void Send(byte[] data, IPEndPoint endPoint) 
		{
			if (Socket != null) 
			{
				try
				{
					var dataLength = data.Length;
					var length = await Socket.SendAsync(data, dataLength, endPoint);
					if (dataLength == length)
					{

					}
				}
				catch (Exception e)
				{
					Debug.LogError($"[USocket.Send] {e.Message}");
					Close();
				}
			}
		}

		public void SendAck(BufferEntity ackPackage, IPEndPoint endPoint)
		{
			Debug.Log($"[USocket.SendAck] target sessionId : {ackPackage.SessionId}");
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
		#endregion public-method

		#region private-method
		private async void Recevice()
		{
			if (Socket != null)
			{
				try
				{
					var result = await Socket.ReceiveAsync();
					Debug.Log("[USocket.Recevice] Get client message");
					_awaitHandle.Enqueue(result);
					Recevice();
				}
				catch (Exception e)
				{
					Debug.LogError($"[USocket.Recevice] {e.Message}");
					Close();
				}
			}
		}

		private void Handle()
		//private async Task Handle() 
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
						Debug.Log($"[USocket.Handle] Create client sessionId : {_sessionId}");
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

			Socket?.Close();
			Socket = null;

			_dispatchNetEvent = null;

		}
		#endregion private-method
	}
}
