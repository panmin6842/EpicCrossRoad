using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SpaceKing : MonoBehaviour
{
    private bool skill1Start;
    private bool skilling;
    private bool skill2ing;
    private bool right;
    public bool skill2Rush;
    private bool skill1PlusHit;
    private bool skill2down;

    public bool patternAppear;
    public bool skill1End; //내려찍기 끝
    public bool skill2End; //베어올리기 끝
    public bool skill3End; //참격 끝

    public bool sound; //사운드 여부

    float movSpeed;

    Rigidbody2D rb;
    SpriteRenderer render;
    Animator ani;

    [SerializeField] Transform jumpZone; //보스가 위로 올라가는 장소
    [SerializeField] GameObject player;
    [SerializeField] GameObject skill1HitZone;
    [SerializeField] GameObject spaceKingJumpZone;

    [SerializeField] GameObject skill1;
    [SerializeField] Transform skill1AppearZone;
    [SerializeField] GameObject skill1Fire;

    [SerializeField] GameObject skill2;
    private Transform skill2AppearZone;
    [SerializeField] GameObject skill2Collider;

    [SerializeField] GameObject skill2PatternAppear1;
    [SerializeField] GameObject skill2PatternAppear2;

    [SerializeField] GameObject skill3PatternAppear;
    [SerializeField] ThreePhasePatternAppear theThreePhasePatternAppear;
    [SerializeField] BossSKill3PlayerCollider theBossSKill3PlayerCollider;
    [SerializeField] GameObject skill3Collider;

    [SerializeField] GameObject rushEffect;
    [SerializeField] GameObject attack2RushEffect;
    [SerializeField] GameObject attack1SwingEffect;
    [SerializeField] GameObject attack3SwingEffect;
    public GameObject dark;
    [SerializeField] GameObject attack2DownEffect;

    private GameObject newSkill1;
    private GameObject newSkill1Fire1;
    private GameObject newSkill1Fire2;

    private GameObject newSkill2;
    private GameObject newAttack2RushEffect;

    private GameObject newRushEffect;

    private PlayerManager thePlayerManager;
    private SoundManager theSoundManager;

    public float dist;
    public float time;
    public float patternTime;
    public int skill2Count;

    public float fadeTime;
    Color color;

    CinemachineVirtualCamera virtualCamera;
    CinemachineBasicMultiChannelPerlin noise;

    static public SpaceKing instance;

    public enum State //스킬 상태
    {
        skill2Idle,
        skill1Idle,
        skill1Start,
        skill1ing,
        skillExit,
        skill2Start,
        skill2ing,
        phase3Skill2,
        skill2End,
        skill3Start,
        skill3ing,
        skill3Hit,
        skill3Exit,
        skillRandom1,
        skillRandom2,
        skillRandom3,
    }

    public State state;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        skill1Start = false;
        skilling = false;

        skill2ing = false;
        patternAppear = false;

        skill1End = false;
        skill2End = false;
        skill3End = false;

        skill2Rush = false;
        skill1PlusHit = false;
        skill2down = false;

        sound = false;

        dist = 0;
        time = 0;
        skill2Count = 0;
        movSpeed = 10;
        patternTime = 0;

        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        thePlayerManager = FindObjectOfType<PlayerManager>();
        theSoundManager = FindObjectOfType<SoundManager>();
        skill2AppearZone = skill2Collider.GetComponent<Transform>();
        color = dark.GetComponent<SpriteRenderer>().color;

        virtualCamera = GameObject.Find("Camera").GetComponent<CinemachineVirtualCamera>();
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        SoundManager.instance.bgmAudioSource.clip = SoundManager.instance.bossStageClip;
        SoundManager.instance.bgmAudioSource.Play();

        state = State.skill2Idle;
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(new Vector3(transform.position.x, 0, 0), new Vector3(player.transform.position.x, 0, 0));
        SkillState();
        
        if (patternAppear)
        {
            patternTime += Time.deltaTime;
            if (patternTime > 5) //문양사라짐
            {
                thePlayerManager.PartternDestroy();
                patternAppear = false;
            }
        }
        else if (!patternAppear)
        {
            patternTime = 0;
        }
    }

    void SkillState()
    {
        switch (state)
        {
            case State.skillRandom1: //스킬 랜덤 부여 베어올리기랑 내려찍기
                {
                    int random = Random.Range(0, 100);

                    //Debug.Log(random);

                    if (random >= 0 && random < 55)
                    {
                        state = State.skill2Idle;
                    }
                    else if (random >= 55 && random < 100)
                    {
                        state = State.skill1Idle;
                    }
                }
                
                break;
            case State.skillRandom2: //스킬 랜덤 부여 베어올리기랑 참격
                {
                    int random = Random.Range(0, 100);

                    if (random >= 0 && random < 40)
                    {
                        state = State.skill2Idle;
                    }
                    else if (random >= 40 && random < 100)
                    {
                        state = State.skill3Start;
                    }
                }
                break;
            case State.skillRandom3: //스킬 랜덤 부여 내려찍기랑 참격
                {
                    int random = Random.Range(0, 100);

                    if (random >= 0 && random < 55)
                    {
                        state = State.skill1Idle;
                    }
                    else if (random >= 55 && random < 100)
                    {
                        state = State.skill3Start;
                    }
                }
                break;
            case State.skill2Idle: //베기 대기
                {
                    skill2PatternAppear1.SetActive(false);
                    skill2PatternAppear2.SetActive(false);
                    skill2Collider.SetActive(false);
                    skill2Count = 0;
                    if (dist < 10)
                    {
                        ani.SetBool("isWalk", false);
                        ani.SetBool("Attack1", false);
                        skill2End = false;
                        time += Time.deltaTime;
                        rb.velocity = Vector3.zero;
                        if(PhaseState.instance.state == PhaseState.State.onePhaseing)
                        {
                            if (time > 2)
                            {
                                skill2ing = true;
                                time = 0;
                                state = State.skill2Start;
                            }
                        }
                        else
                        {
                            if (time > 2)
                            {
                                skill2ing = true;
                                time = 0;
                                state = State.skill2Start;
                            }
                        }
                    }
                    else
                    {
                        skill2End = false;
                        RightLeft();
                        ani.SetBool("isWalk", true);
                        
                        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movSpeed * Time.deltaTime);
                    }
                }
                break;
            case State.skill2Start:
                {
                    time = 0;
                    RightLeft();
                    StartCoroutine(Skill2Start());
                }
                break;
            case State.phase3Skill2: //돌진
                {
                    StopAllCoroutines();
                    if (transform.position.x < player.transform.position.x && !skill2Rush) //오른쪽
                    {
                        rb.AddForce(Vector2.right * 70, ForceMode2D.Impulse);
                        newRushEffect = Instantiate(rushEffect, transform.position, Quaternion.identity);
                        newRushEffect.GetComponent<SpriteRenderer>().flipX = true;
                        theSoundManager.bossAudioSource.PlayOneShot(theSoundManager.bossDash);
                        skill2Rush = true;
                    }
                    else if (transform.position.x > player.transform.position.x && !skill2Rush) //왼쪽
                    {
                        rb.AddForce(Vector2.left * 70, ForceMode2D.Impulse);
                        newRushEffect = Instantiate(rushEffect, transform.position, Quaternion.identity);
                        newRushEffect.GetComponent<SpriteRenderer>().flipX = false;
                        theSoundManager.bossAudioSource.PlayOneShot(theSoundManager.bossDash);
                        skill2Rush = true;
                    }

                    if (time >= 0.5f)
                    {
                        time = 0;
                        StopAllCoroutines();
                        state = State.skill2ing;
                    }
                    else
                        time += Time.deltaTime;
                }
                break;
            case State.skill2ing: //베는거
                {
                    StopCoroutine("Skill2Start");
                    skill2Rush = false;
                    skill1HitZone.SetActive(false);
                    skill1HitZone.transform.localScale = new Vector3(0.1053198f, 0.2062719f, 0.07282302f);
                    skill1HitZone.transform.rotation = Quaternion.Euler(0, 0, -90);
                    skill1HitZone.GetComponent<SpriteRenderer>().flipX = true;
                    rb.velocity = Vector2.zero;
                    Destroy(newRushEffect);
                    if (skill2ing) //베기 스킬 발동
                    {
                        Skill2SwingEffect();
                        if (sound)
                        {
                            theSoundManager.bossAudioSource.PlayOneShot(theSoundManager.bossUpslashHit);
                            sound = false;
                        }
                        SpaceKingHit.instance.noHit = true;
                        skill2Count++;                        
                        skill2ing = false;
                    }
                    else if(!skill2ing)
                    {
                        ani.SetBool("Attack1", false);
                        if (skill2Count == 1)
                        {
                            time += Time.deltaTime;
                            if (time > 1 && time < 2)
                            {
                                skill2ing = true;
                            }
                        }
                        else if(skill2Count == 2)
                        {
                            time = 0;
                            patternAppear = true;
                            skill2PatternAppear1.SetActive(true);
                            skill2PatternAppear2.SetActive(true);
                            skill2End = true;
                            SpaceKingHit.instance.noHit = false;
                            if (PhaseState.instance.state == PhaseState.State.onePhaseing)
                            {
                                ani.SetBool("Attack1Ready", false);
                                ani.SetBool("Attack1", false);
                                state = State.skill2Idle;
                            }
                            if (PhaseState.instance.state == PhaseState.State.twoPhaseing || PhaseState.instance.state == PhaseState.State.threePhaseing)
                                state = State.skill2End;
                        }
                        else if(skill2Count == 0)
                        {
                            skill2End = true;
                            SpaceKingHit.instance.noHit = false;
                            if (PhaseState.instance.state == PhaseState.State.onePhaseing)
                            {
                                ani.SetBool("Attack1Ready", false);
                                ani.SetBool("Attack1", false);
                                state = State.skill2Idle;
                            }
                            if (PhaseState.instance.state == PhaseState.State.twoPhaseing || PhaseState.instance.state == PhaseState.State.threePhaseing)
                                state = State.skill2End;
                        }
                    }
                }
                break;
            case State.skill2End:
                {
                    ani.SetBool("Attack1Ready", false);
                    ani.SetBool("Attack1", false);
                    skill2PatternAppear1.SetActive(false);
                    skill2PatternAppear2.SetActive(false);
                    skill2Collider.SetActive(false);
                    skill2Count = 0;
                    skill2Rush = false;
                    sound = false;
                    if (time > 2)
                    {
                        time = 0;
                        if (dist < 10)
                        {
                            ani.SetBool("isWalk", false);
                            rb.velocity = Vector3.zero;
                        }
                        else
                        {
                            RightLeft();
                            ani.SetBool("isWalk", true);
                            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movSpeed * Time.deltaTime);
                        }
                    }
                    else
                        time += Time.deltaTime;
                }
                break;
            case State.skill1Idle: //내려찍기 시작
                {
                    ani.SetBool("isWalk", false);
                    skill2PatternAppear1.SetActive(false);
                    skill2PatternAppear2.SetActive(false);
                    skill1Start = true;
                    skilling = false;
                    skill2End = false;
                    skill1PlusHit = false;
                    spaceKingJumpZone.SetActive(true);
                    state = State.skill1Start;
                }
                break;
            case State.skill1Start:
                {
                    if (skill1Start == true)
                    {
                        //StartCoroutine(Jump());
                        if(time > 3)
                        {
                            time += Time.deltaTime;
                            ani.SetBool("Attack2Disappear", true);
                        }
                        if (time > 4) //시야 밖으로 나감
                        {
                            time = 0;
                            transform.position = jumpZone.position;
                            skill1Start = false;
                            state = State.skill1ing;
                        }
                        else
                        {
                            ani.SetBool("Attack2Ready", true);
                            if (!sound)
                            {
                                theSoundManager.bossAudioSource.PlayOneShot(theSoundManager.bossJumpAttackReady);
                                sound = true;
                            }
                            time += Time.deltaTime;
                        }
                    }
                }
                break;

            case State.skill1ing: //위로 올라갔다가 찍는 스킬
                {
                    ani.SetBool("Attack2Disappear", false);
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    if (skilling == false && !skill1End)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, 
                            new Vector2(player.transform.position.x, transform.position.y), Time.deltaTime * 20);
                        //transform.position = new Vector2(player.transform.position.x, transform.position.y);
                        //StartCoroutine(Skill1ing());
                        if (time >= 0.5f)
                        {
                            if (skilling == false)
                            {
                                time = 0;
                                //skill1HitZone.transform.localScale = new Vector3(110.8978f, 26.54624f, 9.3969f);
                                StartCoroutine(Skill1Zone());
                            }
                        }
                        else
                        {
                            skill1HitZone.SetActive(true);
                            skill1HitZone.transform.position = new Vector2(player.transform.position.x, player.transform.position.y - 2);
                            time += Time.deltaTime;
                        }
                    }
                    if (skilling == true)
                    {
                        skill1HitZone.SetActive(false);
                        spaceKingJumpZone.SetActive(false);
                        if (!skill2down)
                        {
                            newAttack2RushEffect = Instantiate(attack2RushEffect, transform.position, Quaternion.Euler(0, 0, 90));
                            skill2down = true;
                        }
                        ani.SetBool("Attack2", true);
                        SpaceKingHit.instance.noHit = true;
                        //Invoke("Skil1End", 2.0f);
                        if (time >= 5)
                        {
                            time = 0;
                            state = State.skillExit;
                        }
                        else
                            time += Time.deltaTime;
                    }
                }
                break;

            case State.skillExit:
                {
                    skill2down = false;
                    sound = false;
                    SpaceKingHit.instance.noHit = false;
                    if (!skill1PlusHit && PhaseState.instance.state == PhaseState.State.threePhaseing)
                    {
                        newSkill1Fire1 = Instantiate(attack2DownEffect, skill1AppearZone.position, Quaternion.identity);
                        skill1PlusHit = true;
                    }
                    patternAppear = true;
                    skill1End = true;
                    skilling = false;
                    skill1HitZone.SetActive(false);
                    skill2PatternAppear1.SetActive(true);
                    skill2PatternAppear2.SetActive(true);
                }
                break;
            case State.skill3Start:
                {
                    skill2PatternAppear1.SetActive(false);
                    skill2PatternAppear2.SetActive(false);
                    ani.SetBool("isWalk", false);
                    dark.SetActive(true);
                    DarkFadeIn();

                    if (time > 3) //스킬 대기
                    {
                        time = 0;
                        skill3PatternAppear.SetActive(true);
                        fadeTime = 0;
                        state = State.skill3ing;
                    }
                    else
                    {
                        ani.SetBool("Attack3Ready", true);
                        if (!sound)
                        {
                            theSoundManager.bossAudioSource.PlayOneShot(theSoundManager.bossHeavySlashReady);
                            sound = true;
                        }
                        time += Time.deltaTime;
                    }
                }
                break;
            case State.skill3ing:
                {
                    theThreePhasePatternAppear.enabled = true;
                    if (time > 2)
                    {
                        time = 0;
                        skill3PatternAppear.SetActive(false);
                        SpaceKingHit.instance.noHit = false;
                        state = State.skill3Hit;
                    }
                    else
                    {
                        SpaceKingHit.instance.noHit = true;
                        time += Time.deltaTime;
                    }
                }
                break;
            case State.skill3Hit: //문양 제대로 먹었는지 확인
                {
                    if(theThreePhasePatternAppear.diamondAppear)
                    {
                        if(theBossSKill3PlayerCollider.diamondGet)
                        {
                            Skill3Success();
                        }
                        else
                        {
                            Skill3Attack();
                        }
                    }
                    else if(theThreePhasePatternAppear.cloverAppear)
                    {
                        if (theBossSKill3PlayerCollider.cloverGet)
                        {
                            Skill3Success();
                        }
                        else
                        {
                            Skill3Attack();
                        }
                    }
                    else if(theThreePhasePatternAppear.spadeAppear)
                    {
                        if (theBossSKill3PlayerCollider.spadeGet)
                        {
                            Skill3Success();
                        }
                        else
                        {
                            Skill3Attack();
                        }
                    }
                    else if(theThreePhasePatternAppear.heartAppear)
                    {
                        if (theBossSKill3PlayerCollider.heartGet)
                        {
                            Skill3Success();
                        }
                        else
                        {
                            Skill3Attack();
                        }
                    }
                }
                break;
            case State.skill3Exit:
                {
                    sound = false;
                    ani.SetBool("Attack3Ready", false);
                    ani.SetBool("Attack3", false);
                    skill3End = true;
                    skill3Collider.SetActive(false);
                    skill2PatternAppear1.SetActive(true);
                    skill2PatternAppear2.SetActive(true);
                    patternAppear = true;
                    time = 0;
                    Skill3ObjectFalse();
                }
                break;
        }
    }

    void RightLeft()
    {
        if (transform.position.x < player.transform.position.x) //오른쪽
        {
            right = true;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (transform.position.x > player.transform.position.x) //왼쪽
        {
            right = false;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    IEnumerator Skill2Start() //공격 표시 범위
    {
        skill1HitZone.transform.localScale = new Vector3(0.9573385f, 0.3866321f, 0.1393209f);
        skill1HitZone.transform.rotation = Quaternion.Euler(0, 0, 0);
        skill1HitZone.SetActive(true);
        ani.SetBool("Attack1Ready", true);
        if (!sound)
        {
            theSoundManager.bossAudioSource.PlayOneShot(theSoundManager.bossUpslashReady);
            sound = true;
        }
        //theSoundManager.bossAudioSource.clip = theSoundManager.bossUpslashReady;
        //theSoundManager.bossAudioSource.Play();
        if (right)
        {
            skill1HitZone.transform.position = new Vector2(transform.position.x + 25, transform.position.y);
            skill1HitZone.GetComponent<SpriteRenderer>().flipX = false;
            
        }
        else if (!right)
        {
            skill1HitZone.transform.position = new Vector2(transform.position.x - 25, transform.position.y);
            skill1HitZone.GetComponent<SpriteRenderer>().flipX = true;
        }
        //skill1HitZone.transform.localScale = new Vector3(0.246877f, 0.3049657f, 0.493754f);
        if (PhaseState.instance.state == PhaseState.State.onePhaseing)
        {
            yield return new WaitForSeconds(3.0f);
            state = State.skill2ing;
        }
        else if(PhaseState.instance.state == PhaseState.State.twoPhaseing)
        {
            yield return new WaitForSeconds(2.0f);
            state = State.skill2ing;
        }
        else if(PhaseState.instance.state == PhaseState.State.threePhaseing)
        {
            yield return new WaitForSeconds(2.0f);
            state = State.phase3Skill2;
        }
    }

    public void Skill2SwingEffect()
    {
        if (transform.eulerAngles == new Vector3(0, 180, 0)) //오른쪽
        {
            ani.SetBool("Attack1", true);
            GameObject newSwingEffect = Instantiate(attack1SwingEffect,
                new Vector3(transform.position.x - 5, transform.position.y), Quaternion.identity);
            newSwingEffect.transform.localScale = new Vector3(-1.398868f, 1.398868f, 1.398868f);
        }
        else if (transform.eulerAngles == new Vector3(0, 0, 0)) //왼쪽
        {
            ani.SetBool("Attack1", true);
            GameObject newSwingEffect = Instantiate(attack1SwingEffect,
                new Vector3(transform.position.x + 5, transform.position.y), Quaternion.identity);
            newSwingEffect.transform.localScale = new Vector3(1.398868f, 1.398868f, 1.398868f);
        }
    }

    public void Skill2FirstAttackEnd()
    {
        skill2Collider.SetActive(false);
    }

    public void Skill2Collider()
    {
        skill2Collider.SetActive(true);
    }

    IEnumerator Skill1ing()
    {
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(Skill1Zone());
    }

    IEnumerator Skill1Zone() 
    {
        skill1HitZone.transform.localScale = new Vector3(0.1053198f, 0.2062719f, 0.07282302f);
        skill1HitZone.transform.rotation = Quaternion.Euler(0, 0, -90);
        skill1HitZone.GetComponent<SpriteRenderer>().flipX = true;
        if (skilling == false)
        {
            skill1HitZone.SetActive(true);
            //skill1HitZone.transform.position = new Vector2(player.transform.position.x, player.transform.position.y - 2);
            skill1HitZone.transform.position = Vector2.MoveTowards(skill1HitZone.transform.position,
                new Vector2(player.transform.position.x, player.transform.position.y - 2), Time.deltaTime * 100);
            
        }
        if (PhaseState.instance.state == PhaseState.State.twoPhaseing)
        {
            yield return new WaitForSeconds(2.0f);
            skilling = true;
        }
        else if (PhaseState.instance.state == PhaseState.State.threePhaseing)
        {
            yield return new WaitForSeconds(1.0f);
            skilling = true;
        }

    }

    void Skil1End()
    {
        state = State.skillExit;
        
    }

    void Skill3Attack()
    {
        if (time > 1 && time < 2)
        {
            ani.SetBool("Attack3", true);
            skill3Collider.SetActive(true);
        }

        if (time > 3)
        {
            DarkFadeOut();
        }
        else
            time += Time.deltaTime;
        
    }

    void Skill3Success()
    {
        if (sound)
        {
            theSoundManager.bossAudioSource.PlayOneShot(theSoundManager.bossHeavySlashFail);
            sound = false;
        }
        DarkFadeOut();
    }

    void DarkFadeIn()
    {
        fadeTime += (0.01f * Time.deltaTime);
        if (color.a <= 1)
        {
            color.a += fadeTime;
            dark.GetComponent<SpriteRenderer>().color = color;
        }
    }

    void DarkFadeOut()
    {
        fadeTime += (0.01f * Time.deltaTime);
        if (color.a >= 0)
        {
            color.a -= fadeTime;
            dark.GetComponent<SpriteRenderer>().color = color;
        }
        else if(color.a <= 0)
        {
            dark.SetActive(false);
            state = State.skill3Exit;
        }
    }

    public void Skill3AttackEffectAppaer()
    {
        if (sound)
        {
            theSoundManager.bossAudioSource.PlayOneShot(theSoundManager.bossHeavySlashHit);
            sound = false;
        }
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 3));
        GameObject newSwingEffect = Instantiate(attack3SwingEffect,
                    new Vector3(pos.x, pos.y, -6), Quaternion.identity);
        noise.m_AmplitudeGain = 10f;
        noise.m_FrequencyGain = 5f;
    }

    void Skill3ObjectFalse()
    {
        theThreePhasePatternAppear.enabled = false;
        skill3PatternAppear.SetActive(false);

        theThreePhasePatternAppear.diamondAppear = false;
        theThreePhasePatternAppear.cloverAppear = false;
        theThreePhasePatternAppear.spadeAppear = false;
        theThreePhasePatternAppear.heartAppear = false;
        theThreePhasePatternAppear.diamond.SetActive(false);
        theThreePhasePatternAppear.clover.SetActive(false);
        theThreePhasePatternAppear.spade.SetActive(false);
        theThreePhasePatternAppear.heart.SetActive(false);
        theBossSKill3PlayerCollider.diamondGet = false;
        theBossSKill3PlayerCollider.cloverGet = false;
        theBossSKill3PlayerCollider.spadeGet = false;
        theBossSKill3PlayerCollider.heartGet = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "StoneGround" && skilling == true)
        {
            theSoundManager.bossAudioSource.PlayOneShot(theSoundManager.bossJumpAttackHit);
            Destroy(newAttack2RushEffect);
            noise.m_AmplitudeGain = 7f;
            noise.m_FrequencyGain = 3f;
            newSkill1Fire1 = Instantiate(attack2DownEffect, skill1AppearZone.position, Quaternion.identity);
            Invoke("CameraNoiseExit", 0.5f);

            //StartCoroutine(Skill1FireAppear());
        }
    }

    public void CameraNoiseExit()
    {
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }

    IEnumerator Skill1FireAppear()
    {
        yield return new WaitForSeconds(1.5f);
        newSkill1Fire1 = Instantiate(attack2DownEffect, skill1AppearZone.position, Quaternion.identity);
        ani.SetBool("Attack2Ready", false);
        ani.SetBool("Attack2", false);
    }
}
