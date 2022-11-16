using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChangeSceneEffect : MonoBehaviour
{
    public static ChangeSceneEffect instance;
    public RawImage img;
    public Image fadeEffect;
    public Texture2D ScreenTexture;
    public void Awake(){
        instance = this;
    }
    public void startBattleEffect(TweenCallback callback){
        StartCoroutine(CaptureScreen(callback));
    }
    public void normalChangeScene(TweenCallback callback){
        StartCoroutine(ChangeSceneCoroutine(callback));
    }

    IEnumerator CaptureScreen(TweenCallback callback){
        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        yield return new WaitForEndOfFrame();
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        tex.Apply();
        img.texture = tex;

        fadeEffect.color = new Color(1,1,1,0);
        img.color = Color.white;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(fadeEffect.DOColor(Color.white, 0.2f).SetLoops(4, LoopType.Yoyo));
        sequence.Append(img.GetComponent<RectTransform>().DOScale(new Vector3(2, 2, 2), 2f));
        sequence.Join(fadeEffect.DOColor(Color.white, 2f));
        sequence.onComplete = callback;
    }

    IEnumerator ChangeSceneCoroutine(TweenCallback callback){
        yield return new WaitForEndOfFrame();

        fadeEffect.color = new Color(1,1,1,0);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(fadeEffect.DOColor(Color.white, 2f));
        sequence.onComplete = callback;
    }
}
