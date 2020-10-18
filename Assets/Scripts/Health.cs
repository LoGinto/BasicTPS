using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    Animator animator;
    public float hp = 100f;
    public float hurtTransitionWait = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
    }
}
