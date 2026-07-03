using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiamondEnemy : EnemyCtrl
{
    //방패병

    Rigidbody2D myRid;
    public Animator ani;

    GameObject player;
    [SerializeField] GameObject diamondItem;
    public GameObject diamondAttack;

    float runSpeed;
    public float time;

    bool itemDrop; //아이템 하나만 나오게
    public bool die;
    
    public bool shield; //방패 상태
    int startCount; //처음에 감지했을 때는 바로 방패상태로 가야하기 때문에 이걸로 구분을 해줌

    public Vector3 offset;

    PlayerCtrl thePlayerCtrl;

    //hpBar를 위한 변수들
    public GameObject enemyHpBar;
    GameObject canvas;
    RectTransform hpBar;
    public Image hpBarImage;
    GameObject hpBarObject;

    float height; //hpBar위치 조정

    public enum State
    {
        shield,
        attack,
        idle,
        die,
    }

    public State state;

    // Start is called before the first frame update
    void Start()
    {
        myRid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        thePlayerCtrl = player.GetComponent<PlayerCtrl>();
        canvas = GameObject.FindGameObjectWithTag("Canvas");

        //hpBar생성
        hpBar = Instantiate(enemyHpBar, canvas.transform).GetComponent<RectTransform>();
        hpBarImage = hpBar.GetComponent<Image>();
        hpBarImage.fillAmount = hp / maxHp;

        hpBarObject = hpBarImage.gameObject;

        runSpeed = 5;
        maxHp = 100;
        hp = maxHp;
        time = 0;
        startCount = 0;
        height = -4;

        hit = false;
        red = false;
        yellow = false;
        gauntlet = false;
        spear = false;
        die = false;
        start = false;//테스트 아닐 땐 false로 바꿔야함
        itemDrop = false;

        shield = false;

        state = State.idle;

        offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //hpBar가 적을 따라가게 함
        if (!GameState.instance.enemyhpBar)
        {
            Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, transform.position.z));
            hpBar.position = _hpBarPos;
        }

        dist = Vector2.Distance(transform.position, player.transform.position); //적과 플레이어 사이의 거리

        if (die == false && start == true)
        {
            
            FlipX(gameObject.GetComponent<SpriteRenderer>(), player, gameObject);

            Attack();

            NnockBack(gauntlet, 1f);
            NnockBack(red, 1f);
            NnockBack(yellow, 0.5f);
            NnockBack(spear, 1f);
        }

        if (hp <= 0)
        {
            state = State.die;
        }
        //Stop(ani);
        //ReStart();

        if (thePlayerCtrl.thePlayerManager.die)
        {
            diamondAttack.SetActive(false);
            state = State.idle;
        }
    }

    public void ReStart()
    {
        die = false;
        state = State.idle;
        diamondAttack.SetActive(false);
        start = false;
        hpBarObject.SetActive(true);
        hpBarImage.enabled = true;
        itemDrop = false;
        gauntlet = false;
        spear = false;
        yellow = false;
        red = false;
    }

    void RightLeft()
    {
        if (right)
            transform.localScale = new Vector3(-1.268735f, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(1.268735f, transform.localScale.y, transform.localScale.z);
    }

    void Attack()
    {
        switch (state)
        {
            case State.idle:
                {
                    shield = false;
                    diamondAttack.SetActive(false);
                    ani.SetBool("Shield", false);
                    ani.SetBool("Attack", false);

                    if (dist < 20 && dist > 5 && !hit)
                    {
                        RightLeft();
                        ani.SetBool("isWalk", true);
                        transform.position = Vector2.MoveTowards(transform.position, 
                            new Vector3(player.transform.position.x, transform.position.y, transform.position.z), runSpeed * Time.deltaTime);
                        if(startCount == 0)
                        {
                            state = State.shield;
                            startCount = 1;
                        }
                        
                    }
                    else if(dist < 5)
                    {
                        ani.SetBool("isWalk", false);
                    }
                    if(dist < 20)
                    {
                        myRid.velocity = Vector2.zero;
                        if (startCount > 0)
                        {
                            time += Time.deltaTime;
                            if (time > 7)
                                state = State.shield;
                        }
                        //ani.SetBool("isWalk", false);
                    }

                    if (hp <= 0)
                    {
                        state = State.die;
                    }
                }
                break;
            case State.shield:
                {
                    ani.SetBool("isWalk", false);
                    time = 0;

                    myRid.velocity = Vector2.zero;
                    RightLeft();
                    ani.SetBool("Shield", true);
                    diamondAttack.SetActive(true);
                    shield = true;

                    if (!hit)
                        Invoke("BackIdle", 3);
                    if(hit)
                        state = State.attack;

                    if (hp <= 0)
                    {
                        state = State.die;
                    }
                }
                break;
            case State.attack:
                {
                    RightLeft();
                    ani.SetBool("Attack", true);
                    ani.SetBool("isWalk", false);
                    ani.SetBool("Shield", false);
                    hit = false;
                    shield = false;
                    
                    //어택 애니메이션 후 idle로 넘어감 애니메이션에 적용
                    Invoke("BackIdle", 0.5f);

                    if (hp <= 0)
                    {
                        state = State.die;
                    }
                }
                break;
            case State.die:
                {
                    if (!itemDrop && GameState.instance.enemyItemDrop)
                    {
                        ItemDrop(diamondItem, gameObject.transform);
                        itemDrop = true;
                    }
                    if (!die)
                    {
                        dieCount++;
                        die = true;
                    }
                    myRid.velocity = Vector2.zero;
                    ani.SetBool("Shield", false);
                    ani.SetBool("Attack", false);
                    ani.SetBool("isWalk", false);
                    ani.SetTrigger("Die");
                    SoundManager.instance.diamondEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyDie);
                    die = true;
                    hpBarImage.enabled = false;
                    
                    StartCoroutine(EnemyDie());
                }
                break;
        }
    }

    void ShieldStart()
    {
        state = State.shield;
    }
    public void BackIdle()
    {
        state = State.idle;
    }

    void NnockBack(bool pattern, float time)
    {
        if (hit == true && pattern == true) //넉백 풀리기
        {
            StartCoroutine(HitNnockBack(time));
            pattern = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "RedSkill" && !thePlayerCtrl.hit) //스페이드 스킬에 맞았을 때
        {
            hit = true;
            if (!shield)
            {
                red = true;
                if (hp > 1)
                {
                    ani.SetTrigger("Hit");
                    SoundManager.instance.diamondEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyHit);
                }
                diamondAttack.SetActive(false);
                SkillHit(10, hpBarImage);
                HitMove(myRid, gameObject, player);
            }
        }

        if (other.tag == "YellowSkill" && !thePlayerCtrl.hit) //하트(스탬프)에 맞았을 때
        {
            hit = true;
            if (!shield)
            {
                yellow = true;
                if (hp > 1)
                {
                    ani.SetTrigger("Hit");
                    SoundManager.instance.diamondEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyHit);
                }
                SkillHit(20, hpBarImage);
                HitMove(myRid, gameObject, player);
            }
        }

        if (other.tag == "Gauntlet" && !thePlayerCtrl.hit)
        {
            hit = true;
            if (!shield)
            {
                gauntlet = true;
                if (hp > 1)
                {
                    ani.SetTrigger("Hit");
                    SoundManager.instance.diamondEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyHit);
                }
            }
            if (!shield)
            {
                diamondAttack.SetActive(false);
                SkillHit(20, hpBarImage);
                HitMove(myRid, gameObject, player);
            }

        }

        if (other.tag == "SpearSkill" && !thePlayerCtrl.hit)
        {
            hit = true;
            if (!shield)
            {
                spear = true;
                if (hp > 1)
                {
                    ani.SetTrigger("Hit");
                    SoundManager.instance.diamondEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyHit);
                }
                diamondAttack.SetActive(false);
                SkillHit(40, hpBarImage);
                HitMove(myRid, gameObject, player);
            }
        }
        if (other.name == "HealCollider")
        {
            if (hp > maxHp - 10)
                hp = maxHp;
            else if (hp <= maxHp - 10)
                hp += 10;

            hpBarImage.fillAmount = hp / maxHp;
        }

        if (other.tag == "SpadeSpecialSkill")
        {
            hit = true;
            if (hp > 1)
            {
                ani.SetTrigger("Hit");
                SoundManager.instance.cloverEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyHit);
            }
            SkillHit(30, hpBarImage);
            HitMove(myRid, gameObject, player);
        }
    }

    IEnumerator EnemyDie()
    {
        //yield return new WaitForSeconds(0.2f);
        
        yield return new WaitForSeconds(1f);

        Die(gameObject);
    }

    public void AttackCollider(string aniName) //애니메이션 도중에는 공격 콜라이더를 키고 애니메이션이 끝나면 공격 콜라이더를 끔
    {
        if (ani.GetCurrentAnimatorStateInfo(0).IsName(aniName) && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            diamondAttack.SetActive(false);
            time += Time.deltaTime;
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName(aniName) && (ani.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.8f && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f))
            diamondAttack.SetActive(true);
    }
}
