using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vortex_Backup : Skill
{
    [SerializeField]
    private GameObject VortexZone;

    //[SerializeField]
    //private GameObject VortexZoneT;
    //private GameObject Vortex_Temp;

    [SerializeField]
    private Transform SpawnPoint;

    protected override void Effect()
    {
        GameObject Vortex = Instantiate(VortexZone, SpawnPoint.position, Player.transform.localRotation);
        Vortex.GetComponent<VortexZone>().SetPlayer(Player);
    }

    public override void Play()
    {
        //Destroy(Vortex_Temp);
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

    public void Teste()
    {
        Vector3 Pos = Player.transform.position;
        Pos.y += 3;
        //Vortex_Temp = Instantiate(VortexZoneT, Pos, Quaternion.identity);
    }

    public override void Brinks()
    {
        return;
        //Teste();
    }
}
