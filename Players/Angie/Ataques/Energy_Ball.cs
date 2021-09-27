using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy_Ball : PlayerHit
{
    protected Rigidbody rb;
    public float vel;
    public float destructionTime;

    //public GameObject VFX_HIT;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.TransformDirection(Vector3.forward).normalized * vel;
        Destroy(gameObject, destructionTime);
    }

    protected override void EnterDamage(GameObject n_gameObject)
    {
        //GameObject VFX = Instantiate(VFX_HIT, transform.position, Quaternion.identity);
        //Destroy(VFX, .8f);

        CrateVFX_HIT();

        if (n_gameObject.CompareTag("ObjetosDeCena"))
        {
            Destroy(gameObject);
            return;
        }

        CalcDamage(n_gameObject);
        Destroy(gameObject);
    }

    //protected override void DamageInteraction()
    //{
    //    //rb.velocity = rb.velocity * 0.3f;
    //    rb.velocity = Vector3.zero;
    //    dano *= 0.5f;
    //    StartCoroutine("Expand");
    //}
}
