using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TownEmissionCollider : MonoBehaviour
{
    public int hp = 1000;
    public Slider healthBar;
    private GameObject scenario;
    // Start is called before the first frame update
    void Start()
    {
        scenario = GameObject.FindGameObjectWithTag("Scenario");
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = hp;
        if (hp <= 0)
        {
            foreach (var go in GetComponentsInChildren<FlameDissolveScript>())
            {
                if (go != null)
                {
                    go.Disolve(0.01f);
                }
            }
        }
    }
    public void GetDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            healthBar.gameObject.SetActive(false);
            scenario.GetComponent<SecondLocationScenarioScript>().sphereDestroyedNum++;
            Destroy(gameObject, 3.5f);
        }
    }
}
