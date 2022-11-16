using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ShowMenuInfo : MonoBehaviour
{   
    public TMP_Text title;
    public TMP_Text hp;
    public TMP_Text mp;
    public Image image;

    public void setStatusData(CharacterData data){
        if(data != null){
            title.text = data.title;
            hp.text = "HP \t: " + data.Hp + " / " + data.getMaxHp() + "\n";
            mp.text = "MP \t: " + data.Mp + " / " + data.getMaxMp() + "\n";
            mp.text += "속도 \t: " + data.getSpeed() + "\n"; 
            mp.text += "공격 \t: " + data.getATK() + "\n"; 
            image.sprite = data.StatusImage;
        }
        setUniqueData(data);
    }

    public abstract void setUniqueData(CharacterData data);
}
