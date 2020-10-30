using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_Snap : MonoBehaviour
{
    public Transform IKRayOriginLeft;
    public Transform IKRayOriginRight;
    public bool useIK;
    public bool leftHandIK;
    public bool rightHandIK;
    public Quaternion leftHandRot;
    public Quaternion rightHandRot;
    public Vector3 leftHandPos;
    public Vector3 rightHandPos;
    private Animator animator;
    LayerMask grabbableLayer;
    //[Header("Ray vectors")]
    //public Vector3 originOffset = new Vector3(0,2f,0.5f);
    //public Vector3 leftVector = new Vector3(-0.5f,0,0);
    //public Vector3 rightVector = new Vector3(0.5f, 0, 0);
    Ledge ledgeScript;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        ledgeScript = GetComponent<Ledge>();
        grabbableLayer = ledgeScript.grabbableLayer;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ledgeScript.GetGrabbing())
        {
            RaycastHit L_hit;
            RaycastHit R_hit;
            //Physics.Raycast(IKRayOrigin.transform.position + originOffset, -IKRayOrigin.transform.up + leftVector, out L_hit, ledgeScript.grabHeight)
            if (Physics.Raycast(IKRayOriginLeft.position, IKRayOriginLeft.forward, out L_hit, grabbableLayer))
            {
                
                    leftHandIK = true;
                    Debug.Log(L_hit.collider.name);
                    leftHandPos = L_hit.point;
                    leftHandRot = Quaternion.FromToRotation(Vector3.forward, L_hit.transform.right);
                
            }
            else
            {
                leftHandIK = false;
            }
            //Physics.Raycast(IKRayOrigin.transform.position + originOffset, -IKRayOrigin.transform.up + rightVector, out R_hit, ledgeScript.grabHeight)
            if (Physics.Raycast(IKRayOriginRight.position, IKRayOriginRight.forward, out R_hit, grabbableLayer))
            {
                
                    Debug.Log(R_hit.collider.name);
                    rightHandIK = true;
                    rightHandPos = R_hit.point;
                    rightHandRot = Quaternion.FromToRotation(Vector3.forward, R_hit.transform.right);
                
            }
            else
            {
                rightHandIK = false;
            }

        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (useIK && ledgeScript.GetGrabbing() && leftHandIK|| rightHandIK)
        {
            if (leftHandIK)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                animator.SetIKPosition(AvatarIKGoal.LeftHand,leftHandPos);

                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandRot);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            }
            if (rightHandIK)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandPos);

                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandRot);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            }
        }
    }
    private void Update()
    {
        ///Debug.DrawRay(IKRayOrigin.transform.position + originOffset, -IKRayOrigin.transform.up + leftVector,Color.green);
        //Debug.DrawRay(IKRayOrigin.transform.position + originOffset, -IKRayOrigin.transform.up + rightVector, Color.green);
        Debug.DrawRay(IKRayOriginRight.position, IKRayOriginRight.forward, Color.green);
        Debug.DrawRay(IKRayOriginLeft.position, IKRayOriginLeft.forward, Color.green);
    }
}
