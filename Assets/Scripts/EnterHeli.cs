using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterHeli : MonoBehaviour
{
    [SerializeField] float rayCastDist = 3f;
    [SerializeField] GameObject[] cams;
    [SerializeField] GameObject[] cinemachineCam;
    // Start is called before the first frame update    
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        if(Physics.Raycast(ray,out hit, rayCastDist))
        {
            if (hit.collider.tag == "Helicopter")
            {
                if (Input.GetKeyDown(KeyCode.F) && hit.collider.GetComponent<Helicopter_Base>())
                {
                    hit.collider.GetComponent<Helicopter_Base>().SetEntryState(true);
                    transform.parent = hit.collider.transform;
                    this.gameObject.SetActive(false);
                    ToggleCams(false);
                    Debug.Log("Exectuted entry");
                }
            }
        }
    }

    public void ToggleCams(bool value)
    {
        if (value == true)
        {
            cams[0].SetActive(value);
            cinemachineCam[0].SetActive(value);
        }
        else
        {
            foreach (GameObject cam in cams)
            {
                foreach (GameObject cinecam in cinemachineCam)
                {
                    cam.SetActive(value);
                    cinecam.SetActive(value);
                }
            }
        }
    }
}
