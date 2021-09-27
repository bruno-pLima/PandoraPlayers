using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaPunch : PlayerHit
{
    public BoxCollider Collider;

    void Start()
    {
        //Player = FindObjectOfType<Angie>();
        //Collider = GetComponent<BoxCollider>();
        StartCoroutine("Size");
        Destroy(gameObject, 1.5f);
    }

    IEnumerator Size()
    {
        while (Collider.size.z <= 7)
        {
            Collider.size += new Vector3(0, 0, 1);
            Collider.center += new Vector3(0, 0, 0.5f);
            //Collider.bounds.Expand(new Vector3(0, 0, 1));
            yield return new WaitForFixedUpdate();
        }

        yield return null;
    }

    //public void Destroy()
    //{
    //    Destroy(gameObject);
    //}
}
