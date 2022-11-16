using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowStatusInfo : ShowMenuInfo
{
    public TMP_Text describe;
    public TMP_Text stats;

    public override void setUniqueData(CharacterData data)
    {
        describe.text = "설명\n" + data.describe;
        stats.text = "스테이터스"+ "\n"; 
        stats.text += "최대 체력\t : " + data.getMaxHp() + "\n"; 
        stats.text += "최대 MP\t : " + data.getMaxMp() + "\n"; 
        stats.text += "속도 \t: " + data.getSpeed() + "\n"; 
        stats.text += "공격력\t: " + data.getATK() + "\n"; 
        stats.text += "방어력\t: " + data.getDEF() + "\n"; 
    }
}
