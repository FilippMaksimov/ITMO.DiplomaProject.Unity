using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondLocationFirstTrigger : MonoBehaviour
{
    public GameObject firstEnemy;
    public GameObject secondEnemy;
    public GameObject spawnEffect;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
 
    private void OnTriggerEnter(Collider other)
    {
        Vector3 spawnCorrection1 = new Vector3(Random.Range(0, 1.5f), 0, Random.Range(0, 2.5f));
        Vector3 spawnCorrection2 = new Vector3(Random.Range(0, 1.5f), 0, Random.Range(0, 2.5f));
        Vector3 spawnCorrection3 = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-2.5f, 2.5f));

        if (other.tag == "Player")
        {
            gameObject.SetActive(false);

            GameObject enemy1 = Instantiate(firstEnemy, spawnPoint1.position + spawnCorrection1, transform.rotation);
            SpawningEffect(enemy1);
            GameObject enemy2 = Instantiate(firstEnemy, spawnPoint1.position + spawnCorrection2, transform.rotation);
            SpawningEffect(enemy2);
            GameObject enemy3 = Instantiate(secondEnemy, spawnPoint1.position + spawnCorrection3, transform.rotation);
            SpawningEffect(enemy3);

            GameObject enemy4 = Instantiate(firstEnemy, spawnPoint2.position + spawnCorrection1, transform.rotation);
            SpawningEffect(enemy4);
            GameObject enemy5 = Instantiate(firstEnemy, spawnPoint2.position + spawnCorrection2, transform.rotation);
            SpawningEffect(enemy5);
        }
    }
    private void SpawningEffect(GameObject enemy)
    {
        GameObject effect = Instantiate(spawnEffect, enemy.transform.position, transform.rotation);
        Destroy(effect, 2f);
    }
}
