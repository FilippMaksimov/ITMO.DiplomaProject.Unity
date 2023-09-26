using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOrcDodgeScript : StateMachineBehaviour
{
    Animator playerAnimator;
    GameObject dodgeEffect;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        //dodgeEffect = animator.gameObject.GetComponent<OrcWarrior>().dodgeEffect;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Hit1")
            || !playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Hit2")
            || !playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Hit3"))
        {
            animator.SetBool("Dodging", false);
            //dodge effect set active false
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that processes and affects root motion
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that sets up animation IK (inverse kinematics)
    }
}
