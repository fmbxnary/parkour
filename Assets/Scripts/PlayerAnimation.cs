using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    Vector2 speedPercent;

    Vector3 rbVelocity = Vector3.zero;


    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        rbVelocity = GetComponent<Rigidbody>().velocity;

        speedPercent = new Vector2(Mathf.Clamp(transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity).x, -1f, 1f),
        Mathf.Clamp(transform.InverseTransformDirection(rbVelocity).z, -1f, 1f));
        
        animator.SetFloat("Xaxis", speedPercent.x, 0.1f, Time.deltaTime);
        animator.SetFloat("Yaxis", speedPercent.y, 0.1f, Time.deltaTime);
    }
}
