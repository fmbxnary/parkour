using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator myAnimator;
    bool isRunning = false;
    bool isJumped = false;
    bool isWalking = false;
    bool isBackWalking = false;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isRunning = Input.GetKey(KeyCode.LeftShift);
        myAnimator.SetBool("run", isRunning);

        isJumped = Input.GetKey(KeyCode.Space);
        myAnimator.SetBool("jump", isJumped);

        isWalking = Input.GetKey(KeyCode.W);
        myAnimator.SetBool("walk", isWalking);

        isBackWalking = Input.GetKey(KeyCode.S);
        myAnimator.SetBool("backWalk", isBackWalking);
    }
}
