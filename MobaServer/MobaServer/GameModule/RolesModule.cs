using Moba.Utility;
using MobaServer.MySql;
using MobaServer.Net;
using MobaServer.Player;
using ProtoMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer.GameModule
{
	public class RolesModule : GameModuleBase<RolesModule>
	{
		#region public-method
		public override void AddListener()
		{
			NetEvent.Instance.AddEventListener(1201, OnGetRolesCreateC2S);
		}

		public override void RemoveListener()
		{
			NetEvent.Instance.RemoveEventListener(1201, OnGetRolesCreateC2S);
		}
		#endregion public-method

		#region private-method
		private void OnGetRolesCreateC2S(BufferEntity request)
		{
			var c2sMSG = ProtobufHelper.FromBytes<RolesCreateC2S>(request.Protocal);
			PlayerManager.TryGetPlayerEntityBySessionId(request.SessionId, out var player);

			var sqlCMD = MySqlCMD.Where("ID", player.UserInfo.ID);
			var selectResult = DBRolesInfo.Instance.Select(sqlCMD);

			var s2cMSG = new RolesCreateS2C();

			if (selectResult == null)
			{
				var rolesInfo = new RolesInfo();
				rolesInfo.NickName = c2sMSG.NickName;
				rolesInfo.ID = player.UserInfo.ID;
				rolesInfo.RolesID = player.UserInfo.ID;

				var isInsertSuccess = DBRolesInfo.Instance.Insert(rolesInfo);
				if (isInsertSuccess)
				{
					s2cMSG.Result = 0;
					s2cMSG.RolesInfo = rolesInfo;
					player.RolesInfo = rolesInfo;
				}
				else
				{
					s2cMSG.Result = 2;
					MobaLogger.LogError($"[RolesModule.OnGetRolesCreateC2S] Insert {c2sMSG.NickName} error.");
				}
			}
			else
			{
				s2cMSG.Result = 1;
			}
			BufferFactory.CreateAndSendPackage(request, s2cMSG);
		}
		#endregion private-method
	}
}
