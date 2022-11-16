using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Experimental.Rendering.LWRP ;
using TMPro;
using UnityEngine.Timeline;

public class BattleCamEffects : MonoBehaviour
{
    public static BattleCamEffects instance;
    public float fadeTime = 2.5f;
    public float ShakeAmount = 0.1f;
    public TMP_Text systemText;
    public Image systemTextBack;
    public Image img;
    public RectTransform turnBar;
    public RectTransform botBar;
    public RawImage videoImage;
    public VideoPlayer videoPlayer;
    public UnityEngine.Experimental.Rendering.Universal.Light2D backgroundLight;
    public TimelineClip playable;
    public GameObject turnMiddle;
    public GameObject cam;
    public GameObject floatingNum;
    void Awake(){
        if(instance == null){
            instance = this;
        }
    }

    public Sequence fadeIn(){
        img.color = Color.white;

        Sequence sequence = DOTween.Sequence().Pause();
        sequence.Append(img.DOFade(0f, fadeTime));

        return sequence;
    }

    void fadeOut(){
        img.DOFade(1f, fadeTime);
    }

    public void lightFade(){
    }

    public void playVideo(VideoClip video, TweenCallback callback){
        videoImage.DOColor(Color.white, 2f);
        videoPlayer.clip = video;

        Sequence sequence = DOTween.Sequence();
        //sequence.Append(videoPlayer.DOPlay());
    }

    public Sequence turnBarBlink(float to, float duration){
        var turnMat = turnMiddle.GetComponent<Image>().material;
        float min = 0.5f;
        Sequence sequence = DOTween.Sequence().Pause();
        sequence.Append(DOTween.To(() => min, x => {
            turnMat.SetFloat("_Glow", x);
            turnMat.SetFloat("_InnerOutlineAlpha", x);
        }, to, duration / 2).SetLoops(2, LoopType.Yoyo));
        return sequence;
    }

    public Sequence setText(string newText, float time = 1f){
        systemText.text = newText;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(systemText.DOFade(1f, 0.5f));
        sequence.Join(systemTextBack.DOFade(0.7f, 0.5f));
        sequence.AppendInterval(time);
        sequence.Append(systemText.DOFade(0f, 0.5f));
        sequence.Join(systemTextBack.DOFade(0f, 0.5f));
        return sequence;
    }

    public Sequence uiFade(){
        Sequence sequence = DOTween.Sequence().Pause();
        sequence.Append(botBar.DOSizeDelta(new Vector2(4000f, 100f), 1f).SetEase(Ease.OutQuad));
        sequence.Join(turnBar.DOAnchorPosY(120f, 1f).SetEase(Ease.OutQuad));
        sequence.Join(botBar.DOAnchorPosY(-810f, 1f).SetEase(Ease.OutQuad));
        return sequence;
    }

    public Sequence startBattleScene(){
        Sequence sequence = DOTween.Sequence().Pause();
        sequence.Append(turnBar.DOAnchorPosY(-120f, 1f).SetEase(Ease.OutQuad));
        sequence.Join(botBar.DOAnchorPosY(-490f, 1f).SetEase(Ease.OutQuad));
        sequence.Join(botBar.DOSizeDelta(new Vector2(1920f, 100f), 1f).SetEase(Ease.OutQuad));

        return sequence;
    }

    public Sequence endBattleScene(){
        Sequence sequence = DOTween.Sequence().Pause();
        sequence.Append(botBar.DOSizeDelta(new Vector2(4000f, 100f), 1f).SetEase(Ease.OutQuad));
        sequence.Append(turnBar.DOAnchorPosY(100f, 1f).SetEase(Ease.OutQuad));
        sequence.Join(botBar.DOAnchorPosY(-810f, 1f).SetEase(Ease.OutQuad));
        sequence.AppendCallback(BattleManager.instance.giveItems);
        sequence.Append(img.DOFade(1f, fadeTime));

        return sequence;
    }

    public Sequence shakeCam(float duration){
        Sequence sequence = DOTween.Sequence().Pause();
        sequence.Append(DOTween.To(() => 0f, x => {
            cam.transform.position = Random.insideUnitSphere * ShakeAmount + new Vector3(0, 0, -10);
        }, 1f, duration));
        sequence.AppendCallback(()=>{
            cam.transform.position = new Vector3(0, 0, -10);
        });

        return sequence;
    }

    public Sequence fadeLight(bool isIn){
        Sequence sequence = DOTween.Sequence().Pause();
        sequence.Append(DOTween.To(() => backgroundLight.intensity, x => backgroundLight.intensity = x, (isIn ? 1f : 0f), 1f));
        return sequence;
    }

    public Sequence playVideo(VideoClip video){
        Sequence sequence = DOTween.Sequence().Pause();
        videoPlayer.clip = video;
        videoPlayer.Prepare();
        
        sequence.Append(uiFade());
        sequence.Join(fadeLight(false));
        sequence.AppendCallback(()=>{
            sequence.Pause();
            if(videoPlayer.isPrepared){
                Debug.Log("prepareCompleted");
                videoImage.color = Color.white;
                videoPlayer.Play();
            }
            videoPlayer.prepareCompleted += (UnityEngine.Video.VideoPlayer vp) => {
                Debug.Log("prepareCompleted");
                videoImage.color = Color.white;
                videoPlayer.Play();
            };
            videoPlayer.loopPointReached += (UnityEngine.Video.VideoPlayer vp) => {
                videoImage.color = new Color(1,1,1,0);
                sequence.Play();
            };
        });
        sequence.AppendInterval(1f);
        return sequence;
    }
    
}
