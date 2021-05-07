using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;

namespace Moba.Utility.Net
{
	public class BufferFactory
    {
        private enum MessageType
        {
            Ack = 0,
            Login = 1,
        }

        #region public-method
        public static BufferEntity CreateAndSendPackage(UClient client, int messageId, IMessage message)
        {
            if (client.IsConnect) 
            {
				MobaLogger.Log($"MessageId : {messageId} \n Package {JsonHelper.SerializeObject(message)}");

                var bufferEntity = new BufferEntity(client.EndPoint, client.SessionId, 0, 0, 
                    (int)MessageType.Login, messageId, ProtobufHelper.ToBytes(message));

                client.Send(bufferEntity);
                return bufferEntity;
            }
            return null;
        }

        public static BufferEntity CreateAndSendPackage(ISocket socket, BufferEntity request, IMessage message)
        {
            var client = socket.GetClient(request.SessionId);
            return CreateAndSendPackage(client, request.MessageId, message);
        }
        #endregion public-method
    }
}
