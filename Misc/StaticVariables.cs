using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticVariables
{
    public static class Animator
    {
        readonly public static string canMove = "canMove";
        readonly public static string canRotate = "canRotate";
        readonly public static string canAtack = "canAtack";
        readonly public static string atackTrigger = "Atack";
        readonly public static string jumpTrigger = "Jump";
        readonly public static string speedFloat = "Speed";
        readonly public static string DashTrigger = "Dash";
        readonly public static string canCast = "canCast";
    }

    public static class Tags
    {
        readonly public static string Untagged = "Untagged";
        readonly public static string Respawn = "Respawn";
        readonly public static string Finish = "Finish";
        readonly public static string EditorOnly = "EditorOnly";
        readonly public static string MainCamera = "MainCamera";
        readonly public static string Terrain = "Terrain";
        readonly public static string Player = "Player";
        readonly public static string Enemy = "Enemy";
    }

    public static class Layers
    {
        readonly public static string Default = "Default";
        readonly public static string TransparentFX = "TransparentFX";
        readonly public static string IgnoreRaycast = "IgnoreRaycast";
        readonly public static string Water = "Water";
        readonly public static string UI = "UI";
        readonly public static string Terrain = "Terrain";
        readonly public static string Players = "Players";
        readonly public static string Enemys = "Enemys";
        readonly public static string Player_Hit = "Player_Hit";
        readonly public static string Enemy_Hit = "Enemy_Hit";
        readonly public static string HitColliders = "HitColliders";
    }

    public static class Names
    {
        readonly public static string Juninho = "Player 1";
        readonly public static string Angie = "Player 2";
    }
}
