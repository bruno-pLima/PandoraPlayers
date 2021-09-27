using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Skill
{
    [SerializeField]
    private GameObject ExplosionZone;

    protected override void Effect()
    {
        //Instantiate(VortexZone, Player.transform.position, Player.transform.localRotation);
        Vector3 SpawnPos = Player.transform.position + ExplosionZone.transform.position;
        GameObject Explosion = Instantiate(ExplosionZone, SpawnPos, Quaternion.identity);
        //GameObject Explosion = Instantiate(ExplosionZone, Player.transform.position, Quaternion.identity);
        Explosion.GetComponent<PlayerHit>().SetPlayer(Player);
    }

    public override void Play()
    {
        Avaliable = false;
        HUD.WaitCD();
        Effect();
    }

    public override void CountCD()
    {
        StartCoroutine("Count_CD");
    }

    IEnumerator Count_CD()
    {
        float ReturnTime = Time.time + CD;
        float currentTime = 0;

        while (ReturnTime >= Time.time)
        {
            currentTime = ReturnTime - Time.time;
            HUD.setCD(currentTime, CD);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        HUD.setCD(0);
        Avaliable = true;
        yield return null;
    }
}
