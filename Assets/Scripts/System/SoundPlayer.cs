using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Bgm{
    public string name;
    public List<AudioClip> audios;
}

public class SoundPlayer : MonoBehaviour
{
    public AudioMixer masterMixer;
    public List<Bgm> bgms;
    public List<Bgm> sfxs;
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    [Range(-40f, 0f)]
    public float BGMvolume = 0f;
    [Range(-40f, 0f)]
    public float SFXvolume = 0f;
    [Range(-40f, 0f)]
    public float MasterVolume = 0f;
    private Bgm nowPlaying;
    private int trackNum;
    private bool isPlayBgmOneShot = false;
    private static SoundPlayer _instance;

    public static SoundPlayer instance
    {
        get
        {
           return _instance;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        if(_instance == null){
            _instance = this;

            foreach (var item in bgms)
            {
                item.name = item.name.ToLower();   
            }

            foreach (var item in sfxs)
            {
                item.name = item.name.ToLower();
            }

            var bgmAudios = new List<AudioClip>(Resources.LoadAll<AudioClip>("Audios/BGM"));        
            foreach (var item in bgmAudios)
            {
                var bgm = new Bgm();
                bgm.name = item.name.ToLower();
                bgm.audios = new List<AudioClip>();
                bgm.audios.Add(item);
                bgms.Add(bgm);
            }

            var sfxAudios = new List<AudioClip>(Resources.LoadAll<AudioClip>("Audios/SFX"));        
            foreach (var item in sfxAudios)
            {
                var sfx = new Bgm();
                sfx.name = item.name.ToLower();
                sfx.audios = new List<AudioClip>();
                sfx.audios.Add(item);
                sfxs.Add(sfx);
            }

            print(PlayerPrefs.GetFloat("MasterVolume", 0f));

            DontDestroyOnLoad(gameObject);
            init();
        }
        else{
            Destroy(gameObject);
            return;
        }
    }
    void Start(){
        setMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 0f));
        setBGMVolume(PlayerPrefs.GetFloat("BGMVolume", 0f));
        setSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 0f));
    }

    public void setMasterVolume(float volume){
        if(volume == -40f) masterMixer.SetFloat("Master", -80f);
        else masterMixer.SetFloat("Master", volume);

        MasterVolume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    public void setBGMVolume(float volume){
        if(volume == -40f) masterMixer.SetFloat("BGM", -80f);
        else masterMixer.SetFloat("BGM", volume);
        
        BGMvolume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }
    public void setSFXVolume(float volume){
        if(volume == -40f) masterMixer.SetFloat("SFX", -80f);
        else masterMixer.SetFloat("SFX", volume);
        
        SFXvolume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    void init(){
        bgmSource.Stop();
        nowPlaying = null;
        trackNum = 0;
    }

    void Update(){
        if(nowPlaying != null && isPlayBgmOneShot == false){
            if(!bgmSource.isPlaying){
                bgmSource.PlayOneShot(nowPlaying.audios[trackNum]);
                if(trackNum < nowPlaying.audios.Count - 1){
                    trackNum++;
                }
            }
        }
    }

    public void startBGM(string name){
        name = name.ToLower();
        if(nowPlaying != null && nowPlaying.name.Equals(name)){
            return;
        }
        
        isPlayBgmOneShot = false;

        foreach (var bgm in bgms)
        {
            if(bgm.name.Equals(name)){
                init();
                nowPlaying = bgm;
                break;
            }
        }
    }

    public void playBgmOneShot(string name){
        name = name.ToLower();
        if(nowPlaying != null && nowPlaying.name.Equals(name)){
            return;
        }

        isPlayBgmOneShot = true;

        foreach (var bgm in bgms)
        {
            if(bgm.name.Equals(name)){
                init();
                nowPlaying = bgm;
                bgmSource.PlayOneShot(nowPlaying.audios[trackNum]);
                break;
            }
        }
    }



    public void startSFX(string name){
        foreach (var sfx in sfxs)
        {
            if(sfx.name.Equals(name)){
                sfxSource.PlayOneShot(sfx.audios[0]);
                break;
            }
        }
    }

    public void startSFX(AudioClip ac){
        sfxSource.PlayOneShot(ac);
    }
}
