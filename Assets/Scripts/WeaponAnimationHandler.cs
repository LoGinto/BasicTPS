using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationHandler : MonoBehaviour
{
    Animator animator;
    bool isAiming = false;
    [SerializeField] float xRotationMultiplier = 3f;
    [Header("To disable")]
    [SerializeField] GameObject camtoDisable;
    [SerializeField] GameObject cineCamToDisable;
    [Header("To enable")]
    [SerializeField] GameObject camtoEnable;
    Camera myCamera;
    [SerializeField] GameObject cineCamToEnable;
    private void Start()
    {
        myCamera = GameObject.FindObjectOfType<Camera>();
        animator = GetComponent<Animator>();
        SwitchCams();
    }
    private void Update()
    {
        SwitchCams();       
        if (!myCamera.isActiveAndEnabled||myCamera == null)
        {
            myCamera = GameObject.FindObjectOfType<Camera>();
            Debug.Log("cam " + myCamera.gameObject.name);
        }
        if (animator.GetBool("isAiming")||isAiming == true)
        {
            OnAiming();
        }
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = !isAiming;
            if (isAiming)
            {              
                Vector3 cameraForward = Vector3.Scale(myCamera.transform.forward, new Vector3(1, 0, 1)).normalized;                
                transform.LookAt(cameraForward + transform.position);
            }
            animator.SetBool("isAiming", isAiming);
        }
    }

    private void SwitchCams()
    {
        cineCamToEnable.SetActive(animator.GetBool("isAiming"));
        camtoEnable.SetActive(animator.GetBool("isAiming"));
        camtoDisable.SetActive(!animator.GetBool("isAiming"));
        cineCamToDisable.SetActive(!animator.GetBool("isAiming"));
    }

    void OnAiming()
    {
        //fuck IK              
        float horintalMouseInput = Input.GetAxis("Mouse X") * xRotationMultiplier;
        transform.Rotate(0, horintalMouseInput, 0);
        if (animator.GetInteger("WeaponType") >0)
        {
            float mousePos = Input.mousePosition.y / Screen.height;
            animator.SetFloat("Aim", mousePos);
        }
    }
    public bool GetIsAiming()
    {
        return isAiming;
    }
}
