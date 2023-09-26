using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Unity.VisualScripting;

public class DragonWalkingStateScipt : StateMachineBehaviour
{
    private float timer;
    private List<Transform> wp = new List<Transform>();
    private NavMeshAgent navAgent;
    private Transform _player;
    Animator enemy1;
    Animator enemy2;
    Animator enemy3;
    bool isDetected = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        navAgent = animator.GetComponent<NavMeshAgent>();
        GameObject ways = GameObject.FindGameObjectWithTag("WayPoints");
        foreach (Transform t in ways.transform)
        {
            wp.Add(t);
        }
        navAgent.SetDestination(wp[Random.Range(0, wp.Count)].position);
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent.speed = 1.2f;

        try
        {
            enemy1 = GameObject.FindGameObjectWithTag("EnemyWizardMale").GetComponent<Animator>();
        }
        catch (System.NullReferenceException ex)
        {
            Debug.Log("Killed" + ex.Message);
        }
        try
        {
            enemy2 = GameObject.FindGameObjectWithTag("OrcWarrior").GetComponent<Animator>();
        }
        catch (System.NullReferenceException ex)
        {
            Debug.Log("Killed" + ex.Message);
        }
        try
        {
            enemy3 = GameObject.FindGameObjectWithTag("EnemyWizard").GetComponent<Animator>();
        }
        catch (System.NullReferenceException ex)
        {
            Debug.Log("Killed" + ex.Message);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float dist = Vector3.Distance(_player.position, animator.transform.position);
        if (navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            navAgent.SetDestination(wp[Random.Range(0, wp.Count)].position);
        }
        timer += Time.deltaTime;
        if (timer > 6 || isDetected == true)
        {
            animator.SetBool("isPatrolling", true);
        }
        if (dist < 12 || isDetected == true)
        {
            animator.SetBool("Chasing", true);
        }

        if ((enemy1 != null && (enemy1.GetCurrentAnimatorStateInfo(0).IsName("getgit") || enemy1.GetCurrentAnimatorStateInfo(0).IsName("death"))) ||
            (enemy2 != null && (enemy2.GetCurrentAnimatorStateInfo(0).IsName("getHit") || enemy2.GetCurrentAnimatorStateInfo(0).IsName("die"))) ||
            (enemy3 != null && (enemy3.GetCurrentAnimatorStateInfo(0).IsName("GetHit") || enemy3.GetCurrentAnimatorStateInfo(0).IsName("Death"))))
        {
            isDetected = true;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navAgent.SetDestination(navAgent.transform.position);
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
