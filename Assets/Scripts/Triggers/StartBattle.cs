using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class StartBattle : MonoBehaviour
{
    public Sprite background;
    public string startBattleBgm = "Battle1";
    public string openingDialouge;
    public List<CharacterData> allies;
    public List<CharacterData> enemies;
    public List<addItemInfo> items;
    public List<addEquipInfo> equips;
    public int gold = 0;
    
    public bool isEndWhenLose;
    public SceneInfo onWin;
    public SceneInfo onLose;

    public void startBattle(){
        SoundPlayer.instance.startBGM(startBattleBgm);
        foreach(var item in allies){
            GameSystem.instance.joinChar(item);
        }
        
        GameSystem.instance.makeFightStage(enemies);

        SceneInfo a = new SceneInfo();
        a.sceneName = "dieMap";

        BattleManager.onBattleWinSceneInfo = onWin;
        BattleManager.onBattleLoseSceneInfo = a;
        BattleManager.giveItemOnWin = items;
        BattleManager.giveEquipOnWin = equips;
        BattleManager.giveGoldOnWin = gold;
        BattleManager.background = background;
        BattleManager.openingDialouge = openingDialouge;

        PlayerStatics.isCutSceneOn = true;
        
        ChangeSceneEffect.instance.startBattleEffect(() => {
            PlayerStatics.isCutSceneOn = false;
            SceneManager.LoadScene("FightScene");
        });
    }
}
