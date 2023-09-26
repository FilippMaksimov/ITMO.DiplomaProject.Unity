using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWizardMaleWalkScript : StateMachineBehaviour
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
        agent = animator.GetComponent<NavMeshAgent>();
        timer = 0;
        GameObject go = GameObject.FindGameObjectWithTag("WayPoints");
        foreach (Transform t in go.transform)
        {
            wayPoints.Add(t);
        }
        agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        player = GameObject.FindGameObjectWithTag("Player").transform;

        try
        {
            enemy1 = GameObject.FindGameObjectWithTag("EnemyWizard").GetComponent<Animator>();
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
            animator.SetBool("Walking", false);
        }
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance < 18 || isDetected == true)
        {
            agent.speed = 3.5f;
            agent.SetDestination(player.position);
        }
        if (distance < 12)
        {
            animator.SetBool("RemoteAttacking", true);
        }

        if ((enemy1 != null && (enemy1.GetCurrentAnimatorStateInfo(0).IsName("GetHit") || enemy1.GetCurrentAnimatorStateInfo(0).IsName("Death"))) ||
            (enemy2 != null && (enemy2.GetCurrentAnimatorStateInfo(0).IsName("getHit") || enemy2.GetCurrentAnimatorStateInfo(0).IsName("die"))) ||
            (enemy3 != null && (enemy3.GetCurrentAnimatorStateInfo(0).IsName("getHit") || enemy3.GetCurrentAnimatorStateInfo(0).IsName("die"))))
        {
            isDetected = true;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("getgit")) 
        {
            agent.SetDestination(player.position);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
        agent.SetDestination(animator.transform.position);
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
