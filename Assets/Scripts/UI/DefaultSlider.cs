using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DefaultSlider : MonoBehaviour
{
    private Slider slider;
    private TMP_Text text;
    public Image barImage;
    float lastPoint = 0;
    float lastMaxPoint = 0;
    public virtual void setBarValue(float curInt, float maxInt){
        slider = gameObject.GetComponent<Slider>();
        text = slider.GetComponentInChildren<TMP_Text>();
        float cur = (float) curInt;
        float max = (float) maxInt;
        DOTween.To(() => lastMaxPoint, x => lastMaxPoint = x, max, 1f);
        DOTween.To(() => lastPoint, x => {
            lastPoint = x;
            slider.value = lastPoint / lastMaxPoint;
            text.text = (int)lastPoint + " / " + (int)lastMaxPoint;
        }, cur, 1f);
    }
}
