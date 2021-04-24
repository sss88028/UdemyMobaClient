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

		private UIHeroSkillDisplay[] _displays;
		#endregion private-field

		#region private-property
		private UIHeroSkillDisplay[] Displays 
		{
			get 
			{
				if (_displays == null) 
				{
					_displays = GetComponentsInChildren<UIHeroSkillDisplay>(true);
				}
				return _displays;
			}
		}
		#endregion private-property

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

		public static void SetHeroSkill(int rolesId, int gridId, int skillId)
		{
			_instance?.SetHeroSkillInternal(rolesId, gridId, skillId);
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

		private void SetHeroSkillInternal(int rolesId, int gridId, int skillId)
		{
			_leftTemList.SetHeroSkill(rolesId, gridId, skillId);
			_rightTemList.SetHeroSkill(rolesId, gridId, skillId);

			if (RoomUIController.Instance.IsSelfRole(rolesId))
			{
				foreach (var display in Displays)
				{
					if (display.GridId == gridId)
					{
						display.SetSkill(skillId);
						_uIRoleSkillInfo.gameObject.SetActive(false);
						break;
					}
				}
			}
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
			foreach (var button in Displays)
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