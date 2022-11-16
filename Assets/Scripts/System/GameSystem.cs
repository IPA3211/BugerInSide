using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct addItemInfo
{
    public ItemData item;
    public int amount;
}

[System.Serializable]
public struct addEquipInfo
{
    public EquipData item;
    public int amount;
}

[System.Serializable]
public class GameSystemSave{
    public int gold;
    public List<CharacterSave> _CharactersIndex;
    public List<CharacterSave> _SupporterIndex;
    public List<EquipSave> _equipIndex;
    public List<int> _equipAmount;
    public List<ItemSave> _itemIndex;
    public List<int> _itemAmount;

    public GameSystemSave(GameSystem raw){
        gold = raw.gold;
        _CharactersIndex = new List<CharacterSave>();
        _SupporterIndex = new List<CharacterSave>();
        _equipIndex = new List<EquipSave>();
        _itemIndex = new List<ItemSave>();
        _equipAmount = new List<int>();
        _itemAmount = new List<int>();

        foreach(var c in raw._Characters){
            _CharactersIndex.Add(new CharacterSave(c));
        }
        foreach(var c in raw._Supporters){
            _SupporterIndex.Add(new CharacterSave(c));
        }
        foreach(var c in raw._EquipsInInventory){
            Debug.Log(c);
            _equipIndex.Add(new EquipSave(c.Key));
            _equipAmount.Add(c.Value);
        }
        foreach(var c in raw._ItemsInInventory){
            _itemIndex.Add(new ItemSave(c.Key));
            _itemAmount.Add(c.Value);
        }
    }

    public void load(GameSystem raw){
        raw.gold = gold;
        raw._Characters.Clear();
        raw._Supporters.Clear();
        raw._EquipsInInventory.Clear();
        raw._ItemsInInventory.Clear();
        
        foreach(var c in _CharactersIndex){
            raw._Characters.Add(c.load());
        }
        foreach(var c in _SupporterIndex){
            raw._Supporters.Add(c.load(true));
        }
        for(int i = 0; i < _equipIndex.Count; i++){
            raw._EquipsInInventory.Add(_equipIndex[i].load(), _equipAmount[i]);
        }
        for(int i = 0; i < _itemIndex.Count; i++){
            raw._ItemsInInventory.Add(_itemIndex[i].load(), _itemAmount[i]);
        }
    }
}

[System.Serializable]
public class GameSystem : MonoBehaviour
{
    public static GameSystem instance;
    public int gold;
    public List<CharacterData> _Characters;
    public List<CharacterData> _Supporters;
    public List<CharacterData> _Enemies;
    public List<ItemData> _giveItemOnBattleEnd;
    public Dictionary<EquipData, int> _EquipsInInventory;
    public Dictionary<ItemData, int> _ItemsInInventory;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null){
            instance = this;
            
            _Characters = new List<CharacterData>();
            _Enemies = new List<CharacterData>();
            _EquipsInInventory = new Dictionary<EquipData, int>();
            _ItemsInInventory = new Dictionary<ItemData, int>();
        }
    }
    public void Start(){
        //SetResolution();
    }

    void test(){
        joinChar(GetComponent<GameObjectDatas>().characters[0]);
        instance.giveGold(1000);
        //_Characters.Add(Instantiate(GetComponent<GameObjectDatas>().characters[3]));
    }

    public void giveItem(ItemData data, int amount){
        if(!(_ItemsInInventory.ContainsKey(data))){
            _ItemsInInventory.Add(data, amount);
        }
        else{
            _ItemsInInventory[data] += amount;
        }
    }

    public void useItem(ItemData data, int amount){
        _ItemsInInventory[data] -= amount;
        if(_ItemsInInventory[data] <= 0){
            _ItemsInInventory.Remove(data);
        }
    }

    public void giveEquip(EquipData data, int amount){
        if(!(_EquipsInInventory.ContainsKey(data))){
            _EquipsInInventory.Add(data, 1);
        }
        else{
            _EquipsInInventory[data] = 1;
        }
    }

    public void useEquip(EquipData data, int amount){
        _EquipsInInventory[data] -= amount;
        if(_EquipsInInventory[data] <= 0){
            _EquipsInInventory.Remove(data);
        }
    }

    public void giveGold(int add){
        gold += add;
    }

    public void useGold(int sub){
        gold -= sub;

        if (gold < 0){
            gold = 0;
        }
    }

    public void give(List<addItemInfo> items, List<addEquipInfo> equips, int addGold){
        var itemMessageList = new List<Dictionary<string, object>>();

        if(items != null && items.Count != 0){
            for(int i = 0; i < items.Count; i++){
                GameSystem.instance.giveItem(items[i].item, items[i].amount);
                var itemMessage = new Dictionary<string, object>();
                itemMessage["title"] = "시스템";
                itemMessage["describe"] = items[i].item.title + "을 " + items[i].amount + "개 획득 하였습니다.";
                itemMessageList.Add(itemMessage);
            }
        }
        
        if(equips != null && equips.Count != 0){
            for(int i = 0; i < equips.Count; i++){
                GameSystem.instance.giveEquip(equips[i].item, equips[i].amount);
                var itemMessage = new Dictionary<string, object>();
                itemMessage["title"] = "시스템";
                itemMessage["describe"] = equips[i].item.title + "을 " + equips[i].amount + "개 획득 하였습니다.";
                itemMessageList.Add(itemMessage);
            }
        }

        if(addGold != 0){
            for(int i = 0; i < equips.Count; i++){
                GameSystem.instance.giveGold(addGold);
                var itemMessage = new Dictionary<string, object>();
                itemMessage["title"] = "시스템";
                itemMessage["describe"] = addGold + "G를 획득 하였습니다.";
                itemMessageList.Add(itemMessage);
            }
        }

        if(equips != null && equips.Count != 0){
            var itemMessage = new Dictionary<string, object>();
            itemMessage["title"] = "시스템";
            itemMessage["describe"] = "잊지말고 X를 눌러 새로운 장비를 장착해주세요!";
            itemMessageList.Add(itemMessage);
        }

        if((items != null && items.Count != 0) || (equips != null && equips.Count != 0)){
            var endMessage = new Dictionary<string, object>();
            endMessage["title"] = "시스템";
            endMessage["describe"] = " <?end=0>";
            itemMessageList.Add(endMessage);
            DialogueCtrl.instance.startDialogueDelay = -1f;
            DialogueCtrl.instance.startDialogue(itemMessageList, null);
        }
    }

    public void joinChar(CharacterData character){
        if(character.isSupporter){
            if(_Supporters.Find(a => a.hash == character.hash) == null){
                var a = Instantiate(character);
                a.initLinkChar();
                _Supporters.Add(a);
            }
        }
        else{
            if(_Characters.Find(a => a.hash == character.hash) == null){
                var a = Instantiate(character);
                a.initLinkChar();
                a.equipDefault();
                _Characters.Add(a);
            }
        }
    }

    public void exitChar(CharacterData character){
        if(character.isSupporter){
            var a = GameSystem.instance._Supporters.Find(a => a.hash == character.hash);
            if(a != null){
                a.linkedChar.linkedChar = null;
                GameSystem.instance._Supporters.Remove(a);
            }
        }
        else{
            var c = GameSystem.instance._Characters.Find(a => a.hash == character.hash);
            if(c != null){
                GameSystem.instance._Characters.Remove(c);
            }
        }
    }

    public void join(List<CharacterData> chars){
        var itemMessageList = new List<Dictionary<string, object>>();

        if(chars != null && chars.Count != 0){
            for(int i = 0; i < chars.Count; i++){
                GameSystem.instance.joinChar(chars[i]);
                var itemMessage = new Dictionary<string, object>();
                itemMessage["title"] = "시스템";
                itemMessage["describe"] = chars[i].title + "이(가) 합류 하였습니다.";
                itemMessageList.Add(itemMessage);
            }
        }

        if((chars != null && chars.Count != 0)){
            var endMessage = new Dictionary<string, object>();
            endMessage["title"] = "시스템";
            endMessage["describe"] = " <?end=0>";
            itemMessageList.Add(endMessage);
            DialogueCtrl.instance.startDialogue(itemMessageList, null);
        }
    }

    public void exit(List<CharacterData> chars){
        var itemMessageList = new List<Dictionary<string, object>>();

        if(chars != null && chars.Count != 0){
            for(int i = 0; i < chars.Count; i++){
                GameSystem.instance.exitChar(chars[i]);
                var itemMessage = new Dictionary<string, object>();
                itemMessage["title"] = "시스템";
                itemMessage["describe"] = chars[i].title + "이(가) 이탈 하였습니다.";
                itemMessageList.Add(itemMessage);
            }
        }

        if((chars != null && chars.Count != 0)){
            var endMessage = new Dictionary<string, object>();
            endMessage["title"] = "시스템";
            endMessage["describe"] = " <?end=0>";
            itemMessageList.Add(endMessage);
            DialogueCtrl.instance.startDialogue(itemMessageList, null);
        }
    }

    public void makeFightStage(List<CharacterData> enemies){
        _Enemies.Clear();
        for(int i = 0; i < enemies.Count; i++){
            _Enemies.Add(Instantiate(enemies[i]));
        }
    }

    public void cutSceneStart(){
        PlayerStatics.isCutSceneOn = true;
    }

    public void cutSceneEnd(){
        PlayerStatics.isCutSceneOn = false;
    }

    public CharacterData makeCharacterInstance(CharacterData data){
        return Instantiate(data);
    }

    public void destroyStatics(){
        Destroy(GameSystem.instance.gameObject);
        GameSystem.instance = null;
        GameSaveManager.instance = null;
        GameObjectDatas.instance = null;
        GameEventStatus.instance = null;
        DontDestroy.Instance = null;

        PlayerStatics.resetStatics();
    }

}
