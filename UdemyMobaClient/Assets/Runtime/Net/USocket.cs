using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Game.Net
{
    public class USocket
	{
		#region private-field
		private const string _ip = "127.0.0.1";
		private const int _prot = 8899;
		private static IPEndPoint _server;

		private UdpClient _updClient;

		private ConcurrentQueue<UdpReceiveResult> _awaitHanlde = new ConcurrentQueue<UdpReceiveResult>();
		#endregion private-field

		#region public-property
		public static UClient ClientAgent 
		{
			get;
			private set;
		}
		#endregion public-property

		#region public-method
		public USocket(Action<BufferEntity> dispatchNetEvent) 
		{
			_updClient = new UdpClient(0);
			ClientAgent = ClientAgent ?? new UClient(this, _server, 0, 0, 0, dispatchNetEvent);
			ReceiveTask();
		}

		public async void ReceiveTask() 
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

		public async void Send(byte[] data)
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

		public async void Send(byte[] data, IPEndPoint endPoint)
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

		public void SendAck(BufferEntity bufferEntity) 
		{
			Send(bufferEntity.Buffer, _server);
		}

		public void Handle() 
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

		public void Close()
		{
			ClientAgent = null;

			_updClient?.Close();
			_updClient = null;

		}
		#endregion public-method

		#region private-method
		static USocket() 
		{
			_server = new IPEndPoint(IPAddress.Parse(_ip), _prot);
		}		
		#endregion private-method
	}
}