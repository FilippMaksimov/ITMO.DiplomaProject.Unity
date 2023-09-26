using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWizardFemaleAtackScript : StateMachineBehaviour
{
    Transform player;
    GameObject hit;
    Animator playerAnimator;
    bool[] dodgePropability = new bool[] { true, false, false, true, true, true, true, false, false, true };
    int num;
    GameObject dodgeEffect;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        hit = GameObject.FindGameObjectWithTag("WizardEnemyHit1");
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        num = Random.Range(0, 10);
        //dodgeEffect = GameObject.FindGameObjectWithTag("DodgeEffect");
        dodgeEffect = animator.gameObject.GetComponent<EnemyWizardFemale>().dodgeEffect;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Add correction
        Vector3 dodgeMove = new Vector3(0.5f, 0f, 0.5f);
        if (player.position.y - animator.gameObject.transform.position.y < 0.1)
        {
            animator.transform.LookAt(player);
        }
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance > 10.5)
        {
            animator.SetBool("Attacking", false);
        }
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Standing Aim Recoil") || 
            playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Hit1") || 
            playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Hit2") ||
            playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Hit3"))
        {
            if (dodgePropability[num] == true)
            {
                animator.SetBool("Dodging", true);
                GameObject de1 = Instantiate(dodgeEffect, animator.transform.position, animator.transform.rotation);
                animator.gameObject.transform.SetPositionAndRotation(animator.transform.position + (animator.transform.right * 0.1f), animator.transform.rotation);
                Debug.Log(num);
                Destroy(de1, 0.5f);
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
      
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that sets up animation IK (inverse kinematics)
    }
}
