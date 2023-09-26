using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClawnColider : MonoBehaviour
{
    private Rigidbody clawnRigidbody;
    //Take this variable from php script (database)
    private int damage;
    public GameObject shieldHit;

    private void Awake()
    {
        clawnRigidbody = GetComponent<Rigidbody>();
    }

    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        Vector3 correction = new Vector3 (-0.1f, 0.4f, 0.4f);
        damage = 20; // Add data script here 
        if (other.tag == "Shield")
        {
            Debug.Log("Hit to shield");
            GameObject sh = GameObject.Instantiate(shieldHit, transform.position + correction, transform.rotation);
            Destroy(sh, 1f);
        }
        if (other.tag == "Player")
        {
            if (other.GetComponent<ThirdPersonController>().shieldObject.active == true)
            {
                return;
            }
            other.GetComponent<ThirdPersonController>().GetDamage(damage);
        }
    }
}
