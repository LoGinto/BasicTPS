using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAgain : MonoBehaviour
{
    [SerializeField] GameObject player;
    bool hasSetToFalse;
    private void Update()
    {
        if (hasSetToFalse&&Input.GetKeyDown(KeyCode.F)&&!player.activeSelf)
        {
            player.SetActive(true);
            player.GetComponent<EnterAndExitCar>().ExitCar();
        }
        if (player.activeSelf)
        {
            hasSetToFalse = false;
        }
    }
    public void SetBool(bool value)
    {
        hasSetToFalse = value;
    }
    
}
