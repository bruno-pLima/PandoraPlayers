using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juninho_Dash : Dash
{
    private Collider Collider;

    void Start()
    {
        Collider = transform.Find("Dash_Collider").GetComponent<Collider>();
        Collider.enabled = false;
    }

    public void DashEffect(bool val)
    {
        TurnCollider(val);
    }

    public void TurnCollider(bool Val)
    {
        Collider.enabled = Val;
    }

    //protected override void Effect()
    //{
    //    //Player.newVelocity(Vector3.zero);
    //    Player.InpulsePlayer();
    //}
}
