using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    public Image[] slots;
    //public GameObject[] arrows;
    public Animator arrowAni;
    public GameObject hpBarBackGround;
    int count;
    Sprite temp;

    int random;
    float time;
    float skillFalseTime;

    public bool noItemGet;
    public bool sameItemGet;

    public Sprite[] patternSprite;

    SkillPlay theSkillPlay;

    private void Start()
    {
        theSkillPlay = FindObjectOfType<SkillPlay>();

        

        noItemGet = false;
        sameItemGet = false;
        time = 0;
        skillFalseTime = 0;
    }

    private void Update()
    {
        if(noItemGet)
        {
            if (time > 10)
            {
                time = 0;
                //arrows[0].SetActive(true);
                //arrows[1].SetActive(false);
                //arrows[2].SetActive(false);
                hpBarBackGround.SetActive(false);
                arrowAni.SetBool("Up", false);
                arrowAni.SetBool("Down", false);
                noItemGet = false;
            }
            else
                time += Time.deltaTime;
        }
    }

    public void SlotChange(Sprite pattern, Animator playerAni, GameObject profile, GameObject normalProfile)
    {
        if (slots[0].sprite == null)
        {
            //slots[0].color = new Color(255, 255, 255, 255);
        }
        if (slots[1].sprite == null && slots[0].sprite != null)
        {
            slots[1].color = new Color(255, 255, 255, 255);
        }

        temp = slots[0].sprite;
        slots[1].sprite = temp;
        slots[0].sprite = pattern;

        arrowAni.SetBool("UpExit", false);

        if (slots[0].sprite != null && slots[1].sprite != null)
        {
            if (slots[0].sprite.name == slots[1].sprite.name)
            {
                random = Random.Range(0, 10);
                if (random >= 0 && random < 5) //정방향
                {
                    sameItemGet = true;
                    slots[0].sprite = null;
                    slots[1].sprite = null;
                    slots[0].color = new Color(0, 0, 0, 0);
                    slots[1].color = new Color(0, 0, 0, 0);
                    //arrows[0].SetActive(false);
                    //arrows[1].SetActive(true);
                    //arrows[2].SetActive(false);
                    arrowAni.SetBool("Up", true);
                    arrowAni.SetBool("Down", false);
                }
                else if (random >= 5 && random < 10) //역방향
                {
                    //arrows[0].SetActive(false);
                    //arrows[1].SetActive(false);
                    //arrows[2].SetActive(true);
                    hpBarBackGround.SetActive(true);
                    arrowAni.SetBool("Up", false);
                    arrowAni.SetBool("Down", true);
                    profile.SetActive(false);
                    normalProfile.SetActive(true);
                    playerAni.ResetTrigger("Sword");
                    playerAni.ResetTrigger("Spear");
                    playerAni.ResetTrigger("Stamp");
                    GameState.instance.theSkillPlay.diamondModeCut = true;
                    playerAni.SetTrigger("Gauntlet");
                    GameState.instance.theSkillPlay.state = SkillPlay.State.Gauntlet;
                    slots[0].sprite = null;
                    slots[1].sprite = null;
                    slots[0].color = new Color(0, 0, 0, 0);
                    slots[1].color = new Color(0, 0, 0, 0);
                    noItemGet = true;
                }
            }
        }
    }

    public void SlotReSet()
    {
        slots[0].sprite = null;
        slots[1].sprite = null;
        slots[0].color = new Color(0, 0, 0, 0);
        slots[1].color = new Color(0, 0, 0, 0);
        arrowAni.SetBool("Up", false);
        arrowAni.SetBool("Down", false);
        noItemGet = false;
        hpBarBackGround.SetActive(false);
    }
}
