using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    BoxCollider2D boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.name == "FootCollider")
    //    {
    //        boxCollider.enabled = true;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.name == "FootCollider")
    //    {
    //        boxCollider.enabled = false;
    //    }
    //}
}
