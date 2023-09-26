using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondLocationSecondTrigger : MonoBehaviour
{
    public GameObject firstEnemy;
    public GameObject secondEnemy;
    public GameObject spawnEffect;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;
    public Transform spawnPoint4;

    private void OnTriggerEnter(Collider other)
    {
        Vector3 spawnCorrection1 = new Vector3(Random.Range(0, 1.5f), 0, Random.Range(0, 1.5f));
        Vector3 spawnCorrection2 = new Vector3(Random.Range(0, 1.5f), 0, Random.Range(0, 1.5f));
        Vector3 spawnCorrection3 = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
        Vector3 spawnCorrection4 = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));

        if (other.tag == "Player")
        {
            gameObject.SetActive(false);

            //First spawn area
            GameObject enemy1 = Instantiate(firstEnemy, spawnPoint1.position + spawnCorrection1, transform.rotation);
            SpawningEffect(enemy1);
            GameObject enemy2 = Instantiate(firstEnemy, spawnPoint1.position + spawnCorrection2, transform.rotation);
            SpawningEffect(enemy2);
            GameObject enemy3 = Instantiate(secondEnemy, spawnPoint1.position + spawnCorrection3, transform.rotation);
            SpawningEffect(enemy3);
            GameObject enemy4 = Instantiate(firstEnemy, spawnPoint1.position + spawnCorrection4, transform.rotation);
            SpawningEffect(enemy4);

            //Second spawn area 
            GameObject enemy5 = Instantiate(firstEnemy, spawnPoint2.position + spawnCorrection1, transform.rotation);
            SpawningEffect(enemy5);
            GameObject enemy6 = Instantiate(firstEnemy, spawnPoint2.position + spawnCorrection2, transform.rotation);
            SpawningEffect(enemy6);
            GameObject enemy7 = Instantiate(secondEnemy, spawnPoint2.position + spawnCorrection3, transform.rotation);
            SpawningEffect(enemy7);
            GameObject enemy8 = Instantiate(firstEnemy, spawnPoint2.position + spawnCorrection4, transform.rotation);
            SpawningEffect(enemy8);

            //Third spawn area
            GameObject enemy9 = Instantiate(firstEnemy, spawnPoint3.position + spawnCorrection1, transform.rotation);
            SpawningEffect(enemy9);
            GameObject enemy10 = Instantiate(firstEnemy, spawnPoint3.position + spawnCorrection2, transform.rotation);
            SpawningEffect(enemy10);
            GameObject enemy11 = Instantiate(secondEnemy, spawnPoint3.position + spawnCorrection3, transform.rotation);
            SpawningEffect(enemy11);
            GameObject enemy12 = Instantiate(firstEnemy, spawnPoint3.position + spawnCorrection4, transform.rotation);
            SpawningEffect(enemy12);

            //Forth spawn area
            GameObject enemy13 = Instantiate(firstEnemy, spawnPoint4.position + spawnCorrection1, transform.rotation);
            SpawningEffect(enemy13);
            GameObject enemy14 = Instantiate(firstEnemy, spawnPoint4.position + spawnCorrection2, transform.rotation);
            SpawningEffect(enemy14);
            GameObject enemy15 = Instantiate(secondEnemy, spawnPoint4.position + spawnCorrection3, transform.rotation);
            SpawningEffect(enemy15);
            GameObject enemy16 = Instantiate(firstEnemy, spawnPoint4.position + spawnCorrection4, transform.rotation);
            SpawningEffect(enemy16);
        }
    }
    private void SpawningEffect(GameObject enemy)
    {
        GameObject effect = Instantiate(spawnEffect, enemy.transform.position, transform.rotation);
        Destroy(effect, 2f);
    }
}
