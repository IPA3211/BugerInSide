using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class test : MonoBehaviour
{
    public PlayableDirector aa;
    public void Start(){
        aa.time = aa.duration;
    }
}
