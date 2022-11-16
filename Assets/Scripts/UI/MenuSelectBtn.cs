using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuSelectBtn : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text hpText;
    public TMP_Text mpText;
    public Image image;
    public Image supporterImg;
    private CharacterData _data;

    public void setData(CharacterData data){
        _data = data;
        refresh();
    }
    public CharacterData getData(){
        return _data;
    }

    public void refresh(){
        nameText.text = _data.title;
        if(!_data.isSupporter){
            hpText.text = _data.Hp + " / " + _data.getMaxHp();
            mpText.text = _data.Mp + " / " + _data.getMaxMp();

            if(_data.linkedChar != null){
                supporterImg.enabled = true;
                supporterImg.sprite = _data.linkedChar.StatusImage;
            }
            else{
                supporterImg.enabled = false;
            }
        }
        else{
            hpText.text = "서포터";
            mpText.text = "";

            supporterImg.enabled = false;
        }
        image.sprite = _data.StatusImage;
    }
}
