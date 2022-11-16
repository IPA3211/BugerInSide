using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBGM : MonoBehaviour
{
    public string bgmName;
    // Start is called before the first frame update
    void Start()
    {
        SoundPlayer.instance.startBGM(bgmName);
    }
}
