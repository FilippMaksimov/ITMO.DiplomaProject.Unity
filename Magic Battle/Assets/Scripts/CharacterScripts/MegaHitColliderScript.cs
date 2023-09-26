using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaHitColliderScript : MonoBehaviour
{
    private Rigidbody megaHitRigidbody;
    private int damage;
    // Start is called before the first frame update
    private void Awake()
    {
        megaHitRigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Destroy(gameObject, 2f);
    }
    private void OnTriggerEnter(Collider other)
    {
        damage = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonController>()._power * 2;
        //Destroy(transform.GetComponent<Rigidbody>());
        if (other.tag == "EnemyDragon")
        {
            other.GetComponent<EnemyDragon>().GetDamage(damage);
        }
        if (other.tag == "EnemyWizard")
        {
            other.GetComponent<EnemyWizardFemale>().GetDamage(damage);
        }
        if (other.tag == "EnemyWizardMale")
        {
            other.GetComponent<EnemyWizardMale>().GetDamage(damage);
        }
        if (other.tag == "OrcWarrior")
        {
            other.GetComponent<EnemyOrcWarrior>().GetDamage(damage);
        }
    }
}
