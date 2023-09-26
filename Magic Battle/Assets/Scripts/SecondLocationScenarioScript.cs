using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SecondLocationScenarioScript : MonoBehaviour
{
    public int enemyKilledNum;
    public int sphereDestroyedNum;
    public GameObject blockFire1;
    public GameObject blockFire2;
    public GameObject blockFire3;
    public GameObject trigger3;
    public GameObject trigger4;
    public GameObject[] enemies;
    public Transform[] spawnPointsTown;
    public Transform[] spawnPointsCastle;
    public GameObject spawnEffect;
    public int enemiesNum;
    private float CooldownTime = 6.5f;
    private float cooldownUntilNextSpawn = 5.5f;
    // Start is called before the first frame update
    void Start()
    {
        enemyKilledNum = 0;
        sphereDestroyedNum = 0;
        enemiesNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesNum > 0)
        {
            enemiesNum -= enemyKilledNum;
        }

        if (enemyKilledNum == 5)
        {
            try
            {
                blockFire1.GetComponent<FlameDissolveScript>().Disolve(0.01f);
            }
            catch (MissingReferenceException ex)
            {
                Debug.Log("First Gate Completed" + ex.ToString());
            }
        }
        if (enemyKilledNum == 21)
        {
            try
            {
                blockFire2.GetComponent<FlameDissolveScript>().Disolve(0.01f);
            }
            catch (MissingReferenceException ex)
            {
                Debug.Log("Second Gate Completed" + ex.ToString());
            }
        }
        if (sphereDestroyedNum == 3 && trigger4.gameObject.activeSelf == true)
        {
            try
            {
                blockFire3.GetComponent<FlameDissolveScript>().Disolve(0.01f);
            }
            catch(MissingReferenceException ex)
            {
                Debug.Log("Third Gate Completed" + ex.ToString());
            }
            GameObject[] enemiesAtTown1 = GameObject.FindGameObjectsWithTag("OrcWarrior");
            GameObject[] enemiesAtTown2 = GameObject.FindGameObjectsWithTag("EnemyWizard");
            GameObject[] enemiesAtTown3 = GameObject.FindGameObjectsWithTag("EnemyWizardMale");
            if (enemiesAtTown1 != null)
            {
                foreach (GameObject enemy in enemiesAtTown1)
                {
                    enemy.GetComponent<EnemyOrcWarrior>().GetDamage(100);
                }
            }
            if (enemiesAtTown2 != null)
            {
                foreach (GameObject enemy in enemiesAtTown2)
                {
                    enemy.GetComponent<EnemyWizardFemale>().GetDamage(100);
                }
            }
            if (enemiesAtTown3 != null)
            {
                foreach (GameObject enemy in enemiesAtTown3)
                {
                    enemy.GetComponent<EnemyWizardMale>().GetDamage(100);
                }
            }
            enemiesNum = 0;
        }
        if (sphereDestroyedNum != 3 && trigger3.gameObject.activeSelf == false && cooldownUntilNextSpawn < Time.time)
        {
            if (enemiesNum <= 20)
            {
                Spawn(spawnPointsTown);
                cooldownUntilNextSpawn = Time.time + CooldownTime;
            }
        }
        if (trigger4.activeSelf == false && cooldownUntilNextSpawn < Time.time)
        {
            if (enemiesNum <= 5)
            {
                Spawn(spawnPointsCastle);
                cooldownUntilNextSpawn = Time.time + CooldownTime;
            }
        }
    }
    private void Spawn(Transform[] points)
    {
        GameObject enemy = Instantiate(enemies[Random.Range(0, enemies.Length - 1)], points[Random.Range(0, points.Length - 1)].position, transform.rotation);
        SpawnEffect(enemy);
        enemiesNum++;
    }
    private void SpawnEffect(GameObject enemy)
    {
        GameObject effect = Instantiate(spawnEffect, enemy.transform.position, transform.rotation);
        Destroy(effect, 2f);
    }
}
