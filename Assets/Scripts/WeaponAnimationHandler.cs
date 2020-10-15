using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationHandler : MonoBehaviour
{
    Animator animator;
    Gun gun = null;
    Transform weaponHolder;
    bool isAiming = false;
    [SerializeField] float xRotationMultiplier = 3f;
    [Header("To disable")]
    [SerializeField] GameObject camtoDisable;
    [SerializeField] GameObject cineCamToDisable;
    [Header("To enable")]
    [SerializeField] GameObject camtoEnable;
    [SerializeField] CanvasGroup reticleImageCanvasGroup;
    Camera myCamera;
    [SerializeField] GameObject cineCamToEnable;
    private void Start()
    {
        myCamera = GameObject.FindObjectOfType<Camera>();
        animator = GetComponent<Animator>();
        SwitchCams();
        weaponHolder = GameObject.FindGameObjectWithTag("WeaponHolder").transform;
    }
    private void Update()
    {
        SwitchCams();
        foreach (Transform weapon in weaponHolder)
        {
            if (weapon.gameObject.activeSelf == true)
            {
                gun = weapon.gameObject.GetComponent<Gun>();
            }
        }
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
                reticleImageCanvasGroup.alpha = 1;
                reticleImageCanvasGroup.interactable = false;
            }
            else
            {
                reticleImageCanvasGroup.alpha = 0;
                reticleImageCanvasGroup.interactable = true;
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
            try
            {
                if(Input.GetMouseButtonDown(0)&&gun != null)
                {
                    gun.Shoot();
                }
                else if (Input.GetMouseButton(0) && gun !=null)
                {
                    gun.ShootContinious();
                }
            }
            catch
            {
                Debug.Log("Catch executed in weapon animation handler");
            }
        }
    }
    public bool GetIsAiming()
    {
        return isAiming;
    }
}
