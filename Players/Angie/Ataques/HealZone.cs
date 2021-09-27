using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealZone : MonoBehaviour
{
    [SerializeField] private LayerMask l_Mask;

    private Collider[] l_Player;

    [SerializeField] private int Power = 10;
    [SerializeField] private int Duration = 5;
    [SerializeField] private float TickTime = 1f;

    private PlayerController Player;

    void Start()
    {
        StartCoroutine(Heal());
    }

    public void SetPlayer(PlayerController n_Player)
    {
        Player = n_Player;
    }

    IEnumerator Heal()
    {
        //GameObject[] Particiles = {transform.GetChild(0).gameObject,
        //                              transform.GetChild(1).gameObject,
        //                              transform.GetChild(2).gameObject,
        //                              transform.GetChild(3).gameObject};

        yield return new WaitForSeconds(.75f);

        float TimeDuration = Time.time + Duration;

        while (TimeDuration >= Time.time)
        {
            l_Player = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius, l_Mask);

            foreach (Collider Player in l_Player)
            {
                Player.GetComponent<PlayerController>().Heal(Power);
            }

            yield return new WaitForSeconds(TickTime);
        }

        //Particiles[0].GetComponent<ParticleSystem>().Stop();
        //Particiles[1].GetComponent<ParticleSystem>().Stop();
        //Particiles[2].GetComponent<ParticleSystem>().Stop();
        //Particiles[3].GetComponent<ParticleSystem>().Stop();

        Player.GetComponent<Heal>().CountCD();
        Destroy(gameObject);
        yield return null;
    }


    private Color Alpha(Color MatCol, float i)
    {
        Color Color = MatCol;
        Color.a = i;
        return Color;
    }
}
