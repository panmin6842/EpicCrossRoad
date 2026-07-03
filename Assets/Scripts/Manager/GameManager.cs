using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Animator pause;
    [SerializeField] GameObject profile;
    [SerializeField] GameObject slot;

    public bool appear;

    static public GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        appear = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerCtrl.instance.enabled && !PlayerCtrl.instance.playerDie)
            PauseAppear();
    }

    void PauseAppear()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            appear = !appear;
        }

        if(appear == false && DialogueManager.instance.quit == true)
        {
            Time.timeScale = 1f;
            pause.SetBool("Up", false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (pause.GetCurrentAnimatorStateInfo(0).IsName("Down") && pause.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
            {
                //if (playerCtrl.enabled)
                //    GameState.instance.enemyhpBar = false;
                //CamCtrl.instance.play = true;
                profile.SetActive(true);
                slot.SetActive(true);
            }
        }
        if(appear == true)
        {
            //GameState.instance.enemyhpBar = true;
            //CamCtrl.instance.play = false;
            profile.SetActive(false);
            slot.SetActive(false);
            pause.SetBool("Up", true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (pause.GetCurrentAnimatorStateInfo(0).IsName("Up") && pause.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                Time.timeScale = 0f;
            }
        }

            
    }
}
