using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Net
{
    public class BufferFactory
    {
        private enum MessageType 
        { 
            Ack = 0,
            Login = 1,
        }

        #region public-method
        public static BufferEntity CreateAndSendPackage(int messageId, IMessage message) 
        {
            var data = ProtobufHelper.ToBytes(message);
            var buffer = new BufferEntity(USocket.ClientAgent.EndPoint, USocket.ClientAgent.SessionId, 0, 0, (int)MessageType.Login, messageId, data);
            USocket.ClientAgent.Send(buffer);
            return buffer;
        }
		#endregion public-method
	}
}