using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingAxe : MonoBehaviour
{
    public float speed = 1.0f;
    public float maxRotation = 90.0f;

    private Vector3 startRotation;

    void Start()
    {
        startRotation = transform.rotation.eulerAngles;
    }

    void Update()
    {
        float movement = Mathf.Sin(Time.time * speed);

        movement *= maxRotation;

        Vector3 newRotation = new Vector3(startRotation.x, startRotation.y, startRotation.z + movement);

        transform.rotation = Quaternion.Euler(newRotation);
    }
}

