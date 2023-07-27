using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController)), RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;

    [SerializeField] Slider hpBar;
    [SerializeField] FloatingJoystick joystick;

    [SerializeField] float maxHP = 100;
    [SerializeField] float speed;
    [SerializeField] float shootingSpeed;
    [SerializeField] float damage;

    CharacterController characterController;
    Animator animator;
    PlayerState state;

    float HP = 100;
    enum PlayerState
    {
        Idle,
        Moving,
        Shooting,
        Dead
    }

    void Start()
    {
        HP = maxHP;
        hpBar.maxValue = maxHP;
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animator.SetFloat("ShootingSpeed", shootingSpeed);
    }


    // Update is called once per frame
    void Update()
    {
        if (state == PlayerState.Dead)
            return;
        if (HP > 0)
            hpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position) - Vector3.up * 50f;
        if (joystick.Direction.magnitude == 0)
        {
            ChangeState(PlayerState.Shooting);
        } else
        {
            ChangeState(PlayerState.Moving);
        }

        switch (state)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Moving:
                Moving();
                break;
            case PlayerState.Shooting:
                Shooting();
                break;
        }
    }

    void ChangeState(PlayerState newState)
    {
        if (state == newState)
            return;
        state = newState;
        switch (newState)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Moving:
                animator.SetBool("Shooting", false);
                animator.SetBool("Moving", true);
                break;
            case PlayerState.Shooting:
                animator.SetBool("Moving", false);
                break;
        }
    }

    public void Shot()
    {
        if (state != PlayerState.Shooting)
            return;
        Transform arrow = Instantiate(arrowPrefab).transform;
        arrow.transform.position = transform.position + Vector3.up;
        arrow.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        arrow.GetComponent<Arrow>().damage = damage;
    }

    void Shooting()
    {
        Enemy closestEnemy = GameManager.instance.GetClosestEnemy(transform.position);
        if (closestEnemy != null)
        {
            animator.SetBool("Shooting", true);
            LookRotation(closestEnemy.transform.position - transform.position);
        } else
        {
            animator.SetBool("Shooting", false);
        }
    }

    void Moving()
    {
        animator.SetBool("Shooting", false);
        Vector3 direction = new Vector3(joystick.Direction.x, 0, joystick.Direction.y).normalized;
        characterController.Move(direction * Time.deltaTime * speed);
        LookRotation(direction);
        animator.SetBool("Moving", true);
    }

    public void DoDamage(float damage)
    {
        if (state == PlayerState.Dead)
            return;
        HP -= damage;
        if (HP <= 0)
        {
            Death();
        }
        else
        {
            hpBar.value = HP;
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    void Death()
    {
        state = PlayerState.Dead;
        animator.SetTrigger("Death");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "End")
        {
            SceneManager.LoadScene(0);
        }
    }

    void LookRotation(Vector3 direction)
    {
        direction.y = 0;
        if (direction.magnitude > 0)
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
}