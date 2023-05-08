using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Transform playerTransform;
    private Coroutine rotationCoroutine;

    public float moveSpeed = 2f;
    public float jumpStrength = 5f;
    public float rotationTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        playerTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }

        float speedMultiplier = Input.GetKey(KeyCode.LeftShift) ? 2f : 1f;

        Vector3 desiredVelocity = playerTransform.TransformDirection(speedMultiplier * moveSpeed * new Vector3(move.x, 0, move.y));
        rb.velocity = new Vector3(desiredVelocity.x, rb.velocity.y, desiredVelocity.z);

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    void Jump()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerTransform.position, -playerTransform.up, out hit, 1.2f))
        {
            rb.AddForce(playerTransform.up * jumpStrength, ForceMode.Impulse);
        }
    }
}
