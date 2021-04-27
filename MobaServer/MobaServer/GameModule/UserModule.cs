using Moba.Utility;
using MobaServer.MySql;
using MobaServer.Net;
using MobaServer.Player;
using ProtoMsg;
using System;
using System.Collections.Generic;
using System.Text;
using BufferFactory = MobaServer.Net.BufferFactory;

namespace MobaServer.GameModule
{
	public class UserModule : GameModuleBase<UserModule>
	{
		#region public-method
		public override void AddListener()
		{
			NetEvent.Instance.AddEventListener(1000, OnGetUserRegisterC2S);
			NetEvent.Instance.AddEventListener(1001, OnGetUserLoginC2S);
		}

		public override void RemoveListener()
		{
			NetEvent.Instance.RemoveEventListener(1000, OnGetUserRegisterC2S);
			NetEvent.Instance.RemoveEventListener(1001, OnGetUserLoginC2S);
		}
		#endregion public-method

		#region private-method
		private void OnGetUserRegisterC2S(BufferEntity request)
		{
			var c2sMSG = ProtobufHelper.FromBytes<UserRegisterC2S>(request.Protocal);

			var sqlCMD = MySqlCMD.Where("Account", c2sMSG.UserInfo.Account);
			var s2cMSG = new UserRegisterS2C();
			if (DBUserInfo.Instance.Select(sqlCMD) != null)
			{
				MobaLogger.LogError("[UserModule.OnGetUserRegisterCSS] Account exist.");
				s2cMSG.Result = 3;
			}
			else 
			{
				var isSuccess = DBUserInfo.Instance.Insert(c2sMSG.UserInfo);
				if (isSuccess)
				{
					s2cMSG.Result = 0;
				}
				else
				{
					s2cMSG.Result = 4;
				}
			}

			BufferFactory.CreateAndSendPackage(request, s2cMSG);
		}

		private void OnGetUserLoginC2S(BufferEntity request)
		{
			var c2sMSG = ProtobufHelper.FromBytes<UserLoginC2S>(request.Protocal);

			var sqlCMD = MySqlCMD.Where("Account", c2sMSG.UserInfo.Account) +
				MySqlCMD.And("Password", c2sMSG.UserInfo.Password);

			var s2cMSG = new UserLoginS2C();
			var userInfo = DBUserInfo.Instance.Select(sqlCMD);
			if (userInfo != null)
			{
				s2cMSG.UserInfo = userInfo;
				s2cMSG.Result = 0;

				var playerEntity = new PlayerEntity();
				playerEntity.UserInfo = userInfo;
				playerEntity.Session = request.SessionId;

				var rolesInfo = DBRolesInfo.Instance.Select(MySqlCMD.Where("ID", c2sMSG.UserInfo.Account));
				if (rolesInfo != null) 
				{
					s2cMSG.RolesInfo = rolesInfo;
					playerEntity.RolesInfo = rolesInfo;
				}

				PlayerManager.Add(request.SessionId, s2cMSG.UserInfo.ID, playerEntity);
			}
			else
			{
				s2cMSG.Result = 2;
			}

			BufferFactory.CreateAndSendPackage(request, s2cMSG);
		}
		#endregion private-method
	}
}
