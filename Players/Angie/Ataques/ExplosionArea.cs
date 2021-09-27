using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionArea : PlayerHit
{
    [SerializeField]
    private float ScaleSpeed = 4;

    [SerializeField]
    private float MaxLenth = 5;

    void Start()
    {
        Player = FindObjectOfType<Angie>();
        StartCoroutine("Explose");
        Destroy(gameObject, 2.5f);
    }

    IEnumerator Explose()
    {
        //Vector3 sr = transform.localScale * MaxLenth;

        yield return new WaitForSeconds(.40f);

        SphereCollider Area = GetComponent<SphereCollider>();
        Area.enabled = true;

        while (Area.radius < MaxLenth)
        {
            Area.radius += ScaleSpeed * Time.deltaTime;
            yield return null;
        }

        //while (transform.localScale.x < sr.x)
        //{
        //    transform.localScale += Vector3.one * ScaleSpeed;
        //    yield return null;
        //}

        Player.GetComponent<Explosion>().CountCD();
        //Destroy(gameObject);
        yield return null;
    }
}
