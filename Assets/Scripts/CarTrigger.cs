using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTrigger : MonoBehaviour
{
    EnterAndExitCar playerCarInteractionScript;
    private void Start()
    {
        playerCarInteractionScript = FindObjectOfType<EnterAndExitCar>();
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            playerCarInteractionScript.SetCar(gameObject.transform.parent);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerCarInteractionScript.SetCar(null);
        }
    }
}
