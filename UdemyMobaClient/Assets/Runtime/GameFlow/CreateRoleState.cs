using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.State
{
    public class CreateRoleState : StateMachineBehaviour
	{
		#region public-method
		public override async void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			RolesUIController.Instance.OpenUI();
		}
		#endregion public-method
	}
}