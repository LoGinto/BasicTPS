using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    public LayerMask grabbableLayer;
    [SerializeField] float rayCastDist = 4f;
    [SerializeField] float heightOfJump = 3.5f;
    [SerializeField] float minWidth = 2f;
    RaycastHit hit;
    Ray ray;
    Animator animator;
    Jump jumpScript;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
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
                //get height and width
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.GetComponent<BoxCollider>())
                {
                    BoxCollider box = hitObject.GetComponent<BoxCollider>();
                    float height = hit.transform.localScale.y * ((BoxCollider)hit.collider).size.y;
                    float width = hit.transform.localScale.z * ((BoxCollider)hit.collider).size.z;
                    if (heightOfJump > height && width>minWidth)
                    {
                        Debug.Log("Grab");
                    }
                }
            }
        }        
    }
}
