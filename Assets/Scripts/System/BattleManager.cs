using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static Sprite background;
    public static SceneInfo onBattleWinSceneInfo;
    public static SceneInfo onBattleLoseSceneInfo;
    public static List<addItemInfo> giveItemOnWin;
    public static List<addEquipInfo> giveEquipOnWin;
    public static string openingDialouge;
    public static int giveGoldOnWin;
    public static BattleManager instance;
    private SceneInfo onBattleEndSceneInfo;
    public SpriteRenderer backgroundImg;
    public float timeToTurn = 700;
    public bool isStarted = false;
    public bool isPlayerTurn = false;
    public bool isEnemyTurn = false;
    public bool isSupporterTurnDone = false;
    public List<BattleCharacter> allies;
    public List<BattleCharacter> supports;
    public List<BattleCharacter> enemies;
    public Queue<List<CharacterData>> phases;
    public List<CharacterData> phaseOnPlaying;
    public List<CharacterData> alliesOnPlaying;
    int phaseNum = 1;
    Queue<BattleCharacter> turn;
    [HideInInspector]public BattleCharacter onPlayingCharacter;
    [HideInInspector]public BattleMenuCtrl battleMenuCtrl;
    int orderType = -1; // 0 : attack, 1 : item, 2: skill, 3: run
    GameObject target;
    public SkillData orderNum = null;
    int spawnOffset = 10;
    List<Vector2> enemyPos;
    List<Vector2> allyPos;

    void Awake(){
        if(instance == null){
            instance = this;
        }

        enemyPos = new List<Vector2>();
        foreach(var item in enemies){
            enemyPos.Add(item.transform.position);
        }

        allyPos = new List<Vector2>();
        foreach(var item in allies){
            allyPos.Add(item.transform.position);
        }
    }

    void Start(){
        backgroundImg.sprite = background;
        battleMenuCtrl = GetComponent<BattleMenuCtrl>();
        battleMenuCtrl.setTitle("");
        turn = new Queue<BattleCharacter>();

        makePhases(GameSystem.instance._Enemies);

        Sequence startSequence = DOTween.Sequence();
        startSequence.AppendCallback(setPhase);
        startSequence.Join(BattleCamEffects.instance.fadeIn());

        alliesOnPlaying = new List<CharacterData>(GameSystem.instance._Characters);

        if(alliesOnPlaying.Count == 3){
            battleMenuCtrl.allies.setDefualtFocusNum(1);
            allies[0].transform.position = allyPos[2];
            allies[1].transform.position = allyPos[0];
            allies[2].transform.position = allyPos[1];
            alliesOnPlaying[0] = GameSystem.instance._Characters[2];
            alliesOnPlaying[1] = GameSystem.instance._Characters[0];
            alliesOnPlaying[2] = GameSystem.instance._Characters[1];
        }
        else{
            battleMenuCtrl.allies.setDefualtFocusNum(0);
            allies[0].transform.position = allyPos[0];
            allies[1].transform.position = allyPos[1];
            allies[2].transform.position = allyPos[2];
        }

        for(int i = 0; i < alliesOnPlaying.Count; i++){
            allies[i].isEnemy = false;
            allies[i].setCharacterData(alliesOnPlaying[i]);
            if(alliesOnPlaying[i].linkedChar != null)
                supports[i].setCharacterData(alliesOnPlaying[i].linkedChar);
            
            allies[i].transform.position = allies[i].transform.position + new Vector3(-spawnOffset, 0, 0);
            startSequence.Join(allies[i].transform.DOMoveX(allies[i].transform.position.x + spawnOffset, 2f));
            supports[i].transform.position = supports[i].transform.position + new Vector3(-spawnOffset, 0, 0);
            startSequence.Join(supports[i].transform.DOMoveX(supports[i].transform.position.x + spawnOffset, 2f));
        }

        if(openingDialouge != null && !openingDialouge.Equals("")){
            Debug.Log(openingDialouge);
            List<UnityEvent> temp = new List<UnityEvent>();
            var e = new UnityEvent();
            e.AddListener(() => {
                startBattle();
            });
            temp.Add(e);
            
            DialogueCtrl.instance.startDialogueDelay = -1f;
            startSequence.AppendCallback(() => {
                DialogueCtrl.instance.startDialogue(CSVReader.Read("/Dialogues/Scripts/" + openingDialouge), temp);
            });
        }
        else{
            startSequence.AppendCallback(startBattle);
        }
        
        
    }

    void Update(){
        if(turn.Count == 0 && isStarted && !isPlayerTurn && !isEnemyTurn){
            battleMenuCtrl.skipText.gameObject.SetActive(true);
            var min = Time.deltaTime;
            if(Input.GetButtonDown("Submit")){
                min = timeToTurn;
                for(int i = 0; i < alliesOnPlaying.Count; i++){
                    if(allies[i].remainTimeToTrun() < min){
                        min = allies[i].remainTimeToTrun();
                    }
                }
                for(int i = 0; i < phaseOnPlaying.Count; i++){
                    enemies[i].remainTimeToTrun();
                    if(enemies[i].remainTimeToTrun() < min){
                        min = enemies[i].remainTimeToTrun();
                    }
                }
            }

            for(int i = 0; i < alliesOnPlaying.Count; i++){
                allies[i].addTimeToTurn(min);
                allies[i].turnBarSlider.setPosition(timeToTurn, allies[i].sumTime);
                if(allies[i].sumTime > timeToTurn){
                    turn.Enqueue(allies[i]);
                    allies[i].sumTime = 0;
                }
            }
            for(int i = 0; i < phaseOnPlaying.Count; i++){
                enemies[i].addTimeToTurn(min);
                enemies[i].turnBarSlider.setPosition(timeToTurn, enemies[i].sumTime);
                if(enemies[i].sumTime > timeToTurn){
                    turn.Enqueue(enemies[i]);
                    enemies[i].sumTime = 0;
                }
            }
        }
        else{
            battleMenuCtrl.skipText.gameObject.SetActive(false);
        }

        if(turn.Count != 0 && isStarted){
            if(onPlayingCharacter == null){
                playTurn();
            }
        }
    }
    void startBattle(){
        var s = BattleCamEffects.instance.startBattleScene();
        s.Append(BattleCamEffects.instance.turnBarBlink(4, 1).Play());
        s.onComplete = () => {
            BattleCamEffects.instance.setText("전투시작");
            DOVirtual.DelayedCall(1f, battleStart, true);
        };
        s.Play();
    }
    public void playTurn(){
        onPlayingCharacter = turn.Peek();
        
        if(onPlayingCharacter.isDead){
            onPlayingCharacter = null;
            turn.Dequeue();
            return;
        }

        if(onPlayingCharacter.charData.isOnStun()){
            onPlayingCharacter.endTurn();
            BattleCamEffects.instance.setText(onPlayingCharacter.charData.title + "은 기절하였습니다.");
            onPlayingCharacter = null;
            turn.Dequeue();
            return;
        }

        if(onPlayingCharacter.charData.linkedChar != null && !isSupporterTurnDone){
            onPlayingCharacter = supports.Find(a => a.charData.hash == onPlayingCharacter.charData.linkedChar.hash);
        }

        orderType = -1;
        orderNum = null;
        target = null;

        BattleCamEffects.instance.turnBarBlink(4, 1).Play();
        BattleCamEffects.instance.setText(onPlayingCharacter.charData.title + "의 턴");
        battleMenuCtrl.setTitle(onPlayingCharacter.charData.title);

        if(!onPlayingCharacter.isEnemy){
            onPlayingCharacter.isOnTurn = true;
            isPlayerTurn = true;
            if(onPlayingCharacter.charData.isSupporter){
                isSupporterTurnDone = true;
                autoSupporter(onPlayingCharacter.charData);
            }
            else{
                isSupporterTurnDone = false;
                battleMenuCtrl.selectTurnBtn();
                turn.Dequeue();
            }
        }
        else{
            onPlayingCharacter.isOnTurn = true;
            isEnemyTurn = true;
            turn.Dequeue();
            autoAI();
        }
    }

    public void endTurn(){
        onPlayingCharacter.endTurn();
        onPlayingCharacter = null;
        isPlayerTurn = false;
        isEnemyTurn = false;
        battleMenuCtrl.setTitle("");
        checkBattleEnd();
    }

    public void setOrderType(int num){
        orderType = num;
    }
    public void setOrderNum(SkillData num){
        orderNum = num;
    }
    public void setTarget(GameObject t){
        target = t;
    }
    public void activeOrder(){
        if(onPlayingCharacter.isOnTurn){
            battleMenuCtrl.endSelect();
            onPlayingCharacter.isOnTurn = false;
            Debug.Log(orderType + " " + orderNum + " " + target);
            switch(orderType){
                case 0: // 0 : attack
                    onPlayingCharacter.useSkill(onPlayingCharacter.charData.deafultAttack, target, () => {
                        endTurn();
                    });
                break;
                case 1: // 1 : item
                    var item = GameObjectDatas.instance.items.Find(a => a.hash == orderNum.hash);
                    onPlayingCharacter.useItem(item, target, () => {
                        endTurn();
                    });
                break;
                case 2: // 2 : skill
                    onPlayingCharacter.useSkill(orderNum, target, () => {
                        endTurn();
                    });
                break;
                case 3: // 3 : run
                break;
            }
        }
    }

    public void autoSupporter(CharacterData data){
        orderType = 2;
        orderNum = data.skills[data.useSkillNum];
        //battleMenuCtrl.selectTurnBtn();
        battleMenuCtrl.setItemMenu(false);
        battleMenuCtrl.itemMenu.startMenu();
    }

    public void autoAI(){
        CharacterData chardata = onPlayingCharacter.charData;
        if(chardata.skillAI.Equals("") || chardata.skillAI[chardata.skillNum] == '0'){
            orderType = 0;
            orderNum = chardata.deafultAttack;
        }
        else{
            orderType = 2;
            orderNum = chardata.skills[chardata.skillAI[chardata.skillNum] - '0' - 1];
        }

        if(chardata.usePer > 0 && chardata.usePerSkill != null){
            if(chardata.Hp < chardata.maxHp * chardata.usePer / 100){
                orderType = 2;
                orderNum = chardata.usePerSkill;
                chardata.usePerSkill = null;
            }
        }

        var candi = new List<BattleCharacter>();

        foreach(var item in allies){
            if(!item.isDead){
                candi.Add(item);
            }
        }

        target = candi[Random.Range(0, candi.Count)].gameObject;

        DOVirtual.DelayedCall(0.1f, ()=>{
            activeOrder();
        }, true);
        
        chardata.skillNum++;
        if(chardata.skillNum >= chardata.skillAI.Length){
            chardata.skillNum = 0;
        }
    }

    public GameObject getTargetFromInt(int num){
        if(num >= 3){
            num -= 3;
            return enemies[num].gameObject;
        }
        else{
            return allies[num].gameObject;
        }
    }

    public void battleStart(){
        foreach (var item in supports)
        {
            if(item.gameObject.activeInHierarchy)
                item.Animate("ready");
        }
        for(int i = 0; i < alliesOnPlaying.Count; i++){
            allies[i].Animate("ready");
        }
        for(int i = 0; i < phaseOnPlaying.Count; i++){
            enemies[i].Animate("ready");
        }

        isStarted = true;
    }

    public void refreshAllBtnValue(){
        foreach(var i in allies){
            i.refreshBtnValue();
        }
        foreach(var i in enemies){
            i.refreshBtnValue();
        }
    }

    public void battleEnd(){
        foreach(var i in alliesOnPlaying){
            i.onEndBattle();
        }
        
        ChangeScene.loadSceneWithoutEffect(onBattleEndSceneInfo);
    }

    public void checkBattleEnd(){
        bool isAllyLose = true;
        bool isEnemyLose = true;
        foreach(var allie in allies){
            if(allie != null && !allie.isDead){
                isAllyLose = false;
            }
        }

        foreach(var enemy in enemies){
            if(enemy != null && !enemy.isDead){
                isEnemyLose = false;
            }
        }

        if(isAllyLose){
            onBattleLose();
        }
        else if (isEnemyLose){
            if(phases.Count != 0){
                isStarted = false;

                setPhase();
                BattleCamEffects.instance.setText( (++phaseNum) + "번째 페이즈", 2);
                
                DOVirtual.DelayedCall(5f, () => {
                    BattleCamEffects.instance.setText("전투시작");
                    isStarted = true;
                }, true);
            }
            else{
                onBattleWin();
            }
        }
    }
    public void giveItems(){
        GameSystem.instance.give(giveItemOnWin, giveEquipOnWin, giveGoldOnWin);
    }

    public void onBattleWin(){
        BattleCamEffects.instance.setText("징버거 팀 승리!", 3f);
        SoundPlayer.instance.playBgmOneShot("win");
        onBattleEndSceneInfo = onBattleWinSceneInfo;
        foreach (var item in allies)
        {
            if(item.gameObject.activeInHierarchy)
                item.Animate("win");
        }
        foreach (var item in supports)
        {
            if(item.gameObject.activeInHierarchy)
                item.Animate("win");
        }
        isStarted = false;
        var s = BattleCamEffects.instance.endBattleScene();
        s.onComplete = () => {
            DOVirtual.DelayedCall(1f, battleEnd, true);
        };
        s.Play();
    }

    public void onBattleLose(){
        Debug.Log("Lose");
        BattleCamEffects.instance.setText("징버거 팀 패배!", 3f);
        for(int i = 0; i < phaseOnPlaying.Count; i++){
            enemies[i].Animate("win");
            onBattleEndSceneInfo = onBattleLoseSceneInfo;
        }
        isStarted = false;
        var s = BattleCamEffects.instance.endBattleScene();
        s.onComplete = () => {
            DOVirtual.DelayedCall(1f, battleEnd, true);
        };
        s.Play();
    }

    void makePhases(List<CharacterData> list){
        phases = new Queue<List<CharacterData>>();
        List<CharacterData> temp = null;
        for(int i = 0; i < list.Count; i++){
            if(i % enemies.Count == 0){
                temp = new List<CharacterData>();
                phases.Enqueue(temp);
            }
            temp.Add(list[i]);
        }
    }

    void setPhase(){
        phaseOnPlaying = phases.Dequeue();
        turn.Clear();

        foreach (var item in allies)
        {
            item.sumTime = 0;
        }
        
        if(phaseOnPlaying.Count == 3){
            battleMenuCtrl.enemies.setDefualtFocusNum(1);
            enemies[0].transform.position = enemyPos[2];
            enemies[1].transform.position = enemyPos[0];
            enemies[2].transform.position = enemyPos[1];
        }
        else{
            battleMenuCtrl.enemies.setDefualtFocusNum(0);
            enemies[0].transform.position = enemyPos[0];
            enemies[1].transform.position = enemyPos[1];
            enemies[2].transform.position = enemyPos[2];
        }

        foreach (var item in enemies){
            item.reset();
            item.transform.position = item.transform.position + new Vector3(spawnOffset, 0, 0);
            item.transform.DOMoveX(item.transform.position.x - spawnOffset, 2f);
        }
        
        if(battleMenuCtrl.enemies.transform.childCount != 0){
            var btns = battleMenuCtrl.enemies.GetComponentsInChildren<Transform>();

            for(int i = 1; i < btns.Length; i++){
                Destroy(btns[i].gameObject);
            }

            battleMenuCtrl.enemies.transform.DetachChildren();
        }

        for(int i = 0; i < phaseOnPlaying.Count; i++){
            enemies[i].isEnemy = true;
            enemies[i].setCharacterData(phaseOnPlaying[i]);
        }
    }
}
