using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsAnimation : MonoBehaviour
{
    protected PlayerController Player;

    protected virtual void Start()
    {
        Player = GetComponentInParent<PlayerController>();
    }

    public virtual void turnOffAtack()
    {
        Player.setCanAtack(false);
    }

    public virtual void turnOnAtack()
    {
        Player.setCanAtack(true);
    }

    public virtual void MoveOnAtack(float force)
    {
        Player.MoveOnAtack(force);
    }

    public void MoveOnAnimation(float val)
    {
        Player.MoveOnAnimation(val);
    }

    public virtual void setCanMove(int _param)
    {
        bool Move = _param == 0 ? true : false;
        Player.AtackOnMoveHandle(Move);
    }

    public void canMove()
    {
        Player.SetBool("canMove", true);
    }

    public void Locomotion()
    {
        Player.SetFloat("Speed", 0.1f);
    }

    public void ExecuteDash()
    {
        Player.Dash.Play();
    }

    public void Morte()
    {
        Invoke("D", 2f);
    }

    void D()
    {
        //Player.transform.GetChild(0).gameObject.SetActive(false);
        //Player.transform.GetChild(2).gameObject.SetActive(false);
        //Player.transform.GetChild(3).gameObject.SetActive(false);
        //Player.transform.GetChild(4).gameObject.SetActive(false);
        //Player.transform.GetChild(5).gameObject.SetActive(false);
        //Player.transform.GetChild(6).gameObject.SetActive(false);
        //Player.transform.GetChild(7).gameObject.SetActive(false);

        //GameController.Singleton.CheckPlayerIsAlive();

        Player.Kill();
    }

    public void SetRotateSpeed(float _value)
    {
        Player.SetRotateSpeed(_value);
    }

    public void setRotate(int val)
    {
        bool b = val == 0 ? false : true;
        Player.SetBool("canRotate", b);
    }

    public void AttackHandle()
    {
        Player.AttackHandle();
    }

    public void Jump()
    {
        Player.Jump();
    }

    public void ZeroVelocity()
    {
        Player.NewVelocity(Vector3.zero);
    }
}
