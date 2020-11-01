using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterAndExitCar : MonoBehaviour
{
    [SerializeField] RuntimeAnimatorController car_enter_exit;
    RuntimeAnimatorController initialActorRuntimeController;
    Animator animator,carAnimator;
    Transform car, carEntryPoint,carDrivingPoint; bool carNearby;
    
    public enum EnterState
    {
        outside,inside
    } 
    EnterState enterState = EnterState.outside;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        initialActorRuntimeController = animator.runtimeAnimatorController;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        carNearby = (car!=null);
        if (carNearby)
        {
            carEntryPoint = car.GetComponent<CarAnimProperties>().AnimEnterPosition();
            carDrivingPoint = car.GetComponent<CarAnimProperties>().AnimDrivePosition();
            carAnimator = car.GetComponent<Animator>();
            if (Input.GetKeyDown(KeyCode.F))//to pay respects
            {
                if(enterState == EnterState.outside)
                {
                    EnterCar();
                }               
            }
        }
        if (enterState == EnterState.inside && Input.GetKeyDown(KeyCode.F))
        {
            ExitCar();
        }

    }
    void EnterCar()
    {   
        
        StartCoroutine(EnterCarAnim());
        
    }
    IEnumerator EnterCarAnim()
    {
        animator.runtimeAnimatorController = car_enter_exit;
        var time = 0f;
        animator.SetTrigger("EnterCar");
        animator.SetBool("InCar", true); 
        float animTime = animator.GetCurrentAnimatorStateInfo(0).length;
        while (time < animTime)
        {
            if (car != null)
            {
                car.GetComponent<Animator>().SetBool("Open", 0.3f < time && time < 1f);
                transform.position = Vector3.Lerp(carEntryPoint.position,carDrivingPoint.position, time / 1.3f);
                transform.rotation = Quaternion.Lerp(carEntryPoint.rotation,carDrivingPoint.rotation, time / 1.3f);
            }
            time += Time.fixedDeltaTime;
            //yield return new WaitForFixedUpdate();
            yield return null;
        }
        //enterState = EnterState.inside;
    }
    void ExitCar()
    {
        StartCoroutine(ExitCarAnim());
        
    }
    IEnumerator ExitCarAnim()
    {
        var time = 0f;
        animator.ResetTrigger("EnterCar");
        animator.SetBool("InCar", false);
        float animTime = animator.GetCurrentAnimatorStateInfo(0).length;
        while (time < animTime)
        {
            try
            {
                carAnimator.SetBool("Open", 0.0f < time && time < 1f);
                transform.position = Vector3.Lerp(carDrivingPoint.position, carEntryPoint.position, time / 1.3f);
                transform.rotation = Quaternion.Lerp(carDrivingPoint.rotation, carEntryPoint.rotation, time / 1.3f);
            }
            catch
            {
                Debug.Log("exiting coroutine error");
                break;
            }
            time += Time.fixedDeltaTime;
            //yield return new WaitForFixedUpdate();
            yield return null;
        }                        
    }
    public void EnterCarAnimEvent()
    {
        enterState = EnterState.inside;
        transform.parent = car;
    }
    public void ExitCarAnimEvent()
    {
        enterState = EnterState.outside;
        transform.parent = null;     
        animator.runtimeAnimatorController = initialActorRuntimeController;
    }
    public Transform GetCar()
    {
        return car;
    }
    public void SetCar(Transform newCar)
    {
        car = newCar;
    }
}
