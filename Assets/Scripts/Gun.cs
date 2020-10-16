using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Float vars")]
    public float damage = 15f;
    public float shotDistance = 20;
    public float rpm = 60;
    public float flashDestructionTime = 0.5f;
    private float secondsBetweenShots;
    private float nextPossibleShootTime;
    Ray ray;
    private Camera cam;
    private Transform tipOfGun;
    [Header("Other vars")]
    public bool isPlayer= true;
    [SerializeField] GameObject flash;
    [SerializeField] LineRenderer bulletTrail;
    RaycastHit hit;
    public enum ShootType
    {
        Semi, Burst, Auto
    }
    public ShootType shootType;
    private void Awake()
    {
        Physics.queriesHitBackfaces = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        secondsBetweenShots = 60 / rpm;
        tipOfGun = transform.Find("Tip");
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
                    if (flash != null && tipOfGun != null)
                    {
                        CreateFlash();
                    }
                    Debug.DrawRay(cam.transform.position, cam.transform.forward);
                    Debug.Log(hit.transform.name);
                    SpawnTrace(hit.point);
                }
                else
                {
                    SpawnTrace(tipOfGun.forward);
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
    void CreateFlash()
    {
        GameObject fgo = Instantiate(flash, tipOfGun.position,Quaternion.identity);
        Destroy(fgo, flashDestructionTime);
    }
    void SpawnTrace(Vector3 endingPos)
    {
        GameObject bulletTrailEffect = Instantiate(bulletTrail.gameObject, tipOfGun.position, Quaternion.identity);
        LineRenderer lineR = bulletTrailEffect.GetComponent<LineRenderer>();
        lineR.SetPosition(0, tipOfGun.position);
        lineR.SetPosition(1, endingPos);
        Destroy(bulletTrailEffect, 0.7f);
    }
}
