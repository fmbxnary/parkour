using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public Animator animator;
    Vector2 speedPercent;

    public bool grounded = true;
    public Vector3 rbVelocity = Vector3.zero;

    public WallRun wallRun;

    void Start()
    {
        wallRun = GetComponent<WallRun>();
        animator = GetComponentInChildren<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        grounded = Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hit, GetComponent<CapsuleCollider>().height / 2f + 0.2f);

        rbVelocity = GetComponent<Rigidbody>().velocity;

        if (grounded)
        {
            speedPercent = new Vector2(Mathf.Clamp(transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity).x, -1f, 1f),
            Mathf.Clamp(transform.InverseTransformDirection(rbVelocity).z, -1f, 1f));

            if (Input.GetKey(KeyCode.LeftShift))
            {
                speedPercent = new Vector3(0f, 2f);
            }
        }
        else 
        {
            if (wallRun.wallLeft)
            {
                speedPercent = new Vector3(-5f, 5f);
            }
           else if (wallRun.wallRight)
            {
                speedPercent = new Vector3(5f, 5f);
            }
            else { 
            speedPercent = new Vector3(10f, 10f); 
        }
        }
        animator.SetFloat("Xaxis", speedPercent.x, 0.1f, Time.deltaTime);
        animator.SetFloat("Yaxis", speedPercent.y, 0.1f, Time.deltaTime);
    }
}
