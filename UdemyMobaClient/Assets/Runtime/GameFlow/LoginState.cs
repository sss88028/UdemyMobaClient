using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.State
{
	public class LoginState : StateMachineBehaviour
	{
		#region public-method
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			LoginUIController.Instance.OpenUI();
		}
		#endregion public-method
	}

}