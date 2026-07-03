using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosChangeCollider : MonoBehaviour
{
    [SerializeField] Transform bg2Pos;
    [SerializeField] Transform bg3Pos;
    [SerializeField] Transform bg4Pos;
    [SerializeField] Transform bg5Pos;
    [SerializeField] Transform bg6Pos;
    [SerializeField] Transform bg7Pos;
    [SerializeField] Transform bg8Pos;

    [SerializeField] GameObject TutoSpadeItem;
    [SerializeField] GameObject castleEntEnemy;
    PlayerManager thePlayerManager;
    SlotManager theSlotManager;

    private void Start()
    {
        thePlayerManager = FindObjectOfType<PlayerManager>();
        theSlotManager = FindObjectOfType<SlotManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!theSlotManager.sameItemGet)
        {
            if (collision.name == "TutoSpade") //두번째 맵에서 기운을 획득했을 경우
            {
                thePlayerManager.spadeProfile.SetActive(true);
                SoundManager.instance.playerAttackSource.PlayOneShot(SoundManager.instance.itemGet);
                GameState.instance.skillCountMinStop = true;
                GameState.instance.AniFalse();
                transform.GetComponent<Animator>().SetTrigger("Sword");
                SkillPlay.instance.state = SkillPlay.State.Sword;
                PlayerManager.instance.spadeBar.fillAmount = 1f;
                DialogueManager.instance.OnDialogue(DialogueText.instance.tutorialSentence3, DialogueText.instance.tutorialSceneName3, DialogueText.instance.tutorialImage3);
                collision.gameObject.SetActive(false);
            }

            if (collision.name == "Potal1")
            {
                CamCtrl.instance.state = CamCtrl.CameraState.backGround2;
                transform.position = bg2Pos.position;
                GameState.instance.state = GameState.State.tutorial2;
                DialogueManager.instance.OnDialogue(DialogueText.instance.tutorialSentence2, DialogueText.instance.tutorialSceneName2, DialogueText.instance.tutorialImage2);
            }
            else if (collision.name == "Potal2")
            {
                GameState.instance.AniFalse();
                GameState.instance.thePlayerCtrl.enabled = false;
                GameState.instance.theSkillPlay.enabled = false;
                SoundManager.instance.bgmAudioSource.clip = SoundManager.instance.villageNormalClip;
                SoundManager.instance.bgmAudioSource.Play();
                CamCtrl.instance.state = CamCtrl.CameraState.backGround3;
                transform.position = bg3Pos.position;
                GameState.instance.state = GameState.State.villageStart;
                DialogueManager.instance.OnDialogue(DialogueText.instance.villageSentence1, DialogueText.instance.villageSceneName1, DialogueText.instance.villageImage1);
            }
            else if (collision.name == "Potal3")
            {
                GameState.instance.AniFalse();
                GameState.instance.thePlayerCtrl.enabled = false;
                GameState.instance.theSkillPlay.enabled = false;
                CamCtrl.instance.state = CamCtrl.CameraState.backGround4;
                transform.position = bg4Pos.position;
                SoundManager.instance.bgmAudioSource.clip = SoundManager.instance.villageBattleClip;
                SoundManager.instance.bgmAudioSource.Play();
                GameState.instance.state = GameState.State.villageBattleStart;
                GameState.instance.VillageBattle1Director.SetActive(true);
                GameState.instance.enemyhpBar = true;
            }
            else if (collision.name == "Potal4")
            {
                GameState.instance.AniFalse();
                GameState.instance.thePlayerCtrl.enabled = false;
                GameState.instance.theSkillPlay.enabled = false;
                CamCtrl.instance.state = CamCtrl.CameraState.backGround5;
                transform.position = bg5Pos.position;
                //적 소환 
                GameState.instance.newDiamondEnemy = Instantiate(GameState.instance.diamondEnemy, new Vector3(606.2395f, -6.6f, 0), Quaternion.identity);
                GameState.instance.newDiamondEnemy = Instantiate(GameState.instance.diamondEnemy, new Vector3(622.4f, 7.5f, 0), Quaternion.identity);
                GameState.instance.newSpadeEnemy = Instantiate(GameState.instance.spadeEnemy, new Vector3(612.2f, -6.6f, 0), Quaternion.identity);
                GameState.instance.newSpadeEnemy = Instantiate(GameState.instance.spadeEnemy, new Vector3(650.1f, 7.5f, 0), Quaternion.identity);
                GameState.instance.newCloverEnemy = Instantiate(GameState.instance.cloverEnemy, new Vector3(616.6f, -6.6f, 0), Quaternion.identity);
                GameState.instance.newCloverEnemy = Instantiate(GameState.instance.cloverEnemy, new Vector3(632.7f, 7.5f, 0), Quaternion.identity);
                GameState.instance.newHealEnemy = Instantiate(GameState.instance.healEnemy, new Vector3(627.5f, 7.5f, 0), Quaternion.identity);
                DialogueManager.instance.OnDialogue(DialogueText.instance.villageSentence3, DialogueText.instance.villageSceneName3, DialogueText.instance.villageImage3);
                GameState.instance.state = GameState.State.villageBattle3Dia;
            }
            else if (collision.name == "Potal5")
            {
                GameState.instance.enemyCount = 0;
                CamCtrl.instance.state = CamCtrl.CameraState.backGround6;
                transform.position = bg6Pos.position;
                //적 소환 
                GameState.instance.newDiamondEnemy = Instantiate(GameState.instance.diamondEnemy, new Vector3(760.199f, -7.08f, 0), Quaternion.identity);
                GameState.instance.newDiamondEnemy = Instantiate(GameState.instance.diamondEnemy, new Vector3(768.16f, 6.28f, 0), Quaternion.identity);
                GameState.instance.newSpadeEnemy = Instantiate(GameState.instance.spadeEnemy, new Vector3(764.1f, -7.08f, 0), Quaternion.identity);
                GameState.instance.newSpadeEnemy = Instantiate(GameState.instance.spadeEnemy, new Vector3(771.63f, 6.28f, 0), Quaternion.identity);
                GameState.instance.newSpadeEnemy = Instantiate(GameState.instance.spadeEnemy, new Vector3(786.5f, -0.47f, 0), Quaternion.identity);
                GameState.instance.newSpadeEnemy = Instantiate(GameState.instance.spadeEnemy, new Vector3(812.3f, 5.7f, 0), Quaternion.identity);
                GameState.instance.newCloverEnemy = Instantiate(GameState.instance.cloverEnemy, new Vector3(775.69f, 6.28f, 0), Quaternion.identity);
                GameState.instance.newCloverEnemy = Instantiate(GameState.instance.cloverEnemy, new Vector3(790.69f, -0.47f, 0), Quaternion.identity);
                GameState.instance.newCloverEnemy = Instantiate(GameState.instance.cloverEnemy, new Vector3(817.2f, 5.7f, 0), Quaternion.identity);
                GameState.instance.newHealEnemy = Instantiate(GameState.instance.healEnemy, new Vector3(767.96f, -7.08f, 0), Quaternion.identity);
                GameState.instance.state = GameState.State.villageBattle4;
            }
            else if (collision.name == "Potal6")
            {
                GameState.instance.enemyCount = 0;
                CamCtrl.instance.state = CamCtrl.CameraState.backGround7;
                transform.position = bg7Pos.position;
                SoundManager.instance.bgmAudioSource.clip = SoundManager.instance.castleEntClip;
                SoundManager.instance.bgmAudioSource.Play();
                GameState.instance.enemyhpBar = true;
                GameState.instance.AniFalse();
                GameState.instance.thePlayerCtrl.enabled = false;
                GameState.instance.theSkillPlay.enabled = false;
                GameState.instance.CastleEntDirector.SetActive(true);
                GameState.instance.newDiamondEnemy = Instantiate(GameState.instance.diamondEnemy, new Vector3(906.989f, -0.2181759f, 0), Quaternion.identity);
                GameState.instance.newDiamondEnemy = Instantiate(GameState.instance.diamondEnemy, new Vector3(921.8f, 3.8f, 0), Quaternion.identity);
                GameState.instance.newSpadeEnemy = Instantiate(GameState.instance.spadeEnemy, new Vector3(912.7f, -0.2181759f, 0), Quaternion.identity);
                GameState.instance.newSpadeEnemy = Instantiate(GameState.instance.spadeEnemy, new Vector3(933.6f, -1.1f, 0), Quaternion.identity);
                GameState.instance.newSpadeEnemy = Instantiate(GameState.instance.spadeEnemy, new Vector3(952.3f, 0.7f, 0), Quaternion.identity);
                GameState.instance.newCloverEnemy = Instantiate(GameState.instance.cloverEnemy, new Vector3(938.8f, -1.1f, 0), Quaternion.identity);
                GameState.instance.newCloverEnemy = Instantiate(GameState.instance.cloverEnemy, new Vector3(957.9f, 0.7f, 0), Quaternion.identity);
                GameState.instance.newCloverEnemy = Instantiate(GameState.instance.cloverEnemy, new Vector3(956.6f, 8.8f, 0), Quaternion.identity);
                GameState.instance.newHealEnemy = Instantiate(GameState.instance.healEnemy, new Vector3(927.5f, 3.8f, 0), Quaternion.identity);
                GameState.instance.newHealEnemy = Instantiate(GameState.instance.healEnemy, new Vector3(961.8f, 8.8f, 0), Quaternion.identity);
                GameState.instance.state = GameState.State.villageBattle6;
            }
            else if (collision.name == "Potal7")
            {
                castleEntEnemy.SetActive(true);
                GameState.instance.enemyCount = 0;
                CamCtrl.instance.state = CamCtrl.CameraState.backGround8;
                SoundManager.instance.bgmAudioSource.clip = SoundManager.instance.castleRoadClip;
                SoundManager.instance.bgmAudioSource.Play();
                transform.position = bg8Pos.position;
                GameState.instance.enemyhpBar = true;
                GameState.instance.AniFalse();
                GameState.instance.thePlayerCtrl.enabled = false;
                GameState.instance.theSkillPlay.enabled = false;
                DialogueManager.instance.OnDialogue(DialogueText.instance.castleEntSentence2, DialogueText.instance.castleEntName2, DialogueText.instance.castleEntImage2);
                GameState.instance.state = GameState.State.castleEnt;
            }
        }
    }
}
