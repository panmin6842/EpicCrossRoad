using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMove : MonoBehaviour
{
    GameObject player;
    SpaceKing theSpaceKing;
    // Start is called before the first frame update
    void Start()
    {
        theSpaceKing = FindObjectOfType<SpaceKing>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!theSpaceKing)
            transform.position = new Vector2(player.transform.position.x + 0.3f, player.transform.position.y);
        if (theSpaceKing)
            transform.position = new Vector2(player.transform.position.x, player.transform.position.y - 1);
    }
}
