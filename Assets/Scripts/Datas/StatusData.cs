using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEditor;
using System;

[System.Serializable]
public class StatusSave{
    public int hash;
    public StatusSave(StatusData data){
        hash = data.hash;
    }
}

[System.Serializable]
public class StatusRaw{
    public string title;
    public string describe;
    public float maxHp;
    public float maxMp;
    public float Speed;
    public float ATK;
    public float DEF;
    public int cost;
    
    public StatusRaw(StatusData data){
        title = data.title;
        describe = data.describe;
        maxHp = data.maxHp;
        maxMp = data.maxMp;
        Speed = data.Speed;
        ATK = data.ATK;
        DEF = data.DEF;
        cost = data.cost;
    }

    public void load(StatusData data){
        data.title = title;
        data.describe = describe;
        data.maxHp = maxHp;
        data.Hp = maxHp;
        data.maxMp = maxMp;
        data.Speed = Speed;
        data.ATK = ATK;
        data.DEF = DEF;
        data.cost = cost;
    }
}

[System.Serializable]
public abstract class StatusData : ScriptableObject
{
    public int sortingNum;
    public int hash;
    [Header ("Status")]
    public string titleEng;
    public string title;
    public Sprite describeImage;
    [TextArea (3, 5)]
    public string describe;
    public float maxHp;
    public float Hp;
    public float maxMp;
    public float Mp;
    public float Speed;
    public float ATK;
    public float DEF;
    public int cost;
    
    public void saveAsFile(string path){
#if UNITY_EDITOR
        string realPath = Application.dataPath + path + this.name + ".json";
        string dirPath = Application.dataPath + path;
        bool isEditor = true;
#elif UNITY_STANDALONE_WIN
        string realPath = System.AppDomain.CurrentDomain.BaseDirectory + path + this.name + ".json";
        string dirPath = System.AppDomain.CurrentDomain.BaseDirectory + path;
        bool isEditor = false;
#endif

        StatusRaw data = new StatusRaw(this);

        DirectoryInfo di = new DirectoryInfo(dirPath);
        if(di.Exists == false){
            di.Create();
        }

        FileStream fileStream = new FileStream(realPath, FileMode.Create);
        byte[] en = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data,true));
        fileStream.Write(en, 0, en.Length);
        fileStream.Close();
    }

    public void loadFile(string path){
        
#if UNITY_EDITOR
        string realPath = Application.dataPath + path + this.name + ".json";
        bool isEditor = true;
#elif UNITY_STANDALONE_WIN
        string realPath = System.AppDomain.CurrentDomain.BaseDirectory + path + this.name + ".json";
        bool isEditor = false;
#endif

        FileInfo fi = new FileInfo(realPath);

        if(fi.Exists){
            FileStream fileStream = new FileStream(realPath, FileMode.Open);
            byte[] data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);
            fileStream.Close();
            string json = Encoding.UTF8.GetString(data);

            StatusRaw sd = JsonUtility.FromJson<StatusRaw>(json);
            sd.load(this);
        }
        
    }

    public void setHash(){
        hash = name.GetHashCode();
    }

    public virtual void addStatus(StatusData other){
        maxHp += other.maxHp;
        Hp += other.Hp;
        maxMp += other.maxMp;
        Mp += other.Mp;

        if(Hp > maxHp){
            Hp = maxHp;
        }
        if(Mp > maxMp){
            Mp = maxMp;
        }

        Speed += other.Speed;
        ATK += other.ATK;
        DEF += other.DEF;
    }

    public void subStatus(StatusData other){
        maxHp -= other.maxHp;
        Hp -= other.Hp;
        maxMp -= other.maxMp;
        Mp -= other.Mp;

        if(Hp > maxHp){
            Hp = maxHp;
        }
        if(Mp > maxMp){
            Mp = maxMp;
        }

        Speed -= other.Speed;
        ATK -= other.ATK;
        DEF -= other.DEF;
    }
}
