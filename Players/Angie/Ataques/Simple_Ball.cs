using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple_Ball : Energy_Ball
{
    protected override void DamageInteraction()
    {
        //rb.velocity = rb.velocity * 0.3f;
        rb.velocity = Vector3.zero;
        dano *= 0.5f;
        StartCoroutine("Expand");
    }

    //public int NewScale = 4;
    //public int NewScale2 = 1;

    IEnumerator Expand()
    {
        Vector3 FinalScale = transform.localScale * 4;

        while (transform.localScale.x < FinalScale.x)
        {
            transform.localScale += Vector3.one * Time.deltaTime * 20;
            yield return null;
        }

        Destroy(gameObject);
        yield return null;
    }
}
