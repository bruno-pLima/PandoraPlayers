
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Skill
{
    public GameObject Escudinho;
    private GameObject InstShield;   

    //public int Duration = 5;

    protected override void Effect()
    {
        Vector3 Position = Player.transform.position + Escudinho.transform.position;
        InstShield = Instantiate(Escudinho, Position, Player.transform.localRotation);
    }

    public override void Play()
    {
        Avaliable = false;
        HUD.WaitCD();
        Effect();
    }

    public override void CountCD()
    {
        DestroyShield();
        StartCoroutine("Count_CD");
    }

    public void DestroyShield()
    {
        Destroy(InstShield);
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
