using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreePhasePatternAppear : MonoBehaviour
{
    public GameObject diamond;
    public GameObject clover;
    public GameObject spade;
    public GameObject heart;

    public bool diamondAppear = false;
    public bool cloverAppear = false;
    public bool spadeAppear = false;
    public bool heartAppear = false;

    void OnEnable()
    {
        int random = Random.Range(0, 4);

        if (random == 0)
        {
            diamond.SetActive(true);
            diamondAppear = true;
        }
        else if (random == 2)
        {
            clover.SetActive(true);
            cloverAppear = true;
        }
        else if (random == 1)
        {
            spade.SetActive(true);
            spadeAppear = true;
        }
        else if (random == 3)
        {
            heart.SetActive(true);
            heartAppear = true;
        }
    }
}
