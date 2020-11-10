using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter_Base : MonoBehaviour
{    
    [SerializeField] GameObject mainRotor;
    [SerializeField] GameObject tailRotor;
    [Header("Floats")]
    [SerializeField] float maxRotorForce = 22241.1081f;
    [SerializeField] float maxRotorVelocity = 7200f;
    [SerializeField] float maxTailRotorForce = 15000f;
    [SerializeField] float maxTailRotorVelocity = 2200f;
    [SerializeField] float forwardRotorTorqueMultiplier = 0.5f;
    [SerializeField] float sidewaysRotortorgueMultiplier = 0.5f;
    [Header("CameraVars")]
    [SerializeField] GameObject cameraToActivate;
    [SerializeField] GameObject cinemachineGameObjToActivate;
    [SerializeField] GameObject player;
    private float rotorRotation;
    static float rotorVelocity;
    private float tailRotorVelocity;
    private float tailRotorRotation;
    static bool main_Rotor_Active = true;
    static bool tail_Rotor_Active = true;
    Rigidbody rb;
    private bool entered = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (entered)
        {            
            /////////////////////
            Vector3 torqueValue = new Vector3();
            Vector3 controlTorque = new Vector3(
                Input.GetAxis("Vertical") * forwardRotorTorqueMultiplier,
                1, -Input.GetAxis("Horizontal") * sidewaysRotortorgueMultiplier
                );
            if (main_Rotor_Active == true)
            {
                if (Input.GetAxis("Vertical") != 0)
                {
                    torqueValue += (controlTorque * maxRotorForce * rotorVelocity);
                    rb.AddRelativeForce(Vector3.up * maxRotorForce * rotorVelocity);
                }
                else
                {
                    rb.AddRelativeForce(Vector3.up  * rotorVelocity);
                }
            }
            if (Vector3.Angle(Vector3.up, transform.up) < 80)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0), Time.deltaTime * rotorVelocity * 2);
            }
            if (tail_Rotor_Active == true)
            { torqueValue -= (Vector3.up * maxTailRotorForce * tailRotorVelocity); }
            rb.AddRelativeTorque(torqueValue);
        }      
        
    }
    private void Update()
    {
        if (entered)
        {
            cameraToActivate.SetActive(true);
            cinemachineGameObjToActivate.SetActive(true);
            StartCoroutine(WaitToExit());
            if (main_Rotor_Active == true) { mainRotor.transform.rotation = transform.rotation * Quaternion.Euler(0, rotorRotation, 0); }
            if (tail_Rotor_Active == true) { tailRotor.transform.rotation = transform.rotation * Quaternion.Euler(tailRotorRotation, 0, 0); }
            rotorRotation += maxRotorVelocity * rotorVelocity * Time.deltaTime;
            tailRotorRotation += maxTailRotorVelocity * rotorVelocity * Time.deltaTime;
            var hover_Rotor_Velocity = (rb.mass * Mathf.Abs(Physics.gravity.y) / maxTailRotorForce);
            Debug.Log(hover_Rotor_Velocity);
            var hover_Tail_Rotor_Velocity = (maxRotorForce * rotorVelocity) / maxTailRotorForce;
            if (Input.GetAxis("Vertical") < 0|| Input.GetAxis("Vertical") > 0)
            {
                rotorVelocity += Input.GetAxis("Vertical") * 0.001f;
            }
            else if(Input.GetAxis("Vertical")==0)
            {
                rotorVelocity = Mathf.Lerp(rotorVelocity, hover_Rotor_Velocity, Time.deltaTime * Time.deltaTime * 5);
            }
            tailRotorVelocity = hover_Rotor_Velocity - Input.GetAxis("Horizontal");
            if (rotorVelocity > 1.0)
            { rotorVelocity = 1.0f; }
            else if (rotorVelocity < 0.0)
            { rotorVelocity = 0f; }
        }
        else
        {
            cameraToActivate.SetActive(false);
            cinemachineGameObjToActivate.SetActive(false);
        }
    }
    public void SetEntryState(bool value)
    {
        entered = value;
    }
    IEnumerator WaitToExit()
    {
        yield return new WaitForSeconds(2f);
        if (Input.GetKeyDown(KeyCode.F) && entered == true)
        {
            Debug.Log("Activated exit");
            player.GetComponent<EnterHeli>().ToggleCams(true);
            player.transform.parent = null;
            player.SetActive(true);
            entered = false;
            cameraToActivate.SetActive(false);
            cinemachineGameObjToActivate.SetActive(false);
        }
    }
}
    
