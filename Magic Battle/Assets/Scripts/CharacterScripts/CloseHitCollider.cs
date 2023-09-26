using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseHitCollider : MonoBehaviour
{
    private Rigidbody hitRigidbody;
    //From php database 
    private int damage;
    public bool isMega;

    private void Awake()
    {
        hitRigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Destroy(gameObject, 0.7f);
    }
    private void OnTriggerEnter(Collider other)
    {
        damage = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonController>()._power;
        if (isMega == true)
        {
            damage *= 2;
        }
        Destroy(transform.GetComponent<Rigidbody>());
        if (other.tag == "EnemyDragon")
        {
            damage -= 5;
            other.GetComponent<EnemyDragon>().GetDamage(damage);
        }
        if (other.tag == "EnemyWizard")
        {
            damage -= 10;
            other.GetComponent<EnemyWizardFemale>().GetDamage(damage);
        }
        if (other.tag == "EnemyWizardMale")
        {
            damage -= 15;
            other.GetComponent<EnemyWizardMale>().GetDamage(damage);
        }
        if (other.tag == "OrcWarrior")
        {
            damage -= 5;
            other.GetComponent<EnemyOrcWarrior>().GetDamage(damage);
        }
    }
}
