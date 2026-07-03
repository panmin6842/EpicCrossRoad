using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedEnemy : EnemyCtrl
{
    Rigidbody2D myRid;
    public Animator ani;

    GameObject player;
    [SerializeField] GameObject spadeItem;
    public GameObject spadeAttack;

    float runSpeed;
    float time;

    bool itemDrop; //아이템 하나만 나오게
    public bool die;

    int startCount;

    public Vector3 offset; //처음에 서있는 자리

    //hpBar를 위한 변수들
    public GameObject enemyHpBar;
    GameObject canvas;
    RectTransform hpBar;
    public Image hpBarImage;
    GameObject hpBarObject;

    float height; //hpBar위치 조정

    PlayerCtrl thePlayerCtrl;

    public enum State
    {
        idle,
        attack,
        quit,
        die,
    }

    public State state;
    // Start is called before the first frame update
    void Start()
    {
        myRid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        thePlayerCtrl= player.GetComponent<PlayerCtrl>();
        canvas = GameObject.FindGameObjectWithTag("Canvas");

        //hpBar생성
        hpBar = Instantiate(enemyHpBar, canvas.transform).GetComponent<RectTransform>();
        hpBarImage = hpBar.GetComponent<Image>();
        hpBarImage.fillAmount = hp / maxHp;

        hpBarObject = hpBarImage.gameObject;

        runSpeed = 5;
        maxHp = 40;
        hp = maxHp;
        time = 0;
        startCount = 0;
        height = -4;

        hit = false;
        red = false;
        yellow = false;
        spear = false;
        gauntlet = false;
        die = false;
        start = false; //테스트 아닐 땐 false로 바꿔야함
        itemDrop = false;

        state = State.idle;

        offset = this.gameObject.transform.position;

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
            spadeAttack.SetActive(false);
            state = State.quit;
        }

    }

    //플레이어가 죽고 다시 시작했을 때
    public void ReStart()
    {
        die = false;
        state = State.idle;
        start = false;
        spadeAttack.SetActive(false);
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
        if(right == true)
            transform.localScale = new Vector3(-1.393847f, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(1.393847f, transform.localScale.y, transform.localScale.z);
    }

    void Attack()
    {
        switch (state)
        {
            case State.idle:
                {
                    spadeAttack.SetActive(false);
                    ani.SetBool("Attack", false);
                    if (dist < 20 && dist > 5 && hit == false)
                    {
                        ani.SetBool("isWalk", true);
                        transform.position = Vector2.MoveTowards(transform.position, 
                            new Vector3(player.transform.position.x, transform.position.y, transform.position.z), runSpeed * Time.deltaTime);
                    }
                    else if (dist <= 5 && hit == false)
                    {
                        ani.SetBool("isWalk", false);
                        myRid.velocity = Vector2.zero;
                        if (startCount == 0)
                        {
                            state = State.attack;
                            startCount = 1;
                        }
                        else if(startCount > 0)
                        {
                            time += Time.deltaTime;
                            if(time > 2.5f)
                                state = State.attack;
                        }
                    }
                    else
                    {
                        ani.SetBool("isWalk", false);
                    }


                    if (hp <= 0)
                    {
                        state = State.die;
                    }
                }
                break;
            case State.attack:
                {
                    time = 0;

                    if (!hit)
                    {
                        spadeAttack.SetActive(true);
                        ani.SetBool("Attack", true);
                        Invoke("BackIdle", 0.7f);
                    }

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
                        ItemDrop(spadeItem, gameObject.transform);
                        itemDrop = true;
                    }
                    if (!die)
                    {
                        dieCount++;
                        die = true;
                    }
                    myRid.velocity = Vector2.zero;
                    ani.SetBool("Attack", false);
                    SoundManager.instance.spadeEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyDie);
                    ani.SetTrigger("Die");
                    die = true;
                    hpBarImage.enabled = false;
                    
                    StartCoroutine(EnemyDie());
                }
                break;
        }
    }

    void BackIdle()
    {
        state = State.idle;
    }
    void AttackStart()
    {
        state = State.attack;
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
                SoundManager.instance.spadeEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyHit);
            }
            spadeAttack.SetActive(false);
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
                SoundManager.instance.spadeEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyHit);
            }
            spadeAttack.SetActive(false);
            SkillHit(5, hpBarImage);
            HitMove(myRid, gameObject, player);
        }

        if(other.tag == "Gauntlet" && !thePlayerCtrl.hit)
        {
            hit = true;
            gauntlet = true;
            if (hp > 0)
            {
                ani.SetTrigger("Hit");
                SoundManager.instance.spadeEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyHit);
            }
            spadeAttack.SetActive(false);
            SkillHit(10, hpBarImage);
            HitMove(myRid, gameObject, player);
        }

        if(other.tag == "SpearSkill" && !thePlayerCtrl.hit)
        {
            hit = true;
            spear = true;
            if (hp > 0)
            {
                ani.SetTrigger("Hit");
                SoundManager.instance.spadeEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyHit);
            }
            spadeAttack.SetActive(false);
            SkillHit(20, hpBarImage);
            HitMove(myRid, gameObject, player);
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
        if (!ani.GetCurrentAnimatorStateInfo(0).IsName("SpadeEnemy_Die"))
        {
            ani.SetTrigger("Die");
        }
        
        yield return new WaitForSeconds(1f);
        
        Die(gameObject);
    }

    public void AttackCollider(string aniName)
    {
        if (ani.GetCurrentAnimatorStateInfo(0).IsName(aniName) && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            spadeAttack.SetActive(false);
            time += Time.deltaTime;
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName(aniName) && (ani.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.8f && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f))
            spadeAttack.SetActive(true);
    }

    public void AttackSound()
    {
        SoundManager.instance.spadeEnemyAudioSource.PlayOneShot(SoundManager.instance.spadeEnemyAttack);
    }

}
