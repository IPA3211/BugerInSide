using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DialogueBtnCtrl : MonoBehaviour
{
    public enum MenuFocus
    {
        TOOL_BAR,
        STATUS
    }
    public GameObject menuObj;
    public GameObject toolBar;
    public GameObject button;
    private List<MenuBtn> toolBarBtns;
    private MenuBtn focusBtn = null;
    private int onFocus = 0;
    private Stack<(List<MenuBtn>, int)> onFocusStack;
    private List<(string text, int jumpNum)> btns;
    private DialogueCtrl dialogueCtrl;

    void Start(){
        dialogueCtrl = GetComponent<DialogueCtrl>();
    }

    void Update(){
        bool submit = Input.GetButtonDown("Submit");
        bool cancel = Input.GetButtonDown("Cancel");
        bool horizontal = Input.GetButtonDown("Vertical");
        float value = Input.GetAxisRaw("Vertical");

        if(PlayerStatics.isDialogueBtnOn && dialogueCtrl.isVisible()){
            if(horizontal){
                onFocus += value < 0 ? -1 : 1;
                if(onFocusStack.Peek().Item1.Count <= onFocus){
                    Debug.Log(onFocusStack.Peek().Item1.Count);
                    onFocus = 0;
                }

                if(onFocus < 0){
                    onFocus = onFocusStack.Peek().Item1.Count - 1;
                }
            }

            if(focusBtn == null){
                focusBtn = onFocusStack.Peek().Item1[onFocus];
            }
            else if((!focusBtn.Equals(onFocusStack.Peek().Item1[onFocus]))){
                focusBtn.endFocus();
                focusBtn = onFocusStack.Peek().Item1[onFocus];
                focusBtn.startFocus();
            }
            
            if(submit){
                focusBtn.onClick();
            }

            focusBtn.startFocusLoop(Time.unscaledDeltaTime);
        }
    }

    void init(){
        onFocusStack = new Stack<(List<MenuBtn>, int)>();
        toolBarBtns = new List<MenuBtn>(toolBar.GetComponentsInChildren<MenuBtn>());
    }

    public void changeFocus(List<MenuBtn> mf){
        if(onFocusStack.Count != 0){
            (List<MenuBtn>, int) temp = onFocusStack.Pop();
            temp.Item2 =onFocus;
            onFocusStack.Push(temp);
        }
        onFocusStack.Push((mf, 0));
        onFocus = 0;
    }

    public void cancelClicked(){

    }

    public void startMenu(){
        changeFocus(toolBarBtns);
        PlayerStatics.isDialogueBtnOn = true;
        menuObj.SetActive(true);
        onFocus = 0;
    }

    public void endMenu(){
        PlayerStatics.isDialogueBtnOn = false;
        menuObj.SetActive(false);
        onFocus = 0;
    }

    public void onClick(int num){
        DialogueCtrl.instance.setCurser(num);
        for(int i = 0; i < toolBarBtns.Count; i++){
            Destroy(toolBarBtns[i].gameObject);
        }
        DialogueCtrl.instance.submitClicked();
        endMenu();
    }

    public void addBtn((string text, int jumpNum) btn){
        var newBtnObj = Instantiate(button, menuObj.transform);
        newBtnObj.GetComponentInChildren<TMP_Text>().text = btn.text;
        newBtnObj.GetComponent<MenuBtn>().onClickBtn.AddListener((o) => {
            onClick(btn.jumpNum);
        });
        init();
    }
}
