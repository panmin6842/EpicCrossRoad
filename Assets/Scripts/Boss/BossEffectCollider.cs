using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEffectCollider : MonoBehaviour
{
    GameObject skill2Effect;
    BoxCollider2D skill2Collider;

    private void Start()
    {
        skill2Effect = GameObject.FindGameObjectWithTag("SpaceKingSkill2Zone");
    }

    public void Skill2FirstAttackEnd()
    {
        skill2Collider.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void Skill2Collider()
    {
        skill2Collider.GetComponent<BoxCollider2D>().enabled = false;
    }
}
