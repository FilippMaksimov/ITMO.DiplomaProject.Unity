using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirstLocationEnemiesSpawnScript : MonoBehaviour
{
    public GameObject spawnEffect;
    private Object enemyRef;
    private List<Transform> spawnPoints = new List<Transform>();
    private TMP_Text timeText;
    private GameObject _mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        TMP_Text[] text = TMP_Text.FindObjectsOfType<TMP_Text>();
        foreach (TMP_Text t in text)
        {
            if (t.tag == "Timer")
            {
                timeText = t;
            }
        }
        //Initializing spawn points
        GameObject sp = GameObject.FindGameObjectWithTag("SpawnPoints");
        foreach (Transform t in sp.transform)
        {
            spawnPoints.Add(t);
        }

        if (gameObject.tag == "OrcWarrior")
        {
            enemyRef = Resources.Load("EnemyOrcWarriorL1");
        }
        if (gameObject.tag == "EnemyDragon")
        {
            enemyRef = Resources.Load("EnemyWhiteDragonL1");
        }
        if (gameObject.tag == "EnemyWizard")
        {
            enemyRef = Resources.Load("EnemyWizardFemaleL1");
        }
        if (gameObject.tag == "EnemyWizardMale")
        {
            enemyRef = Resources.Load("EnemyWizardMaleL1");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(timeText.text);
        if (gameObject.tag == "OrcWarrior")
        {
            if (GetComponent<EnemyOrcWarrior>().hp <= 0)
            {
                Invoke("Spawn", 5f);
            }
        }
        if (gameObject.tag == "EnemyDragon")
        {
            if (GetComponent<EnemyDragon>().hp <=0)
            {
                Invoke("Spawn", 5f);
            }
        }
        if (gameObject.tag == "EnemyWizard")
        {
            if (GetComponent<EnemyWizardFemale>().hp <= 0)
            {
                Invoke("Spawn", 5f);
            }
        }
        if (gameObject.tag == "EnemyWizardMale")
        {
            if (GetComponent<EnemyWizardMale>().hp <= 0)
            {
                Invoke("Spawn", 5f);
            }
        }
        TimeSpawn();
    }
    void Spawn()
    {
        Vector3 spawningDeviation = new Vector3(Random.Range(-2.5f, 2.5f), 0, Random.Range(-1.5f, 1.5f));
        SpawnEnemy(enemyRef, spawnPoints[Random.Range(0, spawnPoints.Count)].position + spawningDeviation);
        Destroy(gameObject);
    }
    void TimeSpawn()
    {
        Vector3 spawningDeviation = new Vector3(Random.Range(-2.5f, 2.5f), 0, Random.Range(-1.5f, 1.5f));
        if (timeText.text == "10 : 00")
        {
            SpawnEnemy(Resources.Load("EnemyOrcWarriorL1"), spawnPoints[0].position + spawningDeviation);
            SpawnEnemy(Resources.Load("EnemyWizardMaleL1"), spawnPoints[1].position + spawningDeviation);
        }
        if (timeText.text == "5 : 00")
        {
            SpawnEnemy(Resources.Load("EnemyWizardFemaleL1"), spawnPoints[3].position + spawningDeviation);
        }
        if (timeText.text == "0 : 00")
        {
            Destroy(gameObject);
            _mainCamera.GetComponent<PlayerDataController>().SpentPotions();
        }
    }
    void SpawnEnemy(Object enemy, Vector3 enemyTransform)
    {
        GameObject enemyGO = (GameObject)Instantiate(enemy, enemyTransform, transform.rotation);
        GameObject effect = Instantiate(spawnEffect, enemyGO.transform.position, transform.rotation);
        Destroy(effect, 2f);
    }
}
