using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    [SerializeField] string[] cutSceneSentence;
    [SerializeField] string[] cutSceneName;
    [SerializeField] GameObject[] cutSceneImages;
    [SerializeField] GameObject[] empty;

    [SerializeField] GameObject cutScene;
    [SerializeField] GameObject tutoItem;

    public Animator playerAni;
    public GameObject player;
    GameObject boss;

    [SerializeField] GameObject TutoDia2Director; //두번째 맵에서 연출 TimeLine
    public GameObject VillageBattle1Director;
    public GameObject CastleEntDirector;
    public GameObject CastleEntDirector2;

    [SerializeField] GameObject[] toolTip; //???npc와 대화할 때 나오는 툴팁
    public GameObject[] arrows; //전투 끝나면 나오는 방향화살표

    public int dialogueCount;
    public bool skillCountMinStop; //스킬 바 줄어들지 않게 하기
    public bool enemyhpBar; //false일 때 적 hpbar 보임
    public bool dialogue; //대사 한번만 나오도록 함
    public bool enemyItemDrop;
    bool aniMove;

    public int enemyCount; //죽은 적 수
    int enemyAppearCount; //적 등장 수

    public GameObject cloverEnemy;
    public GameObject diamondEnemy;
    public GameObject healEnemy;
    public GameObject spadeEnemy;

    public GameObject newCloverEnemy;
    public GameObject newDiamondEnemy;
    public GameObject newHealEnemy;
    public GameObject newSpadeEnemy;

    [SerializeField] GameObject potal2;
    [SerializeField] GameObject potal3;
    [SerializeField] GameObject potal4;
    [SerializeField] GameObject potal5;
    [SerializeField] GameObject potal6;
    [SerializeField] GameObject potal7;

    [SerializeField] GameObject enemy1;
    [SerializeField] GameObject enemy2;
    [SerializeField] GameObject enemy3;
    [SerializeField] GameObject enemy4;
    [SerializeField] GameObject enemy5;

    public GameObject patternAppear1;
    public GameObject patternAppear2;
    public GameObject patternAppear3;
    public GameObject patternAppear4;

    [SerializeField] Transform enemyHpBarParent;

    PlayerManager thePlayerManager;
    DialogueManager theDialogueManager;
    SlotManager theSlotManager;

    public enum State
    {
        prologue,
        prologueing,
        tutorial,
        tutorial1,
        tutorial2,
        tutorial2Dia,
        tutorial2Dia2,
        tutorial2Battle,
        tutorial2End,
        villageStart,
        villageBattleStart,
        villageBattle1Dia,
        villageBattle2,
        villageBattle3Dia,
        villageBattle3,
        villageBattle4,
        villageBattle5,
        villageBattle6,
        castleEntBattle1,
        castleEntBattle2,
        castleEntBattle3,
        castleEnt,
        castleEnt2,
        castleEnt3,
        castleEnd,
        bossStart,
        bossDia,
        bossBattle,
        bossDie,
        End,
        Quit,
    }

    public State state;

    public SkillPlay theSkillPlay;
    public PlayerCtrl thePlayerCtrl;
    public SkillManager theSkillManager;
    EnemyCtrl[] theEnemyCtrl; //화면에 있는 적들 스크립트 가져옴

    static public GameState instance;

    private void Awake()
    {
        instance = this;

        state = State.prologue;
    }

    // Start is called before the first frame update
    void Start()
    {
        theSkillPlay = FindObjectOfType<SkillPlay>();
        thePlayerCtrl = FindObjectOfType<PlayerCtrl>();
        theEnemyCtrl = FindObjectsOfType<EnemyCtrl>();
        theSkillManager = FindObjectOfType<SkillManager>();
        theSlotManager = FindObjectOfType<SlotManager>();
        player = GameObject.FindWithTag("Player");
        playerAni = player.GetComponent<Animator>();

        thePlayerManager = FindObjectOfType<PlayerManager>();

        enemyCount = 0;
        dialogueCount = 0;
        enemyAppearCount = 0;
        skillCountMinStop = false;
        dialogue = false;
        aniMove = false;
        enemyhpBar = false;

        state = State.prologue;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.prologue:
                {
                    theDialogueManager = FindObjectOfType<DialogueManager>();

                    cutScene.SetActive(true);
                    thePlayerCtrl.enabled = false;
                    theSkillPlay.enabled = false;
                    theDialogueManager.OnDialogue(cutSceneSentence, cutSceneName, empty);
                    //DialogueManager.instance.skipButton.SetActive(false);
                    SoundManager.instance.bgmAudioSource.clip = SoundManager.instance.forestNormalClip;
                    SoundManager.instance.bgmAudioSource.Play();
                    state = State.prologueing;
                }
                break;
            case State.prologueing:
                {
                    if (theDialogueManager.sentences.Count == 3)
                    {
                        cutSceneImages[0].SetActive(true);
                    }
                    if (theDialogueManager.sentences.Count == 2)
                        cutSceneImages[1].SetActive(true);
                    if (theDialogueManager.sentences.Count == 1)
                        cutSceneImages[2].SetActive(true);
                    if (theDialogueManager.sentences.Count == 0)
                        cutSceneImages[3].SetActive(true);

                    if (theDialogueManager.quit == true)
                    {
                        //theDialogueManager.sentenceText.text = " ";
                        //theDialogueManager.curSentence = " ";
                        cutScene.SetActive(false);
                        
                        theDialogueManager.OnDialogue(DialogueText.instance.tutorialSentence1, DialogueText.instance.tutorialSceneName1, DialogueText.instance.tutorialImage1);
                        state = State.tutorial;
                    }
                }
                break;
            case State.tutorial:
                {

                    if (DialogueManager.instance.quit == true)
                    {
                        state = State.tutorial1;
                    }
                }
                break;
            case State.tutorial1: //첫번 째 숲 맵
                {
                    //playerAni.SetBool("isGauntlet", true);
                    tutoItem.SetActive(true);
                    PlayerMove();
                    thePlayerCtrl.enabled = true;
                    thePlayerCtrl.move = true;
                    theSkillPlay.enabled = false;
                }
                break;
            case State.tutorial2: //두 번째 숲 맵
                {
                    AniFalse();
                    thePlayerCtrl.walk = false;
                    thePlayerCtrl.run = false;
                    thePlayerCtrl.move = false;
                    theSkillPlay.enabled = false;
                    if (DialogueManager.instance.quit == true) //두번째 맵 도착하고 대화 끝났을 때
                    {
                        //TutoDia2Director.SetActive(true);
                        //Invoke("TutoDia2", 5);
                        state = State.tutorial2Dia;
                    }
                    
                }
                break;
            case State.tutorial2Dia:
                {
                    if (!skillCountMinStop)
                    {
                        PlayerMove();
                        thePlayerCtrl.enabled = true;
                        //theSkillPlay.enabled = true;
                        //TutoDia2Director.SetActive(false);
                        //theSkillPlay.enabled = false;
                        //Invoke("Tuto2DiaMove", 3);
                    }
                    else if(skillCountMinStop)
                    {
                        thePlayerCtrl.enabled = false;
                        theSkillPlay.enabled = false;
                        state = State.tutorial2Dia2;
                    }
                }
                break;
            case State.tutorial2Dia2: //기운을 먹고 난 후 대사
                {
                    AniFalse();
                    thePlayerCtrl.enabled = false;
                    theSkillPlay.enabled = false;

                    if (DialogueManager.instance.quit == true) //대사 끝
                    {
                        enemyhpBar = false;
                        playerAni.SetBool("isGauntlet", true);
                        
                        enemyCount = 0;
                        if (enemyAppearCount < 5)
                        {
                            newDiamondEnemy = Instantiate(diamondEnemy, new Vector3(109.8f, -8.1f, -0.6931853f), Quaternion.identity);
                            enemyAppearCount++;
                            newSpadeEnemy = Instantiate(spadeEnemy, new Vector3(115.7f, -8.1f, -0.6931853f), Quaternion.identity);
                            enemyAppearCount++;
                            newSpadeEnemy = Instantiate(spadeEnemy, new Vector3(170, -8.1f, -0.6931853f), Quaternion.identity);
                            enemyAppearCount++;
                            newCloverEnemy = Instantiate(cloverEnemy, new Vector3(175.5f, -8.1f, -0.6931853f), Quaternion.identity);
                            enemyAppearCount++;
                            newCloverEnemy = Instantiate(cloverEnemy, new Vector3(180.1f, -8.1f, -0.6931853f), Quaternion.identity);
                            enemyAppearCount++;
                        }
                        SoundManager.instance.bgmAudioSource.clip = SoundManager.instance.forestBattleClip;
                        SoundManager.instance.bgmAudioSource.Play();
                        state = State.tutorial2Battle;
                    }
                }
                break;
            case State.tutorial2Battle:
                {
                    if(enemyCount >= 5)
                    {
                        potal2.SetActive(true);
                        arrows[0].SetActive(true);
                        PlayerBattleEnd();
                        GauntletState();
                        DialogueManager.instance.OnDialogue(DialogueText.instance.tutorialSentence1, DialogueText.instance.tutorialSceneName1, DialogueText.instance.tutorialImage1);
                        state = State.tutorial2End;
                    }
                    else
                    {
                        EnemyStart();
                        skillCountMinStop = false;
                        PlayerMove();
                        thePlayerCtrl.enabled = true;
                        theSkillPlay.enabled = true;
                    }
                }
                break;
            case State.tutorial2End:
                {
                    EnemyHpBarDestroy();

                    if (DialogueManager.instance.quit == true) //대사 끝
                    {
                        PlayerMove();
                        thePlayerCtrl.enabled = true;
                        theSkillPlay.enabled = true;
                    }
                }
                break;
            case State.villageStart:
                {
                    enemyItemDrop = true;
                    enemy1.SetActive(true);
                    enemy2.SetActive(true);
                    enemy3.SetActive(true);
                    enemy4.SetActive(true);
                    enemy5.SetActive(true);
                    enemy1.transform.localScale = new Vector3(1.793125f, enemy1.transform.localScale.y, enemy1.transform.localScale.z);
                    enemy2.transform.localScale = new Vector3(1.793125f, enemy2.transform.localScale.y, enemy2.transform.localScale.z);
                    enemy3.transform.localScale = new Vector3(1.393847f, enemy3.transform.localScale.y, enemy3.transform.localScale.z);
                    enemy5.transform.localScale = new Vector3(1.393847f, enemy5.transform.localScale.y, enemy5.transform.localScale.z);
                    enemy4.transform.localScale = new Vector3(1.268735f, enemy4.transform.localScale.y, enemy4.transform.localScale.z);

                    enemy1.GetComponent<EnemyCtrl>().hp = enemy1.GetComponent<EnemyCtrl>().maxHp;
                    enemy2.GetComponent<EnemyCtrl>().hp = enemy2.GetComponent<EnemyCtrl>().maxHp;
                    enemy3.GetComponent<EnemyCtrl>().hp = enemy3.GetComponent<EnemyCtrl>().maxHp;
                    enemy4.GetComponent<EnemyCtrl>().hp = enemy4.GetComponent<EnemyCtrl>().maxHp;
                    enemy5.GetComponent<EnemyCtrl>().hp = enemy5.GetComponent<EnemyCtrl>().maxHp;

                    enemy1.GetComponent<CloverEnemy>().ReStart();
                    enemy2.GetComponent<CloverEnemy>().ReStart();
                    enemy3.GetComponent<RedEnemy>().ReStart();
                    enemy4.GetComponent<DiamondEnemy>().ReStart();
                    enemy5.GetComponent<RedEnemy>().ReStart();
                    if (DialogueManager.instance.quit == true) //마을에 도착하고 대사 끝
                    {
                        PlayerMove();
                        thePlayerCtrl.enabled = true;
                        theSkillPlay.enabled = true;
                        PlayerNPCDist.instance.diaStart = false;
                    }

                    if (DialogueManager.instance.sentences.Count == 15)
                    {
                        toolTip[0].SetActive(true);
                    }
                    else if (DialogueManager.instance.sentences.Count == 14)
                    {
                        toolTip[0].SetActive(false);
                    }
                    else if (DialogueManager.instance.sentences.Count == 13)
                    {
                        toolTip[1].SetActive(true);
                    }
                    else if (DialogueManager.instance.sentences.Count == 12)
                    {
                        toolTip[1].SetActive(false);
                    }
                    else if (DialogueManager.instance.sentences.Count == 8)
                    {
                        toolTip[2].SetActive(true);
                    }
                    else if (DialogueManager.instance.sentences.Count == 6)
                    {
                        toolTip[2].SetActive(false);
                    }
                    else if (DialogueManager.instance.sentences.Count == 5)
                    {
                        toolTip[3].SetActive(true);
                    }
                    else if (DialogueManager.instance.sentences.Count == 4)
                    {
                        toolTip[3].SetActive(false);
                    }
                }
                break;
            case State.villageBattleStart:
                {
                    toolTip[0].SetActive(false);
                    toolTip[1].SetActive(false);
                    toolTip[2].SetActive(false);
                    toolTip[3].SetActive(false);
                    enemyCount = 0;
                    Invoke("VillageBattle1DiaGo", (float)VillageBattle1Director.GetComponent<PlayableDirector>().duration);
                }
                break;
            case State.villageBattle1Dia:
                {
                    if (DialogueManager.instance.quit == true) //마을에 도착하고 대사 끝
                    {
                        patternAppear1.SetActive(true);
                        enemyhpBar = false;
                        if (enemyCount >= 5)
                        {
                            thePlayerManager.PartternDestroy();
                            GauntletState();
                            if (!aniMove)
                            {
                                PlayerBattleEnd();
                                aniMove = true;
                            }
                            else
                            {
                                VillageBattle2Go();
                            }
                        }
                        else
                        {
                            enemyhpBar = false;
                            EnemyStart();
                            PlayerMove();
                            thePlayerCtrl.enabled = true;
                            theSkillPlay.enabled = true;
                        }
                    }
                }
                break;
            case State.villageBattle2:
                {
                    EnemyHpBarDestroy();
                    PatternReSet();
                    thePlayerManager.PartternDestroy();
                    potal4.SetActive(true);
                    arrows[2].SetActive(true);
                    enemyCount = 0;
                    PlayerMove();
                    thePlayerCtrl.enabled = true;
                    theSkillPlay.enabled = true;
                }
                break;
            case State.villageBattle3Dia:
                {
                    
                    if (DialogueManager.instance.quit == true)
                    {
                        EnemyStart();
                        PlayerMove();
                        thePlayerCtrl.enabled = true;
                        theSkillPlay.enabled = true;

                        if(enemyCount >= 7)
                        {
                            PatternReSet();
                            patternAppear2.SetActive(false);
                            thePlayerManager.PartternDestroy();
                            state = State.villageBattle3;
                        }
                        else
                        {
                            patternAppear2.SetActive(true);
                            EnemyStart();
                            PlayerMove();
                            thePlayerCtrl.enabled = true;
                            theSkillPlay.enabled = true;
                        }
                    }
                }
                break;
            case State.villageBattle3:
                {
                    PlayerMove();
                    thePlayerCtrl.enabled = true;
                    theSkillPlay.enabled = true;
                    PatternReSet();
                    EnemyHpBarDestroy();
                    potal5.SetActive(true);
                    arrows[3].SetActive(true);
                    thePlayerManager.PartternDestroy();
                }
                break;
            case State.villageBattle4:
                {
                    EnemyStart();
                    PlayerMove();
                    thePlayerCtrl.enabled = true;
                    theSkillPlay.enabled = true;
                    patternAppear3.SetActive(true);
                    if(enemyCount >= 10)
                    {
                        PatternReSet();
                        dialogue = false;
                        thePlayerManager.PartternDestroy();
                        state = State.villageBattle5;
                    }
                }
                break;
            case State.villageBattle5:
                {
                    PatternReSet();
                    EnemyHpBarDestroy();
                    EnemyDie();
                    PlayerMove();
                    thePlayerCtrl.enabled = true;
                    theSkillPlay.enabled = true;
                    potal6.SetActive(true);
                    arrows[4].SetActive(true);
                    thePlayerManager.PartternDestroy();
                }
                break;
            case State.villageBattle6:
                {
                    enemyCount = 0;
                    Invoke("castleEntBattle1Go", (float)CastleEntDirector.GetComponent<PlayableDirector>().duration);
                }
                break;
            case State.castleEntBattle1:
                { 
                    //확인하기
                    if (DialogueManager.instance.quit)
                    {
                        CastleEntDirector.SetActive(false);
                        enemyhpBar = false;
                        EnemyStart();
                        PlayerMove();
                        thePlayerCtrl.enabled = true;
                        theSkillPlay.enabled = true;

                        if(enemyCount >= 10)
                        {
                            PatternReSet();
                            thePlayerManager.PartternDestroy();
                            potal7.SetActive(true);
                            state = State.castleEntBattle2;
                        }
                    }
                }
                break;
            case State.castleEntBattle2:
                {
                    EnemyStart();
                    PlayerMove();
                    thePlayerCtrl.enabled = true;
                    theSkillPlay.enabled = true;
                    arrows[5].SetActive(true);
                }
                break;
            case State.castleEnt:
                {
                    if (DialogueManager.instance.quit)
                    {
                        CastleEntDirector2.SetActive(true);
                        Invoke("castleBossGo", (float)CastleEntDirector2.GetComponent<PlayableDirector>().duration);
                        //Invoke("castleBossGo", 5);
                    }
                }
                break;
            case State.castleEnt2:
                {
                    CancelInvoke("castleBossGo");
                    CastleEntDirector2.SetActive(false);
                    DialogueManager.instance.OnDialogue(DialogueText.instance.castleEntSentence3, DialogueText.instance.castleEntName3, DialogueText.instance.castleEntImage3);
                    state = State.castleEnt3;
                }
                break;
            case State.castleEnt3:
                {
                    if (DialogueManager.instance.sentences.Count == 3)
                    {
                        //텍스트 사이즈 크게
                        DialogueManager.instance.sentenceText.fontSize = 60;
                        DialogueManager.instance.typingSpeed = 0.3f;
                        DialogueManager.instance.noFast = true;
                    }
                    else
                    {
                        DialogueManager.instance.sentenceText.fontSize = 42.26f;
                        DialogueManager.instance.noFast = false;
                    }

                    if (DialogueManager.instance.quit)
                    {
                        state = State.castleEnd;
                    }
                }
                break;
            case State.castleEnd:
                {
                    PlayerMove();
                    thePlayerCtrl.enabled = true;
                    theSkillPlay.enabled = true;
                }
                break;
            case State.bossStart:
                {
                    Invoke("BossStartGo", 1);
                }
                break;
            case State.bossDia:
                {
                    CancelInvoke("BossStartGo");
                    if (DialogueManager.instance.quit)
                    {
                        boss.GetComponent<SpaceKing>().enabled = true;
                        boss.GetComponent<SpaceKingHit>().enabled = true;
                        boss.GetComponent<PhaseState>().enabled = true;
                        PlayerMove();
                        thePlayerCtrl.enabled = true;
                        theSkillPlay.enabled = true;
                        state = State.bossBattle;
                    }
                }
                break;
            case State.bossBattle:
                {
                    PlayerMove();
                }
                break;
            case State.bossDie:
                {
                    DialogueManager.instance.skipButton.SetActive(false);
                    Invoke("BossEnd", 1);
                    Invoke("PlayerBattleEnd", 2);

                    if (DialogueManager.instance.sentences.Count == 5)
                    {
                        thePlayerCtrl.endPatternAppear.SetActive(true);
                    }

                    if (DialogueManager.instance.quit)
                    {
                        state = State.End;
                    }
                }
                break;
            case State.End:
                {
                    thePlayerCtrl.bossArrow.SetActive(true);
                    CancelInvoke("PlayerBattleEnd");
                    PlayerMove();
                    thePlayerCtrl.enabled = true;
                    theSkillPlay.enabled = true;
                }
                break;
            case State.Quit:
                {
                }
                break;
        }
    }
    
    void BossEnd()
    {
        boss.SetActive(false);
    }

    void EnemyHpBarDestroy() //적 hpbar 삭제
    {
        var enemyHpBars = enemyHpBarParent.GetComponentsInChildren<Image>();
        foreach (var enemyHpBar in enemyHpBars)
        {
            Destroy(enemyHpBar);
        }
    }

    void Tuto2DiaMove()
    {
        PlayerMove();
        thePlayerCtrl.enabled = true;
        theSkillPlay.enabled = true;
    }

    void BossStartGo()
    {
        player = GameObject.FindWithTag("Player");
        playerAni = player.GetComponent<Animator>();
        theSkillPlay = FindObjectOfType<SkillPlay>();
        thePlayerCtrl = FindObjectOfType<PlayerCtrl>();
        theSkillManager = FindObjectOfType<SkillManager>();
        thePlayerCtrl.thePlayerManager = FindObjectOfType<PlayerManager>();
        thePlayerCtrl.theSlotManager = FindObjectOfType<SlotManager>();
        boss = GameObject.Find("SpaceKing");
        playerAni.SetBool("isGauntlet", true);
        AniFalse();
        thePlayerCtrl.enabled = false;
        theSkillPlay.enabled = false;
        
        state = State.bossDia;
    }

    void castleBossGo()
    {
        state = State.castleEnt2;
    }

    void castleEntBattle1Go()
    {
        if (!dialogue)
        {
            patternAppear4.SetActive(true);
            DialogueManager.instance.OnDialogue(DialogueText.instance.castleEntSentence1, DialogueText.instance.castleEntName1, DialogueText.instance.castleEntImage1);
            dialogue = true;
            enemyCount = 0;
            state = State.castleEntBattle1;
        }
    }

    void VillageBattle2Go()
    {
        aniMove = false;
        //SoundManager.instance.bgmAudioSource.clip = SoundManager.instance.villageBattleClip;
        //SoundManager.instance.bgmAudioSource.Play();
        state = State.villageBattle2;
    }

    void VillageBattle1DiaGo()
    {
        VillageBattle1Director.SetActive(false);
        if(!dialogue)
            DialogueManager.instance.OnDialogue(DialogueText.instance.villageSentence2, DialogueText.instance.villageSceneName2, DialogueText.instance.villageImage2);
        dialogue = true;
        state = State.villageBattle1Dia;
    }

    void TutoDia2()
    {
        state = State.tutorial2Dia;
    }

    public void PlayerBattleEnd()
    {
        thePlayerManager.PartternDestroy();
        PatternReSet();
        AniFalse();
        EnemyDie();
        thePlayerCtrl.enabled = false;
        theSkillPlay.enabled = false;
    }

    void PatternReSet()
    {
        thePlayerManager.spadeBar.fillAmount = 0f;
        thePlayerManager.cloverBar.fillAmount = 0f;
        thePlayerManager.heartBar.fillAmount = 0f;

        theSkillManager.spadeSkillCount = 0;
        theSkillManager.heartSkillCount = 0;
        theSkillManager.cloverSkillCount = 0;

        theSlotManager.slots[0].sprite = null;
        theSlotManager.slots[1].sprite = null;
        theSlotManager.slots[0].color = new Color(0, 0, 0, 0);
        theSlotManager.slots[1].color = new Color(0, 0, 0, 0);
    }

    void GauntletState()
    {
        playerAni.SetTrigger("Gauntlet");
        theSkillPlay.state = SkillPlay.State.Gauntlet;
        theSkillPlay.diamondModeCut = true;
        thePlayerManager.spadeProfile.SetActive(false);
        thePlayerManager.diamondProfile.SetActive(false);
        thePlayerManager.heartProfile.SetActive(false);
        thePlayerManager.cloverProfile.SetActive(false);
        theSkillManager.swordCollider.SetActive(false);
        theSkillManager.spearCollider.SetActive(false);
        Invoke("GauntletTriggerReSet", 2);
    }

    void GauntletTriggerReSet()
    {
        playerAni.ResetTrigger("Gauntlet");
    }

    void EnemyStart() //플레이어와의 거리가 30이내이면 공격 시작하게 함
    {
        theEnemyCtrl = FindObjectsOfType<EnemyCtrl>();
        for (int i = 0; i < theEnemyCtrl.Length; i++)
        {
            if (theEnemyCtrl[i].dist < 70)
                theEnemyCtrl[i].start = true;

            if (theEnemyCtrl[i].dieCount == 1)
            {
                enemyCount++;
                theEnemyCtrl[i].dieCount = 0;
            }
        }
    }

    void EnemyDie() //적이 다 죽으면 공격 해제
    {
        if (theEnemyCtrl != null)
        {
            for (int i = 0; i < theEnemyCtrl.Length; i++)
            {
                theEnemyCtrl[i].start = false;
            }
        }
    }

    public void AniFalse()
    {
        SoundManager.instance.sfxStop = true;

        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        playerAni.SetBool(NotChange.instance.IsWalk, false);
        playerAni.SetBool(NotChange.instance.IsRun, false);
        playerAni.SetBool(NotChange.instance.IsJump, false);
        playerAni.SetBool(NotChange.instance.IsAttack1, false);
        playerAni.SetBool(NotChange.instance.IsAttack2, false);
        playerAni.SetBool(NotChange.instance.IsAttack3, false);

        for (int i = 0; i < 3; i++)
        {
            theSkillManager.gAttackCollider[i].SetActive(false);
        }
        theSkillManager.playerRid.gravityScale = 9f;
        theSkillManager.attack2 = false;
        theSkillManager.attack1 = false;
        theSkillManager.attack3 = false;
        theSkillManager.time = 0;
        theSkillManager.time2 = 0;
        theSkillManager.redSkillStart = false;
        theSkillManager.spearStart = false;

        thePlayerCtrl.movSpeed = thePlayerCtrl.walkSpeed;
    } //모든 움직임을 멈춤

    void PlayerMove() //플레이어가 움직일 때 못 움직일 때
    {
        SoundManager.instance.sfxStop = false;

        if (!thePlayerManager.die)
        {
            if ((!theSkillManager.redSkillStart && !thePlayerCtrl.bosshit) || (!theSkillManager.spearStart && !thePlayerCtrl.bosshit) 
                || (!theSkillManager.stampStart && !thePlayerCtrl.bosshit)
                || (!theSkillManager.attack1 && !theSkillManager.attack2 && !thePlayerCtrl.bosshit))
                thePlayerCtrl.move = true;
            if (thePlayerCtrl.hit || theSkillManager.redSkillStart || theSkillManager.spearStart || theSkillManager.stampStart || thePlayerCtrl.bosshit)
                thePlayerCtrl.move = false;
            if (theSkillManager.attack1 || theSkillManager.attack2)
                thePlayerCtrl.move = false;

            if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle") || player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Stamp_Idle") 
                || player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Spear_Idle") || player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Sword_Idle")
                || player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Walk") || player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Stamp_Walk")
                || player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Spear_Walk") || player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Sword_Walk"))
            {
                //thePlayerCtrl.move = true;
                theSkillManager.redSkillStart = false;
                theSkillManager.spearStart = false;
                theSkillManager.stampStart = false;
                //SkillManager.instance.attack1 = false;
                //SkillManager.instance.attack2 = false;
                //SkillManager.instance.attack3 = false;
            }
        }
    }
}
