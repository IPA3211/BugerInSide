using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;

public class CheckFile : MonoBehaviour
{
    public UnityEvent onTrue;

    public void checkFile(string name){
    #if UNITY_EDITOR
        string dirPath = Application.dataPath + "/Saves/";
    #elif UNITY_STANDALONE_WIN
        string dirPath = System.AppDomain.CurrentDomain.BaseDirectory + "/Saves/";
    #endif

        FileInfo fi = new FileInfo(dirPath + "save.json");
        if(fi.Exists){
            onTrue.Invoke();
        }
    }
}
