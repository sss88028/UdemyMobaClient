using Google.Protobuf;
using Moba.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer.Net
{
	public class BufferFactory
	{
		#region public-method
		public static BufferEntity CreateAndSendPackage(BufferEntity request, IMessage message)
		{
			var client = GameManager.USocket.GetClient(request.SessionId);
			var result = Moba.Utility.BufferFactory.CreateAndSendPackage(client, request.MessageId, message);
			return result;
		}
		#endregion public-method
	}
}
