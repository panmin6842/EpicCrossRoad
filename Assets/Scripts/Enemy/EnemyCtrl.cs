using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCtrl : MonoBehaviour
{
    //적 공통적인 부분들

    public float hp;
    public float maxHp;
    public bool hit;
    public float dist; //플레이어와 적 사이의 거리
    public bool start; //움직임 껏다 켰다 하는 용도
    public bool right; //오른쪽 왼쪽 표시
    public int dieCount; //죽은 적 수

    public bool red; //넉백 용
    public bool yellow; //넉백 용
    public bool gauntlet; //넉백 용
    public bool spear; //넉백 용

    GameObject newItem;

    HeartEnemy theHeartEnemy;

    // Start is called before the first frame update
    void Start()
    {
        hit = false;
        dieCount = 0;
    }

    //플레이어가 죽었을 때 멈추기
    public void Stop(Animator ani)
    {
        if (PlayerManager.instance.die)
        {
            ani.SetBool(NotChange.instance.IsRun, false);
            ani.SetBool(NotChange.instance.IsAttack1, false);
            ani.SetBool(NotChange.instance.IsAttack2, false);
            ani.SetBool(NotChange.instance.IsAttack3, false);
        }
    }

    public void AllReSet()
    {
        hp = 50;
        start = false;
    }

    public void SkillHit(float hitHp, Image hpBarImg) //피 감소
    {
        hp -= (hitHp + SkillPlay.instance.strBuff);
        hpBarImg.fillAmount = hp / maxHp;
        if (GameState.instance.newHealEnemy != null)
        {
            theHeartEnemy = FindObjectOfType<HeartEnemy>();
            theHeartEnemy.healStart = true;
        }
    }

    public void Die(GameObject enemy) //적 모습 감추기
    {
        //enemy.SetActive(false);
        if (enemy.tag == "Enemy")
        {
            Destroy(enemy);
        }
        else
        {
            enemy.SetActive(false);
        }
    }

    public void FlipX(SpriteRenderer render, GameObject player, GameObject enemy) //플레이어 있는 곳으로 보게 함
    {
        if(enemy.transform.position.x < player.transform.position.x) //오른쪽
        {
            right = true;
            //render.flipX = true;
        }
        else if (enemy.transform.position.x > player.transform.position.x) //왼쪽
        {
            right = false;
            //render.flipX = false;
        }
    }

    public void HitMove(Rigidbody2D rigid, GameObject enemy, GameObject player) //맞았을 때 뒤로 조금 이동
    {
        if (enemy.tag == "Enemy")
        {
            if (player.transform.position.x < enemy.transform.position.x)
                rigid.AddForce(Vector2.right * 3f, ForceMode2D.Impulse);
            else if (player.transform.position.x > enemy.transform.position.x)
                rigid.AddForce(Vector2.left * 3f, ForceMode2D.Impulse);
        }
    }

    public GameObject ItemDrop(GameObject item, Transform enemy) //스페이드 문향 기운 드랍
    {
        newItem = Instantiate(item, enemy.position, Quaternion.identity);

        return newItem;
    }

    public IEnumerator HitNnockBack(float time)
    {
        yield return new WaitForSeconds(time);
       hit = false;
    }
}
