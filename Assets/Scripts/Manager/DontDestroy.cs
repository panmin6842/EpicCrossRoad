using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static List<string> dontDestroyObjects = new List<string>();
    bool dontDestroy;

    private void Awake()
    {
        

        //if (dontDestroyObjects.Contains(this.gameObject.name))
        //{
        //    Destroy(this.gameObject);
        //}
        //else
        //{
        //    dontDestroyObjects.Add(this.gameObject.name);
        //    DontDestroyOnLoad(this.gameObject);
        //}
    }

    private void Start()
    {
        dontDestroy = false;
    }

    private void Update()
    {
        if(GameState.instance.state == GameState.State.bossStart && !dontDestroy)
        {
            DontDestroyOnLoad(this.gameObject);
            dontDestroy = true;
        }

        if(GameState.instance.state == GameState.State.Quit)
        {
            gameObject.SetActive(false);
        }
    }
}
