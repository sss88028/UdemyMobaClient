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
		private static string _sceneName = "RoomUI";
		private static bool _isOpen = false;

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
		public static void Open()
		{
			_isOpen = true;
			if (_instance == null)
			{
				LoadScene(_sceneName);
			}
			else
			{
				_instance.ActiveInternal();
			}
		}

		public static void Close()
		{
			_isOpen = false;

			_instance?.ActiveInternal();
		}

		public static void CreateTeamInfo(RoomInfo roomInfo) 
		{
			_teamARolesInfos = roomInfo.TeamA;
			_teamBRolesInfos = roomInfo.TeamB;

			_instance?.CreateTeamInternal();
		}

		public static void SetHeroInfo(int rolesId, int heroId)
		{
			if (_instance != null) 
			{
				_instance._leftTemList.SetHeroInfo(rolesId, heroId);
				_instance._rightTemList.SetHeroInfo(rolesId, heroId);
			}
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
			ActiveInternal();
			CreateTeamInternal();
		}
		#endregion MonoBehaviour-method

		#region private-method
		private void ActiveInternal()
		{
			gameObject.SetActive(_isOpen);
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