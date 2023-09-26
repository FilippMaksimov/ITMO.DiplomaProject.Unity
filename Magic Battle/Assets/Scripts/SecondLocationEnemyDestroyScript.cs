using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SecondLocationEnemyDestroyScript : MonoBehaviour
{
    private GameObject scenario;
    private int counter;
    private Transform player;
    void Start()
    {
        scenario = GameObject.FindGameObjectWithTag("Scenario");
        counter = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.position, gameObject.transform.position) >= 12.0f)
        {
            gameObject.GetComponent<NavMeshAgent>().SetDestination(player.position);
            if (gameObject.tag == "OrcWarrior")
            {
                gameObject.GetComponent<Animator>().SetBool("Chasing", true);
                gameObject.GetComponent<NavMeshAgent>().speed = 5.5f;
            }
            else if (gameObject.tag == "EnemyWizard")
            {
                gameObject.GetComponent<Animator>().SetBool("Chasing", true);
                gameObject.GetComponent<NavMeshAgent>().speed = 5.5f;
            }
            else if (gameObject.tag == "EnemyWizardMale")
            {
                gameObject.GetComponent<NavMeshAgent>().speed = 6.5f;
            }
        }

        if (gameObject.tag == "OrcWarrior")
        {
            if (gameObject.GetComponent<EnemyOrcWarrior>().hp <= 0)
            {
                if (counter == 0)
                {
                    scenario.GetComponent<SecondLocationScenarioScript>().enemyKilledNum++;
                    counter++;
                    Destroy(gameObject, 5f);
                }
            }
        }
        else if (gameObject.tag == "EnemyWizard")
        {
            if (gameObject.GetComponent<EnemyWizardFemale>().hp <= 0)
            {
                if (counter == 0)
                {
                    scenario.GetComponent<SecondLocationScenarioScript>().enemyKilledNum++;
                    counter++;
                    Destroy(gameObject, 5f);
                }
            }
        }
        else if (gameObject.tag == "EnemyWizardMale")
        {
            if (gameObject.GetComponent<EnemyWizardMale>().hp <= 0)
            {
                if (counter == 0)
                {
                    scenario.GetComponent<SecondLocationScenarioScript>().enemyKilledNum++;
                    counter++;
                    Destroy(gameObject, 5f);
                }
            }
        }
    }
}
