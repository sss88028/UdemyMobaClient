using ProtoMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer.Player
{
	public class PlayerEntity
	{
		#region public-field
		public int Session;
		public UserInfo UserInfo;
		public RolesInfo RolesInfo;
		#endregion public-field

		#region public-method
		public void Destroy() 
		{
			Debug.Log("[PlayerEntity.Destroy]");
		}
		#endregion public-method
	}
}
