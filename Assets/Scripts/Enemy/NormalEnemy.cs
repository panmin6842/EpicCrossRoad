using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalEnemy : EnemyCtrl
{
    Rigidbody2D myRid;
    Animator ani;

    GameObject player;
    public GameObject attackCollider;

    float runSpeed;
    float time;

    bool itemDrop; //아이템 하나만 나오게
    bool hit;
    public bool die;

    int random;

    public Vector3 offset;

    PlayerCtrl thePlayerCtrl;

    //hpBar를 위한 변수들
    public GameObject enemyHpBar;
    GameObject canvas;
    RectTransform hpBar;
    Image hpBarImage;

    float height; //hpBar위치 조정

    public enum State
    {
        attack1,
        attack2,
        attack3,
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
        thePlayerCtrl = player.GetComponent<PlayerCtrl>();
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        hpBarImage = enemyHpBar.GetComponent<Image>();

        //hpBar생성
        hpBar = Instantiate(enemyHpBar, canvas.transform).GetComponent<RectTransform>();

        runSpeed = 5;
        time = 0;

        hit = false;
        red = false;
        yellow = false;
        gauntlet = false;
        spear = false;
        die = false;
        start = false;
        itemDrop = false;

        state = State.quit;

        offset = transform.position;

        random = Random.Range(0, 4);
    }

    // Update is called once per frame
    void Update()
    {
        //hpBar가 적을 따라가게 함
        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, transform.position.z));
        hpBar.position = _hpBarPos;
        dist = Vector2.Distance(transform.position, player.transform.position); //적과 플레이어 사이의 거리

        if (!die && start)
        {
            RightLeft();
            FlipX(gameObject.GetComponent<SpriteRenderer>(), player, gameObject);

            if (dist < 20 && dist > 5 && !hit)
            {
                ani.SetBool("isRun", true);
                transform.position = Vector2.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), runSpeed * Time.deltaTime);
            }
            else if (dist <= 5 && !hit)
            {
                ani.SetBool("isRun", false);
                if (hit == false)
                {
                    Attack();
                }
            }
            else
            {
                ani.SetBool("isRun", false);
            }

            NnockBack(gauntlet, 1f);
            NnockBack(red, 1f);
            NnockBack(yellow, 1f);
            NnockBack(spear, 1f);
        }

        if (hp <= 0)
        {
            state = State.die;
        }

        Stop(ani);
        ReStart();
        if (PlayerManager.instance.die)
        {
            attackCollider.SetActive(false);
            state = State.quit;
        }
    }
    void ReStart()
    {
        if (ButtonManager.instance.reStart == true)
        {
            Debug.Log("enemy");
            die = false;
            hp = 50;
            transform.position = offset;
            state = State.quit;
            attackCollider.SetActive(false);
            ButtonManager.instance.reStart = false;
        }
    }

    void RightLeft()
    {
        if (right)
            transform.localScale = new Vector3(-1.78335f, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(1.78335f, transform.localScale.y, transform.localScale.z);
    }

    void Attack()
    {
        switch (state)
        {
            case State.attack1:
                {

                    AttackCollider("RedEnemy_Attack1");

                    if (time > 0.3)
                    {
                        time = 0;
                        ani.SetBool("Attack2", true);
                        state = State.attack2;
                    }

                    if (hp <= 0)
                    {
                        state = State.die;
                    }
                }
                break;
            case State.attack2:
                {

                    AttackCollider("RedEnemy_Attack2");

                    if (time > 0.3)
                    {
                        time = 0;
                        ani.SetBool("Attack3", true);
                        state = State.attack3;
                    }

                    if (hp <= 0)
                    {
                        state = State.die;
                    }
                }
                break;
            case State.attack3:
                {

                    AttackCollider("RedEnemy_Attack3");

                    if (time > 0.3)
                    {
                        time = 0;
                        state = State.quit;
                    }

                    if (hp <= 0)
                    {
                        state = State.die;
                    }
                }
                break;
            case State.quit:
                {
                    ani.SetBool("Attack1", false);
                    ani.SetBool("Attack2", false);
                    ani.SetBool("Attack3", false);
                    attackCollider.SetActive(false);

                    if (time > 0.5f)
                    {
                        time = 0;
                        ani.SetBool("Attack1", true);
                        state = State.attack1;
                    }
                    else if (time < 0.5)
                        time += Time.deltaTime;

                    if (hp <= 0)
                    {
                        state = State.die;
                    }
                }
                break;
            case State.die:
                {
                    myRid.velocity = Vector2.zero;
                    ani.SetBool("Attack1", false);
                    ani.SetBool("Attack2", false);
                    ani.SetBool("Attack3", false);
                    ani.SetTrigger("Die");
                    die = true;
                    StartCoroutine(EnemyDie());
                }
                break;
        }
    }

    void NnockBack(bool pattern, float time)
    {
        if (hit && pattern) //넉백 풀리기
        {
            ani.SetTrigger("Hit");
            StartCoroutine(HitNnockBack(time));
            pattern = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "RedSkill") //스페이드 스킬에 맞았을 때
        {
            hit = true;
            red = true;
            attackCollider.SetActive(false);
            SkillHit(4, hpBarImage);
            HitMove(myRid, gameObject, player);
        }

        if (other.tag == "YellowSkill") //하트(스탬프)에 맞았을 때
        {
            hit = true;
            yellow = true;
            SkillHit(2.5f, hpBarImage);
            HitMove(myRid, gameObject, player);

            if (hp <= 0)
            {
                StartCoroutine(EnemyDie());
            }
        }

        if (other.tag == "Gauntlet" && !thePlayerCtrl.hit)
        {
            hit = true;
            gauntlet = true;
            attackCollider.SetActive(false);
            SkillHit(2.5f, hpBarImage);
        }

        if (other.tag == "SpearSkill")
        {
            hit = true;
            spear = true;
            attackCollider.SetActive(false);
            SkillHit(2.5f, hpBarImage);
        }
    }

    IEnumerator EnemyDie()
    {
        yield return new WaitForSeconds(0.2f);
        if (!ani.GetCurrentAnimatorStateInfo(0).IsName("RedEnemy_Die"))
        {
            ani.SetTrigger("Die");
        }
        dieCount++;
        yield return new WaitForSeconds(3f);
        if (!ani.GetCurrentAnimatorStateInfo(0).IsName("RedEnemy_Die"))
        {
            ani.SetTrigger("Die");
        }
        
        Die(gameObject);
    }

    public void AttackCollider(string aniName)
    {
        if (ani.GetCurrentAnimatorStateInfo(0).IsName(aniName) && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            attackCollider.SetActive(false);
            time += Time.deltaTime;
        }
        else if (ani.GetCurrentAnimatorStateInfo(0).IsName(aniName) && (ani.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.8f && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f))
            attackCollider.SetActive(true);
    }
}
