using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDragon : MonoBehaviour
{
    [SerializeField]
    public int hp = 100;
    public Animator _animator;
    public Slider _healthBar;
    public GameObject hitObject;

    private void Update()
    {
        _healthBar.value = hp;
        if (hp <= 0)
        {
            GetComponent<DisolveElementsScript>().Disolve(0.01f);
        }
    }

    public void GetDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0 )
        {
            _animator.SetTrigger("Die");
            _healthBar.gameObject.SetActive(false);
            GetComponent<Collider>().enabled = false;
            hitObject.SetActive(false);
            GetComponent<DisolveElementsScript>().DisolveEffect();
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerDataController>().score += 15;
        }
        else
        {
            _animator.SetTrigger("Damage");
        }
    }
    private void ActiveAtack()
    {
        hitObject.SetActive(true);
    }

    private void OffAtack()
    {
        hitObject.SetActive(false);
    }
}
