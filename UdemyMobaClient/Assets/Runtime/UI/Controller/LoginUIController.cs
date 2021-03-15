﻿using Game.Model;
using Game.Net;
using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
	public class LoginUIController : Singleton<LoginUIController>
	{
		#region public-method
		public LoginUIController()
		{
			NetEvent.Instance.AddEventListener(1000, OnGetUserRegisterS2C);
			NetEvent.Instance.AddEventListener(1001, OnGetUserLoginS2C);
		}

		public void Load() 
		{
			LoginUIViewer.LoadScene();
		}

		public void OpenUI() 
		{
			LoginUIViewer.Open();
		}

		public void CloseUI() 
		{
			LoginUIViewer.Close();
		}

		public void SaveRolesInfo(RolesInfo rolesInfo) 
		{
			PlayerModel.Instance.RolesInfo = rolesInfo;
		}
		#endregion public-method

		#region private-method
		private void AddEventListener()
		{
			NetEvent.Instance.AddEventListener(1000, OnGetUserRegisterS2C);
			NetEvent.Instance.AddEventListener(1001, OnGetUserLoginS2C);
		}

		private void RemoveEventListener()
		{
			NetEvent.Instance.RemoveEventListener(1000, OnGetUserRegisterS2C);
			NetEvent.Instance.RemoveEventListener(1001, OnGetUserLoginS2C);
		}

		private void OnGetUserRegisterS2C(BufferEntity buffer) 
		{
			var s2cMSG = ProtobufHelper.FromBytes<UserRegisterS2C>(buffer.Protocal);
			Debug.Log($"[LoginUIController.OnGetUserLoginS2C] Result {s2cMSG.Result}");
			switch (s2cMSG.Result) 
			{
				//success
				case 0:
					{
						TipUIViewer.SetText("Register Success!!");
						TipUIViewer.Open(GoToCreateRole, GoToCreateRole);
					}
					break;
				//Already register
				case 3:
					{
						TipUIViewer.SetText("Account exist!!");
						TipUIViewer.Open();
					}
					break;
			}
		}

		private void OnGetUserLoginS2C(BufferEntity buffer)
		{
			var s2cMSG = ProtobufHelper.FromBytes<UserLoginS2C>(buffer.Protocal);
			Debug.Log($"[LoginUIController.OnGetUserLoginS2C] Result {s2cMSG.Result}");
			switch (s2cMSG.Result)
			{
				//success
				case 0:
					{
						CloseUI();

						if (s2cMSG.RolesInfo != null)
						{
							SaveRolesInfo(s2cMSG.RolesInfo);
							GoToLobby();
						}
						else 
						{
							GoToCreateRole();
						}
					}
					break;
				//No match
				case 2:
					{
						TipUIViewer.SetText("Account not exist!!");
						TipUIViewer.Open();
					}
					break;
			}
		}

		private void GoToCreateRole()
		{
			CloseUI();
			GameFlow.Instance.Flow.SetTrigger("Next");
		}

		private void GoToLobby()
		{
			CloseUI();
			GameFlow.Instance.Flow.SetBool("HasRole", true);
			GameFlow.Instance.Flow.SetTrigger("Next");
		}
		#endregion private-method
	}
}