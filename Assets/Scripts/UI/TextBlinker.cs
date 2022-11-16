using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextBlinker : MonoBehaviour
{
    public float duration;
    TMP_Text tt;
    void Start(){
        tt = GetComponent<TMP_Text>();
        Sequence sequence = DOTween.Sequence();

        tt.color = new Color(0f, 0f, 0f, 0f);   

        sequence.Append(tt.DOFade(1f, 0.5f));
        sequence.AppendInterval(duration);
        sequence.Append(tt.DOFade(0f, 0.5f));
    }
}
