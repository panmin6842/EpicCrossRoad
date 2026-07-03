using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveEffect2 : MonoBehaviour
{
    Transform boss;
    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.Find("SpaceKing").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(boss.position.x - 2, boss.transform.position.y - 5);
    }
}
