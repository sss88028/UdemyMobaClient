using Moba.Utility.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Moba.Utility.Net
{
	public interface ISocket
	{
		void Send(byte[] data, IPEndPoint endPoint);
		void SendAck(BufferEntity ackPackage, IPEndPoint endPoint);
		void OnClientDisconnect(UClient client);
		UClient GetClient(int sessionId);
	}
}
