using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Float vars")]
    public float damage = 15f;
    public float shotDistance = 20;
    public float rpm = 60;
    private float secondsBetweenShots;
    private float nextPossibleShootTime;
    Ray ray;
    [Header("Other vars")]
    public Camera cam;

    public bool isPlayer= true;
    RaycastHit hit;
    public enum ShootType
    {
        Semi, Burst, Auto
    }
    public ShootType shootType;
    // Start is called before the first frame update
    void Start()
    {
        secondsBetweenShots = 60 / rpm;     
        if(cam == null)
        {
            cam = GameObject.FindGameObjectWithTag("AimCam").GetComponent<Camera>();
        }
    }
    private bool CanShoot()
    {
        bool canShoot = true;
        if (Time.time < nextPossibleShootTime)
        {
            canShoot = false;
        }
        else
        {
            canShoot = true;
        }
        return canShoot;
    }
    public void Shoot()
    {
        Debug.Log("Can shoot " + CanShoot());
        if (CanShoot())
        {
            if (isPlayer)
            {
                //player shooting here 
                ray = new Ray(cam.transform.position, cam.transform.forward);
                if (Physics.Raycast(ray, out hit))
                {

                    Debug.DrawRay(cam.transform.position, cam.transform.forward);
                    Debug.Log(hit.transform.name);
                }
                nextPossibleShootTime = Time.time + secondsBetweenShots;
            }
            else
            {
                //enemy shooting here
                //shooting by reference in ai script ofc
            }
            
        }      
    }
    public void ShootContinious()
    {
        if (shootType == ShootType.Auto)
        {
            Shoot();
        }
    }
}
