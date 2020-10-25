using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    public LayerMask grabbableLayer;
    [SerializeField] float rayCastDist = 4f;
    [SerializeField] float heightOfJump = 3.5f;
    [SerializeField] float minWidth = 2f;
    [SerializeField] float miniwait = 2f;
    Vector3 tangent;
    [SerializeField] RuntimeAnimatorController parcour;
    public bool debugRootMotion = false;
    RuntimeAnimatorController initRuntime;
    bool isInGrabbingStage = false;
    RaycastHit hit;
    Ray ray;
    Animator animator;
    Jump jumpScript;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        initRuntime = animator.runtimeAnimatorController;
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
                //get height and width
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.GetComponent<BoxCollider>())
                {
                    BoxCollider box = hitObject.GetComponent<BoxCollider>();
                    transform.rotation = Quaternion.FromToRotation(tangent, hit.normal);
                    float height = hit.transform.localScale.y * ((BoxCollider)hit.collider).size.y;
                    float width = hit.transform.localScale.z * ((BoxCollider)hit.collider).size.z;
                    if (heightOfJump > height && width>minWidth&&!isInGrabbingStage)
                    {
                        Debug.Log("Grab");
                        ClimbOverFence();
                    }
                }
            }
        }
        animator.applyRootMotion = debugRootMotion;
    }
    void ClimbOverFence()
    {
        isInGrabbingStage = true;
        animator.runtimeAnimatorController = parcour as RuntimeAnimatorController;
        animator.SetBool("JumpClimb", true);
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        StartCoroutine(WaitAndTurnBack());
    }
    IEnumerator WaitAndTurnBack()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("JumpClimb", false);
        yield return new WaitForSeconds(miniwait);
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        animator.runtimeAnimatorController = initRuntime as RuntimeAnimatorController;
        isInGrabbingStage = false;
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
}
