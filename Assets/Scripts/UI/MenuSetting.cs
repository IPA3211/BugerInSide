using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuSetting : MonoBehaviour
{
    public TMP_Text soundMstText;
    public TMP_Text soundBgmText;
    public TMP_Text soundSfxText;
    public TMP_Text displayModeText;
    public TMP_Text displayResText;

    public bool mode = false;
    public int resIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void initItalizeMenu(){
        soundMstText.text = ((int)(SoundPlayer.instance.MasterVolume + 40) / 4).ToString();
        soundBgmText.text = ((int)(SoundPlayer.instance.BGMvolume + 40) / 4).ToString();
        soundSfxText.text = ((int)(SoundPlayer.instance.SFXvolume + 40) / 4).ToString();

        mode = ResolutionFixer.instance.mode;
        resIndex = ResolutionFixer.instance.getIndex();
        setDisplayMenu();
    }

    public void setDisplayMenu(){
        displayModeText.text = screenModeToString(mode);
        displayResText.text = ResolutionFixer.instance.resolutions[resIndex].ToString();
    }

    private string screenModeToString(bool mode){
        return mode ? "전체화면" : "창모드";
    }

    public void changeMasterVolume(bool isUp){
        SoundPlayer.instance.MasterVolume += isUp ? 4 : -4;
        if(SoundPlayer.instance.MasterVolume <= -40f){
            SoundPlayer.instance.MasterVolume = -40f;
        }
        else if(SoundPlayer.instance.MasterVolume >= 0f){
            SoundPlayer.instance.MasterVolume = 0f;
        }
        SoundPlayer.instance.setMasterVolume(SoundPlayer.instance.MasterVolume);
        initItalizeMenu();
    }

    public void changeBGMVolume(bool isUp){
        SoundPlayer.instance.BGMvolume += isUp ? 4 : -4;
        if(SoundPlayer.instance.BGMvolume <= -40f){
            SoundPlayer.instance.BGMvolume = -40f;
        }
        else if(SoundPlayer.instance.BGMvolume >= 0f){
            SoundPlayer.instance.BGMvolume = 0f;
        }
        SoundPlayer.instance.setBGMVolume(SoundPlayer.instance.BGMvolume);
        initItalizeMenu();
    }

    public void changeSFXVolume(bool isUp){
        SoundPlayer.instance.SFXvolume += isUp ? 4 : -4;
        if(SoundPlayer.instance.SFXvolume <= -40f){
            SoundPlayer.instance.SFXvolume = -40f;
        }
        else if(SoundPlayer.instance.SFXvolume >= 0f){
            SoundPlayer.instance.SFXvolume = 0f;
        }
        SoundPlayer.instance.setSFXVolume(SoundPlayer.instance.SFXvolume);
        initItalizeMenu();
    }

    public void changeRes(bool isUp){
        resIndex += isUp ? 1 : -1;

        if(resIndex >= ResolutionFixer.instance.indexLimit){
            resIndex = ResolutionFixer.instance.indexLimit;
        }
        else if(resIndex <= 0){
            resIndex = 0;
        }
        setDisplayMenu();
    }

    public void changeMode(bool isUp){
        mode = isUp;
        setDisplayMenu();
    }

    public void summitDisplay(){
        ResolutionFixer.instance.setMode(mode);
        ResolutionFixer.instance.setIndex(resIndex);
        ResolutionFixer.instance.SetResolution();
        setDisplayMenu();
    }
}
