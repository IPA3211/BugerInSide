using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class debugLoader{
    public List<string> ally; 
    public List<string> enemy;

    public debugLoader(List<string> a, List<string> e){
        ally = a;
        enemy = e;
    }
}

public class DebugEnemy : MonoBehaviour
{
    public void setEnemy(){
#if UNITY_EDITOR
        string realPath = Application.dataPath + "/Debug/debugMapSetting.json";
#elif UNITY_STANDALONE_WIN
        string realPath = System.AppDomain.CurrentDomain.BaseDirectory + "/Debug/debugMapSetting.json";
#endif
        
        FileStream fileStream = new FileStream(realPath, FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string json = Encoding.UTF8.GetString(data);

        debugLoader sd = JsonUtility.FromJson<debugLoader>(json);

        List<CharacterData> enemies = new List<CharacterData>();

        foreach(var a in sd.enemy){
            int hash = a.GetHashCode();
            var e = GameObjectDatas.instance.enemies.Find(a => a.hash == hash);
            if(e == null){
                e = GameObjectDatas.instance.characters.Find(a => a.hash == hash);
                if(e == null){
                    break;
                }
            }
            enemies.Add(e);
        }

        GetComponent<StartBattle>().enemies = enemies;
    }

    public void setAlly(){
#if UNITY_EDITOR
        string realPath = Application.dataPath + "/Debug/debugMapSetting.json";
#elif UNITY_STANDALONE_WIN
        string realPath = System.AppDomain.CurrentDomain.BaseDirectory + "/Debug/debugMapSetting.json";
#endif
        
        FileStream fileStream = new FileStream(realPath, FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string json = Encoding.UTF8.GetString(data);

        debugLoader sd = JsonUtility.FromJson<debugLoader>(json);
        
        List<CharacterData> characters = new List<CharacterData>();
        List<CharacterData> supporters = new List<CharacterData>();
        GameSystem.instance._Characters.Clear();
        GameSystem.instance._Supporters.Clear();
        foreach(var a in sd.ally){
            int hash = a.GetHashCode();
            var e = GameObjectDatas.instance.characters.Find(a => a.hash == hash);
            if(e == null){
                e = GameObjectDatas.instance.supporters.Find(a => a.hash == hash);
                if(e == null){
                    e = GameObjectDatas.instance.enemies.Find(a => a.hash == hash);
                    if(e == null){
                        break;
                    }
                }
            }
            if(e.isSupporter){
                supporters.Add(e);
            }
            else{
                characters.Add(e);
            }
        }
        foreach(var item in characters){
            GameSystem.instance.joinChar(item);
        }
        foreach(var item in supporters){
            GameSystem.instance.joinChar(item);
        }
    }

    void Start(){
#if UNITY_EDITOR
        string realPath = Application.dataPath + "/Debug/debugMapSetting.json";
        string dirPath = Application.dataPath + "/Debug/";
#elif UNITY_STANDALONE_WIN
        string realPath = System.AppDomain.CurrentDomain.BaseDirectory + "/Debug/debugMapSetting.json";
        string dirPath = System.AppDomain.CurrentDomain.BaseDirectory + "/Debug/";
#endif

        DirectoryInfo di = new DirectoryInfo(dirPath);
        if(di.Exists == false){
            di.Create();
        }
        
        FileInfo fi = new FileInfo(realPath);
        if(fi.Exists == false){
            var a = new List<string>();
            var b = new List<string>();

            a.Add("knight");
            b.Add("mu");

            debugLoader data = new debugLoader(a, b);
            FileStream fileStream = new FileStream(realPath, FileMode.Create);
            byte[] en = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data, true));
            fileStream.Write(en, 0, en.Length);
            fileStream.Close();
        }  

        GameSystem.instance.giveGold(99999);
        setAlly();
    }
}
