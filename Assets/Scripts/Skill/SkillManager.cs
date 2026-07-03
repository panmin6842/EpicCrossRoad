using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public Rigidbody2D playerRid;

    //[SerializeField] GameObject swordEffect1;
    //[SerializeField] Transform swordEffect1Pos;
    //[SerializeField] GameObject swordEffect2;
    //[SerializeField] Transform swordEffect2Pos;

    [SerializeField] GameObject spearEffect1;
    [SerializeField] GameObject spearEffect2;
    public Transform spearEffect1Pos;

    public GameObject gauntletEffect1;
    public GameObject gauntletEffect2;
    public GameObject gauntletEffect3;
    public GameObject gauntletNormalEffect1;
    public GameObject gauntletNormalEffect2;
    public GameObject gauntletNormalEffect3;
    public AnimatorOverrideController playerAniOverride;
    public RuntimeAnimatorController playerAni;
    public Transform[] gauntletEffectPos;

    public GameObject stampEffect;

    public GameObject[] gAttackCollider; //건틀렛 콜라이더
    public GameObject swordCollider; //검 콜라이더
    public GameObject spearCollider; //창 콜라이더
    public Transform stampEffectPos; //스태프 콜라이더

    public bool isRight = true;
    public bool attack1; //두번째 공격 하는데 첫번째 스킬이 안 나오게 하기 위해서
    public bool attack2; //세번째 공격 하는데 두번째 스킬이 안 나오게 하기 위해서
    public bool attack3;
    public float time; //공격 타이밍
    public float time2; //공격 타이밍

    public bool redSkillStart;
    public bool gauntletStart;
    public bool spearStart;
    public bool stampStart;
    public bool gauntletNoHit;

    public int spadeSkillCount = 0;
    public int heartSkillCount = 0;
    public int cloverSkillCount = 0;

    //GameObject newSwordEffect2;
    //GameObject newSwordEffect1;
    public GameObject newSpearEffect1;
    public GameObject newSpearEffect2;
    public GameObject newGauntletEffect;
    GameObject player;

    public PlayerCtrl thePlayerCtrl;

    static public SkillManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        attack1 = false;
        attack2 = false;
        attack3 = false;
        redSkillStart = false;
        gauntletStart = false;
        stampStart = false;
        gauntletNoHit = false;

        spadeSkillCount = 3;
        heartSkillCount = 3;
        cloverSkillCount = 3;
        time = 0;
        time2 = 0;

        thePlayerCtrl = FindObjectOfType<PlayerCtrl>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    //public GameObject RedSkillAppear()
    //{
    //    //플레이어 방향에 따라서 이펙트 방향이 달라짐
    //    if (player.transform.eulerAngles == new Vector3(0, 0, 0)) //오른쪽
    //    {
    //        isRight = true;

    //        //newSwordEffect2 = Instantiate(swordEffect2, swordEffect2Pos.position, Quaternion.Euler(0, 180, 0));
    //    }
    //    if (player.transform.eulerAngles == new Vector3(0, 180, 0)) //왼쪽
    //    {
    //        isRight = false;

    //        //newSwordEffect2 = Instantiate(swordEffect2, swordEffect2Pos.position, Quaternion.identity);
    //    }

    //    //return newSwordEffect2;
    //}

    public GameObject SpearEffectAppear()
    {
        //플레이어 방향에 따라서 이펙트 방향이 달라짐
        if (player.transform.eulerAngles == new Vector3(0, 0, 0)) //오른쪽
        {
            newSpearEffect1 = Instantiate(spearEffect1, spearEffect1Pos.position, Quaternion.Euler(0, 0, 0));
            newSpearEffect2 = Instantiate(spearEffect2, spearEffect1Pos.position, Quaternion.Euler(0, 0, 0));
        }
        if (player.transform.eulerAngles == new Vector3(0, 180, 0)) //왼쪽
        {
            newSpearEffect1 = Instantiate(spearEffect1, spearEffect1Pos.position, Quaternion.Euler(0, 180, 0));
            newSpearEffect2 = Instantiate(spearEffect2, spearEffect1Pos.position, Quaternion.Euler(0, 180, 0));
        }

        return newSpearEffect1;
    }

    private float g_attack2RangeMin = 0.1f;
    private float g_attack2RangeMax = 0.4f;
    private float g_attack3RangeMin = 0.1f;
    private float g_attack3RangeMax = 0.3f;
    public void GauntletAttack(Animator ani)
    {
        if (!thePlayerCtrl.hit && !thePlayerCtrl.jump && !ani.GetCurrentAnimatorStateInfo(0).IsName("Gauntlet_Hit")) //맞은 상태가 아니고 점프 상태도 아니여야함
        {
            if ((Input.GetKeyDown(KeyCode.Z)) && !attack1 && !attack2 && !thePlayerCtrl.hit)
            {
                time2 = 0;
                time = 0;
                playerRid.velocity = Vector3.zero;
                //gauntletStart = true;
                ani.SetBool(NotChange.instance.IsAttack1, true);
                attack1 = true;
                
            }

            if (ani.GetCurrentAnimatorStateInfo(0).IsName("Gauntlet_Attack1") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && attack1)
            {
                gAttackCollider[0].SetActive(false);
                SkillPlay.instance.gauntletEffectStart = false;
                time += Time.deltaTime;
            }

            if (time < g_attack2RangeMax && time >= g_attack2RangeMin /*&& attack1*/)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    time = 0;
                    attack2 = true;
                    ani.SetBool(NotChange.instance.IsAttack1, false);
                    ani.SetBool(NotChange.instance.IsAttack2, true);
                    gAttackCollider[1].SetActive(true);

                    SoundManager.instance.playerAttackSource.PlayOneShot(SoundManager.instance.gauntletAttack1);

                    playerRid.AddForce(Vector2.up * 30, ForceMode2D.Impulse);
                    if (isRight)
                        playerRid.AddForce(Vector2.right * 6, ForceMode2D.Impulse);
                    else
                        playerRid.AddForce(Vector2.left * 6, ForceMode2D.Impulse);
                }

            }
            else if (time > g_attack2RangeMax && attack1)
            {
                //gauntletStart = false;
                attack1 = false;
                ani.SetBool(NotChange.instance.IsAttack1, false);
                time = 0;
            }

            if (ani.GetCurrentAnimatorStateInfo(0).IsName("Gauntlet_Attack2"))
            {
                ani.SetBool(NotChange.instance.IsAttack1, false);
            }

            if (ani.GetCurrentAnimatorStateInfo(0).IsName("Gauntlet_Attack2") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && attack2)
            {
                gAttackCollider[1].SetActive(false);
                SkillPlay.instance.gauntletEffectStart = false;
                time2 += Time.deltaTime;
            }

            if (time2 <= g_attack3RangeMax && time2 >= g_attack3RangeMin && attack2)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    gauntletNoHit = true;
                    time2 = 0;
                    ani.SetBool(NotChange.instance.IsAttack2, false);
                    ani.SetBool(NotChange.instance.IsAttack3, true);
                    gAttackCollider[2].SetActive(true);

                    SoundManager.instance.playerAttackSource.PlayOneShot(SoundManager.instance.gauntletAttack2);

                    playerRid.gravityScale = 20f;
                    if (isRight == true)
                        playerRid.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
                    if (isRight == false)
                        playerRid.AddForce(Vector2.left * 10, ForceMode2D.Impulse);

                }
            }
            else if (time2 > g_attack3RangeMax && attack2)
            {
                //gauntletStart = false;
                attack2 = false;
                attack1 = false;
                ani.SetBool(NotChange.instance.IsAttack2, false);
                time2 = 0;
            }

            if (ani.GetCurrentAnimatorStateInfo(0).IsName("Gauntlet_Attack3") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                gauntletNoHit = false;
                SkillPlay.instance.gauntletEffectStart = false;
                gAttackCollider[2].SetActive(false);
                playerRid.gravityScale = 9f;
                ani.SetBool(NotChange.instance.IsAttack1, false);
                ani.SetBool(NotChange.instance.IsAttack2, false);
                ani.SetBool(NotChange.instance.IsAttack3, false);
                ani.SetBool(NotChange.instance.IsWalk, false);
                ani.SetBool(NotChange.instance.IsRun, false);
                attack2 = false;
                attack1 = false;
                time = 0;
                time2 = 0;
                //gauntletStart = false;
            }
        }
    }

    public void SwordAttack(Animator ani)
    {
        if (!thePlayerCtrl.hit && !thePlayerCtrl.jump)
        {
            if ((Input.GetKeyDown(KeyCode.Z)) && !attack1 && !redSkillStart)
            {
                playerRid.velocity = Vector3.zero;
                time = 0;
                redSkillStart = true;
                ani.SetBool(NotChange.instance.IsAttack1, true);
                ani.SetBool(NotChange.instance.IsWalk, false);
                ani.SetBool(NotChange.instance.IsRun, false);
                ani.SetBool(NotChange.instance.IsJump, false);
                thePlayerCtrl.movSpeed = thePlayerCtrl.walkSpeed;
                thePlayerCtrl.move = false;
                attack1 = true;
            }

            if (ani.GetCurrentAnimatorStateInfo(0).IsName("Sword_Attack1") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                swordCollider.SetActive(false);
                ani.SetBool(NotChange.instance.IsAttack1, false);
                thePlayerCtrl.move = true;
                
                attack1 = false;
            }

        }
    }

    public void StampAttack(Animator ani)
    {
        if (!thePlayerCtrl.hit && !thePlayerCtrl.jump)
        {
            if ((Input.GetKeyDown(KeyCode.Z)) && attack1 == false)
            {
                stampStart = true;
                playerRid.velocity = Vector3.zero;
                time = 0;
                ani.SetBool(NotChange.instance.IsAttack1, true);
                thePlayerCtrl.movSpeed = thePlayerCtrl.walkSpeed;
                thePlayerCtrl.move = false;
                attack1 = true;
                SoundManager.instance.playerAttackSource.PlayOneShot(SoundManager.instance.stampAttack1);
            }

            if (ani.GetCurrentAnimatorStateInfo(0).IsName("Stamp_Attack") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                //StampCollider.SetActive(false);
                ani.SetBool(NotChange.instance.IsAttack1, false);
                attack1 = false;
                stampStart = false;
            }
        }
    }

    private float s_attack2RangeMin = 0.1f;
    private float s_attack2RangeMax = 0.5f;
    private float s_attack3RangeMin = 0.1f;
    private float s_attack3RangeMax = 0.5f;

    public void SpearAttack(Animator ani)
    {
        if (!thePlayerCtrl.hit && !thePlayerCtrl.jump && !ani.GetCurrentAnimatorStateInfo(0).IsName("Spear_Hit"))
        {
            if ((Input.GetKeyDown(KeyCode.Z)) && !attack1 && !attack2 && !thePlayerCtrl.hit)
            {
                
                attack1 = true;
                playerRid.velocity = Vector3.zero;
                spearStart = true;
                ani.SetBool(NotChange.instance.IsAttack1, true);
                ani.SetBool(NotChange.instance.IsWalk, false);
                ani.SetBool(NotChange.instance.IsRun, false);
                ani.SetBool(NotChange.instance.IsJump, false);
                thePlayerCtrl.movSpeed = thePlayerCtrl.walkSpeed;
                thePlayerCtrl.move = false;
            }

            if (ani.GetCurrentAnimatorStateInfo(0).IsName("Spear_Attack1") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && attack1)
            {
                SoundManager.instance.playerAttackSource.Stop();
                time += Time.deltaTime;
                ///SpearCollider.SetActive(false);
            }

            if(time < s_attack2RangeMax && time > s_attack2RangeMin && attack1 && !attack2)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    attack2 = true;
                    ani.SetBool(NotChange.instance.IsAttack2, true);
                    ani.SetBool(NotChange.instance.IsAttack1, false);
                    time = 0;
                }
                   
            }
            else if (time > s_attack2RangeMax && attack1)
            {
                spearStart = false;
                time = 0;
                attack1 = false;
                ani.SetBool(NotChange.instance.IsAttack1, false);
                thePlayerCtrl.move = true;
            }

            if (ani.GetCurrentAnimatorStateInfo(0).IsName("Spear_Attack2") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && attack2)
            {
                SoundManager.instance.playerAttackSource.Stop();
                time += Time.deltaTime;
                //SpearCollider.SetActive(false);
            }

            if (time < s_attack3RangeMax && time > s_attack3RangeMin && attack2)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    ani.SetBool(NotChange.instance.IsAttack3, true);
                    ani.SetBool(NotChange.instance.IsAttack2, false);
                    time = 0;
                }

            }
            else if (time > s_attack3RangeMax && attack2)
            {
                spearStart = false;
                time = 0;
                attack1 = false;
                attack2 = false;
                ani.SetBool(NotChange.instance.IsAttack1, false);
                ani.SetBool(NotChange.instance.IsAttack2, false);
                thePlayerCtrl.move = true;
            }

            if (ani.GetCurrentAnimatorStateInfo(0).IsName("Spear_Attack3") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                SoundManager.instance.playerAttackSource.Stop();
                //SpearCollider.SetActive(false);
                spearStart = false;
                attack1 = false;
                attack2 = false;
                ani.SetBool(NotChange.instance.IsAttack1, false);
                ani.SetBool(NotChange.instance.IsAttack2, false);
                ani.SetBool(NotChange.instance.IsAttack3, false);
                thePlayerCtrl.move = true;
            }
        }
    } 

    public bool PlayerRight()
    {
        return isRight;
    }   

    public IEnumerator SpadeSkillQuit(Animator ani) //스페이트 스킬 애니메이션 끝나면
    {
        yield return new WaitForSeconds(1.0f);
        thePlayerCtrl.move = true;
        attack1 = false;
    }

    public void SpadeSkillAppear() //스페이드 스킬 ui에 관련됨
    {
        spadeSkillCount--;

        
    }
    public void HeartSkillAppear() //하트 스킬 ui에 관련됨
    {
        heartSkillCount--;

        
    }
    public void CloverSkillAppear() //클로버 스킬 ui에 관련됨
    {
        cloverSkillCount--;

        
    }

}
