using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWizardMale : MonoBehaviour
{
    public int hp = 100;
    public GameObject hit;
    public GameObject hitEffect;
    public Transform hitPoint;
    public Transform hitPointEnd;
    public Animator _animator;
    public Slider healthBar;
    public GameObject megaHitIndicator;
    public GameObject megaHitPlayerAura;
    public GameObject megaHitObject;
    private Transform player;
    private Transform remoteHitPoint;

    // Update is called once per frame
    void Update()
    {
        healthBar.value = hp;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (hp <= 0)
        {
            GetComponent<DisolveElementsScript>().Disolve(0.01f);
        }
    }
    private void StartAura()
    {
        megaHitPlayerAura.SetActive(true);
    }
    private void EndAura()
    {
        megaHitPlayerAura.SetActive(false);
    }
    private void StartAttack()
    {
        megaHitPlayerAura.SetActive(false);
        Vector3 correction = new Vector3(0f, 1f, 0f);
        GameObject h = Instantiate(hit, hitPoint.position, transform.rotation);
        h.GetComponent<Rigidbody>().AddForce((hitPointEnd.position - hitPoint.position)* 6f, ForceMode.Impulse);
        GameObject effect = Instantiate(hitEffect, transform.position + correction, transform.rotation);
        Destroy(h, 0.2f);
        Destroy(effect, 1f);
    }

    private void RemoteAttackPrepare()
    {
        Vector3 correction = new Vector3(0f, 0.1f, 0f);
        GameObject circle = Instantiate(megaHitIndicator, player.position + correction, player.rotation);
        remoteHitPoint = circle.transform;
    }
    private void RemoteAttackStart()
    {
        GameObject hitStart = Instantiate(megaHitObject, remoteHitPoint.position, transform.rotation);
        Destroy(hitStart, 1f);
        Destroy(GameObject.FindGameObjectWithTag("CircleIndicator"), 0.2f);
    }
    public void GetDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            _animator.SetTrigger("Die");
            healthBar.gameObject.SetActive(false);
            GetComponent<Collider>().enabled = false;
            megaHitPlayerAura?.SetActive(false);
            GetComponent<DisolveElementsScript>().DisolveEffect();
            Destroy(GameObject.FindGameObjectWithTag("CircleIndicator"));
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerDataController>().score += 40;
        }
        else
        {
            _animator.SetTrigger("Damage");
        }
    }
}
