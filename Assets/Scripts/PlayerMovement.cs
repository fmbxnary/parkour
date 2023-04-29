using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    Animator animator;
    [SerializeField] float movementSpeed = 6f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float climbSpeed = 2f; // new variable for climb speed
    [SerializeField] float climbCheckDistance = 1f; // new variable for climb check distance

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;
    [SerializeField] LayerMask climbable;

    bool isClimbing = false; // new variable to track if player is climbing
    Vector3 climbDirection; // new variable to track the direction to climb

    bool isRunning = false;
    bool isJumped = false;
    bool isWalking = false;
    bool isBackWalking = false;
    bool isLeftWalking = false;
    bool isRightWalking = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (!isClimbing) // only move normally if not climbing
        {
            rb.velocity = new Vector3(horizontalInput * movementSpeed, rb.velocity.y, verticalInput * movementSpeed);

            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                Jump();
            }
        }
        else // climb up the wall
        {
            rb.velocity = climbDirection * climbSpeed;

            if (transform.position.y >= climbDirection.y) // if reached the top, stop climbing
            {
                isClimbing = false;
            }
        }

        // check for climbable surfaces
        if (verticalInput > 0) // climbing up
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, climbCheckDistance, climbable))
            {
                climbDirection = Vector3.up + hit.normal; // set climb direction to the surface normal plus up
                climbDirection.Normalize(); // normalize the vector
                isClimbing = true; // start climbing
            }
        }
        else if (verticalInput < 0) // reversing climb
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.forward, out hit, climbCheckDistance, climbable))
            {
                climbDirection = -Vector3.up - hit.normal; // set climb direction to the opposite of surface normal minus down
                climbDirection.Normalize(); // normalize the vector
                isClimbing = true; // start climbing
            }
        }

        // Update animator parameters
        isRunning = Input.GetKey(KeyCode.LeftShift);
        animator.SetBool("run", isRunning);

        isJumped = Input.GetKey(KeyCode.Space);
        animator.SetBool("jump", isJumped);

        isWalking = Input.GetKey(KeyCode.W);
        animator.SetBool("walk", isWalking);

        isBackWalking = Input.GetKey(KeyCode.S);
        animator.SetBool("backWalk", isBackWalking);

        isLeftWalking = Input.GetKey(KeyCode.A);
        animator.SetBool("leftWalk", isLeftWalking);

        isRightWalking = Input.GetKey(KeyCode.D);
        animator.SetBool("rightWalk", isRightWalking);

        animator.SetBool("climb", isClimbing);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy Head"))
        {
            Destroy(collision.transform.parent.gameObject);
            Jump();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, .1f, ground);
    }
}
