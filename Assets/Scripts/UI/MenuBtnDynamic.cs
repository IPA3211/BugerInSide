using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuBtnDynamic : MenuBtn
{
    public Image image;
    public TMP_Text text;
    public TMP_Text number;

    public void setImage(Sprite s){
        image.sprite = s;
        if(image.sprite == null){
            image.color = new Color(1,1,1,0);
        }
        else{
            image.color = new Color(1,1,1,1);
        }
    }
    public void setText(string str){
        text.text = str;
    }
    public void setNumber(string num){
        number.text = num;
    }
    
}
