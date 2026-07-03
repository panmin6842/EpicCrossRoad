using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamCtrl : MonoBehaviour
{
    public Transform playerTr;

    float cameraSpeed;
    float height;
    float width;

    public bool play;

    Vector3 cameraPos = new Vector3(0, 10, -10); //카메라 깊이(현재 테스트 설정)
    public Vector2 mapSize; //맵 사이즈
    public Vector2 center; //맵 센터

    public CinemachineVirtualCamera camera;
    public PolygonCollider2D[] cameraZoneCollider;
    CinemachineConfiner2D cameraCollider;

    public enum CameraState
    {
        backGround1,
        backGround2,
        backGround3,
        backGround4,
        backGround5,
        backGround6,
        backGround7,
        backGround8,
    }

    public CameraState state;

    static public CamCtrl instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerTr = GameObject.Find("MalePlayer").GetComponent<Transform>();
        cameraCollider = camera.GetComponent<CinemachineConfiner2D>();

        height = Camera.main.orthographicSize; //카메라가 비추는 영역의 세로 크기 절반
        width = height * (Screen.width / Screen.height); //Screen.width, Screen.height는 게임의 실제 가로 세로 크기의 픽셀 값을 반환해주는 변수 카메라가 비추는 영역의 가로 크기 절반
        //(Screen.width / Screen.height)는 orthographicSize 가로 구하기 위해서
        cameraSpeed = 10f;

        state = CameraState.backGround1;
        play = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (play == true)
        {
            BackGroundCameraZone();
        }
    }

    void BackGroundCameraZone()
    {
        switch (state)
        {
            case CameraState.backGround1:
                {
                    cameraCollider.m_BoundingShape2D = cameraZoneCollider[0];
                }
                break;
            case CameraState.backGround2:
                {
                    cameraCollider.m_BoundingShape2D = cameraZoneCollider[1];
                }
                break;
            case CameraState.backGround3:
                {
                    cameraCollider.m_BoundingShape2D = cameraZoneCollider[2];
                }
                break;
            case CameraState.backGround4:
                {
                    cameraCollider.m_BoundingShape2D = cameraZoneCollider[3];
                }
                break;
            case CameraState.backGround5:
                {
                    cameraCollider.m_BoundingShape2D = cameraZoneCollider[4];
                }
                break;
            case CameraState.backGround6:
                {
                    cameraCollider.m_BoundingShape2D = cameraZoneCollider[5];
                }
                break;
            case CameraState.backGround7:
                {
                    cameraCollider.m_BoundingShape2D = cameraZoneCollider[6];
                }
                break;
            case CameraState.backGround8:
                {
                    cameraCollider.m_BoundingShape2D = cameraZoneCollider[7];
                }
                break;
        }
    }

    void CameraLimit()
    {
        if (playerTr.gameObject != null) //플레이어가 존재할 경우 따라다니게 함
        {
            transform.position = Vector3.Lerp(transform.position, playerTr.position + cameraPos, Time.deltaTime * cameraSpeed); //자연스럽게 따라가도록

            float lx = mapSize.x - width; //맵사이즈의 가로 절반크기를 구함
            float clampX = Mathf.Clamp(transform.position.x, center.x - lx, lx + center.x); //카메라 움직임 지정 범위(가로)

            float ly = mapSize.y - height; //맵사이즈의 세로 절반크기를 구함
            float clampY = Mathf.Clamp(transform.position.y, center.y - ly, ly + center.y); //카메라 움직임 지정 범위(세로)

            transform.position = new Vector3(clampX, clampY, -10f);
        }
    }
}
