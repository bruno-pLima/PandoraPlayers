using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class playerStats
{
    public int currentLife;
    public int maxLife;
    public float speed;
    public float j_speed;
    public float jumpForce;
    public float force;

    public playerStats(int n_hp, float n_speed, float n_j_speed, float n_jump, float n_force)
    {
        currentLife = maxLife = n_hp;
        speed = n_speed;
        jumpForce = n_jump;
        force = n_force;
        j_speed = n_j_speed;
    }

    public playerStats()
    { }

    public void setValues(int n_hp, float n_speed, float n_j_speed, float n_jump, float n_force)
    {
        currentLife = maxLife = n_hp;
        speed = n_speed;
        jumpForce = n_jump;
        force = n_force;
        j_speed = n_j_speed;
    }
}
