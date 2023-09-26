using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class EnemyWizardFemale : MonoBehaviour
{
    public int hp = 100;
    public GameObject hit;
    public GameObject hitEffect;
    private GameObject player;
    private Transform playerPoint;
    public Animator _animator;
    public GameObject dodgeEffect;
    public Slider healthBar;
    private GameObject[] players;

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in players)
        {
            if (go.gameObject.activeSelf == true)
            {
                player = go;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerPoint = player.transform; 
        healthBar.value = hp;
        if (hp <= 0)
        {
            GetComponent<DisolveElementsScript>().Disolve(0.005f);
        }
    }
    private void Attack()
    {
        Vector3 correction = new Vector3(0f, 1.5f, 0f);
        Vector3 effect = new Vector3(-0.15f, -0.2f, 0f);
        GameObject attack = Instantiate(hit, gameObject.transform.position + correction, transform.rotation);
        GameObject attackEffect = Instantiate(hitEffect, gameObject.transform.position + correction + effect, transform.rotation);
        Vector3 hitDirection = (playerPoint.position + correction - attack.transform.position).normalized;
        attack.GetComponent<Rigidbody>().AddForce(hitDirection * 20f, ForceMode.Impulse);
        Destroy(attackEffect, 1.3f);
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
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerDataController>().score += 70;
        }
        else
        {
            _animator.SetTrigger("Damage");
            gameObject.GetComponent<NavMeshAgent>().SetDestination(playerPoint.position);
        }
    }
}
