using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Skill
{
    [SerializeField]
    private GameObject HealZone;

    protected override void Effect()
    {             
        GameObject Heal = Instantiate(HealZone, Player.transform.position + new Vector3(0, 0.5f, 0), Player.transform.localRotation);
        Heal.GetComponent<HealZone>().SetPlayer(Player);
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
