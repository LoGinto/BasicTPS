using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    [SerializeField] Transform rayCastPoint;
    [SerializeField] LayerMask coverLayer;
    public  float coverDetectionDist = 1f;
    public float moveInCoverSpeed = 3f;
    public bool isFacingRight = false;
    [SerializeField] RuntimeAnimatorController coverOverride;
    [SerializeField] AnimatorOverrideController coverLeftOvveride;
    RuntimeAnimatorController initRuntimeOvverride;
    Ray ray;
    Animator animator;
    RaycastHit hit;
    bool isInCover = false;
    bool detectCover = false;
    Vector3 tangent;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        initRuntimeOvverride = animator.runtimeAnimatorController;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isInCover == false)
            {
                isFacingRight = true;
                TakeCover();
            }
            else
                ExitCover();

        }
        Debug.DrawRay(rayCastPoint.position, rayCastPoint.forward, Color.green);
    }
    void TakeCover()
    {
        ray = new Ray(rayCastPoint.position, rayCastPoint.forward);
        if(Physics.Raycast(ray,out hit, coverDetectionDist, coverLayer))
        {
            //have to ask how to " stick to walls"
            isInCover = true;
            CalculateTangent();
            Debug.Log("tangent " + tangent);
            animator.runtimeAnimatorController = coverOverride;
        }
    }
    void ExitCover()
    {
        isInCover = false;
        animator.runtimeAnimatorController = initRuntimeOvverride;
    }
    public void CalculateTangent()
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
    public Vector3 GetTangent()
    {
        return tangent;
    }
    public bool IsInCover()
    {
        return isInCover;
    }
    public RuntimeAnimatorController CoverOvveride()
    {
        return coverOverride;
    }
    public RuntimeAnimatorController LeftCoverOvveride()
    {
        return coverLeftOvveride as RuntimeAnimatorController;
    }
    public LayerMask GetCoverLayerMask()
    {
        return coverLayer;
    }
}
