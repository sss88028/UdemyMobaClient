using Google.Protobuf;
using Moba.Utility.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Net
{
    public class BufferFactory
    {
        #region public-method
        public static BufferEntity CreateAndSendPackage(int messageId, IMessage message) 
        {
			return Moba.Utility.Net.BufferFactory.CreateAndSendPackage(ClientSocket.ClientAgent, messageId, message);
        }
		#endregion public-method
	}
}