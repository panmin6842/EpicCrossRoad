using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PlayerCtrl : MonoBehaviour
{
    GameObject enemy;

    [SerializeField] GameObject spadeSpecialSkill;
    [SerializeField] GameObject playerShield;
    [SerializeField] GameObject healEffect;
    [SerializeField] GameObject spadeEffect;
    [SerializeField] GameObject cloverEffect;

    public GameObject endPatternAppear;
    public GameObject bossArrow;

    Rigidbody2D rigid;
    public Animator ani;

    public PlayerManager thePlayerManager;
    SkillPlay theSkillPlay;
    public SlotManager theSlotManager;
    PlayerShield thePlayerShield;
    SpaceKing theSpaceKing;
    SpaceKingHit theSpaceKingHit;

    public float movSpeed; //움직임 속도
    public float walkSpeed;
    public float runSpeed;
    float jumpPower; //점프 속도
    float pMove; //축 저장
    float getTime;
    float hitTime;

    int random;
    int cloverSkillCount;

    public bool move;
    public bool jump;
    public bool hit;
    public bool bosshit; //보스의 특정 스킬에 맞았을 때 움직임 멈추게 하기 위해서
    public bool shield;

    public bool walk;
    public bool run;

    bool getItem;
    public bool cloverSkillStart;

    public bool playerDie;

    GameObject[] enemys;
    EnemyCtrl[] theEnemyCtrl;

    static public PlayerCtrl instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        rigid = GetComponent<Rigidbody2D>();
        thePlayerManager = FindObjectOfType<PlayerManager>();
        theSkillPlay = FindObjectOfType<SkillPlay>();
        theSlotManager = FindObjectOfType<SlotManager>();
        ani = GetComponent<Animator>();
        enemy = GameObject.FindGameObjectWithTag("Enemy"); //적이 많을 경우에도 가능한지 나중에 확인

        theSpaceKing = FindObjectOfType<SpaceKing>();
        theSpaceKingHit = FindObjectOfType<SpaceKingHit>();
        //gameObject.transform.position = new Vector2(DataManager.instance.nowPlayer.x, DataManager.instance.nowPlayer.y);

        walkSpeed = 15f;
        runSpeed = 35f;
        jumpPower = 40f;
        movSpeed = walkSpeed;
        getTime = 0;
        hitTime = 0;

        cloverSkillCount = 0;

        move = false;
        jump = false;
        hit = false;
        walk = false;
        run = false;
        playerDie = false;
        getItem = false;
        bosshit = false;
        shield = false;
        cloverSkillStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (move && !thePlayerManager.die && !GameManager.instance.appear)
        {
            playerDie = false;
            ani.ResetTrigger("ReSet");
            Move();
            Jump();
        }

        if (thePlayerManager.hp <= 0 && !playerDie)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            thePlayerManager.hp = 0;
            ani.SetTrigger("Die");
            thePlayerManager.PlayerDie(ani, move);
            playerDie = true;
        }
        
        //if(Input.GetKeyDown(KeyCode.R))
        //{
        //    thePlayerManager.hp = 0;
        //}

        //if (Input.GetKeyDown(KeyCode.G))
        //    ani.SetTrigger(NotChange.instance.Die);
        //if (Input.GetKeyDown(KeyCode.H))
        //    ani.SetTrigger(NotChange.instance.Hit);

        if(ani.GetCurrentAnimatorStateInfo(0).IsName("Idle") || ani.GetCurrentAnimatorStateInfo(0).IsName("Stamp_Idle") 
            || ani.GetCurrentAnimatorStateInfo(0).IsName("Spear_Idle") || ani.GetCurrentAnimatorStateInfo(0).IsName("Sword_Idle"))
        {
            SoundManager.instance.moveAudioSource.Stop();
            SoundManager.instance.moveAudioSource.clip = null;
        }

        if(getTime > 2)
        {
            getItem = false;
            getTime = 0;
        }

        if(getItem)
            getTime += Time.deltaTime;

        if (hit)
        {
            if (hitTime > 2)
            {
                hitTime = 0;
                hit = false;
            }
            else
                hitTime += Time.deltaTime;
        }
        else if (!hit)
            hitTime = 0;

        AllSkill();
    }

    void AllSkill()
    {
        if (spadeSpecialSkill.activeSelf) //gameobject가 활성화 된 상태면(test)
        {
            Invoke("SpadeSpecialSkillFalse", 0.5f);
        }

        if (playerShield.activeSelf)
        {
            Invoke("DiamondSpecialShield", 20);
            Invoke("SameItemGetFalse", 0.1f);
        }

        if (theSpaceKing != null)
        {
            if (!theSpaceKing.gameObject.activeSelf)
            {
                CancelInvoke("CloverSkillEnemyHit");
            }
        }

        if (cloverSkillStart && theSpaceKing == null && (enemys[random] == null || cloverSkillCount == 5))
        {
            CancelInvoke("CloverSkillEnemyHit");
            theSlotManager.sameItemGet = false;
            theSlotManager.arrowAni.SetBool("UpExit", true);
            theSlotManager.arrowAni.SetBool("Down", false);
            theSlotManager.arrowAni.SetBool("Up", false);
            //theSlotManager.arrows[0].SetActive(true);
            //theSlotManager.arrows[1].SetActive(false);
            //theSlotManager.arrows[2].SetActive(false);
            cloverSkillStart = false;
            cloverSkillCount = 0;
        }

        if (cloverSkillStart && theSpaceKing != null && cloverSkillCount == 5)
        {
            CancelInvoke("CloverSkillBossHit");
            theSlotManager.sameItemGet = false;
            theSlotManager.arrowAni.SetBool("UpExit", true);
            theSlotManager.arrowAni.SetBool("Down", false);
            theSlotManager.arrowAni.SetBool("Up", false);
            //theSlotManager.arrows[0].SetActive(true);
            //theSlotManager.arrows[1].SetActive(false);
            //theSlotManager.arrows[2].SetActive(false);
            cloverSkillStart = false;
            cloverSkillCount = 0;
        }
    }

    void SpadeSpecialSkillFalse()
    {
        spadeSpecialSkill.SetActive(false);
        theSlotManager.arrowAni.SetBool("UpExit", true);
        theSlotManager.arrowAni.SetBool("Down", false);
        theSlotManager.arrowAni.SetBool("Up", false);
        //theSlotManager.arrows[0].SetActive(true);
        //theSlotManager.arrows[1].SetActive(false);
        //theSlotManager.arrows[2].SetActive(false);
        theSlotManager.sameItemGet = false;
    }

    void DiamondSpecialShield()
    {
        //theSlotManager.arrows[0].SetActive(true);
        //theSlotManager.arrows[1].SetActive(false);
        //theSlotManager.arrows[2].SetActive(false);
        theSlotManager.arrowAni.SetBool("UpExit", true);
        theSlotManager.arrowAni.SetBool("Down", false);
        theSlotManager.arrowAni.SetBool("Up", false);
        playerShield.GetComponent<Animator>().SetBool("exit", true);
        if (playerShield.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ShieldExit") && playerShield.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            playerShield.GetComponent<Animator>().SetBool("exit", false);
            playerShield.SetActive(false);
            theSlotManager.arrowAni.SetBool("UpExit", true);
            theSlotManager.arrowAni.SetBool("Down", false);
            theSlotManager.arrowAni.SetBool("Up", false);
        }
        shield = false;
    }

    void SameItemGetFalse()
    {
        theSlotManager.sameItemGet = false;
    }

    void SpearJump()
    {
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    void Move()
    {
        pMove = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * pMove, ForceMode2D.Impulse); //리지드바디 힘을 통해서 움직임

        if(Input.GetAxisRaw("Horizontal") > 0) //오른쪽
        {
            walk = true;
            rigid.velocity = new Vector2(movSpeed, rigid.velocity.y);
            transform.eulerAngles = new Vector3(0, 0, 0);

            ani.SetBool(NotChange.instance.IsWalk, true);
        }
        else if(Input.GetAxisRaw("Horizontal") < 0) //왼쪽
        {
            walk = true;
            rigid.velocity = new Vector2(movSpeed * (-1), rigid.velocity.y);
            transform.eulerAngles = new Vector3(0, 180, 0);

            ani.SetBool(NotChange.instance.IsWalk, true);
        }
        if(Input.GetAxisRaw("Horizontal") == 0)
            ani.SetBool(NotChange.instance.IsWalk, false);

        if (Input.GetButtonUp("Horizontal"))
        {
            walk = false;
            run = false;
            movSpeed = walkSpeed;
            SoundManager.instance.moveAudioSource.Stop();
            SoundManager.instance.moveAudioSource.clip = null;
            rigid.velocity = new Vector2(0.2f * rigid.velocity.normalized.x, rigid.velocity.y); //velocity를 통해 움직임 멈춤
            ani.SetBool(NotChange.instance.IsWalk, false);
            ani.SetBool(NotChange.instance.IsRun, false);
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetButton("Horizontal"))
        {
            run = true;
            walk = false;
            movSpeed = runSpeed;
            ani.SetBool(NotChange.instance.IsRun, true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            run = false;
            movSpeed = walkSpeed;
            ani.SetBool(NotChange.instance.IsRun, false);
        }
    }

    void Jump()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.UpArrow)) && !jump)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            ani.SetBool(NotChange.instance.IsJump, true);
            jump = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!SkillManager.instance.redSkillStart && !SkillManager.instance.spearStart && !SkillManager.instance.stampStart
            && !hit)
        {
            theSpaceKing = FindObjectOfType<SpaceKing>();
            theSpaceKingHit = FindObjectOfType<SpaceKingHit>();
            if (collision.tag == "RedItem") //스페이드 문양 먹으면
            {
                if ((!getItem && theSpaceKing == null && !theSlotManager.noItemGet) || (!getItem && SpaceKing.instance.state != SpaceKing.State.skill3ing && !theSlotManager.noItemGet)
                    || (!getItem && SpaceKing.instance.state == SpaceKing.State.skill3ing && Input.GetKey(KeyCode.X)))
                {
                    SoundManager.instance.playerAttackSource.PlayOneShot(SoundManager.instance.itemGet);
                    getItem = true;
                    if (theSpaceKing != null)
                    {
                        if (SpaceKing.instance.state != SpaceKing.State.skill3ing && SpaceKing.instance.state != SpaceKing.State.skill3Hit)
                            Destroy(collision.gameObject);
                    }
                    else if(theSpaceKing == null)
                        Destroy(collision.gameObject);
                    PatternChange("Sword");
                    SkillPlay.instance.state = SkillPlay.State.Sword;
                    thePlayerManager.spadeBar.fillAmount = 1f;
                    thePlayerManager.diamondBar.fillAmount = 0;
                    ProFileImage(thePlayerManager.spadeProfile, thePlayerManager.heartProfile, thePlayerManager.diamondProfile, thePlayerManager.cloverProfile);
                    //SkillManager.instance.spadeSkillCount = 3;

                    ColliderFalse(); //공격 콜라이더 다 끔
                    theSlotManager.SlotChange(theSlotManager.patternSprite[0], ani, thePlayerManager.spadeProfile, thePlayerManager.normalProfile);

                    if (theSlotManager.sameItemGet)
                    {
                        spadeSpecialSkill.SetActive(true);
                        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2));
                        Instantiate(spadeEffect, new Vector3(pos.x, pos.y, -6), Quaternion.identity);
                        SoundManager.instance.playerSkill.PlayOneShot(SoundManager.instance.spadeSkill);
                    }
                }
            }
            if (collision.tag == "HeartItem") //하트 문양 먹으면
            {
                if ((!getItem && theSpaceKing == null && !theSlotManager.noItemGet) || (!getItem && SpaceKing.instance.state != SpaceKing.State.skill3ing && !theSlotManager.noItemGet)
                    || (!getItem && SpaceKing.instance.state == SpaceKing.State.skill3ing && Input.GetKey(KeyCode.X)))
                {
                    SoundManager.instance.playerAttackSource.PlayOneShot(SoundManager.instance.itemGet);
                    getItem = true;
                    if (theSpaceKing != null)
                    {
                        if (SpaceKing.instance.state != SpaceKing.State.skill3ing && SpaceKing.instance.state != SpaceKing.State.skill3Hit)
                            Destroy(collision.gameObject);
                    }
                    else if(theSpaceKing == null)
                        Destroy(collision.gameObject);
                    PatternChange("Stamp");
                    SkillPlay.instance.state = SkillPlay.State.Stamp;
                    thePlayerManager.heartBar.fillAmount = 1f;
                    thePlayerManager.diamondBar.fillAmount = 0;
                    ProFileImage(thePlayerManager.heartProfile, thePlayerManager.spadeProfile, thePlayerManager.diamondProfile, thePlayerManager.cloverProfile);
                    //SkillManager.instance.heartSkillCount = 3;

                    ColliderFalse(); //공격 콜라이더 다 끔
                    theSlotManager.SlotChange(theSlotManager.patternSprite[1], ani, thePlayerManager.heartProfile, thePlayerManager.normalProfile);

                    if (theSlotManager.sameItemGet)
                    {
                        thePlayerManager.HPPlusPercent((thePlayerManager.hp / 100) * 30);
                        if (!theSpaceKing)
                        {
                            SoundManager.instance.playerSkill.PlayOneShot(SoundManager.instance.heartSkill);
                            Instantiate(healEffect, new Vector2(transform.position.x + 0.3f, transform.position.y), Quaternion.identity);
                        }
                        if (theSpaceKing)
                        {
                            SoundManager.instance.playerSkill.PlayOneShot(SoundManager.instance.heartSkill);
                            Instantiate(healEffect, new Vector2(transform.position.x, transform.position.y - 1), Quaternion.identity);
                        }
                    }
                }
            }
            if (collision.tag == "CloverItem") //클로버 문양 먹으면
            {
                if ((!getItem && theSpaceKing == null && !theSlotManager.noItemGet) || (!getItem && SpaceKing.instance.state != SpaceKing.State.skill3ing && !theSlotManager.noItemGet)
                    || (!getItem && SpaceKing.instance.state == SpaceKing.State.skill3ing && Input.GetKey(KeyCode.X)))
                {
                    SoundManager.instance.playerAttackSource.PlayOneShot(SoundManager.instance.itemGet);
                    getItem = true;
                    if (theSpaceKing != null)
                    {
                        if (SpaceKing.instance.state != SpaceKing.State.skill3ing && SpaceKing.instance.state != SpaceKing.State.skill3Hit)
                            Destroy(collision.gameObject);
                    }
                    else if (theSpaceKing == null)
                        Destroy(collision.gameObject);
                    PatternChange("Spear");
                    thePlayerManager.cloverBar.fillAmount = 1f;
                    thePlayerManager.diamondBar.fillAmount = 0;
                    //SkillManager.instance.cloverSkillCount = 3;
                    SkillPlay.instance.state = SkillPlay.State.Spear;
                    ProFileImage(thePlayerManager.cloverProfile, thePlayerManager.spadeProfile, thePlayerManager.diamondProfile, thePlayerManager.heartProfile);

                    ColliderFalse(); //공격 콜라이더 다 끔
                    theSlotManager.SlotChange(theSlotManager.patternSprite[2], ani, thePlayerManager.cloverProfile, thePlayerManager.normalProfile);

                    if (theSlotManager.sameItemGet && theSpaceKing == null)
                    {
                        cloverSkillStart = true;
                        if (GameState.instance.state != GameState.State.villageBattle1Dia)
                            enemys = GameObject.FindGameObjectsWithTag("Enemy");
                        if(GameState.instance.state == GameState.State.villageBattle1Dia)
                            enemys = GameObject.FindGameObjectsWithTag("NoDestroyEnemy");
                        random = Random.Range(0, enemys.Length);
                        InvokeRepeating("CloverSkillEnemyHit", 1, 0.5f);
                        Invoke("SameItemGetFalse", 0.1f);
                    }
                    else if (theSpaceKing != null && theSlotManager.sameItemGet)
                    {
                        cloverSkillStart = true;
                        InvokeRepeating("CloverSkillBossHit", 1, 0.5f);
                        Invoke("SameItemGetFalse", 0.1f);
                    }
                }
            }
            if (collision.tag == "DiamondItem") //다이아 문양 먹으면
            {
                if ((!getItem && theSpaceKing == null && !theSlotManager.noItemGet) || (!getItem && SpaceKing.instance.state != SpaceKing.State.skill3ing && !theSlotManager.noItemGet)
                    || (!getItem && SpaceKing.instance.state == SpaceKing.State.skill3ing && Input.GetKey(KeyCode.X)))
                {
                    SoundManager.instance.playerAttackSource.PlayOneShot(SoundManager.instance.itemGet);
                    getItem = true;
                    if (theSpaceKing != null)
                    {
                        if (SpaceKing.instance.state != SpaceKing.State.skill3ing && SpaceKing.instance.state != SpaceKing.State.skill3Hit)
                            Destroy(collision.gameObject);
                    }
                    else if (theSpaceKing == null)
                        Destroy(collision.gameObject);
                    PatternChange("Gauntlet");
                    thePlayerManager.diamondBar.fillAmount = 1f;
                    //SkillManager.instance.cloverSkillCount = 3;
                    SkillPlay.instance.state = SkillPlay.State.Gauntlet;
                    ProFileImage(thePlayerManager.diamondProfile, thePlayerManager.spadeProfile, thePlayerManager.cloverProfile, thePlayerManager.heartProfile);

                    ColliderFalse(); //공격 콜라이더 다 끔
                    theSlotManager.SlotChange(theSlotManager.patternSprite[3], ani, thePlayerManager.diamondProfile, thePlayerManager.normalProfile);

                    if (theSlotManager.sameItemGet)
                    {
                        shield = true;
                        playerShield.SetActive(true);
                        playerShield.GetComponent<Animator>().SetBool("exit", false);
                        playerShield.GetComponent<Animator>().Play("ShieldStart");
                        SoundManager.instance.playerSkill.PlayOneShot(SoundManager.instance.diamondSkill);
                        thePlayerShield = FindObjectOfType<PlayerShield>();
                    }
                }
            }
        }
    }

    public void CloverSkillEnemyHit()
    {
        if (enemys[random] != null)
        {
            if (enemys[random].name == "CloverEnemy(Clone)" || enemys[random].name == "CloverEnemy")
            {
                float skillRandom = Random.Range(0, -360);
                enemys[random].GetComponent<EnemyCtrl>().hp -= 15;
                enemys[random].GetComponent<EnemyCtrl>().hit = true;
                enemys[random].GetComponent<CloverEnemy>().hpBarImage.fillAmount =
                    enemys[random].GetComponent<EnemyCtrl>().hp / enemys[random].GetComponent<EnemyCtrl>().maxHp;
                enemys[random].GetComponent<CloverEnemy>().ani.SetTrigger("Hit");
                Instantiate(cloverEffect, enemys[random].transform.position, Quaternion.Euler(0, 0, skillRandom));
                //SoundManager.instance.cloverEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyHit);
                SoundManager.instance.playerSkill.PlayOneShot(SoundManager.instance.cloverSkill);
            }
            else if (enemys[random].name == "SpadeEnemy(Clone)" || enemys[random].name == "SpadeEnemy")
            {
                float skillRandom = Random.Range(0, -360);
                enemys[random].GetComponent<EnemyCtrl>().hp -= 15;
                enemys[random].GetComponent<EnemyCtrl>().hit = true;
                enemys[random].GetComponent<RedEnemy>().hpBarImage.fillAmount =
                    enemys[random].GetComponent<EnemyCtrl>().hp / enemys[random].GetComponent<EnemyCtrl>().maxHp;
                enemys[random].GetComponent<RedEnemy>().ani.SetTrigger("Hit");
                Instantiate(cloverEffect, enemys[random].transform.position, Quaternion.Euler(0, 0, skillRandom));
                //SoundManager.instance.cloverEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyHit);
                SoundManager.instance.playerSkill.PlayOneShot(SoundManager.instance.cloverSkill);
            }
            else if (enemys[random].name == "HealEnemy(Clone)" || enemys[random].name == "HealEnemy")
            {
                float skillRandom = Random.Range(0, -360);
                enemys[random].GetComponent<EnemyCtrl>().hp -= 15;
                enemys[random].GetComponent<EnemyCtrl>().hit = true;
                enemys[random].GetComponent<HeartEnemy>().hpBarImage.fillAmount =
                    enemys[random].GetComponent<EnemyCtrl>().hp / enemys[random].GetComponent<EnemyCtrl>().maxHp;
                enemys[random].GetComponent<HeartEnemy>().ani.SetTrigger("Hit");
                Instantiate(cloverEffect, enemys[random].transform.position, Quaternion.Euler(0, 0, skillRandom));
                //SoundManager.instance.cloverEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyHit);
                SoundManager.instance.playerSkill.PlayOneShot(SoundManager.instance.cloverSkill);
            }
            else if (enemys[random].name == "DiamondEnemy(Clone)" || enemys[random].name == "DiamondEnemy")
            {
                float skillRandom = Random.Range(0, -360);
                enemys[random].GetComponent<EnemyCtrl>().hp -= 15;
                enemys[random].GetComponent<EnemyCtrl>().hit = true;
                enemys[random].GetComponent<DiamondEnemy>().hpBarImage.fillAmount =
                    enemys[random].GetComponent<EnemyCtrl>().hp / enemys[random].GetComponent<EnemyCtrl>().maxHp;
                enemys[random].GetComponent<DiamondEnemy>().ani.SetTrigger("Hit");
                Instantiate(cloverEffect, enemys[random].transform.position, Quaternion.Euler(0, 0, skillRandom));
                //SoundManager.instance.cloverEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyHit);
                SoundManager.instance.playerSkill.PlayOneShot(SoundManager.instance.cloverSkill);
            }
        }
        cloverSkillCount++;
    }

    public void CloverSkillBossHit()
    {
        if (theSpaceKing != null)
        {
            float random = Random.Range(0, -360);
            Instantiate(cloverEffect, theSpaceKingHit.gameObject.transform.position, Quaternion.Euler(0, 0, random));
            SoundManager.instance.playerSkill.PlayOneShot(SoundManager.instance.cloverSkill);
            theSpaceKingHit.hp -= 15;
            theSpaceKingHit.hpBar.fillAmount = theSpaceKingHit.hp / theSpaceKingHit.maxHp;
        }
        cloverSkillCount++;
    }

    public void ProFileImage(GameObject file1, GameObject file2, GameObject file3, GameObject file4) //필요한 프로필만 나오도록
    {
        file1.SetActive(true);
        file2.SetActive(false);
        file3.SetActive(false);
        file4.SetActive(false);
        thePlayerManager.normalProfile.SetActive(false);
    }

    void PatternChange(string patternState)
    {
        ItemGet.instance.isGet = false;
        GameState.instance.AniFalse();
        move = true;
        ani.SetTrigger(patternState);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //씬 이동 테스트
        if(collision.name == "TestPotal")
        {
            GameState.instance.state = GameState.State.bossStart;
            SoundManager.instance.bgmAudioSource.clip = SoundManager.instance.bossStageClip;
            SoundManager.instance.bgmAudioSource.Play();
            SceneManager.LoadScene(2);
            DialogueManager.instance.OnDialogue(DialogueText.instance.bossSentence1, DialogueText.instance.bossName1, DialogueText.instance.bossImage1);
        }

        if(collision.name == "EndSpade" && DialogueManager.instance.quit)
        {
            GameState.instance.state = GameState.State.Quit;
            SceneManager.LoadScene("EndingScene");
        }

        //if (collision.name == "DiamondAttack") //적 공격 범위에 닿으면
        //{
        //    PatternStateHit(20, 10, 5, 10);
        //}
        if (collision.tag == "SpadeAttack") //적 공격 범위에 닿으면
        {
            PatternStateHit(10, 5, 10, 20);
        }
        if (collision.tag == "CloverArrow") //적 공격 범위에 닿으면
        {
            PatternStateHit(10, 20, 10, 5);
        }

        if (collision.tag == "SpaceKingSkill1") //보스 내려찍기 치명타 피해
        {
            rigid.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
            if (PhaseState.instance.state == PhaseState.State.twoPhaseing)
                BossPatternHp(15);
                //thePlayerManager.HitHP(20, ani);
            else if(PhaseState.instance.state == PhaseState.State.threePhaseing)
                BossPatternHp(40);
            //thePlayerManager.HitHP(40, ani);
            //hit = true;
        }
        if(collision.tag == "SpaceKingSkillFire") //보스 내려찍기 일반 피해
        {
            //bosshit = true;
            if (PhaseState.instance.state == PhaseState.State.twoPhaseing)
                BossPatternHp(10);
            //thePlayerManager.HitHP(10, ani);
            else if (PhaseState.instance.state == PhaseState.State.threePhaseing)
                BossPatternHp(20);
            //thePlayerManager.HitHP(20, ani);
            //hit = true;
            //StartCoroutine(MoveTrue(3.0f));
        }

        if(collision.tag == "SpaceKingSkill2") //보스의 베어올리기에 당했을 경우
        {
            rigid.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
            if(SpaceKing.instance.skill2Count == 2)
            {
                bosshit = true;
                StartCoroutine(MoveTrue(2.0f));
                if(PhaseState.instance.state == PhaseState.State.onePhaseing)
                    BossPatternHp(15);
                //thePlayerManager.HitHP(15, ani);
                else if (PhaseState.instance.state == PhaseState.State.twoPhaseing)
                    BossPatternHp(15);
                //thePlayerManager.HitHP(15, ani);
                else if (PhaseState.instance.state == PhaseState.State.threePhaseing)
                    BossPatternHp(25);
                //thePlayerManager.HitHP(25, ani);
                //hit = true;
            }
            if(SpaceKing.instance.skill2Count == 1)
            {
                if (PhaseState.instance.state == PhaseState.State.onePhaseing)
                    BossPatternHp(10);
                //thePlayerManager.HitHP(10, ani);
                else if (PhaseState.instance.state == PhaseState.State.twoPhaseing)
                    BossPatternHp(10);
                //thePlayerManager.HitHP(10, ani);
                else if (PhaseState.instance.state == PhaseState.State.threePhaseing)
                    BossPatternHp(20);
                //thePlayerManager.HitHP(20, ani);
                //hit = true;
            }
        }

        if(collision.tag == "SpaceKingSkill3Zone")
        {
            BossPatternHp(60);
            //thePlayerManager.HitHP(60, ani);
            //hit = true;
        }

        if(collision.tag == "SpaceKing" && SpaceKing.instance.skill2Rush)
        {
            BossPatternHp(15);
            //thePlayerManager.HitHP(15, ani);
            //hit = true;
        }
    }

    void BossPatternHp(int hp)
    {
        if(!shield)
        {
            thePlayerManager.HitHP(hp, ani);
            hit = true;
        }
        else if(shield)
        {
            thePlayerShield.hp -= hp;
        }
    }

    public void PatternStateHit(float swordHit, float gauntletHit, float spearHit, float stampHit)
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        thePlayerShield = FindObjectOfType<PlayerShield>();
        if (SkillPlay.instance.state == SkillPlay.State.Sword /*&& !SkillManager.instance.redSkillStart*/ && !hit)
        {
            if (!shield)
            {
                thePlayerManager.HitMove(rigid, enemy, this.gameObject);
                thePlayerManager.HitHP(swordHit, ani);
                hit = true;
            }
            else if(shield && thePlayerShield.hp > 0)
            {
                thePlayerShield.hp -= swordHit;
            }    
        }

        if (SkillPlay.instance.state == SkillPlay.State.Gauntlet && !hit && !SkillManager.instance.gauntletNoHit)
        {
            if (!shield)
            {
                thePlayerManager.HitMove(rigid, enemy, this.gameObject);
                thePlayerManager.HitHP(gauntletHit, ani);
                hit = true;
            }
            else if (shield)
            {
                thePlayerShield.hp -= gauntletHit;
            }
        }

        if (SkillPlay.instance.state == SkillPlay.State.Spear && !hit /*&& !SkillManager.instance.spearStart*/)
        {
            if (!shield)
            {
                thePlayerManager.HitMove(rigid, enemy, this.gameObject);
                thePlayerManager.HitHP(spearHit, ani);
                hit = true;
            }
            else if (shield)
            {
                thePlayerShield.hp -= spearHit;
            }
        }
        if (SkillPlay.instance.state == SkillPlay.State.Stamp && !hit /*&& !SkillManager.instance.stampStart*/)
        {
            if (!shield)
            {
                thePlayerManager.HitMove(rigid, enemy, this.gameObject);
                thePlayerManager.HitHP(stampHit, ani);
                hit = true;
            }
            else if (shield)
            {
                thePlayerShield.hp -= stampHit;
            }
        }
    }

    IEnumerator MoveTrue(float time) //기절 후 움직일 수 있게
    {
        yield return new WaitForSeconds(time);
        bosshit = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "DirtGround" || collision.collider.tag == "StoneGround" || collision.collider.tag == "MarbleGround")
        {
            ani.SetBool(NotChange.instance.IsJump, false);
            jump = false;
        }
        if(collision.collider.tag == "DirtGround")
        {
            if (ani.GetCurrentAnimatorStateInfo(0).IsName("Gauntlet_Attack2")) //점프 공격이 두번째인 상태로 땅에 닿으면 공격 리셋
            {
                ani.SetBool(NotChange.instance.IsAttack1, false);
                ani.SetBool(NotChange.instance.IsAttack2, false);
                ani.SetBool(NotChange.instance.IsAttack3, false);
                move = true;
            }
        }
        if (SoundManager.instance.sfxStop == false) //플레이어가 직접 움직일 수 있을 때 효과음이 나가도록 함
        {
            if (collision.collider.tag == "DirtGround")
            {
                if (jump)
                {
                    if (!SoundManager.instance.jumpAudioSource.isPlaying)
                    {
                        SoundManager.instance.jumpAudioSource.PlayOneShot(SoundManager.instance.dirtJumpDownAudioClip);
                        jump = false;
                    }
                }
            }
            else if (collision.collider.tag == "StoneGround")
            {
                if (jump)
                {
                    if (!SoundManager.instance.jumpAudioSource.isPlaying)
                    {
                        SoundManager.instance.jumpAudioSource.PlayOneShot(SoundManager.instance.stoneJumpDownAudioClip);
                        jump = false;
                    }
                }
            }
            else if (collision.collider.tag == "MarbleGround")
            {
                if (jump)
                {
                    if (!SoundManager.instance.jumpAudioSource.isPlaying)
                    {
                        SoundManager.instance.jumpAudioSource.PlayOneShot(SoundManager.instance.marbleJumpDownAudioClip);
                        jump = false;
                    }
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (SoundManager.instance.sfxStop == false) //플레이어가 직접 움직일 수 있을 때 효과음이 나가도록 함
        {
            if (collision.collider.tag == "DirtGround")
            {
                WalkRunAudio(SoundManager.instance.dirtWalkAudioClip, SoundManager.instance.dirtRunAudioClip);
                if (run && !jump)
                    SoundManager.instance.moveAudioSource.pitch = 1.5f;
            }
            else if (collision.collider.tag == "StoneGround")
            {
                SoundManager.instance.moveAudioSource.pitch = 1f;
                WalkRunAudio(SoundManager.instance.stoneWalkAudioClip, SoundManager.instance.stoneRunAudioClip);
            }
            else if (collision.collider.tag == "MarbleGround")
            {
                WalkRunAudio(SoundManager.instance.marbleWalkAudioClip, SoundManager.instance.marbleRunAudioClip);
                if (run && !jump)
                    SoundManager.instance.moveAudioSource.pitch = 1.5f;
            }
        }
    }

    void ColliderFalse()
    {
        SkillManager.instance.gAttackCollider[0].SetActive(false);
        SkillManager.instance.gAttackCollider[1].SetActive(false);
        SkillManager.instance.gAttackCollider[2].SetActive(false);
        SkillManager.instance.spearCollider.SetActive(false);
        SkillManager.instance.swordCollider.SetActive(false);
    }

    void WalkRunAudio(AudioClip walkAudioClip, AudioClip runAudioClip)
    {
        if (walk && !jump)
        {
            if (!SoundManager.instance.moveAudioSource.isPlaying)
            {
                SoundManager.instance.moveAudioSource.clip = walkAudioClip;
                SoundManager.instance.moveAudioSource.pitch = 1f;
                SoundManager.instance.moveAudioSource.Play();
            }
        }
        else if (run && !jump)
        {
            if (!SoundManager.instance.moveAudioSource.isPlaying)
            {
                SoundManager.instance.moveAudioSource.clip = runAudioClip;
                SoundManager.instance.moveAudioSource.Play();
            }
        }
    }

    public void HitFalse()
    {
        move = true;
        hit = false;
        ani.SetBool(NotChange.instance.IsWalk, false);
        ani.SetBool(NotChange.instance.IsRun, false);
    }

    public void MoveSoundNull()
    {
        SoundManager.instance.moveAudioSource.Stop();
    }
}
