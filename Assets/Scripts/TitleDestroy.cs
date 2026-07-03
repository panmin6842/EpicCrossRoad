using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleDestroy : MonoBehaviour
{
    GameObject canvas;
    GameObject gameManager;
    GameObject sound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        canvas = GameObject.FindGameObjectWithTag("MainCanvas");
        gameManager = GameObject.Find("GameManager");
        sound = GameObject.Find("Sound");
    }

    // Update is called once per frame
    void Update()
    {
        

        if (canvas != null)
            Destroy(canvas);
        if (gameManager != null)
            Destroy(gameManager);
        if (sound != null)
            Destroy(sound);
    }
}
