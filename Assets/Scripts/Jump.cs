using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    Animator animator;
    [SerializeField]Camera myCamera;
    [SerializeField] float jumpForwardDistance = 10;
    [SerializeField] float jumpUpDist = 10;
    private float distanceToGround;
    
    Collider playerCollider;
    Rigidbody rb;
    private bool isJumping;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
        playerCollider = gameObject.GetComponent<Collider>();
        distanceToGround = playerCollider.bounds.extents.y;
        
    }
    public bool isOnGround()
    {
        //grounded or not
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f);
    }
    // Update is called once per frame
    void Update()
    {
        JumpForward();
        if (isOnGround() && animator.GetBool("isAiming"))
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else if(!animator.GetBool("isAiming"))
        {
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        }
        //else{
        //also add falling behaviour in else
        //}
    }

    private void JumpForward()
    {
        if (isOnGround() && animator.GetBool("isAiming") == false)
        {
            float verticalAxis = Input.GetAxis("Vertical");
            float horizontalAxis = Input.GetAxis("Horizontal");
            if (Input.GetKeyDown(KeyCode.Space))
            {

                Vector3 cameraForward = Vector3.Scale(myCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
                Vector3 udpatedVector = verticalAxis * cameraForward + horizontalAxis * myCamera.transform.right;
                transform.LookAt(udpatedVector + transform.position);
                animator.SetTrigger("JumpForward");
                animator.applyRootMotion = true;
                isJumping = true;
                transform.position += transform.forward * Time.deltaTime * jumpForwardDistance;
                transform.position += transform.up * Time.deltaTime * jumpUpDist;
            }
        }
        else if (isOnGround() == false)
        {
            animator.applyRootMotion = false;
            isJumping = false;
            animator.ResetTrigger("JumpForward");
        }
        
    }
    public bool IsJumping()
    {
        return isJumping;
    }
}

    
