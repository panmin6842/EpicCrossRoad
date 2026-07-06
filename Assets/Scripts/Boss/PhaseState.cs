using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseState : MonoBehaviour
{
    public float time;

    Animator ani;

    private float onePhaseHp = 300f;
    private float twoPhaseHp = 175f;

    public enum State //페이즈 상태
    {
        onePhase,
        onePhaseing,
        twoPhase,
        twoPhaseing,
        threePhase,
        threePhaseing,
    }

    public State state;

    static public PhaseState instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        ani = GetComponent<Animator>();
        state = State.onePhase;
    }

    // Update is called once per frame
    void Update()
    {
        BossPhaseState();
    }

    void BossPhaseState()
    {
        switch(state)
        {
            case State.onePhase: //1페이즈 시작
                {
                    SpaceKing.instance.state = SpaceKing.State.skill2Idle;
                    state = State.onePhaseing;
                }
                break;
            case State.onePhaseing: //1페이즈 중
                {
                    if(SpaceKingHit.instance.hp <= onePhaseHp && SpaceKing.instance.state == SpaceKing.State.skill2Idle
                        && !SpaceKing.instance.skill2End)
                    {
                        SpaceKing.instance.state = SpaceKing.State.skillRandom1;
                        state = State.twoPhase;
                    }
                }
                break;
            case State.twoPhase:
                {
                    state = State.twoPhaseing;
                }
                break;
            case State.twoPhaseing:
                {
                    if (SpaceKing.instance.skill2End) //베어올리기 끝
                    {
                        SpaceKing.instance.time = 0;
                        AniFalse();
                        if (time > 2) //spaceking 스크립트에서 2초 후에 나오니까 2초 뺌
                        {
                            SpaceKing.instance.state = SpaceKing.State.skillRandom1;
                            
                            SpaceKing.instance.skill2End = false;
                            time = 0;
                        }
                        else
                            time += Time.deltaTime;
                    }

                    if (SpaceKing.instance.skill1End) //내려찍기 끝
                    {
                        SpaceKing.instance.time = 0;
                        AniFalse();
                        if (time > 3)
                        {
                            SpaceKing.instance.state = SpaceKing.State.skillRandom1;
                            SpaceKing.instance.skill1End = false;
                            time = 0;
                        }
                        else
                            time += Time.deltaTime;
                    }

                    if (SpaceKingHit.instance.hp <= twoPhaseHp && (SpaceKing.instance.state == SpaceKing.State.skillExit
                    || SpaceKing.instance.state == SpaceKing.State.skill2End)
                    && !SpaceKing.instance.skill1End)
                    {
                        state = State.threePhase;
                    }
                }
                break;
            case State.threePhase:
                {
                    //SpaceKing.instance.state = SpaceKing.State.skillRandom2;
                    //SpaceKing.instance.state = SpaceKing.State.skill1Idle;//테스트
                    state = State.threePhaseing;
                }
                break;
            case State.threePhaseing:
                {
                    if (SpaceKing.instance.skill2End) //베어올리기 끝
                    {
                        SpaceKing.instance.time = 0;
                        AniFalse();
                        if (time > 2)
                        {
                            SpaceKing.instance.state = SpaceKing.State.skillRandom3;
                            //SpaceKing.instance.state = SpaceKing.State.skill2Idle; //테스트
                            SpaceKing.instance.skill2End = false;
                            time = 0;
                        }
                        else
                            time += Time.deltaTime;
                    }

                    if (SpaceKing.instance.skill1End) //내려찍기 끝
                    {
                        SpaceKing.instance.time = 0;
                        AniFalse();
                        if (time > 3)
                        {
                            SpaceKing.instance.state = SpaceKing.State.skillRandom2;
                            //SpaceKing.instance.state = SpaceKing.State.skill1Idle; //테스트
                            SpaceKing.instance.skill1End = false;
                            time = 0;
                        }
                        else
                            time += Time.deltaTime;
                    }

                    if (SpaceKing.instance.skill3End) //참격 끝
                    {
                        SpaceKing.instance.time = 0;
                        AniFalse();
                        if (time > 4)
                        {
                            SpaceKing.instance.state = SpaceKing.State.skillRandom1;
                            //SpaceKing.instance.state = SpaceKing.State.skill3Start; //테스트
                            SpaceKing.instance.skill3End = false;
                            time = 0;
                        }
                        else
                            time += Time.deltaTime;
                    }
                }
                break;
        }
    }

    void AniFalse()
    {
        ani.SetBool("Attack3Ready", false);
        ani.SetBool("Attack3", false);
        ani.SetBool("Attack2Ready", false);
        ani.SetBool("Attack2", false);
        ani.SetBool("Attack2Disappear", false);
        ani.SetBool("Attack1Ready", false);
        ani.SetBool("Attack1", false);
    }
}
