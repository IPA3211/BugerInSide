using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeInOut : MonoBehaviour
{
    public float fadeTime = 2.5f;
    public Image img;
    void fadeIn(){
        img.DOFade(0f, fadeTime);
    }

    void fadeOut(){
        img.DOFade(1f, fadeTime);
    }
}
