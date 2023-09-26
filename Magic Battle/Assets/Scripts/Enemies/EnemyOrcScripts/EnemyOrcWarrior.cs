using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyOrcWarrior : MonoBehaviour
{
    public int hp = 100;
    public GameObject hit;
    public GameObject hitEffect;
    public Transform firstHitPointStart;
    public Transform firstHitPointEnd;
    public Transform secondHitPoint;
    public Animator _animator;
    public Slider healthBar;
    public GameObject dodgeEffect;

    // Update is called once per frame
    void Update()
    {
        healthBar.value = hp;
        if (hp <= 0)
        {
            GetComponent<DisolveElementsScript>().Disolve(0.0125f);
        }
    }
    private void FirstHit()
    {
        Vector3 correction = new Vector3(0f, 1.1f, 0f);
        GameObject h = Instantiate(hit, firstHitPointStart.position, transform.rotation);
        h.GetComponent<Rigidbody>().AddForce((firstHitPointEnd.position - firstHitPointStart.position) * 6f, ForceMode.Impulse);
        GameObject effect = Instantiate(hitEffect, transform.position + correction, transform.rotation);
        Destroy(h, 0.2f);
        Destroy(effect, 1f);
    }

    private void SecondHit()
    {
        Vector3 correction = new Vector3(0f, 0.1f, 0f);
        GameObject h = Instantiate(hit, secondHitPoint.position, transform.rotation);
        Destroy(h, 0.8f);
    }
    private void Dodge()
    {
        GameObject dodge = Instantiate(dodgeEffect, transform.position, transform.rotation);
        Destroy(dodge, 1.5f);
    }

    public void GetDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            _animator.SetTrigger("Die");
            healthBar.gameObject.SetActive(false);
            GetComponent<Collider>().enabled = false;
            GetComponent<DisolveElementsScript>().DisolveEffect();
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerDataController>().score += 15;
        }
        else
        {
            _animator.SetTrigger("Damage");
        }
    }
}
