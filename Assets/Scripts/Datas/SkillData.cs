using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
using UnityEngine.Video;

[System.Serializable]
public class SkillSave : StatusSave{
    public SkillSave(SkillData data) : base(data){}
    public SkillData load(){
        SkillData ans = GameObjectDatas.instance.skills.Find(a => a.hash == this.hash);
        return ans;
    }
    
}

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Object/Create Skill", order = 1)]
[System.Serializable]
public class SkillData : StatusData
{
    [Header ("Skill")]
    public Vector2 offset;
    public float power;
    public List<PassiveData> passives;
    
    public bool canUse;
    public bool canUseInBattle;
    public bool canUseInField;
    public bool canUseToFriend;
    public bool canUseToEnemy;
    public bool canUseToAll;
    public bool stopWhenEff = true;
    public bool stopWhenUserEff = false;
    public GameObject skillTargetEffect;
    public GameObject skillAllTargetEffect;
    public GameObject skillUserEffect;
    public AudioClip hitSoundClip;
    public VideoClip skillVideoClip;
    public SkillData helpSkill;

    public string startDialogue;
    public string endDialogue;

    public virtual GameObject useTargetEffect(GameObject user, GameObject target){
        GameObject eff = null;
        if(canUseToAll && skillAllTargetEffect != null){
            eff = GameObject.Instantiate(skillAllTargetEffect);
            eff.transform.position = Vector3.zero;
        }
        else if(skillTargetEffect != null){
            eff = GameObject.Instantiate(skillTargetEffect, target.transform);
        }
        return eff;
    }
    public virtual GameObject useUserEffect(GameObject user, GameObject target){
        GameObject eff = null;
        if(skillUserEffect != null)
            eff = GameObject.Instantiate(skillUserEffect, user.transform);
        
        return eff;
    }

    public virtual void useSkill(CharacterData user){
        user.costMp(cost);
    }

    public virtual void hitSkill(CharacterData user, CharacterData target){
        switch(name){
            default : 
                target.hit(power * user.getATK());
                foreach(var a in passives){
                    var find = target.passives.Find(i => i.hash.Equals(a.hash));
                    if(find != null){
                        find.duration += a.duration;
                    }
                    else{
                        target.passives.Add(Instantiate(a));
                    }
                }
            break;
            case "doo_skill2" : 
                if(!GameSystem.instance._ItemsInInventory.ContainsKey(GameObjectDatas.instance.items.Find( a => a.name.Equals("rewind"))))
                    GameSystem.instance.giveItem(GameObjectDatas.instance.items.Find( a => a.name.Equals("rewind")), 1);
                if(target.Hp > 0)
                    target.hit(target.Hp - 1);
            break;
            case "maid_skill1" : 
                target.hit(-(target.getMaxHp() * (maxHp / 100)));
            break;
            case "maid_skill2" : 
                target.hit(-(target.getMaxHp() * (maxHp / 100)));
            break;
        }
    }
    public virtual bool isUseAble(CharacterData user, CharacterData target = null){
        if(user.Mp - cost < 0){
            BattleCamEffects.instance.setText(user.mpName + "이(가) 부족합니다.");
            return false;
        }
        else
            return true;
    }
}
