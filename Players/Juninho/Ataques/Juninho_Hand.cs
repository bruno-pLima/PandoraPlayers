using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPunch { normal, cleave, upper };

public class Juninho_Hand : PlayerHit
{
    public float normal_punch_damage = 12;
    public float cleave_punch_damage = 9;
    public float uppercut_punch_damage = 15;

    IPunch PunchType;

    private void Start()
    {
        SetPlayer(GetComponentInParent<PlayerController>());
        PunchType = new Normal(normal_punch_damage);
        dano = PunchType.Dano;
    }

    public void SetPunchDamage(EPunch punch)
    {
        switch (punch)
        {
            case EPunch.normal:
                PunchType = new Normal(normal_punch_damage);
                break;
            case EPunch.cleave:
                PunchType = new Cleave(cleave_punch_damage);
                break;
            case EPunch.upper:
                PunchType = new Uppercut(uppercut_punch_damage);
                break;
            default:
                break;
        }

        dano = PunchType.Dano;
    }

    protected override void DamageInteraction()
    {
        PunchType.Effect();
        CrateVFX_HIT();
    }

    protected override void DamageInteraction(GameObject n_gameObject)
    {
        PunchType.Effect(n_gameObject);
        CrateVFX_HIT();
    }
}

interface IPunch
{
    float Dano { get; set; }
    void Effect();
    void Effect(GameObject obj);
}

class Normal : IPunch
{
    public Normal(float n_Dano) { Dano = n_Dano; }

    public float Dano { get; set; }

    public void Effect() { return; }
    public void Effect(GameObject obj) { return; }
}

class Cleave : IPunch
{
    public Cleave(float n_Dano) { Dano = n_Dano; }

    public float Dano { get; set; }

    public void Effect() { return; }
    public void Effect(GameObject obj) { return; }

}

class Uppercut : IPunch
{
    public Uppercut(float n_Dano) { Dano = n_Dano; }

    public float Dano { get; set; }

    public void Effect() { return; }
    public void Effect(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.AddForce(obj.transform.TransformDirection(Vector3.forward) * 15, ForceMode.Impulse);
    }
}