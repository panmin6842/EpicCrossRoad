using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartEnemy : EnemyCtrl
{
    Rigidbody2D myRid;
    public Animator ani;

    GameObject player;
    [SerializeField] GameObject heartItem;
    public GameObject healCollider;

    float runSpeed;
    float time;

    bool itemDrop; //아이템 하나만 나오게
    bool firstHeal; //startCount를 조정해줌
    public bool healStart;
    public bool die;

    int startCount;

    public Vector3 offset;

    PlayerCtrl thePlayerCtrl;

    //hpBar를 위한 변수들
    public GameObject enemyHpBar;
    GameObject canvas;
    RectTransform hpBar;
    public Image hpBarImage;

    float height; //hpBar위치 조정

    public enum State
    {
        idle,
        heal,
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

        runSpeed = 5;
        time = 0;
        startCount = 0;
        height = -4;

        hit = false;
        red = false;
        yellow = false;
        gauntlet = false;
        spear = false;
        die = false;
        start = false; //테스트 아닐 땐 false로 바꿔야함
        itemDrop = false;
        firstHeal = false;
        healStart = false;

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
            //RightLeft();
            //FlipX(gameObject.GetComponent<SpriteRenderer>(), player, gameObject);

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
            healCollider.SetActive(false);
            state = State.idle;
        }
    }

    void ReStart()
    {
        if (ButtonManager.instance.reStart == true)
        {
            die = false;
            hp = 50;
            transform.position = offset;
            state = State.idle;
            healCollider.SetActive(false);
            ButtonManager.instance.reStart = false;
        }
    }

    void RightLeft()
    {
        if (right == true)
            transform.localScale = new Vector3(-1.601639f, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(1.601639f, transform.localScale.y, transform.localScale.z);
    }

    void Attack()
    {
        switch (state)
        {
            case State.idle:
                {
                    healCollider.SetActive(false);
                    if (healStart && !firstHeal)
                    {
                        firstHeal = true;
                        startCount = 1;
                    }

                    if(startCount == 1 && firstHeal)
                    {
                        state = State.heal;
                        startCount = 2;
                    }
                    else if(startCount > 1 && firstHeal)
                    {
                        time += Time.deltaTime;
                        if (time > 9)
                            state = State.heal;
                    }
                    

                    if (hp <= 0)
                    {
                        state = State.die;
                    }
                }
                break;
            case State.heal:
                {
                    time = 0;

                    if (!hit)
                    {
                        ani.SetBool("isAttack", true);
                        healCollider.SetActive(true);
                    }
                    else if (hit)
                        BackIdle();
                    Invoke("BackIdle", 1);

                    if (hp <= 0)
                    {
                        state = State.die;
                    }
                }
                break;
            case State.die:
                {
                    if (!itemDrop)
                    {
                        ItemDrop(heartItem, gameObject.transform);
                        itemDrop = true;
                    }
                    if (!die)
                    {
                        dieCount++;
                        die = true;
                    }
                    myRid.velocity = Vector2.zero; 
                    ani.SetBool("isAttack", false);
                    ani.SetTrigger("Die");
                    SoundManager.instance.healEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyDie);
                    die = true;
                    hpBarImage.enabled = false;
                    
                    StartCoroutine(EnemyDie());
                }
                break;
        }
    }
    void BackIdle()
    {
        ani.SetBool("isAttack", false);
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
                SoundManager.instance.healEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyHit);
            }
            healCollider.SetActive(false);
            SkillHit(20, hpBarImage);
        }

        if (other.tag == "YellowSkill" && !thePlayerCtrl.hit) //하트(스탬프)에 맞았을 때
        {
            hit = true;
            yellow = true;
            if (hp > 0)
            {
                ani.SetTrigger("Hit");
                SoundManager.instance.healEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyHit);
            }
            SkillHit(10, hpBarImage);
        }

        if (other.tag == "Gauntlet" && !thePlayerCtrl.hit)
        {
            hit = true;
            gauntlet = true;
            if (hp > 0)
            {
                ani.SetTrigger("Hit");
                SoundManager.instance.healEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyHit);
            }
            healCollider.SetActive(false);
            SkillHit(10, hpBarImage);
        }

        if (other.tag == "SpearSkill" && !thePlayerCtrl.hit)
        {
            hit = true;
            spear = true;
            if (hp > 0)
            {
                ani.SetTrigger("Hit");
                SoundManager.instance.healEnemyAudioSource.PlayOneShot(SoundManager.instance.enemyHit);
            }
            healCollider.SetActive(false);
            SkillHit(5, hpBarImage);
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
        if (!ani.GetCurrentAnimatorStateInfo(0).IsName("HealEnemy_Die"))
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
            healCollider.SetActive(false);
            time += Time.deltaTime;
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName(aniName) && (ani.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.8f && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f))
            healCollider.SetActive(true);
    }

    public void AttackSound()
    {
        SoundManager.instance.healEnemyAudioSource.PlayOneShot(SoundManager.instance.healEnemyAttack);
    }
}
