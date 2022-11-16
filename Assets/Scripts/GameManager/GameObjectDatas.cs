using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameObjectDatas : MonoBehaviour
{
    public static GameObjectDatas instance;
    public List<CharacterData> characters;
    public List<CharacterData> supporters;
    public List<CharacterData> enemies;
    public List<ItemData> items;
    public List<EquipData> equips;   
    public List<SkillData> skills; 
    public List<PassiveData> passives; 

    void Awake()
    {
        if(instance == null){
            instance = this;
            init();
        }
    }
    public void init(){
        var temp = new List<CharacterData>(Resources.LoadAll<CharacterData>("Datas/Characters"));
        characters = new List<CharacterData>();
        supporters = new List<CharacterData>();
        foreach(var item in temp){
            if(item.isSupporter){
                supporters.Add(item);
            }
            else{
                characters.Add(item);
            }
        }
        
        enemies = new List<CharacterData>(Resources.LoadAll<CharacterData>("Datas/Enemies"));
        equips = new List<EquipData>(Resources.LoadAll<EquipData>("Datas/Equips"));
        items = new List<ItemData>(Resources.LoadAll<ItemData>("Datas/Items"));
        skills = new List<SkillData>(Resources.LoadAll<SkillData>("Datas/Skills"));
        passives = new List<PassiveData>(Resources.LoadAll<PassiveData>("Datas/Passives"));

        foreach(var c in characters){
            c.setHash();
            c.loadFile("/Datas/Character/");
            c.saveAsFile("/Datas/Character/");
        }
        foreach(var c in supporters){
            c.setHash();
            c.loadFile("/Datas/supporters/");
            c.saveAsFile("/Datas/supporters/");
        }
        foreach(var c in enemies){
            c.setHash();
            c.loadFile("/Datas/enemies/");
            c.saveAsFile("/Datas/enemies/");
        }
        foreach(var c in items){
            c.setHash();
            c.loadFile("/Datas/items/");
            c.saveAsFile("/Datas/items/");
        }
        foreach(var c in equips){
            c.setHash();
            c.loadFile("/Datas/equips/");
            c.saveAsFile("/Datas/equips/");
        }
        foreach(var c in skills){
            c.setHash();
            c.loadFile("/Datas/skills/");
            c.saveAsFile("/Datas/skills/");
        }
        foreach(var c in passives){
            c.setHash();
            c.loadFile("/Datas/passives/");
            c.saveAsFile("/Datas/passives/");
        }
    }
}
