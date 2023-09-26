using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyOrcAttackOneScript : StateMachineBehaviour
{
    Transform player;
    Animator playerAnimator;
    bool[] dodgePropability = new bool[] { true, false, false, true, true, true, true, false, false, true };
    int num;
    //Get from OrcScript public GameObject latter 
    GameObject dodgeEffect;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        num = Random.Range(0, 10);
        //dodgeEffect = animator.gameObject.GetComponent<OrcWarrior>().dodgeEffect;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player.position.y - animator.gameObject.transform.position.y < 0.1)
        {
            animator.transform.LookAt(player);
        }
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance >= 1.5f)
        {
            animator.SetBool("Attacking", false);
        }
        if (playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Hit1")
            || playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Hit2")
            || playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Hit3"))
        {
            if (dodgePropability[num] == true)
            {
                animator.SetBool("Dodging", true);
                //dodgeEffect Set Active true 
            }
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
