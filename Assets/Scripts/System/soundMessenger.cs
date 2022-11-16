using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundMessenger : MonoBehaviour
{
    public void startSFX(string s){
        SoundPlayer.instance.startSFX(s);
    }
    public void startBGM(string s){
        SoundPlayer.instance.startBGM(s);
    }
}
