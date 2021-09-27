using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuakeArea : PlayerHit
{
    [SerializeField]
    private float ScaleSpeed = 4;

    [SerializeField]
    private float MaxLenth = 5;

    void Start()
    {
        //Player = FindObjectOfType<Angie>();
        StartCoroutine("Expand");
        Destroy(gameObject, 2.1f);
    }

    protected override void DamageInteraction(GameObject n_gameObject)
    {
        if (n_gameObject.GetComponent<SwordMan>())
        {
            n_gameObject.GetComponent<SwordMan>().KnockBack(transform.position);
        }

        if (n_gameObject.GetComponent<BowMan>())
        {
            n_gameObject.GetComponent<BowMan>().KnockBack(transform.position);
        }
    }

    IEnumerator Expand()
    {
        SphereCollider Area = GetComponent<SphereCollider>();
        Area.enabled = true;

        while (Area.radius < MaxLenth)
        {
            Area.radius += ScaleSpeed * Time.deltaTime;
            yield return null;
        }

        Player.GetComponent<QuakePunch>().CountCD();
        //Destroy(gameObject);
        yield return null;
    }
}
