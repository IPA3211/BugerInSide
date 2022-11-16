using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class EndEffect : MonoBehaviour
{
    public GameObject destoryObj;
    public UnityEvent OnStartEff;
    public UnityEvent OnHitChar;
    public UnityEvent OnEndEff;
    // Start is called before the first frame update
    void Start()
    {
        OnStartEff.Invoke();
    }
    public void aniHitCallback(){
        OnHitChar.Invoke();
    }

    public void OnAniEnd(){
        OnEndEff.Invoke();
        if(destoryObj == null){
            destoryObj = gameObject;
        }
        Destroy(destoryObj);
    }

    public void playSFX(Object name){
        var a = name as AudioClip;        
        SoundPlayer.instance.startSFX(a);
    }

    public void shakeCam(float duration){
        var s = BattleCamEffects.instance.shakeCam(duration);
        s.Play();
    }
}
