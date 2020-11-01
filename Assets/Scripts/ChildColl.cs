using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildColl : MonoBehaviour
{
    VaultOver vault;
    private bool detected = false;
    private void Start()
    {
        vault = GameObject.FindGameObjectWithTag("Player").GetComponent<VaultOver>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == vault.GetVaultTag())
        {
            detected = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == vault.GetVaultTag())
        {
            detected = false;
        }
    }
    public bool IsDetected()
    {
        return detected;
    }

}
