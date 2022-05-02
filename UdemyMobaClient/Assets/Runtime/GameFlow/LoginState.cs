using CCTU.UIFramework;
using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.State
{
	public class LoginState : StateMachineBehaviour
	{
		#region public-method
		public override async void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);

			await UIManager.Instance.TriggerUIEvent(new LoginUIShowEvent());
			await UIManager.Instance.TriggerUIEvent(new LoginUIGetInputEvent());
		}
		#endregion public-method
	}

}