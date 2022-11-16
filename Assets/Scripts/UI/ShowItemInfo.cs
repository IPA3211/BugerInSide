using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class ShowItemInfo : MonoBehaviour
{
    ItemData onFocuseItem;
    public GameObject itemHolder;
    public GameObject statHolder;
    public GameObject itemBtn;
    public GameObject statBtn;
    public TMP_Text describe;
    public void setUniqueData()
    {
        if(itemHolder.transform.childCount != 0){
            var btns = itemHolder.GetComponentsInChildren<Transform>();

            for(int i = 1; i < btns.Length; i++){
                Destroy(btns[i].gameObject);
            }

            itemHolder.transform.DetachChildren();
        }

        var sortedItem = GameSystem.instance._ItemsInInventory.OrderBy(x => x.Key.sortingNum);
        
        foreach(var itemInven in sortedItem){
            var item = itemInven.Key;
            var newObj = Instantiate(itemBtn, itemHolder.transform);
            var mbd = newObj.GetComponent<MenuBtnDynamic>();
            mbd.text.text = item.title;
            mbd.setImage(item.describeImage);
            mbd.setNumber(itemInven.Value + "");
            mbd.onFocusOneShot.AddListener((o) => {
                describe.text = item.title + "\n"
                    + item.describe;
            });
            mbd.onClickBtn.AddListener((o) => {
                onFocuseItem = item;
                if(onFocuseItem.canUseInField){
                    statHolder.GetComponent<MenuObj>().changeToNextMenu();
                }
            });
        }
        
        itemHolder.GetComponent<MenuObj>().renewBtns();

        if(statHolder.transform.childCount != 0){
            var btns = statHolder.GetComponentsInChildren<Transform>();

            for(int i = 1; i < btns.Length; i++){
                Destroy(btns[i].gameObject);
            }

            statHolder.transform.DetachChildren();
        }

        foreach(var character in GameSystem.instance._Characters){
            var newObj = Instantiate(statBtn, statHolder.transform);
            var mbd = newObj.GetComponent<MenuBtn>();

            newObj.GetComponent<MenuSelectBtn>().setData(character);

            mbd.onClickBtn.AddListener((o) => {
                if(onFocuseItem.isUseAble(character)){
                    onFocuseItem.useItem(character);
                    GameSystem.instance.useItem(onFocuseItem, 1);
                    if(!GameSystem.instance._ItemsInInventory.ContainsKey(onFocuseItem)){
                        statHolder.GetComponent<MenuObj>().changeToPrevMenu();
                    }
                    setUniqueData();
                }
            });
        }
        
        statHolder.GetComponent<MenuObj>().renewBtns();
    }
}
