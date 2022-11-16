using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Menu
{
    STATUS,
    SKILLS,
    EQUIPS,
    ITEMS,
}

public class MenuCtrl : MonoBehaviour
{
    [HideInInspector]
    public int menuSelect;
    [HideInInspector]
    public CharacterData selectedData;
    [Header("UIs")]
    public GameObject menu;
    public GameObject status;
    public GameObject skills;
    public GameObject euqips;
    public GameObject items;
    [Header("Holders")]
    public GameObject itemHolder;
    public GameObject equipHolder;
    public GameObject charHolder;
    public GameObject suppHolder;
    public GameObject skillHolder;
    [Header("Prefabs")]
    public GameObject statusBtn;
    public GameObject itemBtn;
    public MenuObj menuObj;
    private CharacterData _selectedSupporter;

    void Update(){
        bool cancel = Input.GetButtonDown("Cancel");

        if(cancel && !PlayerStatics.isCanMove()){
            startMenu();
        }
    }

    public void startMenu(){
        PlayerStatics.isMenuOn = true;
        Time.timeScale = 0;
        menu.SetActive(true);
        menuObj.startMenu();
        initChar();
    }

    public void endMenu(){
        PlayerStatics.isMenuOn = false;
        Time.timeScale = 1;
        menu.SetActive(false);
    }
    
    public void setMenuSelect(int e){
        menuSelect = e;
    }

    public void setCharSelect(CharacterData e){
        selectedData = e;
    }

    public void initChar(){
        var temp = GameSystem.instance._Characters;
        var temp2 = GameSystem.instance._Supporters;

        if(charHolder.transform.childCount != 0){
            var btns = charHolder.GetComponentsInChildren<Transform>();

            for(int i = 1; i < btns.Length; i++){
                Destroy(btns[i].gameObject);
            }

            charHolder.transform.DetachChildren();
        }

        if(suppHolder.transform.childCount != 0){
            var btns = suppHolder.GetComponentsInChildren<Transform>();

            for(int i = 1; i < btns.Length; i++){
                Destroy(btns[i].gameObject);
            }

            suppHolder.transform.DetachChildren();
        }

        for(int i = 0; i < temp2.Count; i++){
            var obj = Instantiate(statusBtn, suppHolder.transform);
            obj.GetComponent<MenuSelectBtn>().setData(temp2[i]);
            obj.GetComponent<MenuSelectBtn>().refresh();
            obj.GetComponent<MenuBtn>().onClickBtn.AddListener((btn) => {
                this.setCharSelect(btn.GetComponent<MenuSelectBtn>().getData());
                showInner();
            });
        }

        for(int i = 0; i < temp.Count; i++){
            var obj = Instantiate(statusBtn, charHolder.transform);
            obj.GetComponent<MenuSelectBtn>().setData(temp[i]);
            obj.GetComponent<MenuSelectBtn>().refresh();
            obj.GetComponent<MenuBtn>().onClickBtn.AddListener((btn) => {
                this.setCharSelect(btn.GetComponent<MenuSelectBtn>().getData());
                showInner();
            });
        }

        charHolder.GetComponent<MenuObj>().init();
        charHolder.GetComponent<MenuObj>().renewBtns();
        suppHolder.GetComponent<MenuObj>().init();
        suppHolder.GetComponent<MenuObj>().renewBtns();
    }

    public void refreshSelectBtn(){
        foreach(var a in charHolder.GetComponentsInChildren<MenuSelectBtn>()){
            a.refresh();
        }
        foreach(var a in suppHolder.GetComponentsInChildren<MenuSelectBtn>()){
            a.refresh();
        }
    }

    public void showInner(){
        Debug.Log(menuSelect);
        switch (menuSelect)
        {
            case 0 : //status
                status.SetActive(true);
                status.GetComponent<MenuObj>().changeToNextMenu();
                status.GetComponent<ShowMenuInfo>().setStatusData(selectedData);
            break;
            case 1 : //skills
                skills.SetActive(true);
                skillHolder.GetComponent<MenuObj>().changeToNextMenu();
                skills.GetComponent<ShowMenuInfo>().setStatusData(selectedData);
            break;
            case 2 : //equips
                euqips.SetActive(true);
                equipHolder.GetComponent<MenuObj>().changeToNextMenu();
                euqips.GetComponent<ShowMenuInfo>().setStatusData(selectedData);
            break;
            case 5 : 
                if(selectedData.isSupporter){
                    _selectedSupporter = selectedData;
                    charHolder.GetComponent<MenuObj>().changeToNextMenu();
                }
                else{
                    selectedData.LinkChar(_selectedSupporter);
                    refreshSelectBtn();
                }
            break;
            default:

            break;
        }
    }
    public void shutDownGame(){
        Application.Quit();
    }
}
