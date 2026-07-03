using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingMove : MonoBehaviour
{
    public float speed;

    private float y;

    // Start is called before the first frame update
    void Start()
    {
        y = gameObject.GetComponent<RectTransform>().position.y;
    }

    // Update is called once per frame
    void Update()
    {
        y += speed;
        gameObject.GetComponent<RectTransform>().position = new Vector3(gameObject.GetComponent<RectTransform>().position.x, y, 0);

        if(gameObject.GetComponent<RectTransform>().position.y > 1792)
        {
            y = 1792;
            gameObject.GetComponent<RectTransform>().position = new Vector3(gameObject.GetComponent<RectTransform>().position.x, y, 0);
            Invoke("SceneLoad", 3);
        }
    }

    void SceneLoad()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
