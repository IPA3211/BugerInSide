using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopCtrl : MonoBehaviour
{
    public static ShopCtrl instance;
    public static List<ItemData> itemToSell;
    public static List<EquipData> equipToSell;
    [Header("Config")]
    public GameObject shop;
    public MenuObj shopMenuObj;
    public float sellDiscount = 0.75f;
    [Header("Text")]
    public TMP_Text goldText;
    public TMP_Text describeText;
    public TMP_Text buySellText;
    [Header("Holder")]
    public GameObject itemContainer;
    [Header("Prefabs")]
    public GameObject itemBtn;

    public void startShop(List<ItemData> items, List<EquipData> equips){
        PlayerStatics.isMenuOn = true;
        Time.timeScale = 0;
        shop.SetActive(true);
        shopMenuObj.startMenu();
        refreshGoldText();

        itemToSell = items;
        equipToSell = equips;
    }

    public void endShop(){
        PlayerStatics.isMenuOn = false;
        Time.timeScale = 1;
        shop.SetActive(false);
    }

    public void Start(){
        instance = this;
    }
    public void refreshGoldText(){
        goldText.text = GameSystem.instance.gold + "";
    }

    public void setSeller(){
        if(itemToSell.Count != 0){
            setItemSeller(itemToSell); 
        }
        else{
            setEquipSeller(equipToSell);
        }
    }

    public void setBuyer(){
        if(itemToSell.Count != 0){
            setSellItem(); 
        }
        else{
            setSellEquip();
        }
    }

    public void setItemSeller(List<ItemData> datas){
        clearContainer();
        foreach(var d in datas){
            var btn = Instantiate(itemBtn, itemContainer.transform);
            var mbd = btn.GetComponent<MenuBtnDynamic>();
            mbd.setText(d.title);
            mbd.setNumber(d.cost + "");
            mbd.setImage(d.describeImage);

            mbd.onFocusOneShot.AddListener((o) => {
                describeText.text = d.describe;
                buySellText.text = "구매가격 : " + d.cost + "G";
            });

            mbd.onClickBtn.AddListener((o) => {
                if(GameSystem.instance.gold >= d.cost){
                    GameSystem.instance.giveItem(d, 1);
                    GameSystem.instance.useGold(d.cost);
                    refreshGoldText();
                }
            });
        }
        itemContainer.GetComponent<MenuObj>().renewBtns();
    }

    public void setSellItem(){
        clearContainer();
        foreach(var d in GameSystem.instance._ItemsInInventory){
            var btn = Instantiate(itemBtn, itemContainer.transform);
            var mbd = btn.GetComponent<MenuBtnDynamic>();
            mbd.setText(d.Key.title);
            mbd.setNumber(d.Value + "");
            mbd.setImage(d.Key.describeImage);

            mbd.onFocusOneShot.AddListener((o) => {
                describeText.text = d.Key.describe;
                buySellText.text = "판매가격 : " + d.Key.cost * sellDiscount + "G";
            });

            mbd.onClickBtn.AddListener((o) => {
                if(GameSystem.instance._ItemsInInventory.ContainsKey(d.Key)){
                    GameSystem.instance.useItem(d.Key, 1);
                    GameSystem.instance.giveGold((int)(d.Key.cost * sellDiscount));
                    refreshGoldText();
                    if(!GameSystem.instance._ItemsInInventory.ContainsKey(d.Key)){
                        setSellItem();
                    }
                }
            });
        }
        itemContainer.GetComponent<MenuObj>().renewBtns();
    }
    

    public void setEquipSeller(List<EquipData> datas){
        clearContainer();
        foreach(var d in datas){
            var btn = Instantiate(itemBtn, itemContainer.transform);
            var mbd = btn.GetComponent<MenuBtnDynamic>();
            mbd.setText(d.title);
            mbd.setNumber(d.cost + "");
            mbd.setImage(d.describeImage);

            mbd.onFocusOneShot.AddListener((o) => {
                describeText.text = d.describe;
                buySellText.text = "구매가격 : " + d.cost + "G";
            });

            mbd.onClickBtn.AddListener((o) => {
                if(GameSystem.instance.gold >= d.cost){
                    GameSystem.instance.giveEquip(d, 1);
                    GameSystem.instance.useGold(d.cost);
                    refreshGoldText();
                }
            });
        }
        itemContainer.GetComponent<MenuObj>().renewBtns();
    }

    public void setSellEquip(){
        clearContainer();
        foreach(var d in GameSystem.instance._EquipsInInventory){
            var btn = Instantiate(itemBtn, itemContainer.transform);
            var mbd = btn.GetComponent<MenuBtnDynamic>();
            mbd.setText(d.Key.title);
            mbd.setText(d.Value + "");
            mbd.setImage(d.Key.describeImage);

            mbd.onFocusOneShot.AddListener((o) => {
                describeText.text = d.Key.describe;
                buySellText.text = "판매가격 : " + d.Key.cost * sellDiscount + "G";
            });

            mbd.onClickBtn.AddListener((o) => {
                if(GameSystem.instance._EquipsInInventory.ContainsKey(d.Key)){
                    if(GameSystem.instance._EquipsInInventory[d.Key] == 1){
                        Destroy(btn);
                        itemContainer.GetComponent<MenuObj>().renewBtns();
                    }
                    GameSystem.instance.useEquip(d.Key, 1);
                    GameSystem.instance.giveGold((int)(d.Key.cost * sellDiscount));
                    refreshGoldText();
                }
            });
        }
        itemContainer.GetComponent<MenuObj>().renewBtns();
    }

    public void clearContainer(){
        if(itemContainer.transform.childCount != 0){
            var btns = itemContainer.GetComponentsInChildren<Transform>();

            for(int i = 1; i < btns.Length; i++){
                Destroy(btns[i].gameObject);
            }

            itemContainer.transform.DetachChildren();
        }

    }
}
