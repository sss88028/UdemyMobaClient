using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;

namespace MobaServer.Net
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
                Debug.Log(messageId, message);

                var bufferEntity = new BufferEntity(client.EndPoint, client.SessionId, 0, 0, 
                    (int)MessageType.Login, messageId, ProtobufHelper.ToBytes(message));

                client.Send(bufferEntity);
                return bufferEntity;
            }
            return null;
        }

        public static BufferEntity CreateAndSendPackage(BufferEntity request, IMessage message)
        {
            UClient client = GameManager.USocket.GetClient(request.SessionId);
            return CreateAndSendPackage(client, request.MessageId, message);
        }
        #endregion public-method
    }
}
