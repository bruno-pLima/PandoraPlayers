using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerController : MonoBehaviour
{
    protected float rotateSpeed = 10.0f;
    protected float animSpeed = 1.0f;
    protected playerStats stats;

    protected Rigidbody rb;
    protected Animator anim;
    protected HUD_Player HUD;

    [SerializeField]
    GameObject SparkleParticle;


    public bool isPlayer2 = false;

    protected void init()
    {
        ZoneInterction = GetComponent<SphereCollider>();
        ZoneInterction.enabled = false;

        rb = GetComponent<Rigidbody>();
        try
        {
            anim = transform.GetChild(0).GetComponent<Animator>();
        }
        catch
        {
            anim = GetComponent<Animator>();
        }

        rb.angularDrag = 999;
        //rb.drag = 4;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    #region Movimentação

    protected float Distance;
    //public float MaxDistance = 50;

    void Walk(Vector3 moveDir, float moveAmout)
    {
        if (!getCanMove() || anim.GetBool("onAttack"))
            return;

        anim.SetFloat(StaticVariables.Animator.speedFloat, moveAmout);

        float speed = OnGround() ? stats.speed : stats.j_speed;

        if (moveDir == Vector3.zero)
            transform.GetChild(3).GetChild(0).GetComponent<ParticleSystem>().Stop();
        else
            transform.GetChild(3).GetChild(0).GetComponent<ParticleSystem>().Play();

        if (Parter)
        {
            if (Parter.ToVivo)
            {
                Vector3 FuturePos = transform.position + transform.forward * 2;
                Vector3 PosComparacao = Parter.transform.position;
                FuturePos.y = 0;
                PosComparacao.y = 0;

                float MaxDistance = GameController.Singleton ? GameController.Singleton.MaxDistancePlayers : 20;

                if ((FuturePos - PosComparacao).magnitude > MaxDistance)
                {
                    rb.velocity = new Vector3(0, rb.velocity.y, 0);
                    return;
                }
            }
        }

        moveDir *= speed * moveAmout;
        rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);
    }

    //[SerializeField]
    //float MaxDistanceWalk = 15;

    GameObject TargetInFront;

    void Rotate(Vector3 moveDir)
    {
        if (!getCanRotate())
            return;

        LookRotation(moveDir);
    }

    void LookRotation(Vector3 Target, float RotateSpeedVariant = 0)
    {
        if (Target == Vector3.zero)
            Target = transform.forward;

        Quaternion tr;
        Target.Normalize();
        Target.y = 0;
        tr = Quaternion.LookRotation(Target);

        float speed = RotateSpeedVariant > 0 ? RotateSpeedVariant : rotateSpeed * (anim.GetBool("Charging") ? Time.deltaTime / 3 : Time.deltaTime);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, speed);

        transform.localRotation = targetRotation;
    }

    public void AttackHandle()
    {
        GameObject TargetHandle = ForwardRaycast();
        if (TargetHandle)
        {
            Vector3 Target = (TargetHandle.transform.position - transform.position).normalized;
            Target.y = 0;
            Quaternion tr = Quaternion.LookRotation(Target);
            transform.localRotation = Quaternion.Slerp(transform.rotation, tr, 1);
        }
    }

    public void SetRotateSpeed(float _value)
    {
        rotateSpeed = _value;
    }

    private GameObject ForwardRaycast()
    {
        float[] Dist = new float[9];
        Quaternion[] Quarts = new Quaternion[Dist.Length];
        RaycastHit[] hit = new RaycastHit[Dist.Length];
        Vector3 central = this.transform.forward;

        for (int i = 0; i < Dist.Length; i++)
        {
            Dist[i] = 750;
        }

        float angle = -18;
        for (int i = 0; i < Quarts.Length; i++)
        {
            Quarts[i] = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));
            Debug.DrawRay(this.transform.position + new Vector3(0, 1, 0), Quarts[i] * central * 40, Color.black);
            angle += 4.5f;
        }

        for (int i = 0; i < hit.Length; i++)
        {
            if (Physics.Raycast(this.transform.position + new Vector3(0, 1, 0), Quarts[i] * central, out hit[i], 40))
            {
                if (hit[i].collider.gameObject.CompareTag("Enemy"))
                {
                    Dist[i] = hit[i].distance;
                }
            }
        }

        float finalDist = Mathf.Min(Dist);
        if (finalDist != 750)
        {
            for (int i = 0; i < Dist.Length; i++)
            {
                if (finalDist == Dist[i]) TargetInFront = hit[i].collider.gameObject;
            }
        }
        else
        {
            TargetInFront = null;
        }

        return TargetInFront;
    }

    protected float vertical;
    protected float horizontal;
    Vector3 moveDir;

    protected void GetInput(float axis_v, float axis_h)
    {
        vertical = axis_v;
        horizontal = axis_h;
    }

    protected virtual void Move()
    {
        Vector3 v = vertical * Vector3.forward;
        Vector3 h = horizontal * Vector3.right;
        moveDir = (v + h).normalized;

        float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        float moveAmout = Mathf.Clamp01(m);

        if (!GetBool("Attacking"))
            Walk(moveDir, moveAmout);

        Rotate(moveDir);
    }


    //public float JumpVar = 1.5f;
    protected virtual void Jump(bool Jump)
    {
        if (OnGround() && Jump && GetBool("canJump"))
        {
            anim.SetTrigger(StaticVariables.Animator.jumpTrigger);
        }
    }

    public void Jump()
    {
        InpulsePlayer(Vector3.up, stats.jumpForce, 0);
    }


    protected bool OnGround()
    {
        bool onGround = false;
        RaycastHit terrain;
        if (Physics.Raycast(this.transform.position, Vector3.down, out terrain, 999))
        {
            if (terrain.transform.CompareTag("Terrain") || terrain.transform.CompareTag("StaticObject") || terrain.transform.CompareTag("ObjetosDeCena") || terrain.transform.CompareTag("Ponte"))
            {
                float Distance = terrain.distance;
                Distance = float.Parse(Distance.ToString("0.00"));

                if (Distance < .4f)
                {
                    onGround = true;
                    anim.SetFloat("groundDistance", 0);
                }
                else
                {
                    onGround = false;
                    transform.GetChild(3).GetChild(0).GetComponent<ParticleSystem>().Stop();
                    anim.SetFloat("groundDistance", Distance);
                }

                anim.SetBool("onGround", onGround);
            }
        }

        return onGround;
    }

    public void MoveOnAtack(float force)
    {
        if (moveDir != Vector3.zero)
        {
            rb.velocity = Vector3.zero;
            Vector3 Direction = transform.TransformDirection(Vector3.forward) * force;
            InpulsePlayer(Direction, 0);
        }
    }

    public void MoveOnAnimation(float v)
    {
        if (getFloat("Speed") > 0)
        {
            Vector3 Direction = transform.TransformDirection(Vector3.forward) * v;
            InpulsePlayer(Direction);
        }
    }

    public bool getCanAtack()
    {
        return anim.GetBool(StaticVariables.Animator.canAtack);
    }

    public void setCanAtack(bool _val)
    {
        anim.SetBool(StaticVariables.Animator.canAtack, _val);
        anim.SetBool("onAttack", !_val);
    }

    public bool getCanMove()
    {
        return anim.GetBool(StaticVariables.Animator.canMove);
    }

    public bool getCanRotate()
    {
        return anim.GetBool(StaticVariables.Animator.canRotate);
    }

    public void setCanMove(bool _val)
    {
        anim.SetBool(StaticVariables.Animator.canMove, _val);
    }

    public void SetBool(string name, bool val)
    {
        anim.SetBool(name, val);
    }

    #endregion

    #region Ataques

    float getFloat(string floatName)
    {
        return anim.GetFloat(floatName);
    }

    protected virtual void Atack(bool CanAtack)
    {
        if (getCanAtack() && CanAtack && OnGround())
        {
            anim.SetTrigger(StaticVariables.Animator.atackTrigger);
            anim.SetFloat(StaticVariables.Animator.speedFloat, 0);
        }
    }

    //float chargeTime = 0.3f;
    bool TimingCharge = false;
    bool StartCharge = false;

    public float MinTimeCharge = 1.5f;
    public float MaxTimeCharge = 5f;

    protected virtual void Atack(string InputControl)
    {
        if (OnGround())
        {
            if (StartCharge)
            {
                if (Input.GetButton(InputControl) && (anim.GetBool("canUseChargeAtack") || anim.GetBool("Charging")))
                {
                    anim.SetFloat("chargeValue", getFloat("chargeValue") + Time.fixedDeltaTime);

                    if (getFloat("chargeValue") > MinTimeCharge && !TimingCharge)
                    {
                        TimingCharge = true;
                        GameObject a = Instantiate(SparkleParticle, transform.position + (Vector3.up * 1.5f) + Vector3.right, SparkleParticle.transform.rotation);
                        Destroy(a, 1.0f);
                    }

                    if (getFloat("chargeValue") > MaxTimeCharge)
                    {
                        TimingCharge = false;
                        anim.SetTrigger(StaticVariables.Animator.atackTrigger);
                    }
                }
            }

            if (Input.GetButtonDown(InputControl))
            {
                StartCharge = true;
            }

            if (Input.GetButtonUp(InputControl))
            {
                if (getCanAtack())
                {
                    TimingCharge = false;
                    StartCharge = false;
                    anim.SetTrigger(StaticVariables.Animator.atackTrigger);
                    //anim.SetFloat("chargeValue", 0);
                }

            }
        }
    }

    public void AtackOnMoveHandle(bool _value)
    {
        if (!anim.GetBool("onAttack"))
        {
            SetBool("canMove", _value);
        }

        SetBool("canRotate", _value);
    }

    #endregion

    protected void DustControl()
    {
        if (!GetBool(StaticVariables.Animator.canMove))
        {
            transform.GetChild(3).GetChild(0).GetComponent<ParticleSystem>().Stop();
        }
    }

    #region Stats

    public playerStats getStats()
    {
        return stats;
    }

    SphereCollider ZoneInterction;

    [HideInInspector]
    public bool getUpFriend = false;

    [HideInInspector]
    public bool getUp = true;

    [HideInInspector]
    public bool isAlive;

    //[HideInInspector]
    public bool ToVivo = false;

    bool Invensible = false;

    public IEnumerator DashInvunable()
    {
        Invensible = true;
        yield return 3;
        Invensible = false;
    }

    protected abstract void SoundDamage();

    // Ambos metodos vão retornar o valor da vida apos dar o dano
    public int TakeDamage(int dano)
    {
        if (GetBool("Fallen") || Invensible)
            return 0;

        rb.velocity = Vector3.zero;
        SoundDamage();

        if (stats.currentLife > 0)
        {
            stats.currentLife -= dano;
            SetLife();

            if (stats.currentLife <= 0)
            {
                stats.currentLife = 0;
                SetLife();

                if (!Parter.ToVivo || !getUp || Parter.Caido)
                {
                    ToVivo = false;

                    rb.useGravity = false;
                    GetComponent<Collider>().enabled = false;
                    ZoneInterction.enabled = false;

                    if (!GetBool("Fallen"))
                    {
                        anim.SetTrigger("Fall");
                    }

                    anim.SetTrigger("Death");
                    StartCoroutine(Dissolve());

                    //GameController.Singleton.CheckPlayerIsAlive();
                }
                else if (getUp)
                {
                    anim.SetTrigger("Fall");
                    ZoneInterction.enabled = true;
                    getUp = false;
                    rb.velocity = Vector3.zero;
                    setCanMove(false);
                    StartCoroutine(Fall());
                }
            }
            else
            {
                //anim.SetTrigger("Stag");
            }
        }

        return stats.currentLife;
    }

    public int SetDamage(int dano, bool fallDamage = false)
    {
        if (stats.currentLife > 0)
        {
            stats.currentLife -= dano;
            SetLife();

            if (stats.currentLife <= 0)
            {
                stats.currentLife = 0;
                SetLife();

                if (fallDamage)
                    return 0;

                if (!Parter.ToVivo || !getUp)
                {
                    ToVivo = false;

                    rb.useGravity = false;
                    GetComponent<Collider>().enabled = false;
                    ZoneInterction.enabled = false;

                    if (!GetBool("Fallen"))
                    {
                        anim.SetTrigger("Fall");
                    }

                    anim.SetTrigger("Death");
                    StartCoroutine(Dissolve());
                }
                else if (getUp)
                {
                    anim.SetTrigger("Fall");
                    ZoneInterction.enabled = true;
                    getUp = false;
                    StartCoroutine("Fall");
                }
            }
        }

        return stats.currentLife;
    }

    public bool Caido { get => GetBool("Fallen"); }
    public GameObject pressA;

    IEnumerator Fall()
    {
        rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.velocity = Vector3.zero;

        rb.useGravity = false;
        GetComponent<CapsuleCollider>().enabled = false;
        transform.GetChild(4).GetComponent<Collider>().enabled = false;

        stats.currentLife += (int)(stats.maxLife * 0.3f);
        SetLife();
        yield return new WaitForSeconds(1f);

        while (GetBool("Fallen"))
        {
            pressA.SetActive(true);
            if (Parter && Parter.Caido)
            {
                SetDamage(stats.currentLife);
                break;
            }
            else
            {
                int d = Mathf.RoundToInt(stats.maxLife * 0.01f);
                SetDamage(d);
                yield return new WaitForSeconds(1f);
            }
        }

        //GetComponent<CapsuleCollider>().enabled = true;
        //Invoke("BackGravy", .5f);

        transform.GetChild(4).GetComponent<Collider>().enabled = true;

        rb.constraints = ~RigidbodyConstraints.FreezePosition;

        yield return null;
    }

    public void _Revive()
    {
        stats.currentLife = stats.maxLife;
        SetLife();

        Renderer[] rends = GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in rends)
        {
            try
            {
                rend.material.SetFloat("DissolveAmount", 1);
            }
            catch (System.Exception) { }
        }

        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(true);
        transform.GetChild(4).gameObject.SetActive(true);
        transform.GetChild(5).gameObject.SetActive(true);
        transform.GetChild(6).gameObject.SetActive(true);
        //transform.GetChild(7).gameObject.SetActive(true);

        GetComponent<CapsuleCollider>().enabled = true;
        //rb.useGravity = true;
        Invoke("BackGravy", 0.5f);

        getUp = true;
        ToVivo = true;
    }

    void BackGravy()
    {
        rb.useGravity = true;
    }

    void RemoveCollider()
    {
        GetComponent<CapsuleCollider>().enabled = false;
    }

    IEnumerator Dissolve()
    {
        Renderer[] rends = GetComponentsInChildren<Renderer>();

        for (float i = -1; i < 1; i += Time.deltaTime * 3)
        {
            foreach (Renderer rend in rends)
            {
                try
                {
                    rend.material.SetFloat("DissolveAmount", i);
                }
                catch (System.Exception) { }
            }

            yield return null;
        }

        yield return null;
    }

    public void Kill()
    {
        rb.velocity = Vector3.zero;
        stats.currentLife = 0;
        SetLife();

        rb.useGravity = false;
        Invoke("RemoveCollider", 0.5f);
        //GetComponent<BoxCollider>().enabled = false;

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);
        transform.GetChild(5).gameObject.SetActive(false);
        transform.GetChild(6).gameObject.SetActive(false);
        transform.GetChild(7).gameObject.SetActive(false);
        GameController.Singleton.CheckPlayerIsAlive();

        ToVivo = false;
    }

    public void Test()
    {
        ToVivo = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);
    }

    public void HealFull()
    {
        stats.currentLife = stats.maxLife;
        getUp = true;
        SetLife();
    }

    public void Heal(int value)
    {
        if (Caido)
            return;

        if (ToVivo)
        {
            if (stats.currentLife > 0)
            {
                stats.currentLife += value;
                if (stats.currentLife > stats.maxLife)
                    stats.currentLife = stats.maxLife;

                SetLife();
            }
        }
    }

    public void SetLifeFull()
    {
        getUp = true;
        stats.currentLife = stats.maxLife;
        SetLife();
    }

    void SetLife()
    {
        HUD.setLife(stats.currentLife, stats.maxLife);
    }

    public void SetFloat(string name, float val)
    {
        anim.SetFloat(name, val);
    }

    public void Revive()
    {
        GetComponent<CapsuleCollider>().enabled = true;
        Invoke("BackGravy", .5f);
        anim.SetBool("Fallen", false);
        pressA.SetActive(false);
        stats.currentLife += (int)(stats.maxLife * 0.4f);
        SetLife();
        ZoneInterction.enabled = false;
    }

    float revive_value = 0;
    const float revive_time = 2;
    bool StartRevive = false;
    public ReviveAlly revi;

    protected void ReviveFriend(string Button)
    {
        if (Input.GetButtonDown(Button) && !StartRevive)
        {
            StartRevive = true;
            SetBool("Reviving", true);
            transform.rotation.SetLookRotation(Parter.transform.position);
        }

        if (StartRevive)
        {
            if (Input.GetButton(Button))
            {
                revive_value += Time.deltaTime;
                Parter.revi.FillPercentage(revive_value, revive_time);

                if (revive_value >= revive_time)
                {
                    Parter.revi.ResetPercentage();
                    Parter.Revive();
                    ResetRevive();
                }
            }
            else
            {
                Parter.revi.ResetPercentage();
                ResetRevive();
            }
        }
    }

    protected void ResetRevive()
    {
        revive_value = 0;
        StartRevive = false;
        SetBool("Reviving", false);
    }

    public void BackToLife(bool alive = true)
    {
        ToVivo = alive;
        getUp = alive;
        transform.GetChild(0).gameObject.SetActive(alive);
        transform.GetChild(2).gameObject.SetActive(alive);
        transform.GetChild(3).gameObject.SetActive(alive);
        GetComponent<Rigidbody>().useGravity = alive;
        GetComponent<Collider>().enabled = alive;
    }

    #endregion

    public void InpulsePlayer(Vector3 Direction, float newDrag = -1)
    {
        if (Parter)
        {
            if (Parter.ToVivo)
            {
                Vector3 FuturePos = transform.position + transform.forward * 2;
                Vector3 PosComparacao = Parter.transform.position;
                FuturePos.y = 0;
                PosComparacao.y = 0;


                float MaxDistance = GameController.Singleton ? GameController.Singleton.MaxDistancePlayers : 20;

                if ((FuturePos - PosComparacao).magnitude > MaxDistance)
                {
                    rb.velocity = new Vector3(0, rb.velocity.y, 0);
                    return;
                }
            }
        }

        if (newDrag >= 0)
            rb.drag = newDrag;

        rb.AddForce(Direction, ForceMode.Impulse);
    }

    public void InpulsePlayer(Vector3 Direction, float Force, float newDrag = -1)
    {
        if (newDrag >= 0)
            rb.drag = newDrag;

        rb.AddForce(Direction * Force, ForceMode.Impulse);
    }

    public void newVelocity(Vector3 n_Vel)
    {
        rb.velocity = n_Vel;
    }

    public void DoDash()
    {
        Vector3 moveDir = getMoveDir();

        InpulseRotate(moveDir);
        rb.velocity = moveDir * getStats().force;
    }

    Vector3 getMoveDir()
    {
        Vector3 v = vertical * Vector3.forward;
        Vector3 h = horizontal * Vector3.right;
        Vector3 moveDir = (v + h).normalized;
        return moveDir;
    }

    public void InpulsePlayer()
    {
        if (getMoveDir() == Vector3.zero)
        {
            Vector3 Direction = transform.TransformDirection(Vector3.forward) * getStats().force;
            InpulsePlayer(Direction, 0);
        }
        else
        {
            InpulseRotate(moveDir);
            //Vector3 Direction = moveDir * getStats().force;
            Vector3 Direction = transform.TransformDirection(Vector3.forward) * getStats().force;
            rb.velocity = Vector3.zero;
            InpulsePlayer(Direction, 0);
        }
    }

    void InpulseRotate(Vector3 moveDir)
    {
        Vector3 targetDir = moveDir;
        targetDir.y = 0;
        Quaternion tr;
        tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotateSpeed);
        transform.localRotation = targetRotation;
    }

    public Transform GetPosition()
    {
        return transform;
    }

    #region Skills

    [HideInInspector]
    public Dash Dash;
    [HideInInspector]
    public Skill Sup;
    [HideInInspector]
    public Skill Control;
    [HideInInspector]
    public Skill Damage;

    protected virtual void UseSkill(Skill Skill, float Use)
    {
        if (Skill.IsAvaliable() && Use == 1 && OnGround() && GetBool("canCast"))
        {
            //Skill.Play();
            SetTrigger(Skill.StringTrigger);
        }
    }

    protected virtual void UseSkill(Skill Skill, bool Use)
    {
        if (Skill.IsAvaliable() && Use && OnGround() && GetBool("canCast"))
        {
            //Skill.Play();
            SetTrigger(Skill.StringTrigger);
        }
    }

    public void SetTrigger(string Trigger)
    {
        anim.SetTrigger(Trigger);
    }

    public bool GetBool(string Bool)
    {
        return anim.GetBool(Bool);
    }

    protected void UseDash(float Use)
    {
        if (Dash.IsAvaliable() && Use == 1 && GetBool("canDash") && Dash.getCount() > 0) /*&& OnGround()*/
        {
            anim.SetTrigger(Dash.StringTrigger);
        }
    }

    #endregion

    #region Mark Controller

    protected GameObject MarkIndicator;

    private void LateUpdate()
    {
        if (ToVivo && !Caido)
        {
            MarkOnGround();
            rb.useGravity = anim.GetBool("Gravity");
            rb.constraints = anim.GetBool("Charging") ? RigidbodyConstraints.FreezePosition : ~RigidbodyConstraints.FreezePosition;
        }
    }

    private void MarkOnGround()
    {
        RaycastHit ground;
        if (Physics.Raycast(transform.position, Vector3.down, out ground, 5))
        {
            if (ground.collider.gameObject.CompareTag("Terrain") || ground.collider.gameObject.CompareTag("StaticObject"))
            {
                MarkIndicator.SetActive(true);
                MarkIndicator.transform.localPosition = new Vector3(0, ground.distance * -1, 0);
            }
        }
        else
        {
            MarkIndicator.SetActive(false);
        }
    }

    #endregion

    protected void Cheats()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            SetDamage(stats.currentLife);
        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
            Heal(stats.maxLife);
            ToVivo = true;
            gameObject.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if (ToVivo)
        {
            //ForwardRaycast();
            Move();

            if (Parter)
            {
                if (!GetBool("canMove"))
                {
                    if (Parter.ToVivo)
                    {
                        Vector3 FuturePos = transform.position + transform.forward * 2;
                        Vector3 PosComparacao = Parter.transform.position;
                        FuturePos.y = 0;
                        PosComparacao.y = 0;


                        float MaxDistance = GameController.Singleton ? GameController.Singleton.MaxDistancePlayers : 20;

                        if ((FuturePos - PosComparacao).magnitude > MaxDistance)
                        {
                            rb.velocity = new Vector3(0, rb.velocity.y, 0);
                            return;
                        }
                    }
                }
            }
        }

        //transform.GetChild(7).LookAt(Camera.main.gameObject.transform);
        //Quaternion[] Quarts = new Quaternion[9];
        //Vector3 central = this.transform.forward;
        //float angle = -18;
        //for (int i = 0; i < Quarts.Length; i++)
        //{
        //    Quarts[i] = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));
        //    angle += 4.5f;
        //}

        //foreach (Quaternion item in Quarts)
        //{
        //    Debug.DrawRay(this.transform.position + new Vector3(0, 1, 0), item * central * 40, Color.black);
        //}
    }

    protected virtual void Start()
    {
        init();
        //StartCoroutine("CombatZone");
        Physics.IgnoreCollision(transform.GetChild(4).GetComponent<Collider>(), GetComponent<Collider>()); ;
    }

    bool inCombat = false;
    bool asCombat = false;
    protected List<EnemyController_Old> l_Inimigos;

    IEnumerator CombatZone()
    {
        yield return new WaitForSeconds(5f);

        while (ToVivo)
        {
            try
            {
                l_Inimigos = new List<EnemyController_Old>();
                Collider[] l_collider = Physics.OverlapSphere(transform.position, 25, 16384);

                foreach (Collider obj in l_collider)
                {
                    EnemyController_Old inimigo = obj.GetComponent<EnemyController_Old>();
                    if (inimigo)
                    {
                        if ((inimigo.Enemy.isAlive && inimigo.Enemy.inCombat) && !l_Inimigos.Contains(inimigo))
                        {
                            l_Inimigos.Add(inimigo);
                            inCombat = true;
                        }
                    }
                }

                if (l_Inimigos.Count == 0)
                {
                    if (inCombat)
                        asCombat = true;

                    inCombat = false;
                }

                if (asCombat)
                {
                    StartCoroutine("AutoHeal");
                    asCombat = false;
                }

                //yield return new WaitForFixedUpdate();
            }
            catch (System.Exception e)
            {
                string t = e.Message;
                //yield return new WaitForFixedUpdate();
            }

            yield return new WaitForFixedUpdate();

        }

        yield return null;
    }

    IEnumerator AutoHeal()
    {
        yield return new WaitForSeconds(7f);

        while (stats.currentLife < stats.maxLife)
        {
            if (inCombat || GetBool("Fallen"))
                break;

            Heal(Mathf.RoundToInt(stats.maxLife * 0.05f));
            yield return new WaitForSeconds(3f);
        }

        yield return null;
    }


    PlayerController Parceiro;

    protected PlayerController Parter
    {
        get
        {
            if (Parceiro)
                return Parceiro;
            else
                return null;
        }
        set { Parceiro = value; }
    }

    public void SetParter(PlayerController n_Parter)
    {
        Parter = n_Parter;
    }

    private Vector3 LastIslandPosition;

    public void SetIslandPoint(Vector3 Pos)
    {
        LastIslandPosition = Pos;
    }

    int FallDamage = 10;
    public void RespawnOnLastIsland()
    {
        if (LastIslandPosition != Vector3.zero)
        {
            if (SetDamage(FallDamage, true) > 0)
            {
                StopCoroutine(Pisca());

                if (Parter.ToVivo)
                {
                    Vector3 FuturePos = LastIslandPosition;
                    Vector3 PosComparacao = Parter.transform.position;
                    FuturePos.y = 0;
                    PosComparacao.y = 0;

                    float MaxDistance = GameController.Singleton ? GameController.Singleton.MaxDistancePlayers : 20;

                    if ((FuturePos - PosComparacao).magnitude > MaxDistance)
                    {
                        transform.position = Parter.transform.position + Vector3.right * 2;
                    }
                    else
                    {
                        transform.position = LastIslandPosition;
                    }
                }
                else
                {
                    transform.position = LastIslandPosition;
                }


                StartCoroutine(Pisca());
            }
            else
            {
                Kill();
            }
        }
    }

    IEnumerator Pisca()
    {
        float BlinkTime = Time.time + 1;

        SkinnedMeshRenderer[] Meshs = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        MeshRenderer[] MeshsR = transform.GetComponentsInChildren<MeshRenderer>();

        while (BlinkTime > Time.time)
        {
            foreach (SkinnedMeshRenderer Mesh in Meshs)
            {
                Mesh.enabled = !Mesh.enabled;
            }

            foreach (MeshRenderer Mesh in MeshsR)
            {
                Mesh.enabled = !Mesh.enabled;
            }

            yield return new WaitForSeconds(0.2f);
        }

        foreach (SkinnedMeshRenderer Mesh in Meshs)
        {
            Mesh.enabled = true;
        }

        foreach (MeshRenderer Mesh in MeshsR)
        {
            Mesh.enabled = true;
        }

        yield return null;
    }

    public void NewVelocity(Vector3 Velocity)
    {
        rb.velocity = Velocity;
    }
}