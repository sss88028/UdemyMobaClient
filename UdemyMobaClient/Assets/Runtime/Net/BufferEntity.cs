using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Game.Net
{
	public class BufferEntity
	{
		#region public-field
		public int RetryCount = 0;
		#endregion public-field

		#region private-field
		private IPEndPoint _endPoint;

		private byte[] _protocol;
		#endregion private-field

		#region public-property
		public int ProtocolSize
		{
			get;
			private set;
		}

		public int SessionId
		{
			get;
			private set;
		}

		public int SN
		{
			get;
			set;
		}

		public int ModuleId
		{
			get;
			private set;
		}

		public long Time
		{
			get;
			set;
		}

		public int MessageType
		{
			get;
			private set;
		}

		public int MessageId
		{
			get;
			private set;
		}

		public byte[] Buffer
		{
			get;
			private set;
		}
		#endregion public-property

		#region private-field
		//4 * 6 + 8 = 32
		private const int _defaultSize = sizeof(int) * 6 + sizeof(long);
		#endregion private-field

		#region public-method
		public BufferEntity(IPEndPoint endPoint, int seesion, int sId, int moudleId, int messageType, int messageId, byte[] protocal) 
		{
			ProtocolSize = protocal.Length;
			SessionId = seesion;
			SN = sId;
			ModuleId = moudleId;
			MessageType = messageType;
			MessageId = messageId;
			_protocol = protocal;
		}

		public BufferEntity(IPEndPoint endPoint, byte[] buffer) 
		{
			_endPoint = endPoint;
			Buffer = buffer;
			Decode();
		}

		public BufferEntity(BufferEntity buffer) 
		{
			ProtocolSize = 0;
			_endPoint = buffer._endPoint;
			SessionId = buffer.SessionId;
			SN = buffer.SN;
			ModuleId = buffer.ModuleId;
			Time = 0;
			MessageType = 0;
			MessageId = buffer.MessageId;

			Buffer = Encode(true);
		}

		public byte[] Encode(bool isAck)
		{
			var dataSize = isAck ? _defaultSize : _defaultSize + ProtocolSize;
			var data = new byte[dataSize];

			var size = BitConverter.GetBytes(ProtocolSize);
			var session = BitConverter.GetBytes(SessionId);
			var sid = BitConverter.GetBytes(SN);
			var moduleId = BitConverter.GetBytes(ModuleId);
			var time = BitConverter.GetBytes(Time);
			var messageType = BitConverter.GetBytes(MessageType);
			var messageId = BitConverter.GetBytes(MessageId);

			Array.Copy(size, 0, data, 0, 4);
			Array.Copy(session, 0, data, 4, 4);
			Array.Copy(sid, 0, data, 8, 4);
			Array.Copy(moduleId, 0, data, 12, 4);
			Array.Copy(time, 0, data, 16, 8);
			Array.Copy(messageType, 0, data, 24, 4);
			Array.Copy(messageId, 0, data, 28, 4);
			if (!isAck)
			{
				Array.Copy(_protocol, 0, data, 32, _protocol.Length);
			}

			Buffer = data;

			return data;
		}

		[Obsolete("Slower, more GC")]
		public byte[] Encoder(bool isAck)
		{
			byte[] data = null;

			using (var ms = new MemoryStream())
			{
				using (var writer = new BinaryWriter(ms))
				{
					writer.Write(ProtocolSize);
					writer.Write(SessionId);
					writer.Write(SN);
					writer.Write(ModuleId);
					writer.Write(Time);
					writer.Write(MessageType);
					writer.Write(MessageId);
					if (!isAck)
					{
						writer.Write(_protocol);
					}
				}
				data = ms.ToArray();
			}

			return data;
		}
		#endregion public-method

		#region private-method
		private void Decode()
		{
			var startIndex = 0;
			ProtocolSize = BitConverter.ToInt32(Buffer, startIndex);

			startIndex += 4;
			SessionId = BitConverter.ToInt32(Buffer, startIndex);

			startIndex += 4;
			SN = BitConverter.ToInt32(Buffer, startIndex);

			startIndex += 4;
			ModuleId = BitConverter.ToInt32(Buffer, startIndex);

			startIndex += 4;
			Time = BitConverter.ToInt64(Buffer, startIndex);

			startIndex += 8;
			MessageType = BitConverter.ToInt32(Buffer, startIndex);

			startIndex += 4;
			MessageId = BitConverter.ToInt32(Buffer, startIndex);

			var isAck = MessageType == 0;
			if (!isAck) 
			{
				_protocol = new byte[ProtocolSize];
				startIndex += 4;
				Array.Copy(Buffer, startIndex, _protocol, 0, ProtocolSize);
			}
		}

		[Obsolete("Slower, more GC")]
		private void Decode2()
		{
			using (var ms = new MemoryStream(Buffer))
			{
				using (var reader = new BinaryReader(ms))
				{
					ProtocolSize = reader.ReadInt32();
					SessionId = reader.ReadInt32();
					SN = reader.ReadInt32();
					ModuleId = reader.ReadInt32();
					Time = reader.ReadInt64();
					MessageType = reader.ReadInt32();
					MessageId = reader.ReadInt32();

					var isAck = MessageType == 0;
					if (!isAck)
					{
						_protocol = reader.ReadBytes(ProtocolSize);
					}
				}
			}
		}
		#endregion private-method
	}
}