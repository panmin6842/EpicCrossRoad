using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    [SerializeField] Transform patternZone;
    [SerializeField] Transform enemyZone;

    [SerializeField] GameObject cloverEnemy;
    [SerializeField] GameObject diamondEnemy;
    [SerializeField] GameObject spadeEnemy;
    [SerializeField] GameObject heartEnemy;

    [SerializeField] GameObject cloverItem;
    [SerializeField] GameObject spadeItem;
    [SerializeField] GameObject heartItem;
    [SerializeField] GameObject diamondItem;

    GameObject newCloverEnemy;
    GameObject newDiamondEnemy;
    GameObject newSpadeEnemy;
    GameObject newHeartEnemy;

    GameObject newClvoerItem;
    GameObject newSpadeItem;
    GameObject newHeartItem;
    GameObject newDiamondItem;

    [SerializeField] Text enemyCountText;

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    PlayerManager.instance.hp = PlayerManager.instance.maxHp;
        //    PlayerManager.instance.hpBar.fillAmount = PlayerManager.instance.hp / PlayerManager.instance.maxHp;
        //}
        //if (Input.GetKeyDown(KeyCode.Q))
        //    newCloverEnemy = Instantiate(cloverEnemy, enemyZone.position, Quaternion.identity);
        //if (Input.GetKeyDown(KeyCode.W))
        //    newDiamondEnemy = Instantiate(diamondEnemy, enemyZone.position, Quaternion.identity);
        //if (Input.GetKeyDown(KeyCode.E))
        //    newSpadeEnemy = Instantiate(spadeEnemy, enemyZone.position, Quaternion.identity);
        //if (Input.GetKeyDown(KeyCode.R))
        //    newHeartEnemy = Instantiate(heartEnemy, enemyZone.position, Quaternion.identity);
        //if (Input.GetKeyDown(KeyCode.A))
        //    newClvoerItem = Instantiate(cloverItem, patternZone.position, Quaternion.identity);
        //if (Input.GetKeyDown(KeyCode.S))
        //    newSpadeItem = Instantiate(spadeItem, patternZone.position, Quaternion.identity);
        //if (Input.GetKeyDown(KeyCode.D))
        //    newHeartItem = Instantiate(heartItem, patternZone.position, Quaternion.identity);
        //if (Input.GetKeyDown(KeyCode.F))
        //    newDiamondItem = Instantiate(diamondItem, patternZone.position, Quaternion.identity);

        enemyCountText.text = GameState.instance.enemyCount.ToString();
    }
}
