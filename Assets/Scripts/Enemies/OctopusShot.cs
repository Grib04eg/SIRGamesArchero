using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusShot : MonoBehaviour
{
    [SerializeField] GameObject hitPrefab;
    [SerializeField] float speed;
    public float damage;
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
            other.GetComponent<PlayerController>().DoDamage(damage);
            Instantiate(hitPrefab, other.transform.position, Quaternion.identity);
        } else if (other.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }
}
