using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] GameObject cutScene;
    [SerializeField] Transform potal1;
    [SerializeField] Transform startZone;
    [SerializeField] Transform potal2;
    [SerializeField] Transform potal3;
    [SerializeField] Transform potal4;
    [SerializeField] Transform potal5;
    [SerializeField] Transform potal6;
    [SerializeField] GameObject PlayerDieImage;
    GameObject player;
    Animator playerAni;

    public bool reStart = false;

    GameObject[] enemy;
    GameObject[] enemyHpBar;

    [SerializeField] AudioSource titleSFX;
    [SerializeField] AudioClip titleSFXClip;

    static public ButtonManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        startZone = GameObject.Find("StartZone").GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (player != null)
            playerAni = player.GetComponent<Animator>();
    }

    private void Update()
    {
        if (reStart)
            Invoke("ReSart", 3);

        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        enemyHpBar = GameObject.FindGameObjectsWithTag("EnemyHpBar");

        if(SceneManager.GetActiveScene().name == "TitleScene")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void ReSart()
    {
        reStart = false;
    }

    public void GameStart()
    {
        titleSFX.PlayOneShot(titleSFXClip);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("MainScene");
    }

    public void GameExit()
    {
        //titleSFX.PlayOneShot(titleSFXClip);
        Application.Quit();
    }

    public void Continue()
    {
        SoundManager.instance.uiAudioSource.PlayOneShot(SoundManager.instance.buttonClickClip);
        GameManager.instance.appear = false;
    }

    public  void Title()
    {
        SoundManager.instance.uiAudioSource.PlayOneShot(SoundManager.instance.buttonClickClip);
        GameState.instance.dialogueCount = 0;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("TitleScene");
    }

    public void ReStart()
    {
        SoundManager.instance.uiAudioSource.PlayOneShot(SoundManager.instance.buttonClickClip);
        Time.timeScale = 1f;
        GameState.instance.state = GameState.State.prologue;
        SceneManager.LoadScene("MainScene");
        //player.transform.position = startZone.position;
        //CamCtrl.instance.play = true;
        //CamCtrl.instance.state = CamCtrl.CameraState.backGround1;
        //GameState.instance.state = GameState.State.tutorial1;
    }

    public void GameOverReStart()
    {
        SoundManager.instance.uiAudioSource.PlayOneShot(SoundManager.instance.buttonClickClip);
        GameState.instance.playerAni.SetTrigger("ReSet");
        reStart = true;
        PlayerDieImage.SetActive(false);
        GameState.instance.playerAni.ResetTrigger(NotChange.instance.Die);
        GameState.instance.thePlayerCtrl.move = true;
        GameState.instance.theSkillPlay.enabled = true;
        GameState.instance.playerAni.SetTrigger("Gauntlet");
        GameState.instance.theSkillPlay.state = SkillPlay.State.Gauntlet;
        GameState.instance.thePlayerCtrl.thePlayerManager.die = false;
        GameState.instance.thePlayerCtrl.hit = false;
        GameState.instance.AniFalse();
        GameState.instance.thePlayerCtrl.thePlayerManager.hp = GameState.instance.thePlayerCtrl.thePlayerManager.maxHp;
        GameState.instance.thePlayerCtrl.thePlayerManager.hpBar.fillAmount = GameState.instance.thePlayerCtrl.thePlayerManager.hp / GameState.instance.thePlayerCtrl.thePlayerManager.maxHp;
        GameState.instance.thePlayerCtrl.thePlayerManager.hpText.text = GameState.instance.thePlayerCtrl.thePlayerManager.hp.ToString();
        GameState.instance.thePlayerCtrl.theSlotManager.SlotReSet();
        GameState.instance.thePlayerCtrl.thePlayerManager.profileReSet();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (GameState.instance.state != GameState.State.bossBattle)
        {
            GameState.instance.patternAppear1.SetActive(false);
            GameState.instance.patternAppear2.SetActive(false);
            GameState.instance.patternAppear3.SetActive(false);
            GameState.instance.patternAppear4.SetActive(false);
        }
        //EnemyDestroy();

        if (GameState.instance.state == GameState.State.tutorial2Battle)
        {
            player.transform.position = startZone.position;
            CamCtrl.instance.play = true;
            CamCtrl.instance.state = CamCtrl.CameraState.backGround1;
            SoundManager.instance.bgmAudioSource.clip = SoundManager.instance.forestNormalClip;
            SoundManager.instance.bgmAudioSource.Play();
            GameState.instance.state = GameState.State.tutorial1;
        }
        if (GameState.instance.state == GameState.State.villageBattle1Dia)
        {
            player.transform.position = potal2.position;
            SoundManager.instance.bgmAudioSource.clip = SoundManager.instance.villageNormalClip;
            SoundManager.instance.bgmAudioSource.Play();
            CamCtrl.instance.play = true;
            GameState.instance.dialogue = false;
            CamCtrl.instance.state = CamCtrl.CameraState.backGround3;
            GameState.instance.state = GameState.State.villageStart;
        }
        if (GameState.instance.state == GameState.State.villageBattle3Dia)
        {
            player.transform.position = potal3.position;
            SoundManager.instance.bgmAudioSource.clip = SoundManager.instance.villageBattleClip;
            SoundManager.instance.bgmAudioSource.Play();
            CamCtrl.instance.play = true;
            CamCtrl.instance.state = CamCtrl.CameraState.backGround4;
            GameState.instance.state = GameState.State.villageBattle2;
        }
        if (GameState.instance.state == GameState.State.villageBattle4)
        {
            player.transform.position = potal4.position;
            SoundManager.instance.bgmAudioSource.clip = SoundManager.instance.villageBattleClip;
            SoundManager.instance.bgmAudioSource.Play();
            CamCtrl.instance.play = true;
            CamCtrl.instance.state = CamCtrl.CameraState.backGround5;
            GameState.instance.state = GameState.State.villageBattle3;
        }
        if (GameState.instance.state == GameState.State.castleEntBattle1)
        {
            player.transform.position = potal5.position;
            SoundManager.instance.bgmAudioSource.clip = SoundManager.instance.villageBattleClip;
            SoundManager.instance.bgmAudioSource.Play();
            CamCtrl.instance.play = true;
            GameState.instance.dialogue = false;
            CamCtrl.instance.state = CamCtrl.CameraState.backGround6;
            GameState.instance.state = GameState.State.villageBattle5;
        }
        if (GameState.instance.state == GameState.State.bossBattle)
        {
            CamCtrl.instance.play = true;
            GameState.instance.dialogue = false;
            GameState.instance.state = GameState.State.bossStart;
            SceneManager.LoadScene("BossScene");
        }
    }

    public void EnemyDestroy()
    {
        if(enemy != null)
        {
            for(int i = 0; i < enemy.Length; i++)
            {
                Destroy(enemy[i]);
            }
        }

        if(enemyHpBar != null)
        {
            for(int i = 0; i < enemyHpBar.Length; i++)
            {
                enemyHpBar[i].SetActive(false);
            }
        }
    }
}
