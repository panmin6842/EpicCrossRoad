using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueText : MonoBehaviour
{
    public string[] tutorialSentence1;
    public string[] tutorialSceneName1;
    public GameObject[] tutorialImage1;

    public string[] tutorialSentence2;
    public string[] tutorialSceneName2;
    public GameObject[] tutorialImage2;

    public string[] tutorialSentence3;
    public string[] tutorialSceneName3;
    public GameObject[] tutorialImage3;

    public string[] tutorialSentence4;
    public string[] tutorialSceneName4;
    public GameObject[] tutorialImage4;

    public string[] villageSentence1;
    public string[] villageSceneName1;
    public GameObject[] villageImage1;

    public string[] npc1Sentence;
    public string[] npc1SceneName;
    public GameObject[] npc1Image;

    public string[] npc2Sentence;
    public string[] npc2SceneName;
    public GameObject[] npc2Image;

    public string[] npc3Sentence;
    public string[] npc3SceneName;
    public GameObject[] npc3Image;

    public string[] npc4Sentence;
    public string[] npc4SceneName;
    public GameObject[] npc4Image;

    public string[] villageSentence2;
    public string[] villageSceneName2;
    public GameObject[] villageImage2;

    public string[] villageSentence3;
    public string[] villageSceneName3;
    public GameObject[] villageImage3;

    public string[] castleEntSentence1;
    public string[] castleEntName1;
    public GameObject[] castleEntImage1;

    public string[] castleEntSentence2;
    public string[] castleEntName2;
    public GameObject[] castleEntImage2;

    public string[] castleEntSentence3;
    public string[] castleEntName3;
    public GameObject[] castleEntImage3;

    public string[] bossSentence1;
    public string[] bossName1;
    public GameObject[] bossImage1;

    public string[] bossSentence2;
    public string[] bossName2;
    public GameObject[] bossImage2;

    static public DialogueText instance;

    private void Awake()
    {
        instance = this;
    }
}
