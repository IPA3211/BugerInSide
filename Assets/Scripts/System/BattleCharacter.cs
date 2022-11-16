using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class BattleCharacter : MonoBehaviour
{
    public bool isEnemy;
    public bool isSupporter;
    public bool isOnTurn;
    public bool isDead = true;
    public float sumTime;
    public float blankTime = 0.5f;
    public TurnBarSlider turnBarSlider;
    public CharacterData charData;
    [HideInInspector]public BattleBtn battleBtnManager;
    public PassiveContainer passiveContainer;
    private MenuBtn btn;
    private Animator animator;
    private Vector2 allyOffset = new Vector2(-2, 0);
    private Vector2 enemyOffset = new Vector2(2, 0);
    private UnityEvent hitCallback;
    private SkillData curSkill;
    private ItemData curItem;
    private GameObject curTarget;
    private Sequence sequence;
    private Material charMat;
    private BattleMenuCtrl battleMenuCtrl;
    private bool canUseSkillFrame;

    public void setCharacterData(CharacterData data){
        sumTime = 0;
        battleMenuCtrl = BattleManager.instance.battleMenuCtrl;
        animator = GetComponent<Animator>();
        charData = data;
        animator.runtimeAnimatorController = charData.AnimatorController;
        isSupporter = data.isSupporter;
        isDead = false;

        charMat = gameObject.GetComponent<Renderer>().material;
        charMat.SetFloat("_OutlineGlow", 5f);
        charMat.EnableKeyword("OUTBASEPIXELPERF_ON");
        charMat.SetInt("_OutlinePixelWidth", 1);

        if(!isSupporter){
            var btn = Instantiate(battleMenuCtrl.charBtn, isEnemy ? battleMenuCtrl.enemies.transform : battleMenuCtrl.allies.transform);
            var mb = btn.GetComponent<MenuBtn>();
            var bb = btn.GetComponent<BattleBtn>();
            mb.onClickBtn.AddListener((o) => {
                if(BattleManager.instance.orderNum.isUseAble(BattleManager.instance.onPlayingCharacter.charData, this.charData)){
                    BattleManager.instance.setTarget(this.gameObject);
                    BattleManager.instance.activeOrder();
                    mb.endFocus();
                }
            });
            mb.onFocusLoop.AddListener((o) => {
                charMat.SetFloat("_OutlineAlpha", Mathf.Lerp(1, 0, Mathf.PingPong(o.sumTime, blankTime) / blankTime));
            });
            mb.onEndFocus.AddListener((o) => {
                charMat.SetFloat("_OutlineAlpha", 0f);
            });

            battleBtnManager = bb;
            this.btn = mb;
            
            passiveContainer.setTarget(gameObject);
            
            battleBtnManager.gameObject.SetActive(true);
            turnBarSlider.gameObject.SetActive(true);
            turnBarSlider.setData(data);
            refreshBtnValue();
        }
        gameObject.SetActive(true);

        if(isEnemy)
            GetComponent<SpriteRenderer>().flipX = (!data.flip);
        else
            GetComponent<SpriteRenderer>().flipX = (data.flip);
    }

    public void reset(){
        isDead = true;
        sumTime = 0;
        charData = null;
        turnBarSlider.gameObject.SetActive(false);
        gameObject.SetActive(false);
        passiveContainer.resetContainer();
    }

    public void LateUpdate(){
        canUseSkillFrame = true;
    }

    public void Animate (string state)
	{
        if(animator.HasState(0, Animator.StringToHash(state)))
		    animator.Play(state);
        else{
            switch (state)
            {
                case "hit" :
                    float blend = 0f;
                    charMat.SetFloat("_HitEffectGlow", 1f);
                    charMat.SetFloat("_HitEffectBlend", blend);
                    charMat.EnableKeyword("HITEFFECT_ON");
                    
                    Sequence h = DOTween.Sequence();
                    h.Append(
                        DOTween.To(() => blend, x => {
                            blend = x;
                            charMat.SetFloat("_HitEffectBlend", blend);
                        }, 1f, 0.1f).SetLoops(4, LoopType.Yoyo));

                    h.AppendCallback(()=>{
                        charMat.SetFloat("_HitEffectGlow", 1f);
                        charMat.SetFloat("_HitEffectBlend", blend);
                        charMat.EnableKeyword("HITEFFECT_ON");
                    });
                break;
                case "dead" :
                    Vector3 ori = transform.position;
                    float alpha = 1f;
                    Sequence d = DOTween.Sequence();
                    d.AppendInterval(0.5f);
                    d.Append(transform.DOMoveY(transform.position.y + 1f, 1f));
                    d.Join(DOTween.To(() => alpha, x => {
                            alpha = x;
                            GetComponent<SpriteRenderer>().color = new Color(1,1,1,alpha);
                        }, 0f, 1f));
                    d.AppendCallback(()=>{
                        reset();
                        transform.position = ori;
                        GetComponent<SpriteRenderer>().color = Color.white;
                    });
                break;
                default:
                break;
            }
        }
	}

    public float getAnimationLength(string name){
        
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach(var c in clips){
            if(c.name.Equals(charData.titleEng+ "_" + name)){
                return c.length + 0.5f;
            }
        }
        return 0.0f;
    }

    public void playSFX(Object name){
        var a = name as AudioClip;
        
        SoundPlayer.instance.startSFX(a);
    }

    public void useSkill(SkillData skillNum, GameObject target, TweenCallback onFinish){
        curSkill = skillNum; 
        curTarget = target;

        playAnimationSequence(target, onFinish);
    }

    public void useItem(ItemData itemNum, GameObject target, TweenCallback onFinish){
        curTarget = target;
        curSkill = itemNum;
        
        GameSystem.instance.useItem(itemNum, 1);

        playAnimationSequence(target, onFinish);
    }

    public void playAnimationSequence(GameObject target, TweenCallback onFinish){
        Vector3 oriPos = transform.position;
        sequence = DOTween.Sequence();

        Vector3 offset = isEnemy ? -curSkill.offset : curSkill.offset;
        curSkill.useSkill(charData);

        hitCallback = new UnityEvent();
        hitCallback.AddListener(() => {
            foreach(var item in setTarget(curSkill, target)){
                if(!item.isDead){
                    hitCharactor(curSkill, item);
                }
            }
        });
        
        if(!isSupporter)
            battleBtnManager.refreshData(charData);

        gameObject.GetComponent<Renderer>().sortingOrder = 1;

        if(curSkill.startDialogue != null && !curSkill.startDialogue.Equals("")){
            sequence.Append(BattleCamEffects.instance.uiFade());
            sequence.AppendCallback(() => {
                DialogueCtrl.instance.startDialogue(curSkill.startDialogue);
            });
        }

        if(curSkill.canUseToAll){
            if(curSkill.canUseToEnemy)
                sequence.Append(transform.DOMove(offset + new Vector3(0, -0.44f, 0), 1f));
            else
                sequence.Append(transform.DOMove(transform.position, 1f));
        }
        else{
            sequence.Append(transform.DOMove(target.transform.position + offset, 1f));
        }
        
        sequence.AppendCallback(() => {
            animator.Play(curSkill.titleEng);
            sequence.Pause();
        });
        sequence.AppendInterval(0.5f);
        sequence.Append(transform.DOMove(oriPos, 1f));
        sequence.Join(BattleCamEffects.instance.fadeLight(true));

        if(curSkill.startDialogue != null && !curSkill.startDialogue.Equals("")){
            if(curSkill.endDialogue != null && !curSkill.endDialogue.Equals("")){
                sequence.AppendCallback(() => {
                    DialogueCtrl.instance.startDialogue(curSkill.endDialogue);
                });
            }
        }
        sequence.Join(BattleCamEffects.instance.startBattleScene());
        sequence.AppendCallback(() => {
            gameObject.GetComponent<Renderer>().sortingOrder = 0;
        });

        sequence.onComplete = onFinish;
    }

    public List<BattleCharacter> setTarget(SkillData skill, GameObject defaultTarget){
        List<BattleCharacter> targets = null;
        if(skill.canUseToAll){
            if(skill.canUseToEnemy){
                targets = isEnemy ? BattleManager.instance.allies : BattleManager.instance.enemies;
            }
            else if(skill.canUseToFriend){
                targets = isEnemy ? BattleManager.instance.enemies : BattleManager.instance.allies;
            }
        }
        else{
            targets = new List<BattleCharacter>();
            targets.Add(defaultTarget.GetComponent<BattleCharacter>());
        }
        return targets;
    }

    public void hitCharactor(SkillData data, BattleCharacter target){
        float curhp = target.charData.Hp;
        curSkill.hitSkill(charData, target.charData);
        if((int)target.charData.Hp - (int)curhp != 0){
            var obj = Instantiate(BattleCamEffects.instance.floatingNum, target.gameObject.transform);
            obj.GetComponent<FloatingNumber>().init((int)target.charData.Hp - (int)curhp);
        }
        if(curhp > target.charData.Hp + 1f){
            target.Animate("hit");
            if(curSkill.hitSoundClip != null)
                SoundPlayer.instance.sfxSource.PlayOneShot(curSkill.hitSoundClip);
            else{
                SoundPlayer.instance.startSFX("hit");
            }
        }
        else if(curhp < target.charData.Hp - 1f){
        }

        if(target.charData.Hp <= 0){
            target.charData.Hp = 0;
            target.isDead = true;
            target.Animate("dead");
            target.btn.isClickAble = false;
        }
        
        BattleManager.instance.refreshAllBtnValue();
    }

    public void refreshBtnValue(){
        if(charData != null){
            battleBtnManager.refreshData(charData);

            foreach (var item in charData.passives)
            {
                if(!item.isUIShown){
                    item.isUIShown = true;
                    passiveContainer.addPassive(item);
                }
            }
        }
    }

    public void endTurn(){
        foreach(var a in charData.passives){
            a.duration -= 1;
            if(a.duration <= 0){
                charData.passives.Remove(a);
                break;
            }
        }
    }

    public void targetEffectTiming(){
        var targets = setTarget(curSkill, curTarget);
        bool useToOne = true;

        if(curSkill.stopWhenEff){
            animator.enabled = false;
        }
        
        
        for(int i = 0; i < targets.Count; i++){
            var item = targets[i];

            if(!item.isDead){
                var eff = curSkill.useTargetEffect(gameObject, item.gameObject);
                if(curSkill.skillAllTargetEffect != null){
                    var ee = eff.GetComponent<EndEffect>();
                    ee.OnHitChar.AddListener(() => {
                        aniHitCallback();
                    });
                    ee.OnEndEff.AddListener(() => {
                        if(curSkill.stopWhenEff){
                            animator.enabled = true;
                            Debug.Log("end Call");
                        }
                    });
                    break;
                }
                else{
                    if(useToOne){
                        var ee = eff.GetComponent<EndEffect>();
                        ee.OnHitChar.AddListener(() => {
                            aniHitCallback();
                        });
                        ee.OnEndEff.AddListener(() => {
                            if(curSkill.stopWhenEff){
                                animator.enabled = true;
                            }
                        });
                        useToOne = false;
                    }
                }
            }
        }
    }

    public void userEffectTiming(){
        if(curSkill.stopWhenUserEff){
            animator.enabled = false;
        }

        var eff = curSkill.useUserEffect(gameObject, curTarget);
        var ee = eff.GetComponent<EndEffect>();
        ee.OnHitChar.AddListener(() => {
            aniHitCallback();
        });
        ee.OnEndEff.AddListener(() => {
            if(curSkill.stopWhenEff){
                animator.enabled = true;
            }
        });
    }

    public void addTimeToTurn(float delta){
        if(!isDead){
            sumTime += delta * charData.getSpeed();
        }
    }

    public float remainTimeToTrun(){
        float ans = BattleManager.instance.timeToTurn;
        if(!isDead){
            ans -= sumTime;
            ans /= charData.getSpeed();
        }
        return ans;
    }

    public void isSelectable(){

    }

    public void aniPlayVideo(){
        animator.enabled = false;
        var s = BattleCamEffects.instance.playVideo(curSkill.skillVideoClip);
        s.onComplete = () => {
            animator.enabled = true;
        };
        s.Play();
    }

    public void aniEndCallback(){
        sequence.Play();
        sequence = null;
    }

    public void shakeCam(float duration){
        var s = BattleCamEffects.instance.shakeCam(duration);
        s.Play();
    }

    public void aniHitCallback(){
        if(canUseSkillFrame){
            hitCallback.Invoke();
            canUseSkillFrame = false;
        }
    }

    public void readyFinish(string aa){
        Debug.Log(aa);
    }
}
