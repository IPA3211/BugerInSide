using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipPos
{
    HEAD,
    BODY,
    WEAPON,
    SHOES,
    NULL,
}

public enum CharType{
    KNIGHT,
    MAID,
    PIRATE,
    GUINNESS,
    NULL,
}
[System.Serializable]
public class EquipSave : StatusSave{
    public EquipSave(EquipData data) : base(data){}

    public EquipData load(){
        EquipData ans = GameObjectDatas.instance.equips.Find(a => a.hash == this.hash);

        return ans;
    }
    
}
[CreateAssetMenu(fileName = "Equip", menuName = "Scriptable Object/Create Equip", order = 1)]
[System.Serializable]
public class EquipData : StatusData
{
    [Header ("Equip")]
    public EquipPos equipPos;
    public CharType charType;
    public List<SkillData> skill2give;
}
