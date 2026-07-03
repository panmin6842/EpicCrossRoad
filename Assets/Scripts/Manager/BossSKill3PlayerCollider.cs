using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSKill3PlayerCollider : MonoBehaviour
{
    [SerializeField] GameObject pattern;

    public bool diamondGet = false;
    public bool cloverGet = false;
    public bool spadeGet = false;
    public bool heartGet = false;

    private void OnCollisionStay2D(Collision2D collision)
    {
        //if (SpaceKing.instance.state == SpaceKing.State.skill3ing)
        //{
        //    if (collision.collider.name == "다이아" && Input.GetKeyDown(KeyCode.X))
        //    {
        //        diamondGet = true;
        //        pattern.SetActive(false);
        //    }
        //    else if (collision.collider.name == "스페이드" && Input.GetKeyDown(KeyCode.X))
        //    {
        //        spadeGet = true;
        //        pattern.SetActive(false);
        //    }
        //    else if (collision.collider.name == "클로버" && Input.GetKeyDown(KeyCode.X))
        //    {
        //        cloverGet = true;
        //        pattern.SetActive(false);
        //    }
        //    else if (collision.collider.name == "하트" && Input.GetKeyDown(KeyCode.X))
        //    {
        //        heartGet = true;
        //        pattern.SetActive(false);
        //    }
        //}
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (SpaceKing.instance.state == SpaceKing.State.skill3ing)
        {
            if (collision.name == "다이아" && Input.GetKey(KeyCode.X))
            {
                diamondGet = true;
                pattern.SetActive(false);
            }
            else if (collision.name == "스페이드" && Input.GetKey(KeyCode.X))
            {
                spadeGet = true;
                pattern.SetActive(false);
            }
            else if (collision.name == "클로버" && Input.GetKey(KeyCode.X))
            {
                cloverGet = true;
                pattern.SetActive(false);
            }
            else if (collision.name == "하트" && Input.GetKey(KeyCode.X))
            {
                heartGet = true;
                pattern.SetActive(false);
            }
        }
    }
}
