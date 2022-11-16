using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class LoadMapFromSave : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
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
        gameObject.GetComponent<ChangeScene>().sceneToChange = sd.scene;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
