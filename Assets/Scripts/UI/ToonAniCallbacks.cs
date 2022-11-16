using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToonAniCallbacks : MonoBehaviour
{
    // Start is called before the first frame update
    void aniEndCallback()
    {
        ToonScene.isEnded = true;
    }

    public void playSFX(Object name){
        var a = name as AudioClip;
        Debug.Log(a.name);
        
        SoundPlayer.instance.startSFX(a);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
