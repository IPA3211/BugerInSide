using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BarCtrl : MonoBehaviour
{
    public GameObject defaultSlider;
    public GameObject pirateSlider;
    private GameObject slider;
    public TMP_Text text;
    
    public void setSlider(bool isPirate = false){
        if(isPirate){
            slider = pirateSlider;
            pirateSlider.SetActive(true);
            defaultSlider.SetActive(false);
        }
        else{
            slider = defaultSlider;
            pirateSlider.SetActive(false);
            defaultSlider.SetActive(true);
        }
    }

    public void setBarValue(float curInt, float maxInt){
        slider.GetComponent<DefaultSlider>().setBarValue(curInt, maxInt);
    }

    public void setTitle(string title){
        text.text = title;
    }
}
