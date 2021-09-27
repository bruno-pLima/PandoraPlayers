using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Juninho : PlayerController
{
    protected override void Start()
    {
        base.Start();
        stats = new playerStats(150, 7f, 8f, 12f, 12f);

        HUD = GameObject.Find("HUD/Juninho").GetComponent<HUD_Player>();
        Dash = GetComponent<Juninho_Dash>();
        Sup = GetComponent<Shield>();
        Control = GetComponent<Shout>();
        Damage = GetComponent<QuakePunch>();

        HUD.Init(this);
        Dash.Init(GameObject.Find("HUD/Juninho/Dash").GetComponent<HUD_Dash>(), this, "Dash");
        Sup.Init(GameObject.Find("HUD/Juninho/SkillLT").GetComponent<HUD_Skill>(), this, "Shield");
        Control.Init(GameObject.Find("HUD/Juninho/SkillY").GetComponent<HUD_Skill>(), this, "Shout");
        Damage.Init(GameObject.Find("HUD/Juninho/SkillB").GetComponent<HUD_Skill>(), this, "QPunch");

        MarkIndicator = this.transform.GetChild(2).gameObject;

        SetParter(FindObjectOfType<Angie>());
    }

    protected override void SoundDamage()
    {
        GetComponentInChildren<BoySoundEvents>().BoyDamage();
    }

    void Update()
    {
        if (Time.timeScale < 1) { return; }

        //Cheats();

        if (Input.GetKeyDown(KeyCode.F6))
        {
            Kill();
        }

        if (ToVivo)
        {
            rb.WakeUp();

            OnGround();
            DustControl();

            if (Parter && Parter.ToVivo)
                Distance = Vector3.Distance(transform.position, Parter.transform.position);

            if (!isPlayer2)
            {
                Atack("P1_X");
                GetInput(Input.GetAxis("P1_L_Joystick_Vertical"), Input.GetAxis("P1_L_Joystick_Horizontal"));
                UseDash(Input.GetAxis("P1_RT"));
                if (!GetBool("Charging"))
                {
                    if (Parter && Parter.ToVivo)
                    {
                        if (Parter.GetBool("Fallen") && Distance <= 2f)
                        {
                            ReviveFriend("P1_A");
                        }
                        else
                        {
                            Jump(Input.GetButtonDown("P1_A"));
                            ResetRevive();
                        }
                    }
                    else
                    {
                        Jump(Input.GetButtonDown("P1_A"));
                        ResetRevive();
                    }


                    ShieldUP(this.Sup, Input.GetAxis("P1_LT"));
                    UseSkill(this.Control, Input.GetButtonDown("P1_Y"));
                    UseSkill(this.Damage, Input.GetButtonDown("P1_B"));
                }
            }
            else
            {
                Atack("P2_X");
                GetInput(Input.GetAxis("P2_L_Joystick_Vertical"), Input.GetAxis("P2_L_Joystick_Horizontal"));
                UseDash(Input.GetAxis("P2_RT"));
                if (!GetBool("Charging"))
                {
                    if (Parter && Parter.ToVivo)
                    {
                        if (Parter.GetBool("Fallen") && Distance <= 2f)
                        {
                            ReviveFriend("P2_A");
                        }
                        else
                        {
                            Jump(Input.GetButtonDown("P2_A"));
                        }
                    }
                    else
                    {
                        Jump(Input.GetButtonDown("P2_A"));
                        ResetRevive();
                    }

                    ShieldUP(this.Sup, Input.GetAxis("P2_LT"));
                    UseSkill(this.Control, Input.GetButtonDown("P2_Y"));
                    UseSkill(this.Damage, Input.GetButtonDown("P2_B"));
                }
            }
        }
    }

    public void NewMass(float val = 1f)
    {
        rb.mass = val;
    }

    bool CanTrigger = true;

    public float ShieldMaxDuration = 5f;
    private float ShieldTime;
 
    public void ShieldUP(Skill Skill, float Trigger)
    {
        if (Skill.IsAvaliable() && Trigger == 1 && OnGround() && CanTrigger && GetBool("canCast"))
        {
            CanTrigger = false;
            SetTrigger(Skill.StringTrigger);
            ShieldTime = Time.time + ShieldMaxDuration;
        }

        if (OnGround() && GetBool("ShieldUP"))
        {
            if (Trigger == 0 || Time.time > ShieldTime)
            {
                CanTrigger = true;
                SetBool("ShieldUP", false);
            }

            if (Caido)
            {
                Sup.CountCD();
            }
        }
    }
}
