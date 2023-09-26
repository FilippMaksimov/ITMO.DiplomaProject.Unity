using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondLocationThirdTrigger : MonoBehaviour
{
    public GameObject[] enemies;
    public Transform[] spawnPoints;
    public Transform initSpawn;
    public GameObject spawnEffect;
    private GameObject scenario;

    private void Start()
    {
        scenario = GameObject.FindGameObjectWithTag("Scenario");
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        scenario.GetComponent<SecondLocationScenarioScript>().enemyKilledNum = 0;

        gameObject.GetComponent<BoxCollider>().enabled = false;
        Vector3 initSpawnCorrection = new Vector3(-2.0f, 0, 0);
        GameObject enemy1 = Instantiate(enemies[Random.Range(0, enemies.Length - 1)], initSpawn.position + initSpawnCorrection, transform.rotation);
        SpawnEffect(enemy1);
        GameObject enemy2 = Instantiate(enemies[Random.Range(0, enemies.Length - 1)], initSpawn.position, transform.rotation);
        SpawnEffect(enemy2);
        GameObject enemy3 = Instantiate(enemies[Random.Range(0, enemies.Length - 1)], initSpawn.position - initSpawnCorrection, transform.rotation);
        SpawnEffect(enemy3);

        gameObject.SetActive(false);
    }
    private void SpawnEffect(GameObject enemy)
    {
        GameObject effect = Instantiate(spawnEffect, enemy.transform.position, transform.rotation);
        scenario.GetComponent<SecondLocationScenarioScript>().enemiesNum++;
        Destroy(effect, 2f);
    }
}
