using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWizardRemoteHitCollider : MonoBehaviour
{
    private Rigidbody remoteHitRigidbody;
    private int damage;
    private void Awake()
    {
        remoteHitRigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Destroy(gameObject, 2f);
    }
    private void OnTriggerEnter(Collider other)
    {
        damage = 25;
        Destroy(transform.GetComponent<Rigidbody>());
        if (other.tag == "Shield")
        {
            return;
        }
        if (other.tag == "Player")
        {
            other.GetComponent<ThirdPersonController>().GetDamage(damage);
        }
    }
}
