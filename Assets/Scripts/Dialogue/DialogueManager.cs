using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public float typingSpeed;

    public Queue<string> sentences; //대화 문자열
    Queue<string> nameWord;
    Queue<GameObject> faceImage;
    public string curSentence; //현재 문장
    string curName;
    GameObject curFaceImage;

    public bool quit;
    public bool start;

    public bool noFast;

    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject name;
    [SerializeField] GameObject text;
    public GameObject skipButton;

    public GameObject profile;
    public GameObject slot;
    [SerializeField] Transform playerImage;
    [SerializeField] Transform otherImage;

    public TextMeshProUGUI sentenceText;
    [SerializeField] TextMeshProUGUI nameText;

    static public DialogueManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        sentences = new Queue<string>();
        nameWord = new Queue<string>();
        faceImage = new Queue<GameObject>();

        typingSpeed = 0.1f;
        quit = false;
        start = false;
        noFast = false;
    }

    public void OnDialogue(string[] lines, string[] names, GameObject[] images) //대화 시작
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        quit = false;
        start = true;
        typingSpeed = 0.1f;
        curSentence = string.Empty;
        sentenceText.text = string.Empty;
        curName = "";
        sentences.Clear(); //문장 초기화
        nameWord.Clear();
        faceImage.Clear();

        foreach (string line in lines) //다른 스크립트에서 넣은 문자열들을 sentences에 넣어줌
        {
            sentences.Enqueue(line);
        }

        foreach(string name in names)
        {
            nameWord.Enqueue(name);
        }

        foreach(GameObject image in images)
        {
            faceImage.Enqueue(image);
        }

        profile.SetActive(false);
        slot.SetActive(false);

        dialogueBox.SetActive(true);
        name.SetActive(true);
        text.SetActive(true);
        skipButton.SetActive(true);

        NextSentence();
    }

    private void Update()
    {
        if (sentenceText.text.Equals(curSentence)) //대사 하나가 끝나면
        {
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Space)) && start == true)
            {
                curFaceImage.SetActive(false);
                NextSentence(); //다음 대사 나옴
            }
        }

        if(quit == true)
        {
            start = false;
            dialogueBox.SetActive(false);
            name.SetActive(false);
            text.SetActive(false);
            skipButton.SetActive(false);
            curFaceImage.SetActive(false);
            profile.SetActive(true);
            slot.SetActive(true);
        }

        TextSpeed();
    }

    public void NextSentence() //문자열 출력
    {
        if(sentences.Count != 0) //대사가 있음
        {
            curSentence = sentences.Dequeue(); //sentences에서 문자열을 꺼내 현재 문자열로 만듦
            curName = nameWord.Dequeue();
            curFaceImage = faceImage.Dequeue();

            if(curName == "디플로")
            {
                curFaceImage.transform.position = playerImage.transform.position;
            }
            else
            {
                curFaceImage.transform.position = otherImage.transform.position;
            }
            curFaceImage.SetActive(true);

            StartCoroutine(Typing(curSentence, curName));
        }
        else //대사 없음 즉 대사 끝남
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            quit = true;
        }
    }

    IEnumerator Typing(string line, string name) //글자 하나하나씩 나오게함
    {
        sentenceText.text = string.Empty; //글자 초기화
        nameText.text = name;

        foreach(char letter in line.ToCharArray()) //글자를 한글자씩 뽑음
        {
            sentenceText.text += letter;

            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void TextSpeed()
    {
        if(Input.GetKey(KeyCode.Space) && quit == false && !noFast)
        {
            typingSpeed = 0.01f;
        }

        if (Input.GetKeyUp(KeyCode.Space) && quit == false)
        {
            typingSpeed = 0.1f;
        }
    }

    public void SkipButton()
    {
        StopAllCoroutines();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        quit = true;
    }
}
