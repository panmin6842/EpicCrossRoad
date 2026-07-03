using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GauntletDiamondEnemyAttack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "DiamondAttack") //적 공격 범위에 닿으면
        {
            PlayerCtrl.instance.PatternStateHit(20, 10, 5, 10);
        }
    }
}
