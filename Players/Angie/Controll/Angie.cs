using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Angie : PlayerController
{
    protected override void Start()
    {
        base.Start();
        stats = new playerStats(100, 8f, 9f, 15f, 12f);

        HUD = GameObject.Find("HUD/Angie").GetComponent<HUD_Player>();
        Dash = GetComponent<Angie_Dash>();
        Sup = GetComponent<Heal>();
        Control = GetComponent<Vortex>();
        Damage = GetComponent<Explosion>();


        HUD.Init(this);
        Dash.Init(GameObject.Find("HUD/Angie/Dash").GetComponent<HUD_Dash>(), this, "Dash");
        Sup.Init(GameObject.Find("HUD/Angie/SkillLT").GetComponent<HUD_Skill>(), this, "Heal");
        Control.Init(GameObject.Find("HUD/Angie/SkillY").GetComponent<HUD_Skill>(), this, "Vortex");
        Damage.Init(GameObject.Find("HUD/Angie/SkillB").GetComponent<HUD_Skill>(), this, "Explosion");

        MarkIndicator = this.transform.GetChild(2).gameObject;

        SetParter(FindObjectOfType<Juninho>());
    }

    protected override void SoundDamage()
    {
        GetComponentInChildren<GirlSoundEvents>().GirlDamage();
    }

    void Update()
    {
        if (Time.timeScale < 1) { return; }

        //Cheats();

        if (Input.GetKeyDown(KeyCode.F7))
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

            if (isPlayer2)
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
                            ResetRevive();
                        }
                    }
                    else
                    {
                        Jump(Input.GetButtonDown("P2_A"));
                        ResetRevive();
                    }

                    UseSkill(this.Sup, Input.GetAxis("P2_LT"));
                    UseSkill(this.Control, Input.GetButtonDown("P2_Y"));
                    UseSkill(this.Damage, Input.GetButtonDown("P2_B"));
                }
            }
            else
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
                        }
                    }
                    else
                    {
                        Jump(Input.GetButtonDown("P1_A"));
                        ResetRevive();
                    }

                    UseSkill(this.Sup, Input.GetAxis("P1_LT"));
                    UseSkill(this.Control, Input.GetButtonDown("P1_Y"));
                    UseSkill(this.Damage, Input.GetButtonDown("P1_B"));
                }
            }
        }
    }

    protected override void Atack(string InputControl)
    {
        if (OnGround())
        {
            if (Input.GetButton(InputControl) && !GetBool("Attacking"))
            {
                anim.SetTrigger(StaticVariables.Animator.atackTrigger);
            }
            else if (Input.GetButton(InputControl) && GetBool("Attacking"))
            {
                rotateSpeed = 5;
                if (anim.speed <= 2)
                {
                    anim.speed += 0.001f;
                }
            }

            if (GetBool("Attacking"))
            {
                if (!Input.GetButton(InputControl))
                {
                    SetBool("Attacking", false);
                    rotateSpeed = 10;
                    anim.speed = 1;
                }
            }
        }
    }
}
