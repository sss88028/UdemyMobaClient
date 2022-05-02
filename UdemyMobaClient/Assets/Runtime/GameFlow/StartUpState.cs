using CCTU.UIFramework;
using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.State
{
    public class StartUpState : StateMachineBehaviour
	{
		#region public-method
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			UIManager.Instance.Init();
			GameFlow.Instance.Flow.SetTrigger("Next");
		}
		#endregion public-method
	}
}