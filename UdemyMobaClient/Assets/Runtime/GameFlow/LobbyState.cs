using CCTU.GameDevTools.MonoSingleton;
using CCTU.UIFramework;
using Game.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.State
{
    public class LobbyState : StateMachineBehaviour
	{
		#region private-field
		private Coroutine _waitRoutine;
		#endregion private-field

		#region public-method
		public override async void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);

			var asyncOperation = SceneManager.LoadSceneAsync("Empty", LoadSceneMode.Single);
			while (asyncOperation.isDone) 
			{
				await Task.Yield();
			}
			await UIManager.Instance.TriggerUIEvent(new LobbyUIShowEvent());

		}
		#endregion public-method
	}
}