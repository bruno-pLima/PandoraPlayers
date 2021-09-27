using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObject : MonoBehaviour
{
    public float dano;

    public float getDano()
    {
        return dano;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject game = other.gameObject;
        EnterDamage(game);
    }

    protected virtual void EnterDamage(GameObject n_gameObject)
    {
        DamageInteraction();
        return;
    }

    protected virtual void DamageInteraction()
    {
        return;
    }
}
