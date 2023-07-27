using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Octopus : Enemy
{
    [SerializeField] GameObject shotPrefab;
    [SerializeField] float moveDistance = 5f;
    [SerializeField] float movePause = 2f;
    [SerializeField] float shootingSpeed = 1f;

    float currentDistance = 0;
    NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        base.Update();
        Ray ray = new Ray(transform.position, (player.position+Vector3.up - transform.position).normalized);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Player")
            {
                ChangeState(EnemyState.Shooting);
            } else
            {
                ChangeState(EnemyState.Moving);
            }
        }
        switch (state)
        {
            case EnemyState.Moving:
                agent.SetDestination(player.position);
                break;
            case EnemyState.Shooting:
                LookRotation(player.position - transform.position);
                break;
        }
    }

    public void Shot()
    {
        if (state != EnemyState.Shooting)
            return;
        Transform shot = Instantiate(shotPrefab).transform;
        shot.transform.position = transform.position + Vector3.up;
        shot.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        shot.GetComponent<OctopusShot>().damage = damage;
    }

    protected void ChangeState(EnemyState newState)
    {
        if (state == newState)
            return;
        state = newState;
        switch (newState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Moving:
                agent.isStopped = false;
                animator.SetBool("Shooting", false);
                animator.SetBool("Moving", true);
                break;
            case EnemyState.Shooting:
                agent.isStopped = true;
                animator.SetBool("Moving", false);
                animator.SetBool("Shooting", true);
                break;
        }
    }

    void LookRotation(Vector3 direction)
    {
        direction.y = 0;
        if (direction.magnitude > 0)
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
}
