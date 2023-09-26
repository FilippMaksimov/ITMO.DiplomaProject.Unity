using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Spells : MonoBehaviour
{
    private Rigidbody spellRigidbody;
    private int damageAmount;
    private float distance;
    public bool isMega;

    private void Awake()
    {
        spellRigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Destroy(gameObject, 10);
    }
    private void Update()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("EnemyWizard");
    }
    private void OnTriggerEnter(Collider other)
    {
        damageAmount = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonController>()._power;
        if (isMega == true)
        {
            damageAmount *= 2;
        }
        Destroy(transform.GetComponent<Rigidbody>());
        if (other.tag == "EnemyDragon")
        {
            other.GetComponent<EnemyDragon>().GetDamage(damageAmount);
        }
        if (other.tag == "EnemyWizard")
        {
            other.GetComponent<EnemyWizardFemale>().GetDamage(damageAmount);
        }
        if (other.tag == "EnemyWizardMale")
        {
            other.GetComponent<EnemyWizardMale>().GetDamage(damageAmount);
        }
        if (other.tag == "OrcWarrior")
        {
            if (other.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("dodge"))
            {
                return;
            }
            else
            {
                other.GetComponent<EnemyOrcWarrior>().GetDamage(damageAmount);
            }
        }
        if (other.tag == "TownSphere")
        {
            other.GetComponent<TownEmissionCollider>().GetDamage(damageAmount);
        }
    }
}
