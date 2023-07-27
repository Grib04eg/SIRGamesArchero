using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Golem : Enemy
{
    [SerializeField] float moveDistance = 5f;
    [SerializeField] float movePause = 2f;

    Coroutine movingCoroutine;
    NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        movingCoroutine = StartCoroutine(Moving());
    }

    IEnumerator Moving()
    {
        while (true)
        {
            Vector3 position;
            while (true)
            {
                position = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * moveDistance + transform.position;
                Ray ray = new Ray(position + Vector3.up * 5, Vector3.down);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Ground")
                    {
                        animator.SetBool("Moving", true);
                        agent.isStopped = false;
                        agent.SetDestination(position);
                        break;
                    }
                }
                yield return new WaitForEndOfFrame();
            }
            while (true)
            {
                if (Vector3.Distance(transform.position, position) < 0.5f)
                {
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
            animator.SetBool("Moving", false);
            agent.isStopped = true;
            yield return new WaitForSeconds(movePause);
        }
    }
}
