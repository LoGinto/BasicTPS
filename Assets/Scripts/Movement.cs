using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] float aimWalkingSpeed = 1.5f;
    Animator animator;
    Cover coverScript;
    Camera myCamera;
    CharacterController characterController;
    private void Start()
    {
        myCamera = GameObject.FindObjectOfType<Camera>();
        animator = GetComponent<Animator>();
        coverScript = GetComponent<Cover>();
        characterController = GetComponent<CharacterController>();
    }
    private void FixedUpdate()
    {
        if(!myCamera.isActiveAndEnabled)
        {
            myCamera = GameObject.FindObjectOfType<Camera>();
            Debug.Log("cam " + myCamera.gameObject.name);
        }
        if (coverScript.IsInCover())
        {
            MoveInCover();
        }
        else
        {
            if (!animator.GetBool("isAiming") || gameObject.GetComponent<WeaponAnimationHandler>().GetIsAiming() == false)
            {
                Move();
            }
            else if (animator.GetBool("isAiming") || gameObject.GetComponent<WeaponAnimationHandler>().GetIsAiming())
            {
                AimMove();
            }
        }
        
    }
    void MoveInCover()
    {
        float axis = Input.GetAxis("Horizontal");
        Debug.Log(axis);          
        if (axis > 0)
        {
            //facingRight = false;
            coverScript.isFacingRight = true;
            animator.runtimeAnimatorController = coverScript.CoverOvveride();
            
        }
        else if (axis < 0)
        {
            //facingRight = true;
            coverScript.isFacingRight = false;
            animator.runtimeAnimatorController = coverScript.LeftCoverOvveride();
        }
        if(axis != 0)
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);    
        }
        Vector3 moveDirection = axis * coverScript.GetTangent();
        //transform.Translate(moveDirection * coverScript.moveInCoverSpeed, Space.World);
        characterController.Move(moveDirection * coverScript.moveInCoverSpeed);
    }
    void Move()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");
        Vector3 cameraForward = Vector3.Scale(myCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 udpatedVector = verticalAxis * cameraForward + horizontalAxis * myCamera.transform.right;
        transform.LookAt(udpatedVector+transform.position);
        Vector3 actualMovement = udpatedVector * speed * Time.fixedDeltaTime;
        animator.SetBool("Walk", true);
        characterController.Move(actualMovement);
        if (verticalAxis == 0 && horizontalAxis == 0)//if player doesn't move
        {
            animator.SetBool("Walk", false);
        }
    }
    void AimMove()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");
        transform.position += verticalAxis * transform.forward * Time.deltaTime * aimWalkingSpeed;
        transform.position += horizontalAxis * transform.right * Time.deltaTime * aimWalkingSpeed;
    }
}
