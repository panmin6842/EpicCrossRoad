using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    Animator ani;
    SlotManager theSlotManager;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        theSlotManager = FindObjectOfType<SlotManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ani.GetCurrentAnimatorStateInfo(0).IsName("SwordEffect2") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName("SwordEffect1") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName("SpearEffect1") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName("GauntletEffect") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName("DiamondEffect1") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName("DiamondEffect2") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName("DiamondEffect3") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName("Gauntlet_Nomal_Effect1") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName("Gauntlet_Normal_Effect2") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName("Gauntlet_Normal_Effect3") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName("SpearEffect1") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName("SpearEffect2") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName("Boss_Rush_Effect") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName("BossAttack1_Swing") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName("BossAttack3_AttackEffect") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName("BossAttack2_DownEffect") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName("HealEffect_Exit") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            //theSlotManager.arrows[0].SetActive(true);
            //theSlotManager.arrows[1].SetActive(false);
            //theSlotManager.arrows[2].SetActive(false);
            theSlotManager.arrowAni.SetBool("UpExit", true);
            theSlotManager.arrowAni.SetBool("Down", false);
            theSlotManager.arrowAni.SetBool("Up", false);
            theSlotManager.sameItemGet = false;
            Destroy(this.gameObject);
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName("SpadeSkillEffect") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName("CloverSkillEffect") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName("BossHitEffect") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
        }
    }
}
