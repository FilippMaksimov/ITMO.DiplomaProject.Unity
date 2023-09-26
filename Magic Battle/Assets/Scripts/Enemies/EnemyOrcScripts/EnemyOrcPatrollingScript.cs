using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyOrcPatrollingScript : StateMachineBehaviour
{
    float timer;
    List<Transform> wayPoints = new List<Transform>();
    NavMeshAgent agent;
    Transform player;
    Animator enemy1;
    Animator enemy2;
    Animator enemy3;
    bool isDetected = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = 2.5f;
        timer = 0;
        GameObject go = GameObject.FindGameObjectWithTag("WayPoints");
        foreach (Transform t in go.transform)
        {
            wayPoints.Add(t);
        }
        agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);

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
            enemy2 = GameObject.FindGameObjectWithTag("EnemyWizard").GetComponent<Animator>();
        }
        catch (System.NullReferenceException ex)
        {
            Debug.Log("Killed" + ex.Message);
        }

        try
        {
            enemy3 = GameObject.FindGameObjectWithTag("EnemyDragon").GetComponent<Animator>();
        }
        catch (System.NullReferenceException ex)
        {
            Debug.Log("Killed" + ex.Message);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        }
        timer += Time.deltaTime;
        if (timer > 10)
        {
            animator.SetBool("Patrolling", false);
        }
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance < 15f || isDetected == true)
        {
            animator.SetBool("Chasing", true);
        }

        if ((enemy1 != null && (enemy1.GetCurrentAnimatorStateInfo(0).IsName("getgit") || enemy1.GetCurrentAnimatorStateInfo(0).IsName("death"))) ||
            (enemy2 != null && (enemy2.GetCurrentAnimatorStateInfo(0).IsName("GetHit") || enemy2.GetCurrentAnimatorStateInfo(0).IsName("Death"))) ||
            (enemy3 != null && (enemy3.GetCurrentAnimatorStateInfo(0).IsName("getHit") || enemy3.GetCurrentAnimatorStateInfo(0).IsName("die"))))
        {
            isDetected = true;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position); 
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
