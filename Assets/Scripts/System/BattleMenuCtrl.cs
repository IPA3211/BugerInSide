using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;
using System.Linq;


public class BattleMenuCtrl : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text describe;
    public TMP_Text skipText;
    public GameObject itemBtn;
    public GameObject charBtn;
    public MenuObj turnMenu;
    public MenuObj itemMenu;
    public MenuObj allies;
    public MenuObj enemies;

    public void Start(){
        allies.onClickAllBtns.AddListener(() => {
            foreach (var item in allies._menuBtns)
            {
                item.endFocus();
            }
            BattleManager.instance.activeOrder();
        });

        enemies.onClickAllBtns.AddListener(() => {
            foreach (var item in enemies._menuBtns)
            {
                item.endFocus();
            }
            BattleManager.instance.activeOrder();
        });
    }
    
    public void selectTurnBtn(){
        if(BattleManager.instance.onPlayingCharacter.charData.isSupporter){
            var c = BattleManager.instance.onPlayingCharacter.charData;
            var skill = c.skills[c.useSkillNum];
            setDescribe(skill);
            if(skill.canUseToEnemy){
                if(skill.canUseToAll){
                    enemies.renewBtns();
                    enemies.startMenu(true);
                }
                else{
                    enemies.renewBtns();
                    enemies.startMenu();
                }
            }
            else if(skill.canUseToFriend){
                if(skill.canUseToAll){
                    allies.renewBtns();
                    allies.startMenu(true); 
                }
                else{
                    allies.renewBtns();
                    allies.startMenu();
                }
            }
        }
        else{
            turnMenu.startMenu();
        }
    }

    public void setTitle(string text){
        title.text = text;
    }

    public void setItemMenu(bool isItem){
        if(itemMenu.transform.childCount != 0){
            var btns = itemMenu.GetComponentsInChildren<Transform>();

            for(int i = 1; i < btns.Length; i++){
                Destroy(btns[i].gameObject);
            }

            itemMenu.transform.DetachChildren();
        }
        int loop = 0;
        if(isItem){
            var sortedItem = GameSystem.instance._ItemsInInventory.OrderBy(x => x.Key.sortingNum);
            foreach (var aa in sortedItem)
            {
                var item = aa.Key;
                if(!item.canUseInBattle){
                    continue;
                }
                GameObject obj = Instantiate(itemBtn, itemMenu.transform);
                obj.GetComponent<MenuBtnDynamic>().setText(item.title);
                obj.GetComponent<MenuBtnDynamic>().setNumber(GameSystem.instance._ItemsInInventory[item].ToString());
                obj.GetComponent<MenuBtnDynamic>().onClickBtn.AddListener((o) => {
                    BattleManager.instance.setOrderNum(item);
                    setDescribe(item);
                    if(item.canUseInBattle){
                        if(item.canUseToEnemy){
                            selectEnemy();
                        }
                        else if(item.canUseToFriend){
                            selectAlly();
                        }
                    }
                });
                loop++;
            }

            itemMenu.renewBtns();
        }
        else{
            foreach (var skill in BattleManager.instance.onPlayingCharacter.charData.skills)
            {
                GameObject obj = Instantiate(itemBtn, itemMenu.transform);
                obj.GetComponent<MenuBtnDynamic>().setText(skill.title);
                if(skill.cost > 0)
                    obj.GetComponent<MenuBtnDynamic>().setNumber(skill.cost.ToString());
                else if(skill.cost == 0)
                    obj.GetComponent<MenuBtnDynamic>().setNumber("");
                else
                    obj.GetComponent<MenuBtnDynamic>().setNumber(Mathf.Abs(skill.cost) + "회복");

                obj.GetComponent<MenuBtnDynamic>().onClickBtn.AddListener((o) => {
                    if(skill.isUseAble(BattleManager.instance.onPlayingCharacter.charData)){
                        BattleManager.instance.setOrderNum(skill);
                        setDescribe(skill);
                        if(skill.canUseToEnemy){
                            selectEnemy(skill.canUseToAll);
                        }
                        else if(skill.canUseToFriend){
                            selectAlly(skill.canUseToAll);
                        }
                    }
                });
                loop++;
            }
            itemMenu.renewBtns();
        }
    }

    public void itemMenuBack(){
        if(!BattleManager.instance.onPlayingCharacter.isSupporter){
            itemMenu.gameObject.SetActive(false);
            turnMenu.gameObject.SetActive(true);
        }
    }

    public void selectAlly(){
        selectAlly(false);
    }
    public void selectEnemy(){
        selectEnemy(false);
    }
    public void selectAttack(){
        var skill = BattleManager.instance.onPlayingCharacter.charData.deafultAttack;
        BattleManager.instance.setOrderNum(skill);
        setDescribe(skill);
        if(skill.canUseToEnemy){
            selectEnemy(skill.canUseToAll);
        }
        else if(skill.canUseToFriend){
            selectAlly(skill.canUseToAll);
        }
    }

    public void selectAlly(bool canUseToAll){
        allies.renewBtns();
        allies.changeToNextMenu(canUseToAll);
    }

    public void selectEnemy(bool canUseToAll = false){
        enemies.renewBtns();
        enemies.changeToNextMenu(canUseToAll);
    }

    public void setDescribe(StatusData skill){
        turnMenu.gameObject.SetActive(false);
        itemMenu.gameObject.SetActive(false);
        describe.gameObject.SetActive(true);
        describe.text = skill.title + "\n" + skill.describe;
    }

    public void endSelect(){
        MenuObj.onFocusMenuObj = null;
        turnMenu.gameObject.SetActive(false);
        itemMenu.gameObject.SetActive(false);
        describe.gameObject.SetActive(false);
    }
}
