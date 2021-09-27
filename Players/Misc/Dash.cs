using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public interface IDash
//{
//    public void DashEffect();

//}

public class Dash : Skill
{
    public int MaxSlotDash = 2;
    public float CD_Slots = 0.5f;
    int CountDash;
    HUD_Dash HUD_Dash;

    public Image DashCount;

    [SerializeField]
    private GameObject VFX;

    //void Start()
    //{
    //    StopVFX();
    //}

    public override void Init(HUD_Skill HUD_Skill, PlayerController Player, string Trigger)
    {
        base.Init(HUD_Skill, Player, Trigger);
        CountDash = MaxSlotDash;
        HUD_Dash = HUD as HUD_Dash;
        HUD_Dash.setSlotCount(CountDash);
        HUD_Dash.setSlotCD(0);
        StaminaDash(CountDash);
        StopVFX();
    }

    public int getCount()
    {
        return CountDash;
    }

    protected void StaminaDash(float Count)
    {
        DashCount.fillAmount = Count / MaxSlotDash;
    }

    protected override void ResetAllCDs()
    {
        base.ResetAllCDs();
        CountDash = MaxSlotDash;
        HUD_Dash.setSlotCount(CountDash);
        HUD_Dash.setSlotCD(0);
    }

    protected override void Effect()
    {
        Player.newVelocity(Vector3.zero);
        Player.InpulsePlayer();
        StartCoroutine(Player.DashInvunable());

        ParticleSystem[] Particles = VFX.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem item in Particles)
        {
            item.Play();
        }

        Invoke("StopVFX", 1.2f);
    }

    private void StopVFX()
    {
        ParticleSystem[] Particles = VFX.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem item in Particles)
        {
            item.Stop();
        }
    }

    public override void CountCD()
    {
        int lastSlot = CountDash;
        CountDash--;
        StaminaDash(CountDash);
        Avaliable = false;

        if (lastSlot == MaxSlotDash) StartCoroutine("Slot_CD");
        StartCoroutine("Count_CD");
    }

    IEnumerator Slot_CD()
    {
        HUD_Dash.setSlotCD(CD_Slots, CD_Slots);

        while (CountDash < MaxSlotDash)
        {
            float ReturnTimeSlot = Time.time + CD_Slots;
            float CurrentSlotTIme = 0;

            while (ReturnTimeSlot >= Time.time)
            {
                CurrentSlotTIme = ReturnTimeSlot - Time.time;
                HUD_Dash.setSlotCD(CurrentSlotTIme, CD_Slots);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            if (CountDash == 0)
            {
                HUD_Dash.setCD(0);
                Avaliable = true;
            }

            CountDash++;
            HUD_Dash.setSlotCount(CountDash);
            HUD_Dash.setSlotCD(0);
            StaminaDash(CountDash);
        }

        yield return null;
    }

    IEnumerator Count_CD()
    {
        HUD_Dash.setSlotCount(CountDash);
        HUD.setCD(CD, CD);

        if (CountDash == 0)
        {
            yield return null;
        }
        else
        {
            float ReturnTime = Time.time + CD;
            float currentTime = 0;

            while (ReturnTime >= Time.time)
            {
                currentTime = ReturnTime - Time.time;
                HUD_Dash.setCD(currentTime, CD);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            HUD_Dash.setCD(0);
            Avaliable = true;
            yield return null;
        }
    }
}
