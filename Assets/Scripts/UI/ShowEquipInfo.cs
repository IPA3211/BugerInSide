using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowEquipInfo : ShowMenuInfo
{
    private CharacterData onFocuse;
    private int curEquipNum;
    [Header("Equip Items")]
    public GameObject equipHolder;
    public GameObject equipBtn;
    [Header("Equip parts")]
    public GameObject equipPartHolder;
    public MenuBtnDynamic head;
    public MenuBtnDynamic weapon;
    public MenuBtnDynamic body;
    public MenuBtnDynamic shoes;
    [Header("describe")]
    public TMP_Text describe;
    public TMP_Text statDiff;
    
    public override void setUniqueData(CharacterData data)
    {
        onFocuse = data;
        setEquipPartText(data);
        //equipPartHolder.GetComponent<MenuObj>().changeToNextMenu();
    }

    public void setEquipPartText(CharacterData data){
        //head.text.text = ("머리 : " + (data.head != null ? data.head.title : "없 음"));
        weapon.setText("무기 : " + (data.weapon != null ? data.weapon.title : "없 음"));
        body.setText("갑옷 : " + (data.body != null ? data.body.title : "없 음"));
        //shoes.setText("신발 : " + (data.shoes != null ? data.shoes.title : "없 음"));
        
    }

    public void setEquips(int equipNum){
        curEquipNum = equipNum;
        EquipPos equipPos = EquipPos.HEAD;

        if(equipHolder.transform.childCount != 0){
            var btns = equipHolder.GetComponentsInChildren<Transform>();

            for(int i = 1; i < btns.Length; i++){
                Destroy(btns[i].gameObject);
            }

            equipHolder.transform.DetachChildren();
        }

        switch (equipNum)
        {
            case 0 : // head
                equipPos = EquipPos.HEAD;
                if(onFocuse.head != null){
                    setEquipBtn(null, equipPos);
                }
            break;
            case 1 : // weapon
                equipPos = EquipPos.WEAPON;
                if(onFocuse.weapon != null){
                    setEquipBtn(null, equipPos);
                }
            break;
            case 2 : // body
                equipPos = EquipPos.BODY;
                if(onFocuse.body != null){
                    setEquipBtn(null, equipPos);
                }
            break;
            case 3 : // shoes
                equipPos = EquipPos.SHOES;
                if(onFocuse.shoes != null){
                    setEquipBtn(null, equipPos);
                }
            break;
        }       
        

        foreach(var equips in GameSystem.instance._EquipsInInventory){
            var equip = equips.Key;
            if(equip.charType == onFocuse.charType){
                if(equip.equipPos == equipPos){
                    setEquipBtn(equip);
                }
            }
        }
        equipHolder.GetComponent<MenuObj>().renewBtns();
    }
    void setEquipBtn(EquipData newData, EquipPos pos = default){
        if(newData == null){
            newData = new EquipData();
            newData.title = "없 음";
            newData.charType = CharType.NULL;
            newData.equipPos = pos;
        }

        var newObj = Instantiate(equipBtn, equipHolder.transform);
        var mbd = newObj.GetComponent<MenuBtnDynamic>();
        mbd.text.text = newData.title;
        if(newData.describeImage != null)
            mbd.setImage(newData.describeImage);

        mbd.onFocusOneShot.AddListener((o) => {
            describe.text = newData.title + "\n" + newData.describe;
            statDiff.text = onFocuse.diffEquipData(newData);
        });

        mbd.onClickBtn.AddListener((o) => {
            onFocuse.setEquipData(newData);
            setEquips(curEquipNum);
            base.setStatusData(onFocuse);
        });
    }
}
