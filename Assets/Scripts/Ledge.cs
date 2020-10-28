using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    public LayerMask grabbableLayer;
    [SerializeField] float rayCastDist = 4f;
    [SerializeField] float heightOfJump = 3f;
    public float grabHeight = 1f;
    [SerializeField] float minWidth = 2f;
    [SerializeField] float miniwait = 2f;
    Vector3 tangent;
    //[SerializeField] RuntimeAnimatorController parcour;
    public bool debugRootMotion = false;
    //RuntimeAnimatorController initRuntime;
    bool isInGrabbingStage = false;
    RaycastHit hit;
    Ray ray;
    Animator animator;
    Jump jumpScript;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
       // initRuntime = animator.runtimeAnimatorController;
        jumpScript = gameObject.GetComponent<Jump>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (jumpScript.IsJumping())
        {
            ray = new Ray(transform.position,transform.forward);
            if (Physics.Raycast(ray,out hit,rayCastDist,grabbableLayer))
            {
                CalculateTangent();
                Debug.DrawRay(transform.position,ray.direction);
                //get height and width
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.GetComponent<BoxCollider>())
                {
                    BoxCollider box = hitObject.GetComponent<BoxCollider>();
                    //transform.eulerAngles = tangent;
                    float height = hit.transform.localScale.y * ((BoxCollider)hit.collider).size.y;
                    float width = hit.transform.localScale.z * ((BoxCollider)hit.collider).size.z;
                    if (heightOfJump > height && width>minWidth&&!isInGrabbingStage)
                    {
                        Debug.Log("Grab");
                        //ClimbOverFence();
                    }
                }
            }
        }
        animator.applyRootMotion = debugRootMotion;
    }
    
    void CalculateTangent()
    {
        Vector3 normal = hit.normal;
        Vector3 t1 = Vector3.Cross(normal, Vector3.forward);
        Vector3 t2 = Vector3.Cross(normal, Vector3.up);
        if (t1.magnitude > t2.magnitude)
        {
            tangent = t1;
        }
        else
        {
            tangent = t2;
        }
    }
    public bool GetGrabbing()
    {
        return isInGrabbingStage;
    }
}
