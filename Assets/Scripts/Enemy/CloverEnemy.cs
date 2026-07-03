using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloverEnemy : EnemyCtrl
{
    Rigidbody2D myRid;
    public Animator ani;

    GameObject player;
    [SerializeField] GameObject cloverItem;
    [SerializeField] GameObject cloverArrow;
    [SerializeField] Transform arrowZone;

    float runSpeed;
    float time;

    bool itemDrop; //아이템 하나만 나오게
    bool arrowAttack; //화살 하나만 나오도록
    public bool die;
    public bool enemyRight;

    int startCount;

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
        idle,
        attack,
        ready,
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
        maxHp = 20;
        hp = maxHp;
        time = 0;
        startCount = 0;
        height = -5;

        hit = false;
        red = false;
        yellow = false;
        gauntlet = false;
        spear = false;
        die = false;
        start = false; //테스트 아닐 땐 false로 바꿔야함
        itemDrop = false;
        arrowAttack = false;

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

        enemyRight = right;
        dist = Vector2.Distance(transform.position, player.transform.position); //적과 플레이어 사이의 거리

        if (die == false && start == true)
        {
            RightLeft();
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
            state = State.idle;
        }
    }

    public void ReStart()
    {
        die = false;
        state = State.idle;
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
        if (right == true)
            transform.localScale = new Vector3(-1.78335f, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(1.78335f, transform.localScale.y, transform.localScale.z);
    }

    void Attack()
    {
        switch (state)
        {
            case State.idle:
                {
                    ani.SetBool("Attack", false);
                    arrowAttack = false;
                    if (dist < 20 && dist > 15 && !hit && !die)
                    {
                        ani.SetBool("isWalk", true);
                        transform.position = Vector2.MoveTowards(transform.position, 
                            new Vector3(player.transform.position.x, transform.position.y, transform.position.z), runSpeed * Time.deltaTime);
                    }
                    else if (dist < 15 && !hit && !die)
                    {
                        ani.SetBool("isWalk", false);

                        myRid.velocity = Vector2.zero;
                        //애니메이션 들어가면 대기 2초 중 1초에 시전 동작이 있어야함
                        if (startCount == 0)
                        {
                            state = State.attack;
                            startCount = 1;
                        }
                        else if (startCount > 0)
                        {
                            time += Time.deltaTime;

                            if (time > 2)
                                state = State.attack;
                        }
                    }
                    else
                    {
                        myRid.velocity = Vector2.zero;
                        //ani.SetBool("isRun", false);
                    }

                    if (hp <= 0)
                    {
                        state = State.die;
                    }
                }
                break;
            case State.attack:
                {
                    ani.SetBool("isWalk", false);
                    ani.SetBool("Attack", true);
                    time = 0;
                    if (!arrowAttack)
                    {
                        GameObject newCloverArrow = Instantiate(cloverArrow, arrowZone.position, Quaternion.identity);
                        arrowAttack = true;
                    }
                    Invoke("BackIdle", 1);

                    if (hp <= 0)
                    {
                        state = State.die;
                    }
                }
                break;
            case State.ready:
                {
                    //시전동작

                    if (hp <= 0)
                    {
                        state = State.die;
                    }
                }
                break;
            case State.die:
                {
                    if (!die)
                    {
                        dieCount++;
                        die = true;
                    }
                    myRid.velocity = Vector2.zero;
                    ani.SetBool("isWalk", false);
                    ani.SetBool("Attack", false);
                    ani.SetTrigger("Die");
                    SoundManager.instance.cloverEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyDie);

                    die = true;
                    hpBarImage.enabled = false;
                    if (!itemDrop && GameState.instance.enemyItemDrop)
                    {
                        ItemDrop(cloverItem, gameObject.transform);
                        itemDrop = true;
                    }

                    StartCoroutine(EnemyDie());
                }
                break;
        }
    }

    void BackIdle()
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
            red = true;

            if (hp > 0)
            {
                ani.SetTrigger("Hit");
                SoundManager.instance.cloverEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyHit);
            }
            SkillHit(10, hpBarImage);
            HitMove(myRid, gameObject, player);
        }

        if (other.tag == "YellowSkill" && !thePlayerCtrl.hit) //하트(스탬프)에 맞았을 때
        {
            hit = true;
            yellow = true;
            if (hp > 0)
            {
                ani.SetTrigger("Hit");
                SoundManager.instance.cloverEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyHit);
            }
            SkillHit(20, hpBarImage);
            HitMove(myRid, gameObject, player);
        }

        if (other.tag == "Gauntlet" && !thePlayerCtrl.hit) //다이아몬드(건틀렛)에 맞았을 때
        {
            hit = true;
            gauntlet = true;
            if (hp > 0)
            {
                ani.SetTrigger("Hit");
                SoundManager.instance.cloverEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyHit);
            }
            SkillHit(5, hpBarImage);
            HitMove(myRid, gameObject, player);
        }

        if (other.tag == "SpearSkill" && !thePlayerCtrl.hit) //클로버(창)에 맞았을 때
        {
            hit = true;
            spear = true;
            if (hp > 0)
            {
                ani.SetTrigger("Hit");
                SoundManager.instance.cloverEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyHit);
            }
            SkillHit(10, hpBarImage);
            HitMove(myRid, gameObject, player);
        }

        if(other.name == "HealCollider")
        {
            if (hp > maxHp - 10)
                hp = maxHp;
            else if (hp <= maxHp - 10)
                hp += 10;

            hpBarImage.fillAmount = hp / maxHp;
        }

        if(other.tag == "SpadeSpecialSkill")
        {
            hit = true;
            if (hp > 0)
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
        yield return new WaitForSeconds(0.2f);
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("CloverEnemy_Die") == false)
        {
            ani.SetTrigger("Die");
        }
        
        yield return new WaitForSeconds(1f);
        
        Die(gameObject);
    }
}
