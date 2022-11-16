using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuObj : MonoBehaviour
{
    public static MenuObj onFocusMenuObj;
    public enum GridType{
        VERTICAL,
        HORIZONTAL,
        MIXED
    }
    public int mixHorizon = 1;
    private bool isInitFrame;
    private bool _isAllFocus;
    private MenuObj _prev, _next;
    private MenuBtn _focusBtn = null;
    private int _onFocusNum;
    private int _defaultFocusNum = 0;
    public List<MenuBtn> _menuBtns;
    public GridType gridType;
    public UnityEvent onStartFocus;
    public UnityEvent onEndFocus;
    public UnityEvent onClickAllBtns;

    void Start(){
        //init();
        renewBtns();
    }
    public void setDefualtFocusNum(int num){
        _defaultFocusNum = num;
    }
    public void renewBtns(){
        _focusBtn = null;
        isInitFrame = true;
        _menuBtns = new List<MenuBtn>();
        foreach(Transform i in gameObject.transform){
            if(i.GetComponent<MenuBtn>() != null){
                _menuBtns.Add(i.GetComponent<MenuBtn>());
            }
        }
        _onFocusNum = _defaultFocusNum;
    }
    // Start is called before the first frame update
    public void init()
    {
        if(_focusBtn != null){
            _focusBtn.endFocus();
        }
        _prev = null;
        _next = null;
        _focusBtn = null;
        _onFocusNum = _defaultFocusNum;
    }
    public void startMenu(){
        startMenu(false);
    }
    public void startMenu(bool isAllFocus = false){
        this._isAllFocus = isAllFocus;
        onFocusMenuObj = this;
        onFocusMenuObj.onStartFocus.Invoke();
        onFocusMenuObj.init();
        isInitFrame = true;
    }
    public void changeToNextMenu(){
        changeToNextMenu(false);
    }
    public void changeToNextMenu(bool isAllFocus){
        Debug.Log(isAllFocus);
        onFocusMenuObj._next = this;

        this._prev = onFocusMenuObj;
        this._isAllFocus = isAllFocus;

        onFocusMenuObj = this;
        isInitFrame = true;
        onFocusMenuObj.onStartFocus.Invoke();
    }

    public void changeToPrevMenu(){
        Debug.Log(_prev);
        onFocusMenuObj.onEndFocus.Invoke();

        if(_prev != null){
            onFocusMenuObj = _prev;
            _prev._next = null;
            onFocusMenuObj.onStartFocus.Invoke();
            onFocusMenuObj.isInitFrame = true;

            this.init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isInitFrame){
            isInitFrame = false;
            return;
        }

        bool submit = Input.GetButtonDown("Submit");
        bool cancel = Input.GetButtonDown("Cancel");

        bool horizontal = Input.GetButtonDown("Horizontal");
        float Hvalue = Input.GetAxisRaw("Horizontal");

        bool vertical = Input.GetButtonDown("Vertical");
        float Vvalue = Input.GetAxisRaw("Vertical");

        if(this.Equals(onFocusMenuObj)){
            
            if(cancel){
                SoundPlayer.instance.startSFX("undo");
                if(_isAllFocus){
                    foreach(var item in _menuBtns){
                        item.endFocus();
                    }
                }
                changeToPrevMenu();
                return;
            }

            if(_menuBtns.Count != 0){

                if(_isAllFocus){
                    foreach(var item in _menuBtns){
                        item.startFocusLoop(Time.unscaledDeltaTime);
                    }
                }
                else{
                    if(_focusBtn == null){
                        _focusBtn = _menuBtns[_onFocusNum];
                        _focusBtn.startFocus();
                    }
                    else if((!_focusBtn.Equals(_menuBtns[_onFocusNum]))){
                        _focusBtn.endFocus();
                        _focusBtn = _menuBtns[_onFocusNum];
                        _focusBtn.startFocus();
                    }

                    _focusBtn.startFocusLoop(Time.unscaledDeltaTime);
                }

                if(submit){
                    if(_isAllFocus){
                        SoundPlayer.instance.startSFX("click");
                        onClickAllBtns.Invoke();
                    }
                    else
                        _focusBtn.onClick();
                }
            }

            switch(gridType){
                case GridType.VERTICAL :
                    if(vertical){
                        _onFocusNum += Vvalue < 0 ? 1 : -1;
                        if(_menuBtns.Count <= _onFocusNum){
                            _onFocusNum = 0;
                        }

                        if(_onFocusNum < 0){
                            _onFocusNum = _menuBtns.Count - 1;
                        }
                    }
                break;
                case GridType.HORIZONTAL :
                    if(horizontal){
                        _onFocusNum += Hvalue < 0 ? -1 : 1;
                        if(_menuBtns.Count <= _onFocusNum){
                            _onFocusNum = 0;
                        }

                        if(_onFocusNum < 0){
                            _onFocusNum = _menuBtns.Count - 1;
                        }
                    }
                break;
                case GridType.MIXED :
                    if(horizontal){
                        _onFocusNum += Hvalue < 0 ? -1 : 1;
                        if(_menuBtns.Count <= _onFocusNum){
                            _onFocusNum = 0;
                        }

                        if(_onFocusNum < 0){
                            _onFocusNum = _menuBtns.Count - 1;
                        }
                    }
                    if(vertical){
                        _onFocusNum += Vvalue < 0 ? mixHorizon : -mixHorizon;
                        if(_menuBtns.Count <= _onFocusNum){
                            _onFocusNum = 0;
                        }

                        if(_onFocusNum < 0){
                            _onFocusNum = _menuBtns.Count - 1;
                        }
                    }
                break;
            }
        }
    }
}
