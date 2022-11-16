using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Potion", menuName = "Scriptable Object/Create Item/Posion", order = 1)]
[System.Serializable]
public class PotionData : ItemData
{
    public override void useItem(CharacterData target){
        base.useItem(target);
    }

    public override bool isUseAble(CharacterData user, CharacterData target = null){
        if((Mathf.Abs(target.getMaxHp() - target.Hp) < 0.1f && this.Hp > 0.9f) || (Mathf.Abs(target.getMaxMp() - target.Mp) < 0.1f && this.Mp > 0.9f)){
            BattleCamEffects.instance.setText("대상이 잘못되었습니다.");
            return false;
        }
        else{
            return true;
        }
    }
}
