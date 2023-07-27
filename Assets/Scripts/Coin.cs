using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Vector3 destination;
    private void Start()
    {
        destination = transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * 6f);
        transform.Rotate(Vector3.up * Time.deltaTime * 50f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.AddCoins(1);
            Destroy(gameObject);
        }
    }
}
