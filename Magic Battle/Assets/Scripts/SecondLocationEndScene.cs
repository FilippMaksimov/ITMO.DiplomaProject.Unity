using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondLocationEndScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerDataController>().SpentPotions();
        }
    }
}
