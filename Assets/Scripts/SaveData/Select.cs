using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Select : MonoBehaviour
{
    [SerializeField]
    Transform playerTr; //플레이어 위치
    [SerializeField]
    Text[] slotText;

    bool[] savefile = new bool[2]; //세이브 파일 존재유무 저장
    
    // Start is called before the first frame update
    void Start()
    {
        //슬롯별로 저장된 데이터가 존재하는지 판단
        for (int i = 0; i < 2; i++)
        {
            if(File.Exists(DataManager.instance.path + $"{i}")) //슬롯에 데이터가 있는 경우 Exists는 지정된 파일이 있는지 없는지 확인함
            {
                savefile[i] = true; //해당 슬롯은 있다는 것을 확인하기 위해 true
                DataManager.instance.nowSlot = i; //선택한 슬롯 번호 저장
                DataManager.instance.LoadData(); //해당 슬롯 데이터 불러옴
                slotText[i].text = "저장완료"; //슬롯에 저장완료 표시
            }
            else
            {
                slotText[i].text = "비어있음";
            }
        }
    }

    public void Slot(int number) //슬롯 기능
    {
        DataManager.instance.nowSlot = number; //슬롯의 번호를 슬롯번호로 입력함

        if(savefile[number]) //bool 배열에서 현재 슬롯번호가 true라면 데이터가 존재한다는 뜻
        {
            DataManager.instance.LoadData(); //데이터를 로드
            //GoGame();
        }

    }

    public void GoGame()
    {
        if (!savefile[DataManager.instance.nowSlot]) //현재 슬롯번호의 데이터가 없다면
        {
            DataManager.instance.nowPlayer.x = playerTr.position.x; //플레이어 위치 저장
            DataManager.instance.nowPlayer.y = playerTr.position.y;
            DataManager.instance.SaveData(); //현재 정보를 저장
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
