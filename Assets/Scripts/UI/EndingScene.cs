using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using DG.Tweening;

public class EndingScene : MonoBehaviour
{
    float startDialogueDelay = 0f;
    public PlayableDirector pd;
    public AudioSource source;
    // Update is called once per frame
    public void changeAudioSource(){
        Sequence s = DOTween.Sequence();
        s.Append(DOTween.To(() => SoundPlayer.instance.bgmSource.volume, x => SoundPlayer.instance.bgmSource.volume = x, 0, 1));
        s.Append(DOTween.To(() => source.volume, x => source.volume = x, 1, 0));
    }
    void Update()
    {
        if(Input.GetButtonDown("Submit")){
            pd.Play();
        }
    }

    public void endScene(){
        Destroy(SoundPlayer.instance.gameObject);
    }
}
