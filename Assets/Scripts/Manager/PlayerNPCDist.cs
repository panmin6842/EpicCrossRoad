using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNPCDist : MonoBehaviour
{
    [SerializeField] Transform npc1;
    [SerializeField] Transform npc2;
    [SerializeField] Transform npc3;
    [SerializeField] Transform npc4;

    float dist1;
    float dist2;
    float dist3;
    float dist4;

    int count;
    public bool diaStart;

    [SerializeField] GameObject potal3;
    [SerializeField] GameObject[] npcKeyInfo;

    public static PlayerNPCDist instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        diaStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        dist1 = Vector2.Distance(transform.position, npc1.position);
        dist2 = Vector2.Distance(transform.position, npc2.position);
        dist3 = Vector2.Distance(transform.position, npc3.position);
        dist4 = Vector2.Distance(transform.position, npc4.position);

        if(dist1 < 5 && Input.GetKeyDown(KeyCode.X) && !diaStart)
        {
            DialogueManager.instance.OnDialogue(DialogueText.instance.npc1Sentence, DialogueText.instance.npc1SceneName, DialogueText.instance.npc1Image);
            GameState.instance.AniFalse();
            GameState.instance.thePlayerCtrl.enabled = false;
            GameState.instance.theSkillPlay.enabled = false;
            count++;
            diaStart = true;
        }
        else if (dist2 < 5 && Input.GetKeyDown(KeyCode.X) && !diaStart)
        {
            DialogueManager.instance.OnDialogue(DialogueText.instance.npc2Sentence, DialogueText.instance.npc2SceneName, DialogueText.instance.npc2Image);
            GameState.instance.AniFalse();
            GameState.instance.thePlayerCtrl.enabled = false;
            GameState.instance.theSkillPlay.enabled = false;
            count++;
            diaStart = true;
        }
        else if (dist3 < 5 && Input.GetKeyDown(KeyCode.X) && !diaStart)
        {
            DialogueManager.instance.OnDialogue(DialogueText.instance.npc3Sentence, DialogueText.instance.npc3SceneName, DialogueText.instance.npc3Image);
            GameState.instance.AniFalse();
            GameState.instance.thePlayerCtrl.enabled = false;
            GameState.instance.theSkillPlay.enabled = false;
            count++;
            diaStart = true;
        }
        else if (dist4 < 5 && Input.GetKeyDown(KeyCode.X) && count >= 3 && !diaStart)
        {
            diaStart = true;
            npcKeyInfo[3].SetActive(false);
            DialogueManager.instance.OnDialogue(DialogueText.instance.npc4Sentence, DialogueText.instance.npc4SceneName, DialogueText.instance.npc4Image);
            GameState.instance.AniFalse();
            GameState.instance.thePlayerCtrl.enabled = false;
            GameState.instance.theSkillPlay.enabled = false;
            potal3.SetActive(true);
            GameState.instance.arrows[1].SetActive(true);
        }

        if(count >= 3 && !diaStart)
        {
            npcKeyInfo[0].SetActive(false);
            npcKeyInfo[1].SetActive(false);
            npcKeyInfo[2].SetActive(false);
            npcKeyInfo[3].SetActive(true);
        }
    }
}
