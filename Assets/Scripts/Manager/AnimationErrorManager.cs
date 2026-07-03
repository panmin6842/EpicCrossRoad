using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationErrorManager : MonoBehaviour
{
    Animator ani;

    public float time;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Gauntlet_Attack1") || ani.GetCurrentAnimatorStateInfo(0).IsName("Spear_Attack1"))
        {
            time += Time.deltaTime;
        }
        if ((ani.GetCurrentAnimatorStateInfo(0).IsName("Run") || ani.GetCurrentAnimatorStateInfo(0).IsName("Sword_run")
            || ani.GetCurrentAnimatorStateInfo(0).IsName("Stamp_Run") || ani.GetCurrentAnimatorStateInfo(0).IsName("Spear_Run")))
        {
            time += Time.deltaTime;
        }
        else
        {
            time = 0;
        }

        if(time >= 2)
        {
            SkillManager.instance.attack1 = false;
            SkillManager.instance.attack2 = false;
            SkillManager.instance.attack3 = false;
            SkillManager.instance.redSkillStart = false;
            SkillManager.instance.spearStart = false;
            SkillManager.instance.stampStart = false;
            ani.SetBool("isAttack1", false);
            ani.SetBool("isAttack2", false);
            ani.SetBool("isAttack3", false);
            time = 0;
        }
    }
}
