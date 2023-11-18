using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Variables 
{
    public static bool isGrounded;
    public static bool isOnPlatform;
    public static bool isTarget;
    public static bool isShooting;
    public static bool isDownFromPlatform;
    public static Vector2 playerVelocity;
    public static GameObject currentPlatform;
    public static bool bossDead;
    public static bool lose;
    public static bool win;
    public static bool platformChecking;
    public static bool isGrounded2;
    public static bool isOnPlatform2;
    public static bool isTarget2;
    public static bool isShooting2;
    public static bool isDownFromPlatform2;
    public static Vector2 playerVelocity2;
    public static GameObject currentPlatform2;
    public static bool platformChecking2;
    public static bool vSync;
    public static float speedFroGround;
    public static int bossAllPhaseHealth;
    public static int bossFirstHealth;
    public static int[] starForEachLevel = new int[30];
    public static bool[] simpleForEachLevel = new bool[30];
    public static bool[] missileForEachLevel = new bool[30];
    public static bool[] atomicBombForEachLevel = new bool[30];
    public static bool isGamePaused;
    public static bool isMissileMode;
    public static Color32 uiNormalColor = new Color32(32, 29 , 27 , 255);
    //public static Color32 uiHighlightedColor = new Color32(0, 114, 255, 255);
    public static Color32 uiHighlightedColor = new Color32(11, 0, 255, 255);
    public static bool uiAcceptAction;
    public static bool uiCancelAction;
    public static Vector2 uiMoveVectorAction;
    public static bool uiAllowMoveAction;
    public static bool playBtnTapSound;
    public static bool uiActivated;
    public static bool isPaused;
    public static bool resumeGame;
    public static bool[] playerDamage = new bool[2];
    public static bool[] playerRevive = new bool[2];
    public static bool isMultiPlayer;
    public static int[] playerHealth = new int[2];
}
