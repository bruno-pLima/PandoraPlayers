using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(AudioSource))]

public class PlayerHit : HitObject
{
    public PlayerController Player;
    protected EnemyController Target;

    //public AudioClip HitAudio;
    //private AudioSource audioSource;

    //private void Start()
    //{
    //    audioSource = GetComponent<AudioSource>();
    //}

    public GameObject VFX_HIT;

    public void CrateVFX_HIT()
    {
        GameObject VFX = Instantiate(VFX_HIT, transform.position, transform.localRotation);
        Destroy(VFX, .8f);
    }

    public void SetPlayer(PlayerController n_Player)
    {
        Player = n_Player;
    }

    protected override void EnterDamage(GameObject n_gameObject)
    {
        CalcDamage(n_gameObject);
        DamageInteraction();
        DamageInteraction(n_gameObject);
    }

    protected override void DamageInteraction()
    {
        return;
    }

    protected virtual void DamageInteraction(GameObject n_gameObject)
    {
        return;
    }

    protected virtual void CalcDamage(GameObject n_gameObject)
    {
        Target = n_gameObject.GetComponent<EnemyController>();
        try
        {
            Target.TakeDamage(RoundDamage(dano), Player.name);
            //audioSource.PlayOneShot(HitAudio);
        }
        catch (System.Exception e)
        {
            if (Target is null)
            {
                Debug.Log("Enemy Controller nao encontrado no inimigo : " + n_gameObject.name);
            }
            else
            {
                Debug.Log(e.Message);
            }
        }
    }

    private int RoundDamage(float Dano)
    {
        return Mathf.RoundToInt(Dano);
    }
}
