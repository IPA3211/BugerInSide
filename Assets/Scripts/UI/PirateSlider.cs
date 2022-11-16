using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PirateSlider : DefaultSlider
{
    public Sprite emptyImage;
    public Sprite fullImage;
    private List<Image> images;
    int last = 0;
    public override void setBarValue(float curInt, float maxInt){
        images = new List<Image>(gameObject.GetComponentsInChildren<Image>());
        int cur = (int) curInt;
        int max = (int) maxInt;
        DOTween.To(() => last, x => {
            last = x;
            foreach(var item in images){
                item.sprite = emptyImage;
            }
            for(int i = 0; i < last; i++){
                images[i].sprite = fullImage;
            }
        }, cur, 1f);
    }
}
