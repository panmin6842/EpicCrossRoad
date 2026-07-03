using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    public float hp;

    PlayerCtrl thePlayerCtrl;
    SlotManager theSlotManager;

    Animator ani;

    // Start is called before the first frame update
    private void OnEnable()
    {
        hp = 50;
        thePlayerCtrl = FindObjectOfType<PlayerCtrl>();
        theSlotManager = FindObjectOfType<SlotManager>();

        ani = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0)
        {
            thePlayerCtrl.shield = false;
            theSlotManager.sameItemGet = false;
            theSlotManager.arrowAni.SetBool("UpExit", true);
            theSlotManager.arrowAni.SetBool("Down", false);
            theSlotManager.arrowAni.SetBool("Up", false);
            ani.SetBool("exit", true);
            if (ani.GetCurrentAnimatorStateInfo(0).IsName("ShieldExit") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                theSlotManager.arrowAni.SetBool("UpExit", true);
                theSlotManager.arrowAni.SetBool("Down", false);
                theSlotManager.arrowAni.SetBool("Up", false);
                this.gameObject.SetActive(false);
            }
            
        }
    }
}
