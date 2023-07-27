using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] float groundWidth = 5;
    [SerializeField] float speed = 7;
    void Start()
    {
        GetComponent<Camera>().orthographicSize = Screen.height / (float)Screen.width * groundWidth;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Vector3.forward * target.position.z + offset, Time.deltaTime * speed);
    }
}
