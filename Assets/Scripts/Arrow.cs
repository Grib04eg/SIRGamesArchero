using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
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
        if (other.tag == "Enemy")
        {
            Destroy(gameObject);
            other.GetComponent<Enemy>().DoDamage(damage);
            Instantiate(hitPrefab, other.transform.position, Quaternion.identity);
        } else if (other.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }
}
