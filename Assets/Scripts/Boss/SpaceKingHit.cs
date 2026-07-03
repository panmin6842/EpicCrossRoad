using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpaceKingHit : MonoBehaviour
{
    public float hp;
    public float maxHp;

    public bool noHit;
    private bool die;

    PlayerCtrl thePlayerCtrl;
    SkillPlay theSkillPlay;

    public Image hpBar;
    public TextMeshProUGUI hpText;

    [SerializeField] GameObject endBoss;
    public GameObject hitEffect;

    GameObject newHitEffect;
    Vector3 hitEffectPosition;

    static public SpaceKingHit instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        hp = 500;
        maxHp = 500;

        noHit = false;

        thePlayerCtrl = FindObjectOfType<PlayerCtrl>();
        theSkillPlay = FindObjectOfType<SkillPlay>();
        hpBar.fillAmount = hp / maxHp;
        hpText.text = hp.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    hp = 300;
        //    hpBar.fillAmount = hp / maxHp;
        //}
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    hp = 175;
        //    hpBar.fillAmount = hp / maxHp;
        //}

        if(newHitEffect != null)
        {
            newHitEffect.transform.position = hitEffectPosition;
        }

        if(hp <= 0)
        {
            hp = 0;
            hpText.text = hp.ToString();
        }

        if(hp <= 0 && !die && (SpaceKing.instance.state == SpaceKing.State.skill2End || SpaceKing.instance.state == SpaceKing.State.skillExit
            || SpaceKing.instance.state == SpaceKing.State.skill3Exit))
        {
            DialogueManager.instance.OnDialogue(DialogueText.instance.bossSentence2, DialogueText.instance.bossName2, DialogueText.instance.bossImage2);
            endBoss.SetActive(true);
            GameState.instance.state = GameState.State.bossDie;
            die = true;
        }
    }

    void Hit(float onePhaseHit, float twoPhaseHit, float threePhaseHit)
    {
        if (PhaseState.instance.state == PhaseState.State.onePhaseing)
        {
            hp -= onePhaseHit;
            hpBar.fillAmount = hp / maxHp;
            hpText.text = hp.ToString();
        }
        else if (PhaseState.instance.state == PhaseState.State.twoPhaseing)
        {
            hp -= twoPhaseHit;
            hpBar.fillAmount = hp / maxHp;
            hpText.text = hp.ToString();
        }
        else if (PhaseState.instance.state == PhaseState.State.threePhaseing)
        {
            hp -= threePhaseHit;
            hpBar.fillAmount = hp / maxHp;
            hpText.text = hp.ToString();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!noHit)
        {
            if (collision.tag == "RedSkill" && !thePlayerCtrl.hit) //스페이드 스킬에 맞았을 때
            {
                Hit(8, 7f, 6f);
                HitEffectAppear(collision);
            }

            if (collision.tag == "YellowSkill" && !thePlayerCtrl.hit) //하트(스탬프)에 맞았을 때
            {
                Hit(5f, 5f, 5f);
                HitEffectAppear(collision);
            }

            if (collision.tag == "Gauntlet" && !thePlayerCtrl.hit && theSkillPlay.diamondMode) //다이아몬드(건틀렛)에 맞았을 때
            {
                Hit(15, 12, 10);
                HitEffectAppear(collision);
            }

            if (collision.tag == "SpearSkill" && !thePlayerCtrl.hit) //클로버(창)에 맞았을 때
            {
                Hit(8, 7f, 6f);
                HitEffectAppear(collision);
            }

            if (collision.tag == "Gauntlet" && !thePlayerCtrl.hit && !theSkillPlay.diamondMode) //일반(건틀렛)에 맞았을 때
            {
                Hit(10, 8, 6);
                HitEffectAppear(collision);
            }

            if (collision.tag == "SpadeSpecialSkill")
            {
                Hit(30, 30, 30);
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            if(collision.tag == "CloverSkillEffect")
            {
                HitEffectAppear(collision);
            }
        }
    }

    void HitEffectAppear(Collider2D collision)
    {
        //플레이어 콜라이더 중심과 몬스터 콜라이더 중심을 찾기
        Vector3 playerColliderCenter = collision.bounds.center;
        Vector3 bossColliderCenter = this.GetComponent<BoxCollider2D>().bounds.center;

        //두 중심 지점 사이의 방향 벡터 계산
        Vector3 directionToMonster = (bossColliderCenter - playerColliderCenter).normalized;

        hitEffectPosition = bossColliderCenter - directionToMonster;

        newHitEffect = Instantiate(hitEffect, new Vector3(hitEffectPosition.x, hitEffectPosition.y - 20, hitEffectPosition.z), Quaternion.identity);
        SoundManager.instance.bossHitAudioSource.PlayOneShot(SoundManager.instance.bossHit);
    }
}
