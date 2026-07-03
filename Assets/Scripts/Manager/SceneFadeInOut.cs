using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeInOut : MonoBehaviour
{
    [SerializeField] GameObject blackImage;
    Image image; //페이드 인 아웃에 쓰일 이미지

    bool fadeIn;
    Color color;

    float time;

    // Start is called before the first frame update
    private void OnEnable()
    {
        image = blackImage.GetComponent<Image>();

        blackImage.SetActive(true);
        fadeIn = false;
        color = image.color;

        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!fadeIn) //검정 화면이 나올 때 까지
        {
            if (color.a >= 1)
                fadeIn = true;
            else
            {
                time += (0.01f * Time.deltaTime);
                color.a += time;
                image.color = color;
            }
        }
        else if (fadeIn)
        {
            if (color.a <= 0)
            {
                blackImage.SetActive(false);
                gameObject.GetComponent<SceneFadeInOut>().enabled = false;
            }
            else
            {
                Invoke("FadeOut", 6);
            }
        }
    }

    void FadeOut()
    {
        time += (0.001f * Time.deltaTime);
        color.a -= time;
        image.color = color;
    }
}
