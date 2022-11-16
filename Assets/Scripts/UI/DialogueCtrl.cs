using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Febucci.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Networking;  // UnityWebRequest   
using System.Threading.Tasks; // for wait
using System.IO; // for Path functions

public class DialogueCtrl : MonoBehaviour
{
    public static DialogueCtrl  instance = null;
    const string SPRITE_PATH    = "Dialogues/Scripts/";
    const string IMAGE_PATH     = "Dialogues/Images/";

    public GameObject _dialouge;
    public GameObject _title;
    public GameObject _describe;
    public GameObject _Limg, _Rimg, _Cimg;
    public TMP_Text infoText;
    public float startDialogueDelay = 1;

    public float fadeInTime = 1f;
    public float fadeOutTime = 1f;

    private Image _Limage, _Rimage, _Cimage;
    private TextMeshProUGUI _titleTM;
    private TextMeshProUGUI _describeTM;
    private TextAnimatorPlayer _textAnimatorPlayer;
    private int _curser = 0;
    private bool _isAnimationEnd = false;
    private bool _isDialogueVisible = true;
    private bool _isImgAnimationSkip = false;
    private bool _isDialoguePlaying = false;
    private bool _reserveClick = false;
    private Dictionary<Image, Sequence> fadeSequences = null;
    private List<UnityEvent> _callback = null;
    
    enum ImageNum
    {
        R_IMAGE,
        C_IMAGE,
        L_IMAGE,

    }
    List<Dictionary<string, object>> data;
    void Awake(){
        instance = this;
        init();
    }

    void Update(){
        if(startDialogueDelay > 0){
            startDialogueDelay -= Time.deltaTime;
        }
        if(_isDialoguePlaying){
            if(infoText != null){
                if(!_isAnimationEnd){
                    infoText.text = "Z : SKIP";
                }
                else{
                    if(!PlayerStatics.isDialogueBtnOn){
                        infoText.text = "Z : NEXT    X : HIDE UI";
                    }
                    else{
                        infoText.text = "Z : SELECT";
                    }
                }
            }

            if(Input.GetButtonDown("Submit") && !_reserveClick && _isDialogueVisible){
                if(!_isAnimationEnd){
                    skipDialogue();
                }
                else{
                    if(!PlayerStatics.isDialogueBtnOn){
                        submitClicked();
                    }
                }
            }

            if(Input.GetButtonDown("Cancel") && _isAnimationEnd){
                hideDialogue(!_isDialogueVisible);
            }
        }
    }

    void init(){
        _titleTM = _title.GetComponentInChildren<TextMeshProUGUI>();
        _describeTM = _describe.GetComponentInChildren<TextMeshProUGUI>();
        _textAnimatorPlayer = _describe.GetComponentInChildren<TextAnimatorPlayer>();
        _textAnimatorPlayer.onTextShowed.AddListener(dialogueAniEnd);
        _textAnimatorPlayer.textAnimator.onEvent += dialogueEventListener;
        fadeSequences = new Dictionary<Image, Sequence>();

        _Limage = _Limg.GetComponent<Image>();
        _Cimage = _Cimg.GetComponent<Image>();
        _Rimage = _Rimg.GetComponent<Image>();

        fadeSequences.Add(_Limage, DOTween.Sequence());
        fadeSequences.Add(_Cimage, DOTween.Sequence());
        fadeSequences.Add(_Rimage, DOTween.Sequence());
        
        _Limage.enabled = false;
        _Cimage.enabled = false;
        _Rimage.enabled = false;
    }

    void dialogueEventListener(string message){
        var parameters = message.Split('=');
        switch(parameters[0]){
            case "end" :
                Debug.Log(message);
                endDialogue(int.Parse(parameters[1]));
                break;
            case "btn" :
                var parameters2 = parameters[1].Split('&');
                GetComponent<DialogueBtnCtrl>().addBtn((parameters2[0], int.Parse(parameters2[1]) - 2));
                GetComponent<DialogueBtnCtrl>().startMenu();
                break;
            case "jump" :
                DialogueCtrl.instance.setCurser(int.Parse(parameters[1]) - 1);
                setNextDialogue();
                break;
            case "bgm" :
                SoundPlayer.instance.startBGM(parameters[1]);
                break;
            case "sfx" :
                SoundPlayer.instance.startSFX(parameters[1]);
                break;
        }
    }
    public bool isVisible(){
        return _isDialogueVisible;
    }

    public void submitClicked(){
        _curser++;
        if(data.Count - 1 == _curser){
            endDialogue(0);
        }
        else{
            setNextDialogue();
        }
    }

    public void setCurser(int i){
        _curser = i;
    }

    void dialogueAniEnd(){
        _isAnimationEnd = true;
    }

    public void skipDialogue(){
        _textAnimatorPlayer.SkipTypewriter();
        fadeSequences[_Limage].GotoWithCallbacks(fadeSequences[_Limage].Duration());
        fadeSequences[_Cimage].GotoWithCallbacks(fadeSequences[_Cimage].Duration());
        fadeSequences[_Rimage].GotoWithCallbacks(fadeSequences[_Rimage].Duration());
    }

    public void hideDialogue(bool visible){
        _isDialogueVisible = visible;
        _title.SetActive(_isDialogueVisible);
        _describe.SetActive(_isDialogueVisible);
    }

    public void startDialogue(string file, List<UnityEvent> callback = null){
        if(file == null || file.Equals("")){
            callback[0].Invoke();
            return;
        }
        data = CSVReader.Read("/Dialogues/Scripts/" + file);

        startDialogue(data, callback);
    }

    public void startDialogue(List<Dictionary<string, object>> data, List<UnityEvent> callback = null){
        if(startDialogueDelay > 0){
            return;
        }
        PlayerStatics.isDialogueOn = true;
        Time.timeScale = 0;
        
        resetImages();

        _isDialoguePlaying = true;
        this.data = data;
        _dialouge.SetActive(true);
        _curser = 0;
        _callback = callback;

        StartCoroutine(reserveTime());

        setNextDialogue();
    }

    public void endDialogue(int endNum){
        startDialogueDelay = 1f;
        PlayerStatics.isDialogueOn = false;
        Time.timeScale = 1;
        _isDialoguePlaying = false;
        _dialouge.SetActive(false);

        resetImages();

        if(_callback == null){
            return;
        }
        
        if(_callback[endNum] != null)
            _callback[endNum].Invoke();
    }
    void resetImg(Image image){
        image.sprite = null;
        image.color = new Color(1,1,1,0);
        image.enabled = false;
    }

    void resetImages(){
        fadeSequences[_Limage].GotoWithCallbacks(fadeSequences[_Limage].Duration());
        fadeSequences[_Cimage].GotoWithCallbacks(fadeSequences[_Cimage].Duration());
        fadeSequences[_Rimage].GotoWithCallbacks(fadeSequences[_Rimage].Duration());

        resetImg(_Rimage);
        resetImg(_Cimage);
        resetImg(_Limage);
    }

    void changeImg(string imgStr, Image image){
        if(!data[_curser].ContainsKey(imgStr)){
            fadeSequences[image] = fadeOut(image, () => {
                image.enabled = false;
            });
            return;
        }

        Sprite newSprite = Resources.Load<Sprite>(IMAGE_PATH + data[_curser][imgStr].ToString());
        
        if(newSprite != null){
            if(image.enabled == false){
                image.sprite = newSprite;
                image.enabled = true;
                setImageSize(imgStr, image);
                fadeSequences[image] = fadeIn(image, () => {});
            }
            else {
                if(image.sprite != newSprite){
                    fadeSequences[image] = blank(image, () => {
                        image.sprite = newSprite;
                        setImageSize(imgStr, image);
                    }, () => {});
                }
            }
        }
        else{
            fadeSequences[image] = fadeOut(image, () => {
                image.enabled = false;
            });
        }
    }

    public Sequence fadeIn(Image image, TweenCallback callback){
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOFade(1f, fadeInTime));
        sequence.AppendCallback(callback);
        sequence.SetUpdate(true);
        return sequence;
    }

    public Sequence blank(Image image, TweenCallback midCallback, TweenCallback endCallback){
        var sequence = DOTween.Sequence();
        sequence.Append(fadeOut(image, midCallback));
        sequence.Append(fadeIn(image, endCallback));
        sequence.SetUpdate(true);
        return sequence;
    }

    public Sequence fadeOut(Image image, TweenCallback callback){
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOFade(0f, fadeInTime));
        sequence.AppendCallback(callback);
        sequence.SetUpdate(true);
        return sequence;
    }


    public void setImageSize(string imgStr, Image image){
        if(data[_curser][imgStr].ToString() == "300고세구" || data[_curser][imgStr].ToString() == "300고세구2"){
            if(image.rectTransform.localScale.x < 0)
                image.rectTransform.localScale = new Vector3(-1, 1.7f, 1f);
            else
                image.rectTransform.localScale = new Vector3(1, 1.7f, 1f);
        }
        else if((data[_curser][imgStr].ToString() == "팬덤")){
            if(image.rectTransform.localScale.x < 0)
                image.rectTransform.localScale = new Vector3(-1.5f, 1f, 1f);
            else
                image.rectTransform.localScale = new Vector3(1.5f, 1f, 1f);
        }
        else if((data[_curser][imgStr].ToString() == "키메라")){
            if(image.rectTransform.localScale.x < 0)
                image.rectTransform.localScale = new Vector3(-1.5f, 1f, 1f);
            else
                image.rectTransform.localScale = new Vector3(1.5f, 1f, 1f);
        }
        else if((data[_curser][imgStr].ToString() == "버거두")){
            if(image.rectTransform.localScale.x < 0)
                image.rectTransform.localScale = new Vector3(-1.5f, 1f, 1f);
            else
                image.rectTransform.localScale = new Vector3(1.5f, 1f, 1f);
        }
        else{
            if(image.rectTransform.localScale.x < 0)
                image.rectTransform.localScale = new Vector3(-1, 1f, 1f);
            else
                image.rectTransform.localScale = new Vector3(1, 1f, 1f);
        }
    }

    public void setNextDialogue(){
        SoundPlayer.instance.startSFX("dialogue_next");
        setTitle(data[_curser]["title"].ToString());
        setDescribe(data[_curser]["describe"].ToString());

        changeImg("Limg", _Limage);
        changeImg("Rimg", _Rimage);
        changeImg("Cimg", _Cimage);
        
        _isAnimationEnd = false;
    }

    void setTitle(string text){
        _titleTM.text = text;
    }

    void setDescribe(string text){
        text = text.Replace("\"\"", "\"").Replace("''", "'");
        _textAnimatorPlayer.ShowText(text.Equals("") ? " " : text);
    }

    IEnumerator reserveTime(){
        _reserveClick = true;
        yield return new WaitForSecondsRealtime(0.1f);
        _reserveClick = false;
    }

}
