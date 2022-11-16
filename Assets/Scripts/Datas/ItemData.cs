using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSave : StatusSave{
    public ItemSave(ItemData data) : base(data){}
    public ItemData load(){
        ItemData ans = GameObjectDatas.instance.items.Find(a => a.hash == this.hash);

        return ans;
    }
    
}
[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/Create Item/Item", order = 1)]
[System.Serializable]
public class ItemData : SkillData
{
    public ItemData changeItem;

    public void giveChangeItem(){
        if(changeItem != null){
            GameSystem.instance.giveItem(changeItem, 1);
        }
    }
    public override GameObject useUserEffect(GameObject user, GameObject target)
    {
        GameObject item = null;
        
        if(skillUserEffect != null){
            item = GameObject.Instantiate(skillUserEffect, user.transform);
            item.GetComponent<SpriteRenderer>().sprite = describeImage;
        }

        return item;
    }
    public override void hitSkill(CharacterData user, CharacterData target)
    {
        if(helpSkill != null){
            
        }
        else{
            useItem(target);
        }
    }

    public void useHelpSkill(CharacterData user, CharacterData target){

    }

    public virtual void useItem(CharacterData target){
        target.hit(-Hp);
        target.costMp(-Mp);
        giveChangeItem();
    }

    public override bool isUseAble(CharacterData user, CharacterData target = null){
        if(Mathf.Abs(target.getMaxHp() - target.Hp) < 0.1f && this.Hp > 0.9f){
            return false;
        }
        // else if(Mathf.Abs(target.getMaxMp() - target.Mp) < 0.1f && this.Mp > 0.9f){
        //     return false;
        // }
        else{
            return true;
        }
    }
}
