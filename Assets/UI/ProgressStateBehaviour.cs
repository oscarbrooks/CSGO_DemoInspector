using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressStateBehaviour : StateMachineBehaviour {
    
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.gameObject.SetActive(false);
	}
}
