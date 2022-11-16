using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class MenuBtn : MonoBehaviour
{
    public float loopTime = 1;
    public UnityEvent<MenuBtn> onFocusOneShot;
    public UnityEvent<MenuBtn> onFocusLoop;
    public UnityEvent<MenuBtn> onClickBtn;
    public UnityEvent<MenuBtn> onEndFocus;
    public bool isClickAble;
    public bool isClicked;
    [HideInInspector]public float sumTime = 0;
    [SerializeField] private Image outline;
    private Color startColor;
    private Color endColor;
    // Start is called before the first frame update
    public void Awake()
    {
        isClickAble = true;
        startColor = outline.color;
        endColor = new Color(startColor.r, startColor.g, startColor.b, 1);
    }

    public void startFocus(){
        SoundPlayer.instance.startSFX("select");
        onFocusOneShot.Invoke(this);
    }
    public void startFocusLoop(float deltaTime){
        onFocusLoop.Invoke(this);
        sumTime += deltaTime;
        outline.color = Color.Lerp(endColor, startColor, Mathf.PingPong(sumTime, loopTime) / loopTime);
    }

    public void onClick(){
        outline.color = endColor;
        if(isClickAble){
            SoundPlayer.instance.startSFX("click");
            onClickBtn.Invoke(this);
        }
        else{
            
        }
    }

    public void endFocus(){
        onEndFocus.Invoke(this);
        sumTime = 0f;
        outline.color = startColor;
    }
}
