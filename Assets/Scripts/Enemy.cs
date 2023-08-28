using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    [SerializeField] float maxHP = 100;
    [SerializeField] protected float damage = 15;
    float HP;

    protected Transform player;
    Slider hpBar;
    protected Animator animator;
    protected EnemyState state;

    public UnityAction<Enemy> OnDeath;

    protected enum EnemyState
    {
        Idle,
        Moving,
        Shooting
    }
    void Awake()
    {
        animator = GetComponent<Animator>();
        HP = maxHP;
    }

    public void Init(Slider hpBar, Transform player)
    {
        this.player = player;
        this.hpBar = hpBar;
        hpBar.maxValue = maxHP;
        hpBar.value = maxHP;
    }

    protected virtual void Update()
    {
        if (HP > 0)
            hpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 50f;
    }

    public void DoDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Death();
        } else
        {
            hpBar.value = HP;
        }
    }

    void Death()
    {
        Vector3 pos = transform.position;
        pos.y = 0.25f;
        for (int i = 0; i < Random.Range(2,7); i++)
        {
            Instantiate(coinPrefab, pos, Quaternion.identity);
        }
        GetComponent<Collider>().enabled = false;
        Destroy(hpBar.gameObject);
        animator.SetTrigger("Death");
        Destroy(gameObject, 1f);
        OnDeath?.Invoke(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().DoDamage(damage);
        }
    }
}
