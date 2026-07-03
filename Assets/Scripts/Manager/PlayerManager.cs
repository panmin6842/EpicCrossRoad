using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    //이 스크립트는 저장이 되어야함

    public float hp;
    public float maxHp;
    public Image spadeBar;
    public Image cloverBar;
    public Image heartBar;
    public Image diamondBar;
    public Image hpBar;

    public TextMeshProUGUI hpText;

    [SerializeField] GameObject PlayerDieImage;

    int patternBarCount;

    public bool die;

    GameObject[] spadeItem;
    GameObject[] cloverItem;
    GameObject[] heartItem;
    GameObject[] diamondItem;

    EnemyCtrl[] theEnemyCtrl;

    public GameObject diamondProfile;
    public GameObject cloverProfile;
    public GameObject spadeProfile;
    public GameObject heartProfile;
    public GameObject normalProfile;

    static public PlayerManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        die = false;
        maxHp = 300;
        //patternBarCount = 34;

        hpBar.fillAmount = hp / maxHp;
        hpText.text = hp.ToString();
    }

    public void profileReSet()
    {
        diamondProfile.SetActive(false);
        cloverProfile.SetActive(false);
        spadeProfile.SetActive(false);
        heartProfile.SetActive(false);
        normalProfile.SetActive(true);
    }

    public void HitHP(float hitHp, Animator ani) //hp 소모
    {
        ani.SetBool(NotChange.instance.IsAttack1, false);
        ani.SetBool(NotChange.instance.IsAttack2, false);
        ani.SetBool(NotChange.instance.IsAttack3, false);
        hp -= hitHp;
        hpBar.fillAmount = hp / maxHp;
        hpText.text = hp.ToString();
        if(hp > 0)
            ani.SetTrigger(NotChange.instance.Hit);
        for (int i = 0; i < 3; i++)
            SkillManager.instance.gAttackCollider[i].SetActive(false);
        SkillManager.instance.playerRid.gravityScale = 9f;
        SkillManager.instance.attack2 = false;
        SkillManager.instance.attack1 = false;
        SkillManager.instance.time = 0;
        SkillManager.instance.time2 = 0;


    }
    public void HPPlus() //hp 증가
    {
        hp += (hp * 0.1f);
        hpBar.fillAmount = hp / maxHp;
    }

    public void HPPlusPercent(float percent) //hp 증가
    {
        if (hp < 230)
        {
            hp += percent;
            hpBar.fillAmount = hp / maxHp;
            hpText.text = hp.ToString();
        }
        else
        {
            hp = maxHp;
            hpBar.fillAmount = hp / maxHp;
            hpText.text = hp.ToString();
        }
    }

    public void PlayerDie(Animator ani, bool move)
    {
        if(hp <= 0 && die == false)
        {
            die = true;
            move = false;
            ani.SetTrigger(NotChange.instance.Die);
            theEnemyCtrl = FindObjectsOfType<EnemyCtrl>();
            PartternDestroy();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            //for (int i = 0; i < theEnemyCtrl.Length; i++)
            //{
            //    theEnemyCtrl[i].hp = 50;
            //    theEnemyCtrl[i].start = false;
            //}
            SkillPlay.instance.enabled = false;
            StartCoroutine(Die());
        }
    }

    public void PartternDestroy()
    {
        spadeItem = GameObject.FindGameObjectsWithTag("RedItem");
        cloverItem = GameObject.FindGameObjectsWithTag("CloverItem");
        heartItem = GameObject.FindGameObjectsWithTag("HeartItem");
        diamondItem = GameObject.FindGameObjectsWithTag("DiamondItem");

        if (spadeItem != null)
        {
            for (int i = 0; i < spadeItem.Length; i++)
                Destroy(spadeItem[i]);
        }
        if (cloverItem != null)
        {
            for (int i = 0; i < cloverItem.Length; i++)
                Destroy(cloverItem[i]);
        }
        if (heartItem != null)
        {
            for (int i = 0; i < heartItem.Length; i++)
                Destroy(heartItem[i]);
        }
        if (diamondItem != null)
        {
            for (int i = 0; i < diamondItem.Length; i++)
                Destroy(diamondItem[i]);
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(3.0f);
        Time.timeScale = 0f;
        PlayerDieImage.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SoundManager.instance.bgmAudioSource.clip = SoundManager.instance.playerDieClip;
        SoundManager.instance.bgmAudioSource.Play();
        ButtonManager.instance.EnemyDestroy();
        spadeBar.fillAmount = 0;
        heartBar.fillAmount = 0;
        cloverBar.fillAmount = 0;
        diamondBar.fillAmount = 0;
        CamCtrl.instance.play = false;
    }

    //일정 시간동안 게이지가 줄음
    public void SpadeBar(float time)
    {
        //100초 0.1
        spadeBar.fillAmount -= (time * 0.1f);
    }
    public void HeartBar(float time)
    {
        heartBar.fillAmount -= (time * 0.1f);
    }
    public void CloverBar(float time)
    {
        cloverBar.fillAmount -= (time * 0.1f);
    }
    public void DiamondBar(float time)
    {
        diamondBar.fillAmount -= (time * 0.1f);
    }

    public void HitMove(Rigidbody2D rigid, GameObject enemy, GameObject player) //맞았을 때 뒤로 조금 이동
    {
        if (enemy != null)
        {
            if (player.transform.position.x > enemy.transform.position.x) //플레이어가 적 오른쪽에 있음
                rigid.AddForce(Vector2.right * 6f, ForceMode2D.Impulse);
            else if (player.transform.position.x < enemy.transform.position.x)
                rigid.AddForce(Vector2.left * 6f, ForceMode2D.Impulse);
        }
    }
}
