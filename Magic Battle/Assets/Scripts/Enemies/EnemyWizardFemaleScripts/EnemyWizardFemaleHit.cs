using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWizardFemaleHit : MonoBehaviour
{
    private Rigidbody hitRigidbody;
    //Take this variable from php script (database)
    private int damage;
    private void Start()
    {
        Destroy(gameObject, 5.5f);
    }
    private void Awake()
    {
        hitRigidbody = GetComponent<Rigidbody>();
    }

    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        damage = 15;
        if (other.tag == "Player")
        {
            if (other.GetComponent<ThirdPersonController>().shieldObject.active == true)
            {
                return;
            }
            other.GetComponent<ThirdPersonController>().GetDamage(damage);
            Destroy(gameObject);
        }
    }
}
