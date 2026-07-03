using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampEffectManager : MonoBehaviour
{
    float speed;

    float time;

    bool right;

    // Start is called before the first frame update
    void Start()
    {
        speed = 20;
        time = 0;

        if (SkillManager.instance.isRight) //처음 한번만 방향 측정
            right = true;
        else if (!SkillManager.instance.isRight)
            right = false;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(time > 1.25f)
        {
            Destroy(gameObject);
        }

        if (right) //방향 조절
        {
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        if (!right)
        {
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            transform.Translate(Vector3.right * -speed * Time.deltaTime);
        }
    }
}
