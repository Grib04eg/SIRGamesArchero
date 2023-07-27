using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy
{
    [SerializeField] float rotationSpeed;
    [SerializeField] float movingSpeed;
    void Start()
    {

    }
    protected override void Update()
    {
        base.Update();
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);

        transform.position += transform.forward * Time.deltaTime * movingSpeed;
    }
}
