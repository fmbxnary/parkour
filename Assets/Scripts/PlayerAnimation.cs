using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    Vector2 speedPercent;
    Vector3 rbVelocity = Vector3.zero;
    private bool isGrounded = false;
    public Transform groundCheck;
    public LayerMask groundObjects;
    public float checkRadius = 0.5f;


    void Start()
    {
        animator = GetComponentInChildren<Animator>();

    }
    // Update is called once per frame
    void Update()
    {
        rbVelocity = GetComponent<Rigidbody>().velocity;
        isGrounded = Physics.CheckSphere(groundCheck.position, checkRadius, groundObjects);

        if (isGrounded)
        {
            speedPercent = new Vector2(Mathf.Clamp(transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity).x, -1f, 1f),
        Mathf.Clamp(transform.InverseTransformDirection(rbVelocity).z, -1f, 1f));
        }
        else
        {
            speedPercent = new Vector2(10f, 10f);
        }
        
        
        animator.SetFloat("Xaxis", speedPercent.x, 0.1f, Time.deltaTime);
        animator.SetFloat("Yaxis", speedPercent.y, 0.1f, Time.deltaTime);
    }
}
