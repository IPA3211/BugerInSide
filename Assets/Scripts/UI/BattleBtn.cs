using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleBtn : MonoBehaviour
{
    public BarCtrl hpBar;
    public BarCtrl mpBar;
    public TMP_Text title;
    
    public void refreshData(CharacterData data){
        title.text = data.title;
        hpBar.setSlider();
        hpBar.setBarValue(data.Hp, data.getMaxHp());
        mpBar.text.text = data.mpName;
        if(data.charType == CharType.PIRATE){
            mpBar.setSlider(true);
        }
        else{
            mpBar.defaultSlider.GetComponent<DefaultSlider>().barImage.sprite = data.mpBarImage;
            mpBar.setSlider(false);
        }
        mpBar.setBarValue(data.Mp, data.getMaxMp());
    }
}
