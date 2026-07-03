using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotChange : MonoBehaviour
{
    static public NotChange instance;

    private void Awake()
    {
        instance = this;
    }

    public readonly string IsWalk = "isWalk";
    public readonly string IsRun = "isRun";
    public readonly string IsJump = "isJump";
    public readonly string IsAttack1 = "isAttack1";
    public readonly string IsAttack2 = "isAttack2";
    public readonly string IsAttack3 = "isAttack3";
    public readonly string IsGauntlet = "isGauntlet";
    public readonly string Die = "Die";
    public readonly string Hit = "Hit";
}
