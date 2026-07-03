using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PattarnItem : MonoBehaviour
{
    [SerializeField] GameObject spadeItem;
    [SerializeField] GameObject heartItem;
    [SerializeField] GameObject cloverItem;
    [SerializeField] GameObject diamondItem;

    [SerializeField] Transform patternAppearZone;

    GameObject newSpadeItem;
    GameObject newHeartItem;
    GameObject newCloverItem;
    GameObject newDiamondItem;

    int random;

    void OnEnable()
    {
        int random = Random.Range(0, 4);

        if(random == 0)
        {
            newSpadeItem = Instantiate(spadeItem, patternAppearZone.position, Quaternion.identity);
        }
        else if (random == 2)
        {
            newHeartItem = Instantiate(heartItem, patternAppearZone.position, Quaternion.identity);
        }
        else if (random == 1)
        {
            newCloverItem = Instantiate(cloverItem, patternAppearZone.position, Quaternion.identity);
        }
        else if (random == 3)
        {
            newDiamondItem = Instantiate(diamondItem, patternAppearZone.position, Quaternion.identity);
        }
    }
}
