using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
	public class RoomUIViewer : BaseUIViewer<RoomUIViewer>
	{
		#region public-field
		public static event Action<int> OnSelectHeroEvent;
		public static event Action OnClickLockEvent;
		public static event Action<int, int> OnSetSkillEvent;
		public static event Func<int, int> OnGetRoleSkillEvent;
		#endregion public-field

		#region private-field
		private static string _sceneName = "UI/RoomUI.unity";
		private static IEnumerable<RolesInfo> _teamARolesInfos;
		private static IEnumerable<RolesInfo> _teamBRolesInfos;

		[SerializeField]
		private UITeamList _leftTemList;
		[SerializeField]
		private UITeamList _rightTemList;

		[SerializeField]
		private UIHeroSkillInfo _uIRoleSkillInfo;
		#endregion private-field

		#region public-method
		public static async void Open()
		{
			var instance = await GetInstance(_sceneName);
			instance.ActiveInternal(true);
		}

		public static async void Close()
		{
			var instance = await GetInstance(_sceneName);
			instance.ActiveInternal(false);
		}

		public static async void CreateTeamInfo(RoomInfo roomInfo) 
		{
			_teamARolesInfos = roomInfo.TeamA;
			_teamBRolesInfos = roomInfo.TeamB;
			(await GetInstance(_sceneName)).CreateTeamInternal();
		}

		public static async void SetHeroInfo(int rolesId, int heroId)
		{
			var instance = await GetInstance(_sceneName);
			instance._leftTemList.SetHeroInfo(rolesId, heroId);
			instance._rightTemList.SetHeroInfo(rolesId, heroId);
		}

		public void OnClickLock() 
		{
			OnClickLockEvent?.Invoke();
		}

		public void OnSetSkill(int gridId, int skillId) 
		{
			OnSetSkillEvent?.Invoke(gridId, skillId);
		}
		#endregion public-method

		#region MonoBehaviour-method
		private void Start()
		{
			SetUpEvent();
			CreateTeamInternal();
		}
		#endregion MonoBehaviour-method

		#region private-method
		private void ActiveInternal(bool isOpen)
		{
			gameObject.SetActive(isOpen);
		}

		private void CreateTeamInternal() 
		{
			_leftTemList?.CreateRoles(_teamARolesInfos);
			_rightTemList?.CreateRoles(_teamBRolesInfos);
		}

		private void SetUpEvent()
		{
			SetUpHeroSelectButtonEvent();
			SetUpRolesSkillDisplay();
		}

		private void SetUpHeroSelectButtonEvent() 
		{
			var list = GetComponentsInChildren<UISelectHero>(true);
			foreach (var button in list) 
			{
				button.OnSelectEvent += OnSelectHeroHandler;
			}
		}

		private void OnSelectHeroHandler(int heroId)
		{
			OnSelectHeroEvent?.Invoke(heroId);
		}

		private void SetUpRolesSkillDisplay() 
		{
			var list = GetComponentsInChildren<UIHeroSkillDisplay>(true);
			foreach (var button in list)
			{
				button.OnSelectGrid += OnSelectGridHandler;
			}
		}

		private void OnSelectGridHandler(int gridId) 
		{
			var skillId = OnGetRoleSkillEvent(gridId);

			_uIRoleSkillInfo?.ShowSelectInfo(gridId, skillId);
		}
		#endregion private-method
	}
}