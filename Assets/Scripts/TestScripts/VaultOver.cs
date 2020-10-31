using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultOver : MonoBehaviour
{
    [SerializeField] private string vaultTag = "Wall";
    bool canVault;
    [SerializeField] GameObject upperTrans;
    [SerializeField] GameObject lowerTrans;
    [SerializeField] Transform placeOfRaycast;
    [SerializeField] float raydist = 2f;
    [SerializeField] RuntimeAnimatorController leap;
    RuntimeAnimatorController initRuntime;
    private float vaultTime;
    Vector3 vaultEndPos;
    Animator animator;
    //Vector3 recordedStartPosition;
    bool isParcour;
    private float t_parkour;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        initRuntime = animator.runtimeAnimatorController;
    }
   
    private void Update()
    {
        Debug.DrawRay(placeOfRaycast.position, placeOfRaycast.forward, Color.green);
        JumpOverLedge();
        if (t_parkour >= 1f)
        {
            t_parkour = 0f;
            isParcour = false;
            animator.runtimeAnimatorController = initRuntime;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<CharacterController>().enabled = true;
        }
    }

    private void JumpOverLedge()
    {
        if (!upperTrans.GetComponent<ChildColl>().IsDetected() && lowerTrans.GetComponent<ChildColl>().IsDetected())
        {
            canVault = true;
            Debug.Log("Can vault");
            if (canVault && !isParcour)
            {
                isParcour = true;
            }
            // Debug.Log(t_parkour);
            if (isParcour)
            {
                if (t_parkour < 1f)
                {
                    //recordedStartPosition = transform.position;
                    vaultEndPos = GetVaultEnd();
                    animator.runtimeAnimatorController = leap;
                    if (animator.runtimeAnimatorController == leap)
                    {
                        Debug.Log("Leap override"); 
                        vaultTime = animator.GetCurrentAnimatorStateInfo(0).length;
                    }

                    StartCoroutine(LerpTheChar());
                    ///
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    gameObject.GetComponent<CharacterController>().enabled = false;
                    ///
                }                

            }            
        }
    }

    public string GetVaultTag()
    {
        return vaultTag;
    }

    private Vector3 GetVaultEnd()
    {
        RaycastHit hit;
        Ray ray = new Ray(placeOfRaycast.position,placeOfRaycast.forward);
        if (Physics.Raycast(ray, out hit, raydist))
        {
            Debug.Log(hit.collider.name);
            return hit.point;
        }
        else
        {
            return new Vector3();
        }
        
    }
    IEnumerator LerpTheChar()
    {
        
            yield return StartCoroutine(VaultToPoint());
            yield return new WaitForSeconds(0.3f);
            //if (!isParcour|| t_parkour >= 1f)
            //{
            //    break;
            //}
        
    }
    IEnumerator VaultToPoint()
    {
        while (t_parkour < 1f)
        {
            t_parkour += Time.deltaTime / vaultTime;
            transform.position = Vector3.Lerp(transform.position, vaultEndPos, t_parkour);
            
            if (t_parkour >= 1f)
            {
                break;
            }
            yield return null;
        }
    }
}
