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
    [SerializeField] float vaultTime = 3f;
    Vector3 vaultEndPos;
    bool isParcour;
    private float t_parkour;
    private void Update()
    {
        Debug.DrawRay(placeOfRaycast.position, placeOfRaycast.forward, Color.green);
        JumpOverLedge();
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
                    Vector3 recordedStartPosition = transform.position;
                    vaultEndPos = GetVaultEnd();
                    while (t_parkour < 1f)
                    {
                        t_parkour += Time.deltaTime / vaultTime;
                        transform.position = Vector3.Lerp(recordedStartPosition, vaultEndPos, t_parkour);
                        if (t_parkour >= 1f)
                        {
                            break;
                        }
                    }

                    ///
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    gameObject.GetComponent<CharacterController>().enabled = false;
                    ///
                }
                
            }
            if (t_parkour >= 1f)
            {
                t_parkour = 0f;
                isParcour = false;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                gameObject.GetComponent<CharacterController>().enabled = true;
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
}
