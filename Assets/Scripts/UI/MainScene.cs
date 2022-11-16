using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    private float sumTime = 0;
    public Text gameVersion;
    public List<RectTransform> images;  
    public MenuObj menuObj;

    public void shutDownGame(){
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(GameSystem.instance != null){
           GameSystem.instance.destroyStatics();
        }

        gameVersion.text = "Ver. " + Application.version;

        Time.timeScale = 1;
        StartSequence();
        menuObj.startMenu();
    }



    void StartSequence(){
        for(int i = 0; i < 9; i++){
            images[i].localScale = new Vector3(0, 0, 0);
        }
        Sequence sequence = DOTween.Sequence();
        float sequenceTime = 0f;

        for(int i = 0; i < 3; i++){
            sequenceTime += 0.1f;
            sequence.Insert(sequenceTime, images[i].DOScale(new Vector3(1,1,1), 1f).SetEase(Ease.OutBack));
        }

        sequenceTime += 0.5f;

        for(int i = 3; i < 6; i++){
            sequenceTime += 0.1f;
            sequence.Insert(sequenceTime, images[i].DOScale(new Vector3(1,1,1), 1f).SetEase(Ease.OutBack));
        }
        
        sequenceTime += 0.5f;
        sequence.Insert(sequenceTime, images[6].DOScale(new Vector3(1,1,1), 1f).SetEase(Ease.OutBack));
        
        sequenceTime += 0.5f;
        sequence.Insert(sequenceTime, images[7].DOScale(new Vector3(1,1,1), 1f).SetEase(Ease.OutBack));
        
        sequenceTime += 0.5f;
        sequence.Insert(sequenceTime, images[8].DOScale(new Vector3(1,1,1), 1f).SetEase(Ease.OutBack));
    }

    void StartBounce(){
        for(int i = 0; i < 8; i++){
            images[i].DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.5f).SetLoops(2, LoopType.Yoyo);
        }
    }

    void Update(){
        sumTime += Time.deltaTime;
        if(sumTime > 4f){
            StartBounce();
            sumTime = 0;
        }
    }
}
