using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject walkPlatform; 
    [SerializeField] GameObject runPlatform; 
    [SerializeField] GameObject jumpPlatform; 
    [SerializeField] GameObject attackPlatform;
    [SerializeField] GameObject wayPlatform;

    [SerializeField] GameObject director;

    [SerializeField] Transform platformStop;
    [SerializeField] Transform attack2Zone;

    private Rigidbody2D rigid;
    private Animator ani;

    private float movSpeed; //움직임 속도
    private float jumpPower; //점프 속도
    private float platformMovSpeed; //플랫폼 속도
    private float pMove; //축 저장
    public float time;
    public float attackTime; //공격때 씀

    private float walkCount; //요청에 맞는 움직임을 했는지 확인 하는 용도
    private float runCount; //요청에 맞는 움직임을 했는지 확인 하는 용도
    private float jumpCount; //요청에 맞는 움직임을 했는지 확인 하는 용도

    private bool move;
    private bool jump;
    private bool walkExit;
    private bool runExit;
    private bool jumpExit;
    private bool rightMove; //공격할 때 오른쪽이나 왼쪽으로 한번만

    Vector3 offset; //플랫폼들의 처음 위치

    public enum State
    {
        moving,
        walk,
        run,
        jump,
        attack,
        attacking,
    }

    public State state;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();

        movSpeed = 10f;
        jumpPower = 15f;
        platformMovSpeed = 10;

        move = true;
        jump = false;
        walkExit = false;
        runExit = false;
        jumpExit = false;
        rightMove = false;

        walkCount = 0;
        runCount = 0;
        jumpCount = 0;
        time = 0;
        attackTime = 0;
        state = State.walk;

        offset = walkPlatform.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            Move();
            Jump();
        }

        TitleFlow();
    }

    //플레이어 움직임
    void Move()
    {
        pMove = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * pMove, ForceMode2D.Impulse); //리지드바디 힘을 통해서 움직임

        if (Input.GetAxisRaw("Horizontal") > 0) //오른쪽
        {
            rigid.velocity = new Vector2(movSpeed, rigid.velocity.y);
            transform.eulerAngles = new Vector3(0, 0, 0);

            ani.SetBool("isWalk", true);

            walkCount += Time.deltaTime;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0) //왼쪽
        {
            rigid.velocity = new Vector2(movSpeed * (-1), rigid.velocity.y);
            transform.eulerAngles = new Vector3(0, 180, 0);

            ani.SetBool("isWalk", true);

            walkCount += Time.deltaTime;
        }

        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(0.2f * rigid.velocity.normalized.x, rigid.velocity.y); //velocity를 통해 움직임 멈춤
            ani.SetBool("isWalk", false);
            ani.SetBool("isRun", false);
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetButton("Horizontal"))
        {
            movSpeed = 17f;
            ani.SetBool("isRun", true);

            runCount += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            movSpeed = 10f;
            ani.SetBool("isRun", false);
        }
    }

    //플레이어 점프
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !jump)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            ani.SetBool("isJump", true);
            jump = true;

            jumpCount++;
        }
    }

    void TitleFlow()
    {
        switch (state)
        {
            case State.moving:
                {
                    rigid.gravityScale = 3.3f;
                    time = 0;
                    move = true;

                    if (walkCount >= 3 && !walkExit)
                        state = State.run;
                    else if (runCount >= 3 && !runExit)
                        state = State.jump;
                    else if (jumpCount >= 2 && !jumpExit && !jump)
                        state = State.attack;

                    if (jumpExit && Input.GetKeyDown(KeyCode.Z) && !rightMove)
                    {
                        AnimationReSet();
                        ani.SetBool("isGauntlet", true);
                        state = State.attacking;
                    }

                    if (rightMove)
                    {
                        wayPlatform.transform.position = Vector3.MoveTowards(wayPlatform.transform.position,
                        new Vector3(wayPlatform.transform.position.x, platformStop.position.y + 1, wayPlatform.transform.position.z),
                        platformMovSpeed * Time.deltaTime);
                    }
                }
                break;
            case State.walk:
                {
                    move = false;
                    walkPlatform.transform.position = Vector3.MoveTowards(walkPlatform.transform.position, 
                        new Vector3(walkPlatform.transform.position.x, platformStop.position.y, walkPlatform.transform.position.z), 
                        platformMovSpeed * Time.deltaTime);

                    time += Time.deltaTime;

                    if (time > 3)
                        state = State.moving;
                }
                break;
            case State.run:
                {
                    walkExit = true;
                    AnimationReSet();
                    move = false;
                    walkPlatform.transform.position = Vector3.MoveTowards(walkPlatform.transform.position,
                        new Vector3(walkPlatform.transform.position.x, offset.y, walkPlatform.transform.position.z),
                        platformMovSpeed * Time.deltaTime);

                    time += Time.deltaTime;

                    if (time > 3)
                    {
                        runPlatform.transform.position = Vector3.MoveTowards(runPlatform.transform.position,
                        new Vector3(runPlatform.transform.position.x, platformStop.position.y, runPlatform.transform.position.z),
                        platformMovSpeed * Time.deltaTime);
                    }
                    if (time > 5)
                    {
                        state = State.moving;
                    }
                }
                break;
            case State.jump:
                {
                    movSpeed = 10f;
                    runExit = true;
                    AnimationReSet();
                    move = false;
                    runPlatform.transform.position = Vector3.MoveTowards(runPlatform.transform.position,
                        new Vector3(runPlatform.transform.position.x, offset.y, runPlatform.transform.position.z),
                        platformMovSpeed * Time.deltaTime);

                    time += Time.deltaTime;

                    if (time > 3)
                    {
                        jumpPlatform.transform.position = Vector3.MoveTowards(jumpPlatform.transform.position,
                        new Vector3(jumpPlatform.transform.position.x, platformStop.position.y, jumpPlatform.transform.position.z),
                        platformMovSpeed * Time.deltaTime);
                    }
                    if (time > 5)
                    {
                        state = State.moving;
                    }
                }
                break;
            case State.attack:
                {
                    jumpExit = true;
                    AnimationReSet();
                    move = false;
                    jumpPlatform.transform.position = Vector3.MoveTowards(jumpPlatform.transform.position,
                        new Vector3(jumpPlatform.transform.position.x, offset.y, jumpPlatform.transform.position.z),
                        platformMovSpeed * Time.deltaTime);

                    time += Time.deltaTime;

                    if (time > 3)
                    {
                        attackPlatform.transform.position = Vector3.MoveTowards(attackPlatform.transform.position,
                        new Vector3(attackPlatform.transform.position.x, platformStop.position.y, attackPlatform.transform.position.z),
                        platformMovSpeed * Time.deltaTime);
                    }
                    if (time > 5)
                    {
                        state = State.moving;
                    }
                }
                break;
            case State.attacking:
                {
                    move = false;

                    attackTime += Time.deltaTime;
                    if (attackTime > 0 && attackTime < 2)
                    {
                        ani.SetBool("isAttack1", true);
                    }
                    else if(attackTime > 2 && attackTime < 2.5f)
                    {
                        if (!rightMove)
                        {
                            ani.SetBool("isAttack1", false);
                            ani.SetBool("isAttack2", true);
                        }
                        transform.position = Vector3.MoveTowards(transform.position,
                            new Vector3(transform.position.x, attack2Zone.position.y, transform.position.z)
                            , 30 * Time.deltaTime);
                        if (transform.eulerAngles == new Vector3(0, 0, 0) && !rightMove) //오른쪽
                        {
                            rigid.AddForce(Vector2.right * 5, ForceMode2D.Impulse);
                            rightMove = true;
                        }
                        else if (transform.eulerAngles == new Vector3(0, 180, 0) && !rightMove) //왼쪽
                        {
                            rigid.AddForce(Vector2.left * 5, ForceMode2D.Impulse);
                            rightMove = true;
                        }
                    }

                    if(attackTime > 6)
                    {
                        ani.SetBool("isAttack3", false);
                        AnimationReSet();
                        state = State.moving;
                    }
                }
                break;
        }
    }

    void AnimationReSet()
    {
        rigid.velocity = Vector3.zero;

        ani.SetBool("isWalk", false);
        ani.SetBool("isRun", false);
        ani.SetBool("isJump", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            ani.SetBool("isJump", false);
            jump = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Attack2Zone")
        {
            attackPlatform.SetActive(false);
            ani.SetBool("isAttack2", false);
            ani.SetBool("isAttack3", true);
            rigid.gravityScale = 3.3f;
            Invoke("BackAniStart", 0.2f);
        }
    }

    void BackAniStart()
    {
        director.SetActive(true);
    }
}
