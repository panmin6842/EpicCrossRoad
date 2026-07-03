using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloverArrow : MonoBehaviour
{
    Vector3 speed;

    CloverEnemy theCloverEnemy;
    GameObject player;
    BoxCollider2D coll;

    public float dist;

    int random;

    // Start is called before the first frame update
    void Start()
    {
        theCloverEnemy = FindObjectOfType<CloverEnemy>();
        player = GameObject.FindWithTag("Player");
        coll = GetComponent<BoxCollider2D>();

        random = Random.Range(0, 11);

        if (random >= 0 && random < 10) //10%확률로 콜라이더를 꺼서 플레이어에게 데미지를 안 줌
            coll.enabled = true;
        else if (random == 10)
            coll.enabled = false;

        if (theCloverEnemy.right) //방향 조절
        {
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            speed = new Vector3(20, 0, 0);
        }
        else if (!theCloverEnemy.right)
        {
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            speed = new Vector3(-20, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector2.Distance(transform.position, player.transform.position);
        transform.Translate(speed * Time.deltaTime);

        Invoke("GameObjectDestroy", 4); //4초 지나면 파괴되도록

        if (dist < 1.5f) //플레이어와의 거리가 3이하면 파괴되도록
            Destroy(this.gameObject);
    }

    void GameObjectDestroy()
    {
        Destroy(this.gameObject);
    }
}
