using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPlay : MonoBehaviour
{
    SkillManager theSkillManager;
    PlayerManager thePlayerManager;

    Animator ani;
    Rigidbody2D rigid;

    bool yellowSkill;
    float time;
    public float strBuff; //공격력 +- 변수

    float dashSpeed; //창 공격시 대시

    GameObject spearEffect1; //창 이펙트 오브젝트
    bool spearEffectStart; //창 이펙트 나오면

    GameObject gauntletEffect1; //건틀렛 이펙트 오브젝트
    GameObject gauntletEffect2; //건틀렛 이펙트 오브젝트
    GameObject gauntletEffect3; //건틀렛 이펙트 오브젝트
    public bool gauntletEffectStart; //건틀렛 이펙트 나오면

    public bool diamondMode; //다이아몬드 기운을 먹으면 true
    public bool diamondModeCut;

    public enum State
    {
        Normal,
        Gauntlet,
        Sword,
        Stamp,
        Spear,
    }

    public State state;

    static public SkillPlay instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        theSkillManager = FindObjectOfType<SkillManager>();
        thePlayerManager = FindObjectOfType<PlayerManager>();
        ani = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        
        yellowSkill = false;
        spearEffectStart = false;
        gauntletEffectStart = false;
        diamondMode = false;
        diamondModeCut = false;
        dashSpeed = 70;

        state = State.Gauntlet;
    }

    // Update is called once per frame
    void Update()
    {
        theSkillManager = FindObjectOfType<SkillManager>();
        thePlayerManager = FindObjectOfType<PlayerManager>();
        if (!GameManager.instance.appear)
        {
            SkillSwitch();
        }

        if (transform.eulerAngles == new Vector3(0, 0, 0)) //오른쪽
        {
            theSkillManager.isRight = true;
        }
        if (transform.eulerAngles == new Vector3(0, 180, 0)) //왼쪽
        {
            theSkillManager.isRight = false;
        }

    }

    void SkillSwitch()
    {
        switch (state)
        {
            case State.Normal:
                {
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        ani.SetBool(NotChange.instance.IsGauntlet, true);
                        state = State.Gauntlet;
                    }
                }
                break;
            case State.Gauntlet:
                {
                    AnimationTrigger("S", "St", "Sp", "G");
                    theSkillManager.GauntletAttack(ani);

                    theSkillManager.swordCollider.SetActive(false);
                    theSkillManager.spearCollider.SetActive(false);

                    if(PlayerManager.instance.diamondBar.fillAmount > 0 && !diamondModeCut)
                    {
                        //ani.runtimeAnimatorController = SkillManager.instance.playerAniOverride; //애니메이션 바꾸기
                        diamondMode = true;
                        ani.SetBool("isGauntlet", true);
                        time = Time.deltaTime;
                        thePlayerManager.DiamondBar(time);
                        //공격력 버프
                        strBuff = 10;
                    }
                    else if(PlayerManager.instance.diamondBar.fillAmount <= 0 && diamondMode && !PlayerCtrl.instance.hit)
                    {
                        strBuff = 0;
                        theSkillManager.attack1 = false;
                        theSkillManager.attack2 = false;
                        theSkillManager.attack3 = false;
                        diamondMode = false;
                        thePlayerManager.diamondProfile.SetActive(false);
                        thePlayerManager.normalProfile.SetActive(true);
                        time = 0;
                    }

                    if (diamondModeCut)
                    {
                        thePlayerManager.diamondBar.fillAmount = 0;
                        strBuff = 0;
                        theSkillManager.attack1 = false;
                        theSkillManager.attack2 = false;
                        theSkillManager.attack3 = false;
                        diamondModeCut = false;
                        diamondMode = false;
                        thePlayerManager.diamondProfile.SetActive(false);
                        thePlayerManager.normalProfile.SetActive(true);
                        time = 0;
                        //ani.runtimeAnimatorController = SkillManager.instance.playerAni;
                    }

                    if (gauntletEffectStart) //이펙트가 소환된 좌표를 따라가도록 함
                    {
                        if (gauntletEffect1 != null)
                        {
                            gauntletEffect1.transform.position = theSkillManager.gauntletEffectPos[0].position;
                        }
                        if (gauntletEffect2 != null)
                        {
                            gauntletEffect2.transform.position = theSkillManager.gauntletEffectPos[1].position;
                        }
                        if (gauntletEffect3 != null)
                        {
                            gauntletEffect3.transform.position = theSkillManager.gauntletEffectPos[2].position;
                        }
                    }
                }
                break;

            case State.Sword:
                {
                    AnimationTrigger("G", "St", "Sp", "S");
                    theSkillManager.SwordAttack(ani);
                    strBuff = 0;
                    diamondMode = false;

                    theSkillManager.spearCollider.SetActive(false);

                    if (PlayerManager.instance.spadeBar.fillAmount <= 0 && !SkillManager.instance.redSkillStart && !PlayerCtrl.instance.hit) //스킬 3번 다 쓰면 건틀렛으로 감
                    {
                        thePlayerManager.spadeProfile.SetActive(false);
                        thePlayerManager.normalProfile.SetActive(true);
                        theSkillManager.attack1 = false;
                        theSkillManager.attack2 = false;
                        theSkillManager.attack3 = false;
                        theSkillManager.swordCollider.SetActive(false);
                        thePlayerManager.cloverProfile.SetActive(false);
                        time = 0;
                        ani.ResetTrigger("Sword");
                        ani.SetTrigger("Gauntlet");
                        ani.SetBool("isGauntlet", true);
                        state = State.Gauntlet;
                    }
                    else
                    {
                        if(GameState.instance.skillCountMinStop == false)
                            time = Time.deltaTime;

                        thePlayerManager.SpadeBar(time);
                    }
                }
                break;
            case State.Stamp:
                {
                    AnimationTrigger("S", "G", "Sp", "St");
                    theSkillManager.StampAttack(ani);
                    strBuff = 0;
                    diamondMode = false;

                    theSkillManager.swordCollider.SetActive(false);
                    theSkillManager.spearCollider.SetActive(false);

                    if (PlayerManager.instance.heartBar.fillAmount <= 0 && !SkillManager.instance.stampStart && !PlayerCtrl.instance.hit) //스킬 3번 다 쓰면 건틀렛으로 감
                    {
                        thePlayerManager.heartProfile.SetActive(false);
                        thePlayerManager.normalProfile.SetActive(true);
                        theSkillManager.attack1 = false;
                        theSkillManager.attack2 = false;
                        theSkillManager.attack3 = false;
                        thePlayerManager.cloverProfile.SetActive(false);
                        time = 0;
                        ani.ResetTrigger("Stamp");
                        ani.SetTrigger("Gauntlet");
                        state = State.Gauntlet;
                    }
                    else
                    {
                        time = Time.deltaTime;

                        thePlayerManager.HeartBar(time);
                    }
                }
                break;
            case State.Spear:
                {
                    AnimationTrigger("S", "St", "G", "Sp");
                    theSkillManager.SpearAttack(ani);
                    strBuff = 0;
                    diamondMode = false;

                    theSkillManager.swordCollider.SetActive(false);

                    if (PlayerManager.instance.cloverBar.fillAmount <= 0 && !SkillManager.instance.spearStart && !PlayerCtrl.instance.hit) //스킬 3번 다 쓰면 건틀렛으로 감
                    {
                        thePlayerManager.cloverProfile.SetActive(false);
                        thePlayerManager.normalProfile.SetActive(true);
                        theSkillManager.attack1 = false;
                        theSkillManager.attack2 = false;
                        theSkillManager.attack3 = false;
                        thePlayerManager.cloverProfile.SetActive(false);
                        theSkillManager.spearCollider.SetActive(false);

                        time = 0;
                        ani.ResetTrigger("Spear");
                        ani.SetTrigger("Gauntlet");
                        state = State.Gauntlet;
                    }
                    else
                    {
                        time = Time.deltaTime;
                        thePlayerManager.CloverBar(time);
                    }

                    if (spearEffectStart && spearEffect1 != null)
                    {
                        spearEffect1.transform.position = theSkillManager.spearEffect1Pos.position;
                    }
                }
                break;
        }
    }

    void AnimationTrigger(string resetTrigger1, string resetTrigger2, string resetTrigger3, string setTrigger)
    {
        ani.ResetTrigger(resetTrigger1);
        ani.ResetTrigger(resetTrigger2);
        ani.ResetTrigger(resetTrigger3);
        ani.SetTrigger(setTrigger);
    }

    IEnumerator YellowSkillTime()
    {
        yield return new WaitForSeconds(1.5f);
        yellowSkill = false;
    }

    public void SpadeSkillGo()
    {
        SoundManager.instance.playerAttackSource.PlayOneShot(SoundManager.instance.swordAttack);
        theSkillManager.swordCollider.SetActive(true);
        
    }
    public void SpadeAttackSFX()
    {
        SoundManager.instance.playerAttackSource.PlayOneShot(SoundManager.instance.swordAttack);
        theSkillManager.redSkillStart = true;
    }
    public void SpadeColliderFalse()
    {
        theSkillManager.swordCollider.SetActive(false);
        theSkillManager.redSkillStart = false;
    }

    public void StampColliderGo()
    {
        SoundManager.instance.playerAttackSource.PlayOneShot(SoundManager.instance.stampAttack2);
        GameObject newStampEffect = Instantiate(SkillManager.instance.stampEffect, SkillManager.instance.stampEffectPos.position, Quaternion.identity);
        //SkillManager.instance.StampCollider.SetActive(true);
        //SkillManager.instance.HeartSkillAppear();
    }

    public void SpearSkillMove()
    {
        theSkillManager.spearCollider.SetActive(true);
        if (theSkillManager.isRight)
            rigid.velocity = new Vector2(dashSpeed, transform.position.y);
        else
            rigid.velocity = new Vector2(-dashSpeed, transform.position.y);
    }

    public void SpearColliderTrue()
    {
        spearEffectStart = true;
        theSkillManager.spearCollider.SetActive(true);
        //if (SkillManager.instance.isRight)
        //    rigid.velocity = new Vector2(dashSpeed, transform.position.y);
        //else
        //    rigid.velocity = new Vector2(-dashSpeed, transform.position.y);
        
    }
    public void SpearColliderFalse()
    {
        rigid.velocity = Vector2.zero;
        theSkillManager.spearCollider.SetActive(false);
        spearEffectStart = false;
    }
    public void SpearEffectGo()
    {
        SoundManager.instance.playerAttackSource.PlayOneShot(SoundManager.instance.spearAttack);
        //spearEffect1 = SkillManager.instance.SpearEffectAppear();
    }

    public void SpearIdleError()
    {
        theSkillManager.spearCollider.SetActive(false);
        theSkillManager.attack1 = false;
        theSkillManager.attack2 = false;
        theSkillManager.spearStart = false;
    }

    public void GauntletFirstCollider()
    {
        theSkillManager.gAttackCollider[0].SetActive(true);
        PlayerCtrl.instance.move = false;
;
        SoundManager.instance.playerAttackSource.PlayOneShot(SoundManager.instance.gauntletAttack1);
        if (diamondMode)
        {
            if (theSkillManager.isRight)
                gauntletEffect1 = theSkillManager.newGauntletEffect =
                Instantiate(theSkillManager.gauntletEffect1, theSkillManager.gauntletEffectPos[0].position, Quaternion.Euler(0, 0, 0));
            else
                gauntletEffect1 = theSkillManager.newGauntletEffect =
                Instantiate(theSkillManager.gauntletEffect1, theSkillManager.gauntletEffectPos[0].position, Quaternion.Euler(0, 180, 0));
            gauntletEffectStart = true;

        }
        else
        {
            if (theSkillManager.isRight)
                gauntletEffect1 = theSkillManager.newGauntletEffect =
                Instantiate(theSkillManager.gauntletNormalEffect1, theSkillManager.gauntletEffectPos[0].position, Quaternion.Euler(0, 0, 0));
            else
                gauntletEffect1 = theSkillManager.newGauntletEffect =
                Instantiate(theSkillManager.gauntletNormalEffect1, theSkillManager.gauntletEffectPos[0].position, Quaternion.Euler(0, 180, 0));
            gauntletEffectStart = true;
        }
    }
    public void GauntletEffect2Appear()
    {
        if (diamondMode)
        {
            gauntletEffectStart = true;
            if (theSkillManager.isRight)
                gauntletEffect2 = theSkillManager.newGauntletEffect =
                Instantiate(theSkillManager.gauntletEffect2, theSkillManager.gauntletEffectPos[1].position, Quaternion.Euler(0, 0, 0));
            else
                gauntletEffect2 = theSkillManager.newGauntletEffect =
                Instantiate(theSkillManager.gauntletEffect2, theSkillManager.gauntletEffectPos[1].position, Quaternion.Euler(0, 180, 0));
        }
        else
        {
            gauntletEffectStart = true;
            if (theSkillManager.isRight)
                gauntletEffect2 = theSkillManager.newGauntletEffect =
                Instantiate(theSkillManager.gauntletNormalEffect2, theSkillManager.gauntletEffectPos[1].position, Quaternion.Euler(0, 0, 0));
            else
                gauntletEffect2 = theSkillManager.newGauntletEffect =
                Instantiate(theSkillManager.gauntletNormalEffect2, theSkillManager.gauntletEffectPos[1].position, Quaternion.Euler(0, 180, 0));
        }
    }
    public void GauntletEffect3Appear()
    {
        if (diamondMode)
        {
            gauntletEffectStart = true;
            if (theSkillManager.isRight)
                gauntletEffect3 = theSkillManager.newGauntletEffect =
                Instantiate(theSkillManager.gauntletEffect3, theSkillManager.gauntletEffectPos[2].position, Quaternion.Euler(0, 0, 0));
            else
                gauntletEffect3 = theSkillManager.newGauntletEffect =
                Instantiate(theSkillManager.gauntletEffect3, theSkillManager.gauntletEffectPos[2].position, Quaternion.Euler(0, 180, 0));
        }
        else
        {
            gauntletEffectStart = true;
            if (theSkillManager.isRight)
                gauntletEffect3 = theSkillManager.newGauntletEffect =
                Instantiate(theSkillManager.gauntletNormalEffect3, theSkillManager.gauntletEffectPos[2].position, Quaternion.Euler(0, 0, 0));
            else
                gauntletEffect3 = theSkillManager.newGauntletEffect =
                Instantiate(theSkillManager.gauntletNormalEffect3, theSkillManager.gauntletEffectPos[2].position, Quaternion.Euler(0, 180, 0));
        }
    }
    public void GauntletCollider1False()
    {
        theSkillManager.gAttackCollider[0].SetActive(false);
    }
    public void GauntletCollider2False()
    {
        theSkillManager.gAttackCollider[1].SetActive(false);
    }
    public void GauntletCollider3False()
    {
        theSkillManager.gAttackCollider[2].SetActive(false);
    }

    public void Idle()
    {
        theSkillManager.attack1 = false;
        theSkillManager.attack2 = false;
        ani.SetBool("isAttack1", false);
        ani.SetBool("isAttack2", false);
        ani.SetBool("isAttack3", false);
        theSkillManager.gAttackCollider[0].SetActive(false);
        theSkillManager.gAttackCollider[1].SetActive(false);
        theSkillManager.gAttackCollider[2].SetActive(false);
        rigid.gravityScale = 9f;
    }


}
