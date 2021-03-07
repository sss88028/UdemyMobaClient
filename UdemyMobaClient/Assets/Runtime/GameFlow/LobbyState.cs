using CCTU.GameDevTools.MonoSingleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.State
{
    public class LobbyState : StateMachineBehaviour
	{
		#region private-field
		private AsyncOperation _asyncOperation;
		private Coroutine _waitRoutine;
		#endregion private-field

		#region public-method
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);

			if (_waitRoutine != null) 
			{
				GameSystem.Instance.StopCoroutine(_waitRoutine);
				_waitRoutine = null;
			}

			_asyncOperation = SceneManager.LoadSceneAsync("Empty", LoadSceneMode.Single);
			_waitRoutine = GameSystem.Instance.StartCoroutine(WaitOpenLobby());
		}
		#endregion public-method

		#region private-field
		private IEnumerator WaitOpenLobby() 
		{
			yield return new WaitUntil(() => 
			{
				return _asyncOperation.isDone;
			});
			GL.Clear(true, true, Color.clear);
		}
		#endregion private-field
	}
}