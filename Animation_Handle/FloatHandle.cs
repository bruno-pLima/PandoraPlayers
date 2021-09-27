using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatHandle : StateMachineBehaviour
{
    public string FloatName;
    public float value;
    public float Exitvalue;
    public bool ResetOnExit;
    public bool SetOnStart = true;
    public bool SetOnEnd = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (SetOnStart)
            animator.SetFloat(FloatName, value);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ResetOnExit)
            animator.SetFloat(FloatName, 0);
        else if (SetOnEnd)
            animator.SetFloat(FloatName, Exitvalue);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
