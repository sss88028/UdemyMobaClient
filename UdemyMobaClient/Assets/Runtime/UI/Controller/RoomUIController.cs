using Game.Model;
using Game.Net;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
	public class RoomUIController : Singleton<RoomUIController>
	{
		#region private-field
		private bool _isLockHero;
		private int _heroId;
		#endregion private-field

		#region public-method
		public RoomUIController()
		{
		}

		public void Load()
		{
			LoginUIViewer.LoadScene();
		}

		public void OpenUI()
		{
			AddEventListener();
			RoomUIViewer.Open();
			RoomUIViewer.CreateTeamInfo(RolesUIController.Instance.GetRoomInfo());
		}

		public void CloseUI()
		{
			RemoveEventListener();
			RoomUIViewer.Close();
		}

		public int GetTeamID(int rolesId)
		{
			foreach (var role in PlayerModel.Instance.RoomInfo.TeamA)
			{
				if (role.RolesID == rolesId)
				{
					return 0;
				}
			}
			return 1;
		}

		public bool IsSelfRole(int roled) 
		{
			return PlayerModel.Instance.RolesInfo.RolesID == roled;
		}
		#endregion public-method

		#region private-method
		private void AddEventListener()
		{
			RoomUIViewer.OnSelectHeroEvent += OnSelectHeroHandler;
			RoomUIViewer.OnClickLockEvent += OnClickLockHandler;
			RoomUIViewer.OnSetSkillEvent += OnSetSkillHandler;

			RoomUIViewer.OnGetRoleSkillEvent += OnGetRoleSkillHandler;

			NetEvent.Instance.AddEventListener(1400, OnGetRoomSelectHeroS2C);
			NetEvent.Instance.AddEventListener(1401, OnGetRoomSelectHeroSkillS2C);
			NetEvent.Instance.AddEventListener(1403, OnGetRoomCloseS2C);
			NetEvent.Instance.AddEventListener(1404, OnGetRoomSendMsgS2C);
			NetEvent.Instance.AddEventListener(1405, OnGetRoomLockHeroS2C);
			NetEvent.Instance.AddEventListener(1406, OnGetRoomLoadingProgressS2C);
			NetEvent.Instance.AddEventListener(1407, OnGetRoomToBattleS2C);
		}

		private void RemoveEventListener()
		{
			RoomUIViewer.OnSelectHeroEvent -= OnSelectHeroHandler;
			RoomUIViewer.OnClickLockEvent -= OnClickLockHandler;
			RoomUIViewer.OnSetSkillEvent -= OnSetSkillHandler;

			RoomUIViewer.OnGetRoleSkillEvent -= OnGetRoleSkillHandler;

			NetEvent.Instance.RemoveEventListener(1400, OnGetRoomSelectHeroS2C);
			NetEvent.Instance.RemoveEventListener(1401, OnGetRoomSelectHeroSkillS2C);
			NetEvent.Instance.RemoveEventListener(1403, OnGetRoomCloseS2C);
			NetEvent.Instance.RemoveEventListener(1404, OnGetRoomSendMsgS2C);
			NetEvent.Instance.RemoveEventListener(1405, OnGetRoomLockHeroS2C);
			NetEvent.Instance.RemoveEventListener(1406, OnGetRoomLoadingProgressS2C);
			NetEvent.Instance.RemoveEventListener(1407, OnGetRoomToBattleS2C);
		}

		private void OnGetRoomSelectHeroS2C(BufferEntity response)
		{
			var msg = ProtobufHelper.FromBytes<RoomSelectHeroS2C>(response.Protocal);

			RoomUIViewer.SetHeroInfo(msg.RolesID, msg.HeroID);
			if (IsSelfRole(msg.RolesID)) 
			{
				_heroId = msg.HeroID;
			}
		}

		private void OnGetRoomSelectHeroSkillS2C(BufferEntity response)
		{
			var msg = ProtobufHelper.FromBytes<RoomSelectHeroSkillS2C>(response.Protocal);
			RoomUIViewer.SetHeroSkill(msg.RolesID, msg.GridID, msg.SkillID);

			if (IsSelfRole(msg.RolesID))
			{

			}
		}

		private void OnGetRoomCloseS2C(BufferEntity response)
		{
			var msg = ProtobufHelper.FromBytes<RoomCloseS2C>(response.Protocal);
		}

		private void OnGetRoomSendMsgS2C(BufferEntity response)
		{
			var msg = ProtobufHelper.FromBytes<RoomSendMsgS2C>(response.Protocal);
		}

		private void OnGetRoomLockHeroS2C(BufferEntity response)
		{
			var msg = ProtobufHelper.FromBytes<RoomLockHeroS2C>(response.Protocal);
		}

		private void OnGetRoomLoadingProgressS2C(BufferEntity response)
		{
			var msg = ProtobufHelper.FromBytes<RoomLoadProgressS2C>(response.Protocal);
		}

		private void OnGetRoomToBattleS2C(BufferEntity response)
		{
			var msg = ProtobufHelper.FromBytes<RoomToBattleS2C>(response.Protocal);
		}

		private void OnSelectHeroHandler(int heroId) 
		{
			BufferFactory.CreateAndSendPackage(1400, new RoomSelectHeroC2S()
			{
				HeroID = heroId,
			});
			_heroId = heroId;
		}

		private void OnClickLockHandler()
		{
			if (_isLockHero) 
			{
				return;
			}

			BufferFactory.CreateAndSendPackage(1405, new RoomLockHeroC2S()
			{
				HeroID = _heroId,
			});
		}

		private void OnSetSkillHandler(int gridId, int skillId)
		{
			BufferFactory.CreateAndSendPackage(1401, new RoomSelectHeroSkillC2S()
			{
				SkillID = skillId,
				GridID = gridId,
			});
		}

		private int OnGetRoleSkillHandler(int gridId) 
		{
			return 0;
		}
		#endregion private-method
	}
}