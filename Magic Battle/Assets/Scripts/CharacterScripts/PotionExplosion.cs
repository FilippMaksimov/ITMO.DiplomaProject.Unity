using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PotionExplosion : MonoBehaviour
{
    private float explosionForce = 500f;
    private float explosionRadious = 5f;
    private float upwardModifier = 5f;

    private float maxDamage = 100f;
    public GameObject explosionEffect;

    private void OnTriggerEnter(Collider other)
    {
        Explode();
    }

    private void Explode()
    {
        Collider[] hitColliders = new Collider[10];
        int collidersHit = Physics.OverlapSphereNonAlloc(transform.position, explosionRadious, hitColliders);
        for (int i = 0; i < collidersHit; i++)
        {
            if (hitColliders[i].TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadious, upwardModifier);
            }
            if (hitColliders[i].TryGetComponent(out EnemyDragon enemy))
            {
                enemy.GetDamage((int)(maxDamage - Vector3.Distance(transform.position, hitColliders[i].transform.position) * (maxDamage / explosionRadious)));
            }
            if (hitColliders[i].TryGetComponent(out ThirdPersonController player))
            {
                player.GetDamage((int)(maxDamage - Vector3.Distance(transform.position, hitColliders[i].transform.position) * (maxDamage / explosionRadious)));
            }
            if (hitColliders[i].TryGetComponent(out EnemyWizardFemale enemyWizardF))
            {
                enemyWizardF.GetDamage((int)(maxDamage - Vector3.Distance(transform.position, hitColliders[i].transform.position) * (maxDamage / explosionRadious)));
            }
            if (hitColliders[i].TryGetComponent(out EnemyWizardMale enemyWizardM))
            {
                enemyWizardM.GetDamage((int)(maxDamage - Vector3.Distance(transform.position, hitColliders[i].transform.position) * (maxDamage / explosionRadious)));
            }
            if (hitColliders[i].TryGetComponent(out EnemyOrcWarrior enemyOrcWarrior))
            {
                enemyOrcWarrior.GetDamage((int)(maxDamage - Vector3.Distance(transform.position, hitColliders[i].transform.position) * (maxDamage / explosionRadious)));
            }
        }
        GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(effect, 2f);
        Destroy(gameObject);
    }
}
