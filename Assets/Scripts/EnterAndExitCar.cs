using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterAndExitCar : MonoBehaviour
{
    [SerializeField] RuntimeAnimatorController car_enter_exit;
    RuntimeAnimatorController initialActorRuntimeController;
    Animator animator;
    Transform car; bool carNearby;
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
                transform.position = Vector3.Lerp(car.GetComponent<CarAnimProperties>().AnimEnterPosition().transform.position, car.GetComponent<CarAnimProperties>().AnimDrivePosition().transform.position, time / 1.3f);
                transform.rotation = Quaternion.Lerp(car.GetComponent<CarAnimProperties>().AnimEnterPosition().transform.rotation, car.GetComponent<CarAnimProperties>().AnimDrivePosition().transform.rotation, time / 1.3f);
            }
            time += Time.fixedDeltaTime;
            //yield return new WaitForFixedUpdate();
            yield return null;
        }
        enterState = EnterState.inside;
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
            if (car != null)
            {
                car.GetComponent<Animator>().SetBool("Open", 0.0f < time && time < 1f);
                transform.position = Vector3.Lerp(car.GetComponent<CarAnimProperties>().AnimDrivePosition().transform.position, car.GetComponent<CarAnimProperties>().AnimEnterPosition().transform.position, time / 1.3f);
                transform.rotation = Quaternion.Lerp(car.GetComponent<CarAnimProperties>().AnimDrivePosition().transform.rotation, car.GetComponent<CarAnimProperties>().AnimEnterPosition().transform.rotation, time / 1.3f);
            }
            time += Time.fixedDeltaTime;
            //yield return new WaitForFixedUpdate();
            yield return null;
        }
        enterState = EnterState.outside;
        
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
