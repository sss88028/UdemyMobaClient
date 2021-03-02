using CCTU.GameDevTools.MonoSingleton;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Net
{
    public static class USocket
	{
		#region private-field
		private const string _ip = "127.0.0.1";
		private const int _prot = 8899;
		private static IPEndPoint _server;

		private static UdpClient _updClient;

		private static CancellationTokenSource _cancelToken = new CancellationTokenSource();
		private static ConcurrentQueue<UdpReceiveResult> _awaitHanlde = new ConcurrentQueue<UdpReceiveResult>();
		private static ConcurrentQueue<BufferEntity> _bufferEntityQueue = new ConcurrentQueue<BufferEntity>();
		#endregion private-field

		#region public-property
		public static event Action<BufferEntity> DispatchNetEvent;

		internal static UClient ClientAgent 
		{
			get;
			private set;
		}
		#endregion public-property

		#region public-method
		internal static async void Send(byte[] data)
		{
			if (_updClient != null)
			{
				try
				{
					var length = await _updClient.SendAsync(data, data.Length, _ip, _prot);
				}
				catch (Exception e)
				{
					Debug.LogError($"[USocket.Send] {e.Message}");
				}
			}
		}

		internal static async void Send(byte[] data, IPEndPoint endPoint)
		{
			if (_updClient != null) 
			{
				try 
				{
					var length = await _updClient.SendAsync(data, data.Length, endPoint);
				}
				catch (Exception e)
				{
					Debug.LogError($"[USocket.Send] {e.Message}");
				}
			}
		}

		internal static void SendAck(BufferEntity bufferEntity) 
		{
			Send(bufferEntity.Buffer, _server);
		}

		public static void Close()
		{
			_cancelToken.Cancel();
			ClientAgent = null;

			_updClient?.Close();
			_updClient = null;

			DispatchNetEvent = null;
		}
		#endregion public-method

		#region private-method
		static USocket() 
		{
			_server = new IPEndPoint(IPAddress.Parse(_ip), _prot);

			_updClient = new UdpClient(0);
			ClientAgent = ClientAgent ?? new UClient(_server, 0, 0, 0, DispatchNetHandler);
			ReceiveTask();
			Task.Run(Handle, _cancelToken.Token);

			GameSystem.Instance.OnUpdateEvent += HandleBuffer;
		}

		private static async void ReceiveTask()
		{
			while (_updClient != null)
			{
				try
				{
					var result = await _updClient.ReceiveAsync();
					_awaitHanlde.Enqueue(result);
					Debug.Log("[USocket.Recevice] Get server message");
				}
				catch (Exception e)
				{
					Debug.LogError($"[USocket.Recevice] {e.Message}");
				}
			}
		}

		private static void Handle()
		{
			while (!_cancelToken.IsCancellationRequested)
			{
				if (_awaitHanlde.Count > 0)
				{
					if (_awaitHanlde.TryDequeue(out var data))
					{
						var buffer = new BufferEntity(data.RemoteEndPoint, data.Buffer);

						Debug.Log($"[USocket.Handle] MessageId : {buffer.MessageId}, SN : {buffer.SN}");
						ClientAgent.Handle(buffer);
					}
				}
			}
		}

		private static void DispatchNetHandler(BufferEntity bufferEntity)
		{
			_bufferEntityQueue.Enqueue(bufferEntity);
		}

		private static void HandleBuffer(float deltaTime)
		{
			while (_bufferEntityQueue.TryDequeue(out var bufferEntity))
			{
				DispatchNetEvent?.Invoke(bufferEntity);
			}
		}
		#endregion private-method
	}
}