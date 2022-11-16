using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class SaveData{
    public SceneInfo scene;
    public GameSystemSave gameSystem;
    public GameEventStatusSave gameEventStatus;
}
public class GameSaveManager : MonoBehaviour
{
    public static bool isLoaded = false;
    public static GameSaveManager instance;
    void Awake()
    {
        if(instance == null){
            instance = this;
        }
    }
    public void save(){

    #if UNITY_EDITOR
        string dirPath = Application.dataPath + "/Saves/";
#elif UNITY_STANDALONE_WIN
        string dirPath = System.AppDomain.CurrentDomain.BaseDirectory + "/Saves/";
#endif

        string path = dirPath + "save.json";

        if(isLoaded){
            isLoaded = false;
            return;
        }

        SaveData data = new SaveData();
        data.gameSystem = new GameSystemSave(GameSystem.instance);
        data.gameEventStatus = new GameEventStatusSave(GameEventStatus.instance);
        data.scene = new SceneInfo();

        data.scene.sceneName = SceneManager.GetActiveScene().name;
        data.scene.position = GameObject.FindWithTag("Player").transform.position;

        DirectoryInfo di = new DirectoryInfo(dirPath);

        if(di.Exists == false){
            di.Create();
        }

        if(System.IO.File.Exists(path)){
            System.IO.File.Move(path, dirPath + "save" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".json");
        }

        FileStream fileStream = new FileStream(path, FileMode.Create);
        byte[] en = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data, true));
        fileStream.Write(en, 0, en.Length);
        fileStream.Close();
    }
    
    public void load(){
#if UNITY_EDITOR
        string dirPath = Application.dataPath + "/Saves/";
#elif UNITY_STANDALONE_WIN
        string dirPath = System.AppDomain.CurrentDomain.BaseDirectory + "/Saves/";
#endif

        string path = dirPath + "save.json";
        FileStream fileStream = new FileStream(path, FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string json = Encoding.UTF8.GetString(data);

        SaveData sd = JsonUtility.FromJson<SaveData>(json);

        sd.gameSystem.load(GameSystem.instance);
        sd.gameEventStatus.load(GameEventStatus.instance);

        isLoaded = true;
        ChangeScene.loadScene(sd.scene);
    }
}