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
    [SerializeField] Cinemachine.CinemachineFreeLook freeLookCam;
    [SerializeField] LayerMask bars; // The layer mask for bars
    Vector3 climbDirection;
    GameObject hangingBar;

    bool isClimbing = false; 
    bool isRunning = false;
    bool isJumped = false;
    bool isWalking = false;
    bool isBackWalking = false;
    bool isLeftWalking = false;
    bool isRightWalking = false;
    bool isSliding = false;
    bool isHanging = false;

    public AudioSource jumpRaiseSound;
    public AudioSource playerJump;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHanging)
        {
            // Disable the player's gravity while hanging
            rb.useGravity = false;

            // Calculate the position where the player should hang
            Vector3 hangPosition = hangingBar.transform.position;
            hangPosition.y -= 0.7f;  // Adjust this value to match the length of the player's arms
            transform.position = hangPosition;
            // Make the player hang in place
            rb.velocity = Vector3.zero;

            if (Input.GetButtonDown("Jump"))
            {
                // Enable the player's gravity before jumping
                rb.useGravity = true;
                isHanging = false;
                hangingBar = null;  // Clear the bar
                                    // Jump in the forward direction of the player
                JumpInDirection(transform.forward);
            }

            return;
        }
        if (isSliding)
        {
            return;
        }
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (!isClimbing && !isSliding)
        {
            float speed = movementSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed *= 2f;
            }

            // Rotate the player to face the direction the camera is facing
            Quaternion playerRotation = Quaternion.Euler(0, freeLookCam.m_XAxis.Value, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, Time.deltaTime * speed);

            
                Vector3 moveDirection = verticalInput * transform.forward + horizontalInput * transform.right;
                rb.velocity = new Vector3(moveDirection.x * speed, rb.velocity.y, moveDirection.z * speed);
            

            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                playerJump.Play();
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
        RaycastHit hit;
        // check for climbable surfaces
        if (verticalInput > 0) 
        {
            
            if (Physics.Raycast(transform.position, transform.forward, out hit, climbCheckDistance, climbable))
            {
                climbDirection = Vector3.up; 
                climbDirection.Normalize(); 
                isClimbing = true; 
            }
        }




        if (Physics.SphereCast(transform.position, 0.5f, transform.forward, out hit, climbCheckDistance, bars))
        {
            isHanging = true;
            hangingBar = hit.collider.gameObject;  // Store the bar
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
        bool isFalling = !IsGrounded() && rb.velocity.y < 0;
        animator.SetBool("falling", isFalling);

        animator.SetBool("hang", isHanging);

    }

    IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("slide", true);

        // Store the player's original rotation and movement direction
        Quaternion originalRotation = transform.rotation;
        Vector3 slideDirection = rb.velocity.normalized;

        // Temporarily disable the camera's follow speed
        var originalFollowSpeed = freeLookCam.m_XAxis.m_MaxSpeed;
        freeLookCam.m_XAxis.m_MaxSpeed = 0;

        // Set the player's velocity to slide them in their moving direction
        rb.velocity = slideDirection * movementSpeed * 2f;

        // Rotate the player by -90 degrees around the x-axis
        transform.rotation = Quaternion.Euler(-90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Reset the player's rotation to their original rotation
        transform.rotation = originalRotation;

        // Reset the camera's follow speed
        freeLookCam.m_XAxis.m_MaxSpeed = originalFollowSpeed;

        animator.SetBool("slide", false);
        isSliding = false;
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy Head"))
        {
            Destroy(collision.transform.parent.gameObject);
            Jump();
        }
        else if (collision.gameObject.CompareTag("Trampoline"))
        {
            jumpForce = 10f;
            Jump();
            jumpForce = 5f;
            jumpRaiseSound.Play();
        }
        
    }
    void JumpInDirection(Vector3 direction)
    {
        // Combine the forward force with an upward force
        Vector3 force = direction * jumpForce + Vector3.up * jumpForce;
        rb.velocity = force;
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
