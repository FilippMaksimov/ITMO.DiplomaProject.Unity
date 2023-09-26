using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWizardFemaleDodgeScript : StateMachineBehaviour
{
    Animator playerAnimator;
    GameObject dodgeEffect;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    // Можно добавть эффекты
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        dodgeEffect = animator.gameObject.GetComponent<EnemyWizardFemale>().dodgeEffect;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Standing Aim Recoil")
            || !playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Hit1")
            || !playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Hit2")
            || !playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Hit3"))
        {
            animator.SetBool("Dodging", false);
            animator.gameObject.transform.position = animator.transform.position + (animator.transform.right * 0.1f);
            GameObject de1 = Instantiate(dodgeEffect, animator.transform.position, animator.transform.rotation);
            Destroy(de1, 2.0f);
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
