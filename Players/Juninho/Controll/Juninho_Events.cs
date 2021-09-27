using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juninho_Events : EventsAnimation
{
    public Juninho_Hand L_Hand;
    public Juninho_Hand R_Hand;

    private Collider L_Collider;
    private Collider R_Collider;

    public Transform MegaPos;

    protected override void Start()
    {
        base.Start();
        L_Collider = L_Hand.GetComponent<Collider>();
        R_Collider = R_Hand.GetComponent<Collider>();

        TurnOffAllColliders();
    }

    public void OnDashCollider()
    {
        Juninho N_Juninho = Player as Juninho;
        N_Juninho.NewMass(9999);
        Juninho_Dash Dash = Player.Dash as Juninho_Dash;
        Dash.DashEffect(true);
    }

    public void OffDashCollider()
    {
        Juninho N_Juninho = Player as Juninho;
        N_Juninho.NewMass();
        Juninho_Dash Dash = Player.Dash as Juninho_Dash;
        Dash.DashEffect(false);
    }

    public void SetAttack(int v)
    {
        switch (v)
        {
            case 1:
                R_Collider.enabled = false;
                L_Collider.enabled = true;
                SetPunchType(EPunch.normal);
                break;
            case 2:
                R_Collider.enabled = true;
                L_Collider.enabled = false;
                SetPunchType(EPunch.normal);
                break;
            case 3:
                R_Collider.enabled = false;
                L_Collider.enabled = true;
                SetPunchType(EPunch.cleave);
                break;
            case 4:
                R_Collider.enabled = false;
                L_Collider.enabled = true;
                SetPunchType(EPunch.upper);
                break;
            default:
                print("Ta sem parametro no soco");
                break;
        }
    }

    public void TurnOffAllColliders()
    {
        L_Collider.enabled = false;
        R_Collider.enabled = false;
    }

    void SetPunchType(EPunch Punch)
    {
        R_Hand.SetPunchDamage(Punch);
        L_Hand.SetPunchDamage(Punch);
    }

    public void MegaPunch(GameObject AreaEffect)
    {
        GameObject Effect = Instantiate(AreaEffect, MegaPos);
        Effect.transform.localScale = new Vector3(0.014846f, 0.014846f, 0.014846f);
        Effect.GetComponentInChildren<PlayerHit>().SetPlayer(Player);
        Player.AttackHandle();
        MoveOnAnimation(2f);
    }

    public void ShieldUP()
    {
        Player.Sup.Play();
    }

    public void ShieldDown()
    {
        Player.Sup.CountCD();
    }

    public void Shout()
    {
        Player.Control.Play();
    }

    public void QuakePunch()
    {
        Player.Damage.Play();
    }
}
