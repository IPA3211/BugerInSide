using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[System.Serializable]
public class CharacterSave : StatusSave{
    public int level;
    public int xp;
    public int useSkillNum;
    public int charIndex;
    public float hp;
    public float mp;
    public List<EquipSave> equips = new List<EquipSave>();
    public List<SkillSave> skills = new List<SkillSave>();
    public CharacterSave(CharacterData data) : base(data){
        level = data.Level;
        xp = data.Xp;
        hp = data.Hp;
        mp = data.Mp;
        equips.Add(data.head != null ? new EquipSave(data.head) : null);
        equips.Add(data.weapon != null ? new EquipSave(data.weapon) : null);
        equips.Add(data.body != null ? new EquipSave(data.body) : null);
        equips.Add(data.shoes != null ? new EquipSave(data.shoes) : null);
        
        if(data.isSupporter){
            charIndex = -1;
            if(data.linkedChar != null){
                charIndex = GameSystem.instance._Characters.FindIndex(a => a.hash == data.linkedChar.hash);
            }
            useSkillNum = data.useSkillNum;
        }

        foreach(var a in data.skills){
            skills.Add(new SkillSave(a));
        }
    }

    public CharacterData load(bool isSupporter = false){
        CharacterData ans = null;
        if(isSupporter)
            ans = GameSystem.instance.makeCharacterInstance(GameObjectDatas.instance.supporters.Find(a => a.hash == this.hash));
        else
            ans = GameSystem.instance.makeCharacterInstance(GameObjectDatas.instance.characters.Find(a => a.hash == this.hash));
        
        ans.Level = this.level;
        ans.Xp = this.xp;
        ans.Hp = this.hp;
        ans.Mp = this.mp;
        
        ans.head = equips[0].load();
        ans.weapon = equips[1].load();
        ans.body = equips[2].load();
        ans.shoes = equips[3].load();

        if(ans.isSupporter){
            if(charIndex == -1){
                ans.linkedChar = null;
            }
            else{
                ans.LinkChar(GameSystem.instance._Characters[charIndex]);
            }
            ans.useSkillNum = this.useSkillNum;
        }

        ans.skills = new List<SkillData>();
        foreach(var a in this.skills){
            ans.skills.Add(a.load());
        }

        return ans;
    }
}

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Object/Create Character", order = 1)]
[System.Serializable]
public class CharacterData : StatusData
{  
    [Space(10)]
    public Sprite StatusImage;
    public bool flip;
    public RuntimeAnimatorController AnimatorController;

    [Header("Character")]
    public string mpName;
    public Sprite mpBarImage;
    public bool isSupporter = false;
    public int useSkillNum = 0;
    public CharType charType;
    public int Level;
    public List<int> Xp2LevelUp;
    public int Xp;

    [Header("Equips")]
    public EquipData head;
    public EquipData weapon;
    public EquipData body;
    public EquipData shoes;
    
    [Space(10)]
    public SkillData deafultAttack;
    public List<SkillData> skills;
    public List<PassiveData> passives;
    
    [Space(10)]
    public CharacterData linkedChar;
    [Space(10)]
    public string skillAI;
    public int usePer = 0;
    public SkillData usePerSkill;
    [HideInInspector] public int skillNum = 0;
    
    public float getMaxHp(){
        float ans = maxHp;
        if(head != null) ans += head.maxHp;
        if(weapon != null) ans += weapon.maxHp;
        if(body != null) ans += body.maxHp;
        if(shoes != null) ans += shoes.maxHp;

        foreach(var a in passives){
            ans = a.setMaxHP(ans);
        }
        
        if(ans < 0){
            ans = 0f;
        }

        return ans;
    }
    public float getMaxMp(){
        float ans = maxMp;
        if(head != null) ans += head.maxMp;
        if(weapon != null) ans += weapon.maxMp;
        if(body != null) ans += body.maxMp;
        if(shoes != null) ans += shoes.maxMp;

        foreach(var a in passives){
            ans = a.setMaxMp(ans);
        }

        if(ans < 0){
            ans = 0f;
        }

        return ans;
    }
    public float getSpeed(){
        float ans = Speed;
        if(head != null) ans += head.Speed;
        if(weapon != null) ans += weapon.Speed;
        if(body != null) ans += body.Speed;
        if(shoes != null) ans += shoes.Speed;

        foreach(var a in passives){
            ans = a.setMaxSpeed(ans);
        }
        
        if(ans < 0){
            ans = 0f;
        }

        return ans;
    }
    public float getATK(){
        float ans = ATK;
        if(head != null) ans += head.ATK;
        if(weapon != null) ans += weapon.ATK;
        if(body != null) ans += body.ATK;
        if(shoes != null) ans += shoes.ATK;

        foreach(var a in passives){
            ans = a.setMaxATK(ans);
        }
        
        if(ans < 0){
            ans = 0f;
        }

        return ans;
    }
    public float getDEF(){
        float ans = DEF;
        if(head != null) ans += head.DEF;
        if(weapon != null) ans += weapon.DEF;
        if(body != null) ans += body.DEF;
        if(shoes != null) ans += shoes.DEF;

        foreach(var a in passives){
            ans = a.setMaxDEF(ans);
        }
        
        if(ans < 0){
            ans = 0f;
        }
        
        return ans;
    }
    
    public bool isOnStun(){
        foreach(var p in passives){
            if(p.isStun){
                return true;
            }
        }

        return false;
    }

    public int hit(float power){
        Hp -= power;

        if(power > 0){
            switch(charType){
                case CharType.GUINNESS:
                    costMp(-10f);
                break;
                default:
                break;
            }
        }

        if(Hp > getMaxHp()){
            Hp = getMaxHp();
        }
        
        if (Hp < 0){
            Hp = 0;
        }
        return (int)power;
    }

    public void costMp(float power){
        Mp -= power;

        if(Mp > getMaxMp()){
            Mp = getMaxMp();
        }
        
        if (Mp < 0){
            Mp = 0;
        }
    }
    
    public void onEndBattle(){
        passives.Clear();
        Hp = getMaxHp();
        switch(charType){
            case CharType.KNIGHT:
                Mp = 0;
            break;
            default:
                Mp = 0;
            break;
        }
    }

    public override void addStatus(StatusData other)
    {
        maxHp += other.maxHp;
        hit(-other.Hp);
        maxMp += other.maxMp;
        costMp(-other.Mp);

        Speed += other.Speed;
        ATK += other.ATK;
        DEF += other.DEF;
    }

    public void setEquipData(EquipData equip){
        ref EquipData refEquip = ref head;
        switch(equip.equipPos){
            case EquipPos.HEAD : 
                refEquip = ref head;
            break;
            case EquipPos.WEAPON : 
                refEquip = ref weapon;
            break;
            case EquipPos.BODY : 
                refEquip = ref body;
            break;
            case EquipPos.SHOES : 
                refEquip = ref shoes;
            break;
        }

        if(refEquip != null){
            GameSystem.instance.giveEquip(refEquip, 1);
            hit(refEquip.maxHp);
        }

        refEquip = equip;
        
        if(refEquip != null)
            hit(-refEquip.maxHp);
        
        if(refEquip.skill2give != null){
            foreach (var item in refEquip.skill2give)
            {
                if(item == null){
                    continue;
                }
                
                if(!skills.Find(a => a.hash == item.hash)){
                    skills.Add(item);
                }
            }
        }

        if(equip.charType == CharType.NULL)
            refEquip = null;

        if(refEquip != null){
            GameSystem.instance.useEquip(refEquip, 1);
        }
    }

    public string diffEquipData(EquipData newData){
        string ans = "";
        switch(newData.equipPos){
            case EquipPos.HEAD : 
                ans = diffEquips(this.head, newData);
            break;
            case EquipPos.BODY : 
                ans = diffEquips(this.body, newData);
            break;
            case EquipPos.WEAPON : 
                ans = diffEquips(this.weapon, newData);
            break;
            case EquipPos.SHOES : 
                ans = diffEquips(this.shoes, newData);
            break;
        }
        return ans;
    }

    string diffEquips(EquipData oriData, EquipData newData){
        string diffText = "";
        if(oriData == null){
            oriData = new EquipData();
            oriData.title = "없 음";
            oriData.charType = CharType.NULL;
        }

        if(newData == null){
            newData = new EquipData();
            newData.title = "없 음";
            newData.charType = CharType.NULL;
        }

        if(Mathf.Abs(oriData.maxHp - newData.maxHp) > 0.1f){
            diffText += "HP : ";
            diffText += ((oriData.maxHp - newData.maxHp) > 0 ? "<color=#FF0000>" : "<color=green>") + -(oriData.maxHp - newData.maxHp) + "</color>\n";
            Debug.Log((oriData.maxHp - newData.maxHp));
        }
        if(Mathf.Abs(oriData.maxMp - newData.maxMp) > 0.1f){
            diffText += "MP : ";
            diffText += ((oriData.maxMp - newData.maxMp) > 0 ? "<color=#FF0000>" : "<color=green>") + -(oriData.maxMp - newData.maxMp) + "</color>\n";
        }
        if(Mathf.Abs(oriData.Speed - newData.Speed) > 0.1f){
            diffText += "스피드 : ";
            diffText += ((oriData.Speed - newData.Speed) > 0 ? "<color=#FF0000>" : "<color=green>") + -(oriData.Speed - newData.Speed) + "</color>\n";
        }
        if(Mathf.Abs(oriData.ATK - newData.ATK) > 0.1f){
            diffText += "공격력 : ";
            diffText += ((oriData.ATK - newData.ATK) > 0 ? "<color=#FF0000>" : "<color=green>") + -(oriData.ATK - newData.ATK) + "</color>\n";
        }
        if(Mathf.Abs(oriData.DEF - newData.DEF) > 0.1f){
            diffText += "방어력 : ";
            diffText += ((oriData.DEF - newData.DEF) > 0 ? "<color=#FF0000>" : "<color=green>") + -(oriData.DEF - newData.DEF) + "</color>\n";
        }

        return diffText;
    }

    public void LinkChar(CharacterData other){
        if(other.linkedChar != null){
            other.linkedChar.linkedChar = this.linkedChar;
            if(this.linkedChar != null)
                this.linkedChar.linkedChar = other.linkedChar;
            other.linkedChar = this;
            this.linkedChar = other;
        }
        else{
            if(this.linkedChar != null)
                this.linkedChar.linkedChar = null;
            this.linkedChar = other;
            other.linkedChar = this;
        }
    }

    public void initLinkChar(){

        if(isSupporter){
            foreach(var item in GameSystem.instance._Characters){
                if(item.linkedChar == null){
                    this.LinkChar(item);
                    break;
                }
            }
        }
        else{
            foreach(var item in GameSystem.instance._Supporters){
                if(item.linkedChar == null){
                    this.LinkChar(item);
                    break;
                }
            }
        }
    }

    public void equipDefault(){
        if(body != null)
            Hp += body.maxHp;
    }

    public void setSkillNum(int num){
        if(num > skills.Count){
            num = 0;
        }
        else if(num < 0){
            num = 0;
        }

        useSkillNum = num;
    }
}
