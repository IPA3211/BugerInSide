using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SupporterSave : StatusSave{
    public SupporterSave(SupporterData data) : base(data){}
/*
    public SupporterData load(){
        SupporterData ans = GameObjectDatas.instance.supporters.Find(a => a.hash == this.hash);

        return ans;
    }
    */
    
}
[CreateAssetMenu(fileName = "Supporter", menuName = "Scriptable Object/Create Supporter", order = 1)]
[System.Serializable]
public class SupporterData : StatusData
{
    [Space(10)]
    public Sprite StatusImage;
    public bool flip;
    public RuntimeAnimatorController AnimatorController;

    [Header ("Supporter")]
    public List<SkillData> skills;

/*
    public void LinkChar(CharacterData other){
        if(other.supporter != null){
            var a = GameSystem.instance._Characters.Find(a => a.supporter.hash == this.hash);
            a.supporter = other.supporter;
            other.supporter = this;
        }
        other.supporter = this;
    }

    public void initLinkChar(){
        foreach(var item in GameSystem.instance._Characters){
            if(item.supporter == null){
                LinkChar(item);
                break;
            }
        }
    }
    */
}
