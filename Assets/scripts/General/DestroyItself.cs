using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItself : StateMachineBehaviour
{
    //A behaviour that is used mainly for special effects that need to be destroyed imediatly after being done with the animation

     //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(animator.gameObject);
    }

}
