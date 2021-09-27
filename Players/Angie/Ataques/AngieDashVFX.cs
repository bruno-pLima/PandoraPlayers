using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngieDashVFX : PlayerHit
{
    protected Rigidbody rb;
    public float vel;
    public float destructionTime;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.TransformDirection(Vector3.back).normalized * vel;
        Destroy(gameObject, destructionTime);
    }

    //protected override void DamageInteraction()
    //{




    //}
}
