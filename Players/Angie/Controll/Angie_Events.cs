using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angie_Events : EventsAnimation
{
    public Transform Pauzinho;
    public GameObject Antecipation;


    public void CastBall(GameObject Energy_Ball)
    {
        Vector3 pos = Pauzinho.position;
        GameObject Energy = Instantiate(Energy_Ball, pos, Player.transform.localRotation);
        GameObject Pre = Instantiate(Antecipation, pos, Player.transform.localRotation);
        Destroy(Pre, 2f);
        Energy.GetComponent<PlayerHit>().SetPlayer(Player);
        //setCanMove(1);
    }

    public void CastRay(GameObject Ray)
    {
        Vector3 pos = Pauzinho.position;
        GameObject Raio = Instantiate(Ray, pos + Ray.transform.position, Player.transform.localRotation);
        Raio.GetComponent<PlayerHit>().SetPlayer(Player);
        Player.AttackHandle();
        //setCanMove(1);
    }

    public void CleaveCast(GameObject Energy_Ball)
    {
        Quaternion r_1 = Player.transform.localRotation;
        Quaternion r_2 = Player.transform.localRotation;
        Quaternion r_3 = Player.transform.localRotation;

        GameObject Energy_1 = Instantiate(Energy_Ball, Player.transform.localPosition + (Vector3.up / 2), r_1);
        GameObject Energy_2 = Instantiate(Energy_Ball, Player.transform.localPosition + (Vector3.up / 2), r_2);
        GameObject Energy_3 = Instantiate(Energy_Ball, Player.transform.localPosition + (Vector3.up / 2), r_3);

        Energy_1.GetComponent<PlayerHit>().SetPlayer(Player);
        Energy_2.GetComponent<PlayerHit>().SetPlayer(Player);
        Energy_3.GetComponent<PlayerHit>().SetPlayer(Player);

        Energy_2.transform.Rotate(Vector3.up, 20);
        Energy_3.transform.Rotate(Vector3.down, 20);
        //setCanMove(1);
    }

    public void CreateHealZone()
    {
        Player.Sup.Play();
    }

    public void CreateVortexZone()
    {
        Player.Control.Play();
    }

    public void CreateVortexT()
    {
        Player.Control.Brinks();
    }

    public void CreateExplosion()
    {
        Player.Damage.Play();
    }
}
