using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    protected HUD_Skill HUD;
    protected PlayerController Player;

    [HideInInspector]
    public string StringTrigger;

    [SerializeField]
    protected float CD = 5;

    protected bool Avaliable;

    public virtual void Init(HUD_Skill HUD, PlayerController Player, string Trigger)
    {
        this.Player = Player;
        this.HUD = HUD;
        this.HUD.setCD(0);
        Avaliable = true;
        StringTrigger = Trigger;
    }

    protected abstract void Effect();

    public virtual void Play()
    {
        Effect();
        CountCD();
    }

    public virtual void CountCD()
    {
        StartCoroutine("Count_CD");
    }

    public bool IsAvaliable()
    {
        return Avaliable;
    }

    IEnumerator Count_CD()
    {
        Avaliable = false;
        HUD.setCD(CD, CD);
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            ResetAllCDs();
        }
    }

    protected virtual void ResetAllCDs()
    {
        StopAllCoroutines();
        HUD.setCD(0);
        Avaliable = true;
    }

    public virtual void Brinks()
    {
        return;
    }
}
