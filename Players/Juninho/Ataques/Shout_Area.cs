using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shout_Area : PlayerHit
{
    [SerializeField]
    private float ScaleSpeed = 4;

    [SerializeField]
    private float MaxLenth = 5;

    void Start()
    {
        //Player = FindObjectOfType<Angie>();
        StartCoroutine("Expand");
        Destroy(gameObject, 1.1f);
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

        Player.GetComponent<Shout>().CountCD();
        //Destroy(gameObject);
        yield return null;
    }

    protected override void DamageInteraction(GameObject n_gameObject)
    {
        try
        {
            n_gameObject.GetComponent<Mob>().ShoutThreat(Player.gameObject.name);
        }
        catch (System.Exception ex)
        {
            print(ex.Message);
        }
    }
}
