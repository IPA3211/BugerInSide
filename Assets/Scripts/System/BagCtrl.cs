using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BagCtrl : MonoBehaviour
{
    public bool isOpened = false;
    public List<addItemInfo> items;
    public List<addEquipInfo> equips;
    public int gold;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void openBagAndGiveItem(){
        giveItem();
        openBag();
    }

    public void openBag(){
        isOpened = true;
        animator.SetBool("isOpen", true);
    }

    void giveItem(){
        if(!isOpened)
            GameSystem.instance.give(items, equips, gold);
    }
}
