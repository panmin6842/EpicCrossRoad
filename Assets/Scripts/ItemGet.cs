using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGet : MonoBehaviour
{
    Transform malePlayer;

    public float dist;
    public bool isGet;
    float time;

    static public ItemGet instance;

    private void Awake()
    {
        instance = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        malePlayer = GameObject.Find("MalePlayer").GetComponent<Transform>();
        isGet = false;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector2.Distance(malePlayer.position, transform.position);

        if(dist <= 2f)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                isGet = true;
            }
        }

        if(isGet)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(malePlayer.position.x, transform.position.y, transform.position.z), Time.deltaTime * 10);
            time += Time.deltaTime;
        }

        if (time >= 0.5f) //먹히지 않았는데 계속 움직이면 움직이지 않게 해야함
        {
            isGet = false;
            time = 0;
        }
    }

    
}
