using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 5f;

    private Rigidbody rb;
    private bool isJumping = false;

    // Reference to the PlayerAnimation script
    private PlayerAnimation playerAnimation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Get the PlayerAnimation script
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }
    }

    void FixedUpdate()
    {
        if (isJumping)
        {
            rb.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
            isJumping = false;
        }
    }
}
