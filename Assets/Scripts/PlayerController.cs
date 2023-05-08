using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Animator animator;
    [SerializeField] float movementSpeed = 6f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float climbSpeed = 2f;
    [SerializeField] float climbCheckDistance = 1f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;
    [SerializeField] LayerMask climbable;

    Vector3 climbDirection;

    bool isClimbing = false;
    bool isRunning = false;
    bool isJumped = false;
    bool isWalking = false;
    bool isBackWalking = false;
    bool isLeftWalking = false;
    bool isRightWalking = false;
    bool isSliding = false;
    bool isRunningAndJump = false;

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

        if (!isClimbing)
        {
            float speed = movementSpeed;
            if (Input.GetKey(KeyCode.LeftShift) && isWalking && !isSliding)
            {
                speed *= 2f;
            }
            rb.velocity = new Vector3(horizontalInput * speed, rb.velocity.y, verticalInput * speed);

            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.C) && IsGrounded() && isRunning)
            {
                StartCoroutine(Slide());
            }
        }
        else
        {
            rb.velocity = climbDirection * climbSpeed;

            if (transform.position.y >= climbDirection.y)
            {
                isClimbing = false;
            }
        }

        // check for climbable surfaces
        if (verticalInput > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, climbCheckDistance, climbable))
            {
                climbDirection = Vector3.up;
                climbDirection.Normalize();
                isClimbing = true;
            }
        }
    }

    IEnumerator Slide()
    {
        
        // Store the player's original rotation
        Quaternion originalRotation = transform.rotation;

        // Set the player's velocity to slide them in the z direction
        rb.velocity = new Vector3(0f, 0f, rb.velocity.z).normalized * movementSpeed * 2f;

        // Rotate the player by -90 degrees around the x-axis
        transform.rotation = Quaternion.Euler(-90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Reset the player's rotation to their original rotation
        transform.rotation = originalRotation;
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
