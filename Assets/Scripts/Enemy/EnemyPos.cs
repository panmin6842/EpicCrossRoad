using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPos : MonoBehaviour
{
    Animator ani;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, -8.61f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, -8.61f, transform.position.z);
    }
}
