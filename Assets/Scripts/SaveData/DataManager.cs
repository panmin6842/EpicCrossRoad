using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO; //외부폴더로 접근

// 저장하는 방법
// 1. 저장할 데이터가 존재해야함
// 2. 데이터를 제이슨으로 변환해야함
// 3. 제이슨을 외부에 저장해야함

// 불러오는 방법
// 1. 외부에 저장된 제이슨을 가져옴
// 2. 제이슨을 데이터 형태로 변환함
// 3. 불러온 데이터를 이용함

//저장할 데이터

public class PlayerData
{
    public float x = -3.22f;
    public float y = -10.61f;
}

public class DataManager : MonoBehaviour
{

    static public DataManager instance; //싱글톤

    public PlayerData nowPlayer = new PlayerData(); //생성

    public string path; //경로
    public int nowSlot; //현재 슬롯 번호
    string filename = "save";

    private void Awake()
    {
        if (instance == null) //같은 스크립트가 없으면 생성해줌
        {
            instance = this;
        }
        else if (instance != this) //같은 스크립트가 있으면 없애줘서 중복되지 않도록함
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject); //씬을 이동해도 오브젝트가 파괴되지 않도록 해줌

        path = Application.persistentDataPath + "/"; //경로설정
        print(path);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(nowPlayer); //Json으로 변환
        File.WriteAllText(path + filename, data);
    }

    public void LoadData() //데이터 불러오기
    {
        string data = File.ReadAllText(path + filename);
        nowPlayer = JsonUtility.FromJson<PlayerData>(data);
        SceneManager.LoadScene("MainScene");
    }

    public void DataClear()
    {
        nowSlot = -1;
        nowPlayer = new PlayerData();
    }

    public void GameStart()
    {
        SceneManager.LoadScene("MainScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
