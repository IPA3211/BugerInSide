using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Passive", menuName = "Scriptable Object/Create Passive", order = 1)]
[System.Serializable]
public class PassiveData : StatusData
{
    
    [HideInInspector] public bool isUIShown = false;
    [Header ("Passive")]
    public bool isStun;
    public int duration;

    public float setMaxHP(float cur){
        var ans = cur + maxHp;
        return ans;
    }

    public float setMaxMp(float cur){
        var ans = cur + maxMp;
        return ans;
    }

    public float setMaxSpeed(float cur){
        var ans = cur * (Speed + 100) / 100;
        return ans;
    }

    public float setMaxATK(float cur){
        var ans = cur * (ATK + 100) / 100;
        return ans;
    }

    public float setMaxDEF(float cur){
        var ans = cur * (DEF + 100) / 100;
        return ans;
    }
}
