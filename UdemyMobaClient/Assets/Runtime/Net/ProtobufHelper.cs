using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;


namespace Game.Net
{
	public static class ProtobufHelper
	{
		#region public-method
		public static byte[] ToBytes(object message)
		{
			return ((Google.Protobuf.IMessage)message).ToByteArray();
		}

		public static void ToStream(object message, MemoryStream stream)
		{
			((Google.Protobuf.IMessage)message).WriteTo(stream);
		}

		public static object FromBytes(Type type, byte[] bytes, int index, int count)
		{
			var message = Activator.CreateInstance(type);

			((Google.Protobuf.IMessage)message).MergeFrom(bytes, index, count);
			var iSupportInitialize = message as ISupportInitialize;
			if (iSupportInitialize == null)
			{
				return message;
			}
			iSupportInitialize.EndInit();
			return message;
		}

		public static object FromBytes(object message, byte[] bytes, int index, int count)
		{
			((Google.Protobuf.IMessage)message).MergeFrom(bytes, index, count);
			var iSupportInitialize = message as ISupportInitialize;
			if (iSupportInitialize == null)
			{
				return message;
			}
			iSupportInitialize.EndInit();
			return message;
		}

		public static T FromBytes<T>(byte[] bytes) where T : Google.Protobuf.IMessage
		{
			var message = Activator.CreateInstance(typeof(T));

			var length = bytes.Length;
			
			((Google.Protobuf.IMessage)message).MergeFrom(bytes, 0, length);
			var iSupportInitialize = message as ISupportInitialize;
			if (iSupportInitialize == null)
			{
				return (T)message;
			}
			iSupportInitialize.EndInit();
			return (T)message;
		}

		public static object FromStream(Type type, MemoryStream stream)
		{
			var message = Activator.CreateInstance(type);
			((Google.Protobuf.IMessage)message).MergeFrom(stream.GetBuffer(), (int)stream.Position, (int)stream.Length);
			var iSupportInitialize = message as ISupportInitialize;
			if (iSupportInitialize == null)
			{
				return message;
			}
			iSupportInitialize.EndInit();
			return message;
		}

		public static object FromStream(object message, MemoryStream stream)
		{
			((Google.Protobuf.IMessage)message).MergeFrom(stream.GetBuffer(), (int)stream.Position, (int)stream.Length);
			var iSupportInitialize = message as ISupportInitialize;
			if (iSupportInitialize == null)
			{
				return message;
			}
			iSupportInitialize.EndInit();
			return message;
		}
		#endregion public-method
	}
}